   M S B A M L   `   `   `  ���� X  TGestensteuerungVersuchsanlage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:KinectServerWPFKinectServerWPF  .    (GestensteuerungVersuchsanlage.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:KinectServerWPF     -     � Title$#  GestensteuerungVersuchsanlage��$	��700��$
��1200��$	��700��$
��1200�� � 
ResizeMode$ 	CanResize=�.��� 
���iii� � ColumnDefinitions �� $	��55* ��� $��1137* �.+�(�  (� Stretch$ UniformToFill=�$��2��.��� $
��1920��$
��1080��.+��� $��resources\Help.png��$
��Left=�$
��None=��� -   $��camera��$
��Fill=��� -    ��Name$ TankViz��
���iii�$y�Hidden=�.+�� $	��900��$	��720��$
��Left=�$��
200,80,0,0q�.+�� $	��300��$
��Left=�.+��� -   $��tank1��$	��150��$��40�� �� CornerRadius$ 	0,0,40,40��
����� �$��Bottom=�$��0,0,0,4q��� 
���   �$��5q�$	��150��$	��550��$ 40��$��Bottom=��� $��Tank 1��$��Center=�$��60(����	#FFF0F0F0�� -   $��lblTank1��$��-��$��Center=�$��40(����	#FFF0F0F0$��0,70,0,0q�� $	��300��.+��� -   $��tank2��$	��150��$��40��$ 	0,0,40,40��
����� �$��Bottom=�$��0,0,0,4q��� 
���   �$��5q�$	��150��$	��550��$ 40��$��Bottom=��� $��Tank 2��$��Center=�$��60(����	#FFF0F0F0�� -   $��lblTank2��$��-��$��Center=�$��40(����	#FFF0F0F0$��0,70,0,0q�� $	��300��$��Right=�.+��� -   $��tank3��$	��150��$��40��$ 	0,0,40,40��
����� �$��Bottom=�$��0,0,0,4q��� 
���   �$��5q�$	��150��$	��550��$ 40��$��Bottom=��� $��Tank 3��$��Center=�$��60(����	#FFF0F0F0�� -	   $��lblTank3��$��-��$��Center=�$��40(����	#FFF0F0F0$��0,70,0,0q�� $��Right=�$	��400��
���   � J� Opacity$	 0.6Q�$��1��.+��� $��20q�.+��� .+��� $��From:��
�������$��30(��� -
   $��tblOriginTank��$��-��
�������$��45(��� .+��� $	��To:��
�������$��30(��� -   $��tblTargetTank��$��-��
�������$��45(��� $
��0,20q�.+��� $��Status:��
�������$��30(��� -   $��tblPumpStatus��$��Pump stopped��
�������$��45(��� $
��0,40q�.+��� -   $��	tblLevel1��$��Tank1: -��
�������$��30(��� -   $��	tblLevel2��$��Tank2: -��
�������$��30(��� -   $��	tblLevel3��$��Tank3: -��
�������$��30(�� $��2��.+��� -    ��Name$ btnShowTanks��   RoundCorner#���   $��
Show Tanks��$
��Left=�$��Bottom=�$	��145��$��65��
����� �
�������$��21(�$��
20,0,0,100q��� -   $ btnShowCamera��#���   $��Show Camera��$
��Left=�$��Bottom=�$	��145��$��65��
����� �
�������$��21(�$��	20,0,0,20q��� -   $ btnHelp��#���   $��?��$��Right=�$��Bottom=�$��35��$��34��
����� �
�������$��20(�$��	0,0,10,10q�$��1��