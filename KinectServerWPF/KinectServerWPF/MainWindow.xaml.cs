﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using System.Windows.Media.Media3D;
using System.Threading.Tasks;

namespace KinectServerWPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Members
        KinectSensor sensor;
        MultiSourceFrameReader reader;
        IList<Body> bodies;

        bool showCamera = false;

        static string originTank = "-";
        static string targetTank = "-";
        static bool startPump = false;
        static bool succes;

        static int frameCounter = 0;

        ServerCommunication s = new ServerCommunication();

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sensor = KinectSensor.GetDefault();
            sensor.Open();
            if (sensor != null)
            {
                sensor.Open();
                reader = sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
                reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
        }

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Camera
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null && showCamera)
                {
                    int width = frame.FrameDescription.Width;
                    int height = frame.FrameDescription.Height;
                    PixelFormat format = PixelFormats.Bgr32;
            
                    byte[] pixels = new byte[width * height * ((format.BitsPerPixel + 7) / 8)];
                    if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
                    {
                        frame.CopyRawFrameDataToArray(pixels);
                    }
                    else
                    {
                        frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
                    }
                    int stride = width * format.BitsPerPixel / 8;
            
                    camera.Source = BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
                }
            }
            
            // Body
            using ( var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    bodies = new Body[frame.BodyFrameSource.BodyCount];

                    frame.GetAndRefreshBodyData(bodies);

                    foreach (var body in bodies)
                    {
                        if (body.IsTracked)
                        {

                            //Find hand states
                            bool rightHandOpen = false;
                            bool leftHandOpen = false;

                            switch (body.HandRightState)
                            {
                                case HandState.Open:
                                    rightHandOpen = true;
                                    break;
                                case HandState.Closed:
                                    rightHandOpen = false;
                                    break;
                                default:
                                    rightHandOpen = false; // To avoid pumping unintended
                                    break;
                            }

                            switch (body.HandLeftState)
                            {
                                case HandState.Open:
                                    leftHandOpen = true;
                                    break;
                                case HandState.Closed:
                                    leftHandOpen = false;
                                    break;
                                default:
                                    leftHandOpen = false; 
                                    break;
                            }

                            if (rightHandOpen && leftHandOpen) startPump = true;
                            else startPump = false;

                            if (startPump) tblPumpStatus.Text = "Pumping";
                            else tblPumpStatus.Text = "Pump stopped";
    
                          

                            // Find arm position
                            // Find required joints
                            List<Joint> trackedJoints = new List<Joint>();

                            trackedJoints.Add(body.Joints[JointType.WristRight]);
                            trackedJoints.Add(body.Joints[JointType.ElbowRight]);
                            trackedJoints.Add(body.Joints[JointType.ShoulderRight]);

                            trackedJoints.Add(body.Joints[JointType.WristLeft]);
                            trackedJoints.Add(body.Joints[JointType.ElbowLeft]);
                            trackedJoints.Add(body.Joints[JointType.ShoulderLeft]);

                            trackedJoints.Add(body.Joints[JointType.SpineShoulder]);
                            trackedJoints.Add(body.Joints[JointType.SpineBase]);

                            // Calculate Angles
                            if (trackedJoints.TrueForAll(x => x.TrackingState == TrackingState.Tracked))
                            {
                                Vector3D wristRight = new Vector3D(body.Joints[JointType.WristRight].Position.X, body.Joints[JointType.WristRight].Position.Y, body.Joints[JointType.WristRight].Position.Z);
                                Vector3D elbowRight = new Vector3D(body.Joints[JointType.ElbowRight].Position.X, body.Joints[JointType.ElbowRight].Position.Y, body.Joints[JointType.ElbowRight].Position.Z);
                                Vector3D shoulderRight = new Vector3D(body.Joints[JointType.ShoulderRight].Position.X, body.Joints[JointType.ShoulderRight].Position.Y, body.Joints[JointType.ShoulderRight].Position.Z);

                                Vector3D wristLeft = new Vector3D(body.Joints[JointType.WristLeft].Position.X, body.Joints[JointType.WristLeft].Position.Y, body.Joints[JointType.WristLeft].Position.Z);
                                Vector3D elbowLeft = new Vector3D(body.Joints[JointType.ElbowLeft].Position.X, body.Joints[JointType.ElbowLeft].Position.Y, body.Joints[JointType.ElbowLeft].Position.Z);
                                Vector3D shoulderLeft = new Vector3D(body.Joints[JointType.ShoulderLeft].Position.X, body.Joints[JointType.ShoulderLeft].Position.Y, body.Joints[JointType.ShoulderLeft].Position.Z);

                                Vector3D spineShoulder = new Vector3D(body.Joints[JointType.SpineShoulder].Position.X, body.Joints[JointType.SpineShoulder].Position.Y, body.Joints[JointType.SpineShoulder].Position.Z);
                                Vector3D spineBase = new Vector3D(body.Joints[JointType.SpineBase].Position.X, body.Joints[JointType.SpineBase].Position.Y, body.Joints[JointType.SpineBase].Position.Z);

                                // You should only be able to select tank with stretched arms
                                if (isArmStretched(wristLeft, elbowLeft, shoulderLeft))
                                {
                                    // Origin
                                    originTank = selectTank(angleBetween(elbowLeft - shoulderLeft, spineBase - spineShoulder));
                                    tblOriginTank.Text = originTank;
                                }
                                if (isArmStretched(wristRight, elbowRight, shoulderRight))
                                {
                                    // Target
                                    targetTank = selectTank(angleBetween(elbowRight - shoulderRight, spineBase - spineShoulder));
                                    tblTargetTank.Text = targetTank;
                                }
                                if (frameCounter % 100 == 0)
                                // To avoid spamming the server
                                {
                                    communicateWithServer();
                                }
                                frameCounter++;
                            }
                        }
                    }
                }
            }
        }
        public bool isArmStretched(Vector3D wrist, Vector3D elbow, Vector3D shoulder)
        // Return whether the arm is reasonably stretched
        {
            double armAngle = angleBetween(wrist - elbow, shoulder - elbow);
            if (armAngle > 150 || armAngle < 210) return true;
            else return false;
        }

        public double angleBetween(Vector3D a, Vector3D b)
        // Retrun the angle between to vectors
        {
            a.Normalize();
            b.Normalize();
            double dotProduct = Vector3D.DotProduct(a, b);

            return (double)Math.Acos(dotProduct) / Math.PI * 180;
        }

        public string selectTank(double armAngle)
        // Select a tank based on the the arm's angle relativ to the upper body
        // 30 - 70 -> tank 1
        // 70 - 110 -> tank 2
        // 110 - 150 -> tank 3
        {
            if (armAngle >= 30 && armAngle < 70 || armAngle > 290 && armAngle <= 330) return "Tank 1";
            if (armAngle >= 70 && armAngle < 110 || armAngle > 250 && armAngle <= 290) return "Tank 2";
            if (armAngle >= 110 && armAngle < 150 || armAngle > 210 && armAngle <= 250) return "Tank 3";
            else return "-";
        }

        public void communicateWithServer()
        {
            List<string> tankLevels = s.sendSoapReadMessage();

            showLevels(tankLevels);

            if (startPump)
            {
                succes = s.sendSoapWriteMessage(originTank, targetTank, 1);
            }
            else { succes = s.sendSoapWriteMessage(null, null, 0); }
            if (!succes) tblPumpStatus.Text = "No connection to server";   
        }

        private void showLevels(List<string> tankLevels)
        {
            if(tankLevels.Count == 3)
            {
                string level1 = tankLevels[0];
                string level2 = tankLevels[1];
                string level3 = tankLevels[2];

                tblLevel1.Text = "Tank 1: " + level1;
                tblLevel2.Text = "Tank 2: " + level2;
                tblLevel3.Text = "Tank 3: " + level3;
            }
        }

        private void btnShowCamera_Click(object sender, RoutedEventArgs e)
        {
            if (showCamera)
            {
                btnShowCamera.Content = "Show Camera";
                camera.Source = null;
            }
            else
            {
                btnShowCamera.Content = "Hide Camera";
            }
            showCamera = !showCamera;
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Kontrollieren Sie die Versuchsanlage mit Hilfe Ihrer Arme.\nWählen Sie dazu mit Ihrem linken Arm den Starttank aus und mit Ihrem rechten Arm den Zieltank.\n" +
                "Um den Pumpvorgang zu starten öffnen Sie beide Hände.\nSobald eine Hand geschlossen wird, wird der Pumpvorgang gestoppt.", "Help");
        }

    }
}