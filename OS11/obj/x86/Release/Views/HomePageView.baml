   M S B A M L   `   `   `  ����   OS11    OS11.Views.HomePageView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     
�������$��Stretch=�$��Stretch=�$��0q�$��0q�  J� Opacity$  1Q�$	��904��$	��646�� � 	Resources    HomePageBtn �� 
TargetType �� Property �� Value �� 
TargetType'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Transitions%  System.Windows.VisualTransition   GeneratedDuration		   To
   
Storyboard   States   System.Windows.VisualState   
Storyboard �� TargetProperty �� 
TargetName8  2System.Windows.Media.Animation.EasingColorKeyFrame �� KeyTime �� Value ��Name � Cursor �� RadiusY �� RadiusX J� SnapsToDevicePixels �� Triggers P� Property P� Value  OpenSampleBtn  OpenExistingBtn  HelpBtn��%J  &	        &	 K    &	 �    &	 �	    �� " ��.���  � �� �� " ��.R��    CommonStates��   $ 0:0:0.2D�  $ 0:0:0.2D�$	 	MouseOver��.
 �� .   
Normal��  	MouseOver��. �� .���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FFEBEBEB���� $, &(Shape.Stroke).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FFADADAD��  Pressed��  Disabled��.+��� $ 	rectangle��$
 Hand��$ 4��$ 4�����	#FFCFCFCF
��������� #��z����. # z #��z��$�� ���� $��New Reconstruction��$��0q�$��Center=�$��Center=����	#FF3F3F3F$��Center=�$��Disabled=�$��16(�$��Center=�$��0q�$��Lucida Sans)�$
 Hand�� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� " ��.���  � �� �� " ��.R��    CommonStates��   $ 0:0:0.2D�  $ 0:0:0.2D�$	 	MouseOver��.
 �� .   
Normal��  	MouseOver��. �� .���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FFEBEBEB���� $, &(Shape.Stroke).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FFADADAD��  Pressed��  Disabled��.+��� $ 	rectangle��$
 Hand��$ 4��$ 4�����	#FFCFCFCF
��������� #��z����. # z #��z��$�� ���� $��Open Sample��$��0q�$��Center=�$��Center=����	#FF3F3F3F$��Center=�$��Disabled=�$��16(�$��Center=�$��0q�$��Lucida Sans)�$
 Hand�� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� " ��.���  � �� �� " ��.R��    CommonStates��   $ 0:0:0.2D�  $ 0:0:0.2D�$	 	MouseOver��.
 �� .   
Normal��  	MouseOver��. �� .���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FFEBEBEB���� $, &(Shape.Stroke).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FFADADAD��  Pressed��  Disabled��.+��� $ 	rectangle��$
 Hand��$ 4��$ 4�����	#FFCFCFCF
��������� #��z����. # z #��z��$�� ���� $#��Open Existing Reconstructions��$��0q�$��Center=�$��Center=����	#FF3F3F3F$��Center=�$��Disabled=�$��16(�$��Center=�$��0q�$��Lucida Sans)�$
 Hand�� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� " ��.���  � �� �� " ��.R��    CommonStates��   $ 0:0:0.2D�  $ 0:0:0.2D�$	 	MouseOver��.
 �� .   
Normal��  	MouseOver��. �� .���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FFEBEBEB���� $, &(Shape.Stroke).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FFADADAD��  Pressed��  Disabled��.+��� $ 	rectangle��$
 Hand��$ 4��$ 4�����	#FFCFCFCF
��������� #��z����. # z #��z��$�� ���� $
��Help��$��0q�$��Center=�$��Center=����	#FF3F3F3F$��Center=�$��Disabled=�$��16(�$��Center=�$��0q�$��Lucida Sans)�$
 Hand�� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  .��� -    �Name$ 
LayoutRoot��
�������.+�� $��Center=�$��Center=�.+��� 
�������$��0q�$	��400��$��Bottom=�$��Center=�$	��600��$ 20��$ 20�����	#FF0677B2 �� StrokeDashArray$	 4 4R� . �� $��
0,-230,0,0q�$,��&/OS11;component/images/mosaiq_logo.png��$
��Fill=�$��144.674��$��Center=�$	��501��$��Center=�F� -    F�Name$ div_line������ z  �}
� 4I# �}
	
�������$��1��$��	8,169,7,0q� �� Stretch$
 Fill=����	#FF0677B2$	��Top=�$��Center=�$	��600��$	 4 4R��� $��	8,171,8,0q�.+��� -   $��Button��$��Center=�$��40��$��0,20,0,0q�#���   $��Center=�$	��300���� -   $��Button��$��Center=�$��40��$��0,15,0,0q�#���  $��Center=�$	��300���� -   $��Button��$��Center=�$��40��$��0,15,0,0q�#���  $��Center=�$	��300���� -   $��Button��$��Center=�$��40��$��	0,15,0,20q�#���  $��Center=�$	��300��