   M S B A M L   `   `   `  ���� a  ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes  ? ;OS11, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:OS11.Model
OS11.Model    OS11.Views.MosaicView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     I EWpfAnimatedGif, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null,gif"http://wpfanimatedgif.codeplex.com  :d2http://schemas.microsoft.com/expression/blend/2008  Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  iMicrosoft_Windows_ThemesJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero  )
OS11_Modelclr-namespace:OS11.Model  $��0q�$��0q�  J� Opacity$  1Q�$��Stretch=�$��Stretch=�$	��904��$	��606�� � 	Resources    AddBtnTemplate �� 
TargetType �� Property �� Value �� 
TargetType � Cursor'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Transitions%  System.Windows.VisualTransition	   GeneratedDuration	
   To   From   States   System.Windows.VisualState   
Storyboard �� TargetProperty �� 
TargetName8  2System.Windows.Media.Animation.EasingColorKeyFrame �� KeyTime �� Value ��Name �� RadiusX �� RadiusY �� Orientation J� SnapsToDevicePixels �� Triggers P� Property P� Value  SaveBtnTemplate �� StrokeDashArray  AddSampleBtnTemplate  MosaicItBtnTemplate A�Name  SettingsBtnTemplate  ImageListItemTemplate  ListBoxItemStyle1  VerticalScrollBarBackground �� EndPoint �� 
StartPoint � Color � Offset  TornImageItemContainerStyle  �� RelativeSource! �� AncestorType" ��Name# �� Padding$ ]� 
Conditions% �� Property& �� Value 	 ItemsPanelTemplate1' K� IsItemsHost 
 
ListBorder( �� Color  TornImageListTemplate   ScrollViewerControlTemplate1) �� 
TargetName* �Name+ � DataContext  TornImagesSampleData, �� Source- � ColumnDefinitions. � RowDefinitions/ ��Name0 �� CanContentScroll1 �� CanHorizontallyScroll2 �� CanVerticallyScroll3 ��Name4  .System.Windows.Automation.AutomationProperties4   AutomationId5 �� ScrollableHeight6 � Maximum7 � Minimum*8 �� #ComputedVerticalScrollBarVisibility9 �� ViewportHeight: �� ViewportSize  TornImageListVScroll; � LargeChange< � Value= �� Mode> �� ScrollableWidth? �� Orientation,@ �� %ComputedHorizontalScrollBarVisibilityA �� ViewportWidth  ItemTemplate  ScrollBarDisabledBackground  ScrollBarButton+   %Microsoft.Windows.Themes.ScrollChromeB  NameC J� IsMouseOverD   RenderMouseOverE   RenderPressedF   ScrollGlyph  VerticalScrollBarPageButton ! HorizontalScrollBarBackground  ScrollBarThumb ! HorizontalScrollBarPageButtonG ]�NameH ]� DecreaseRepeatButtonI �� PageUpCommandJ ]� IncreaseRepeatButtonK �� PageDownCommandL ]� ThumbM �� TriggersN �� LineLeftCommandO �� PageLeftCommandP �� PageRightCommandQ �� LineRightCommand  RemoveImgBtnR ��NameS R� KeyTime  System.Windows.VisibilityT   VisibleU R� ValueV ��Name  tornImageBtn
W � Tag  HelpBtnStyleX �� CornerRadiusY �� FallbackValue  rotateLeftBtnTemplate  rotateRightBtnTemplate  ReconstructAgainBtnTemp��%^8  &	        &	     &	 [    &	 �    &	 �    &	 �    &	 (    &	 �    &		 �    &	
 �    &	      7 
 &	 �    7  &	 �    &	 �    &	 �    &	 z    &	 )    &	 �    &	 R!    &	 "    7  7  7  7  7  7  7  7  7  7  7  7  &	 �%    &	 �'    &	 \*    &	 �.    &	 �0    &	 2    &	 �3    �� " ��.���  � �� �� " ��.R�� $
 Hand��   CommonStates��   $	 0:0:0.2D�  $	 0D�$
 	MouseOver��  $ 	MouseOver��$	 0D�.   
Normal��  	MouseOver��. �� .���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FFC61611���� $, &(Shape.Stroke).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF910E0A��  Pressed��  Disabled��.+��� $ 	rectangle�����	#FFD14836$��0q����	#FFD4211B$��30��$ 2��$ 2���� $
��15,0q�$��Center=�$��Center=�$ 
Horizontal=�.+��� $	����$��Center=�$��Center=�$��Center=�$��0q�
�������$(��"/OS11;component/Fonts/#FontAwesome)�$
��Bold"�$��Center=�$��0,0,5,0q��� $	��ADD��$��Center=�$��Center=�$��Center=�$��Center=�$��0q�
�������$��Lucida Sans)�$
��Bold"��� #��z����. # z #��z��$�� ��$��0�� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� " ��.���  � �� �� " ��.R�� $
 Hand��   CommonStates��   $	 0:0:0.2D�  $	 0D�$
 	MouseOver��  $ 	MouseOver��$	 0D�.   
Normal��  	MouseOver��. �� .���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF3276B1���� $, &(Shape.Stroke).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF285E8E��  Pressed��  Disabled��.+��� $ 	rectangle�����	#FF428BCA$��0q����	#FF357EBD$��30��$ 2��$ 2��$ 0R��� $
��15,0q�$��Center=�$��Center=�$ 
Horizontal=�.+��� $	����$��Center=�$��Center=�$��Center=�$��0q�
�������$(��"/OS11;component/Fonts/#FontAwesome)�$��Normal"�$��Center=�$��0,0,6,0q�$��14.667(�$��Disabled=��� $
��SAVE��$��Center=�$��Center=�$��Center=�$��Center=�$��0q�
�������$��Lucida Sans)�$
��Bold"��� #��z����. # z #��z��$�� ��$��0�� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� " ��.���  � �� �� " ��.R�� $
 Hand��   CommonStates��   $	 0:0:0.2D�  $	 0D�$
 	MouseOver��  $ 	MouseOver��$	 0D�.   
Normal��  	MouseOver��. �� .���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF3276B1���� $, &(Shape.Stroke).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF285E8E��  Pressed��  Disabled��.+��� $ 	rectangle�����	#FF428BCA$��0q����	#FF357EBD$��30��$ 2��$ 2��$ 0R��� $
��15,0q�$��Center=�$��Center=�$ 
Horizontal=�.+��� $	����$��Center=�$��Center=�$��Center=�$��0q�
�������$(��"/OS11;component/Fonts/#FontAwesome)�$��Normal"�$��Center=�$��0,0,6,0q�$��14.667(�$��Disabled=��� $��ADD TO SAMPLES��$��Center=�$��Center=�$��Center=�$��Center=�$��0q�
�������$��Lucida Sans)�$
��Bold"��� #��z����. # z #��z��$�� ��$��0�� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� " ��.���  � �� �� " ��.R�� $
 Hand��   CommonStates��   $	 0:0:0.2D�  $	 0D�$
 	MouseOver��  $ 	MouseOver��$	 0D�.   
Normal��  	MouseOver��. �� .���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF3276B1���� $, &(Shape.Stroke).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF285E8E���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ ellipse��.g�  $ 0��$ 	#FF3276B1��  Pressed��  Disabled��.+��� $ 	rectangle�����	#FF428BCA$��0q�
�������$��30��$ 2��$ 2��$ 0R��� $
��15,0q�$��Center=�$��Center=�$ 
Horizontal=�.+��� $��
MOSAIQ IT!��$��Center=�$��Center=�$��Center=�$��Center=�$��0q�
�������$��Lucida Sans)�$
��Bold"�$��0,0,20,0q��� #��z����. # z #��z��$�� ��$��0��A� $ ellipse�����	#FF428BCA$��Right=�$��	0,0,-20,0q����	#FFFBFBFB$��45��$��45��$��1���� $	����$��Right=�$��	0,0,-12,0q�$��28��$(��"/OS11;component/Fonts/#FontAwesome)�
�������$��Center=�$��18.667(� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� " ��.���  � �� �� " ��.R�� $
 Hand��   CommonStates��   $	 0:0:0.2D�  $	 0D�$
 	MouseOver��  $ 	MouseOver��$	 0D�.   
Normal��  	MouseOver��. �� .���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF3276B1���� $, &(Shape.Stroke).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF285E8E��  Pressed��  Disabled��.+��� $ 	rectangle�����	#FF428BCA$��0q����	#FF357EBD$��30��$ 2��$ 2��$ 0R��� $
��15,0q�$��Center=�$��Center=�$ 
Horizontal=�.+��� $	����$��Center=�$��Center=�$��Center=�$��0q�
�������$(��"/OS11;component/Fonts/#FontAwesome)�$��Normal"�$��Center=�$��0,0,6,0q�$��14.667(�$��Disabled=��� $��SETTINGS��$��Center=�$��Center=�$��Center=�$��Center=�$��0q�
�������$��Lucida Sans)�$
��Bold"��� #��z����. # z #��z��$�� ��$��0�� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� .R��� $��ListBoxItem��$��Stretch=�$��0,20,0,0q�#���  $��Stretch=�|�.  �� $	 1,0&�$	 0,0&�.��� $ #E1E1E1��$ 0Q�� $ #EDEDED��$
 0.20Q�� $ #EDEDED��$
 0.80Q�� $ #E3E3E3��$ 1Q��� " ��.���  � ��
 ���� ��  � �� �� *HorizontalContentAlignment+  �� "! ����  � �� �� *VerticalContentAlignment+  �� "! ����  � ��$ 2,0,0,0q���  � �� �� " ��.R��� $" Bd��#��z��#��z��#��z��## z�� .    SelectionStates��.   
Unselected��  Selected��  SelectedUnfocused��._��� #��z��# z #��z�� P�  �@��
IsSelected . ]� $ �� % �@��
IsSelected& . �� % �@��IsSelectionActive& .  P�  � {� .  �� .R�$� ' . $��Center=��� $( #828790���� " ��.���  � ��# � ����  � ��8   ��  � ��$ 1q���  � ��# � ����  � ��$
 Auto=���  � ��$
 Auto=���  � �� . ��  �@��PanningMode$
 Both=���  �@��IsFlicksEnabled .  ��  � ��$ Center=���  � �� �� " ��.R��� $" Bd��#��z��#��z��#��z��$# 1q� . ._��� |�.  #��z��#���  .���� # z  P�  � {� .  . ��� $) Bd��# � �� � ��P�  �@��
IsGrouping . . ���  � �� .  �� " ��.R�� $
* Grid��#��z��+ �� 8,   - �� $��* ��� $
��Auto �. �� $��* ��� $
��Auto �.+��� $ Corner��$��1��#��� ��$��1���� $!/ PART_ScrollContentPresenter��#0 z��1 .  2 .  #��z��#��z��$��0��#��z��$��0���� $3 PART_VerticalScrollBar��$4 VerticalScrollBar��$ Arrow��$��1��#6 z5 $7 0Q�$��0��#y�z8 #: z9 #���  $��12��$	; 0.2Q�< �� *VerticalOffset+$= OneWay=�  �� *TemplatedParent+�� $3 PART_HorizontalScrollBar��$4 HorizontalScrollBar��$ Arrow��$��0��#6 z> $7 0Q�$? 
Horizontal=�$��1��#y�z@ #: zA < �� *HorizontalOffset+$= OneWay=�  �� *TemplatedParent+�� .R��� .+��� ���� *	Property1+�� $( #F4F4F4���� " ��.���  �@�OverridesDefaultStyle . ��  � |� .  ��  � �� .  ��  � �� �� " ��.R�  $B Chrome��#D zC #E z�� . #F zF �� " ��.���  �@�OverridesDefaultStyle . ��  � ��
 ���� ��  � |� .  ��  � �� .  ��  � �� �� " ��.R��� #��z��#��z��#��z���� $	 0,1&�$	 0,0&�.��� $ #E1E1E1��$ 0Q�� $ #EDEDED��$
 0.20Q�� $ #EDEDED��$
 0.80Q�� $ #E3E3E3��$ 1Q��� " n�.���  �@�OverridesDefaultStyle . ��  � �� .  ��  � �� �� " n�.R��� $ 	rectangle��$
��Auto��$	��2,0q�$��Stretch=����	#FFA5A5A5$
��Auto��$ 2��$ 2��   CommonStates��.   
Normal��  	MouseOver��. �� .���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF818181��  Pressed��  Disabled���� " ��.���  �@�OverridesDefaultStyle . ��  � ��
 ���� ��  � |� .  ��  � �� .  ��  � �� �� " ��.R��� #��z��#��z��#��z���� " ��.���  � ��8   ��  �@��IsPressAndHoldEnabled .  ��  �@��IsFlicksEnabled .  ��  � ��# � ����  � ��# � r���  � ��# � r���  � �� �� " ��.R�� $* Bg��#��z�� . . �� #��� q�$��0* ��� $��* ��� #��� q�$��0* �.+�]� $G 
PART_Track����. #{�zC $��1��H �� #��ZI 8�� J �� #��ZK 8�� L n� 8�� $F VerticalGripper=���Z�  P�  � {� .  . ��� $) Bg��8   � ��M P�  �@��Orientation$ 
Horizontal=�. ���  � ��$
 Auto����  � ��$ 0����  � ��# � ����  � ��# � ����  � ��8  ��  � �� �� " ��.R�� $* Bg��#��z�� . - �� #��� ���� $��0.00001* ��� #��� ��.+��� #��ZN #{�zC 8�� $F 	LeftArrow=�]� $G 
PART_Track��$��1��#{�zC H �� #��ZO 8�� J �� #��ZP 8�� L n� 8��	 $F HorizontalGripper=��� $��2��#��ZQ #{�zC 8��
 $F 
RightArrow=� P�  � {� .  . ��� $) Bg��8   � ���� " ��.���  � �� �� " ��.R�� .+�A� $ ellipse�����	#FFE02727$��Right=�$��25��$��0q����	#FF999999$��Stretch=�$��25��$y�Visible=��� $R label��$	����$��Center=�$
��Auto��$��0q�$��Center=�$(��"/OS11;component/Fonts/#FontAwesome)����	#FFF3F3F3$y�Visible=��� #��z����. # z #��z��$�� �� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� " ��.���  � �� �� " ��.R�� $
 Hand��|�.    CommonStates��   $	 0D�$
 	MouseOver��.   
Normal��  	MouseOver��. �� .��T� $ (UIElement.Visibility)�$ button��..�i� $S 0��#U ZT   Disabled��  SelectionStates��.   
Unselected��  Selected��. �� .��T� $ (UIElement.Visibility)�$ button��..�i� $S 0��#U ZT   SelectedUnfocused��.+��� $V 
tornImgBtn��-   $��Button��#���  W �� *tornImageUri+�� $V button��-   $��Button��$��Right=�$��25��$��0,-10,-10,0q�#���  $	��Top=�$��25��$y�Hidden=�W �� *tornImageUri+�� " ��.���  � �� �� " ��.R�� $
 Hand��   CommonStates��   $	 0:0:0.2D�  $	 0D�$
 	MouseOver��  $ 	MouseOver��$	 0D�.   
Normal��  	MouseOver��. �� .���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF3276B1���� $, &(Shape.Stroke).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF285E8E��  Pressed��  Disabled��.+��� $ 	rectangle�����	#FF428BCA$��0q����	#FF357EBD$��30��$ 2��$ 2��$ 0R��� $
��15,0q�$��Center=�$��Center=�$ 
Horizontal=�.+��� $	����$��Center=�$��Center=�$��Center=�$��0q�
�������$(��"/OS11;component/Fonts/#FontAwesome)�$��Normal"�$��Center=�$��0,0,6,0q�$��14.667(�$��Disabled=��� $
��HELP��$��Center=�$��Center=�$��Center=�$��Center=�$��0q�
�������$��Lucida Sans)�$
��Bold"��� #��z����. # z #��z��$�� ��$��0�� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� " ��.���  � �� �� " ��.R�� .+��� $ 	rectangle��
�������$��0q����	#FF999999$	��140��$	��180��$ 4��$ 4��$	 4 2R��� $��1q�$��0q�$	��140��$	��180��$X 2��$# 5q�$��Center=�$��Center=���Z� ._��� $��0q����� *tornImageUri+$#Y ../image/mosaiq_logo_mini.png���� #��z����. # z #��z��$�� �� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� " ��.���  � �� �� " ��.R�� .+�A� ���	#FF428BCA
�������$��2���� $	����$��0q�$��Center=�$��Center=�$(��"/OS11;component/Fonts/#FontAwesome)�$��24(�
��������� #��z����. # z #��z��$�� �� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� " ��.���  � �� �� " ��.R�� .+�A� ���	#FF428BCA
�������$��2���� #��z����. # z #��z��$�� ���� $	����$��Center=�$��0q�$��Center=�$(��"/OS11;component/Fonts/#FontAwesome)�
�������$��24(� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� " ��.���  � �� �� " ��.R�� $
 Hand��   CommonStates��.   
Normal��  	MouseOver��. �� .���� $* $(Shape.Fill).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF47A447���� $, &(Shape.Stroke).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 	#FF398439��  Pressed��  Disabled��.+��� $ 	rectangle�����	#FF5CB85C$��0q����	#FF4CAE4C$��Center=�$��Center=�$	��175��$��40��$ 4��$ 4���� $��0q�$ 
Horizontal=�$��Center=�$��Center=�.+��� $	����$��Center=�$��Center=�
�������$(��"/OS11;component/Fonts/#FontAwesome)�$��16(��� $��Reconstruct Again!��$��Center=�$��Center=�
�������$��14.667(��� #��z����. # z #��z��$�� �� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  .��� -   $* 
LayoutRoot��.+�]� -   Z ]�Name$Z MainDockPanel��$��0q�
���������.  .+�]� -   $Z TopButtonsDock��$��76����.  $��0q�$	��Top=�
�������$	��Top=�.+��� $��	10,-1,0,0q�$2��,/OS11;component/images/mosaiq_logo_small.png��$
��Fill=�$
��Left=�$	��Top=�$��58��$��70���� -   $V AddBtn��$��Button��$��Center=�$��10,0,0,0q�#���   $��Center=��� -   $V SaveBtn��$��Button��$��Center=�$��10,0,0,0q�#���  $��Center=��� -   $V AddSampleBtn��$��Button��$��Center=�$��4,0,0,0q�#���  $��Center=��� -	   $V MosaicItBtn��$��Button��$��Center=�$��35,0,0,0q�#���  $��Center=��� -
   $V HelpBtn��$��Button��$��Center=�$��4,0,8,0q�#���  $��Center=�$��Right=��� -   $V SettingsBtn��$��Button��$��Center=�$��4,0,8,0q�#���  $��Center=�$��Right=�]� -   $Z BottomDockPanel����.  $��0q�$��Bottom=����	#FFEBEBEB$��40��� -   $* CentralGrid��
�������$��0q����� *ActualWidth+[ �� ElementName$[ MainDockPanel��$= OneWay=�- �� . �� $��0* ��� �� $��0* �.+�]� -   $Z CentralStackPanel��
�������$��0q�$��1��.+��� -   \ ��Name$\ InputImageListBox��$��0q�#���  $
��Left=�$	��250��$��0,0,1,0q�
����������	#FFDADADA#���  #��� 	 #���  ���� *
TornListOC+� -   $* ParametersGrid��$��0q�$��Right=�
�������$	��250��$��Right=�.+��� ���	#FFDADADA$��1,0,0,0q�$��0q�]� .+�� $��30��$	��Top=�$	��250��$	��Top=�.+��� ���	#FFE9E9E9���	#FFE0E0E0$��0q��� $��Tuning Parameters��$
��Left=�$��13.333(�] � UseLayoutRounding] .  $
��Bold"����	#FF585858$��20,0,0,0q��� $	����$��0,3,0,0q�$��14.667(�$(��"/OS11;component/Fonts/#FontAwesome)����	#FF666666$
��Left=��� -   ^ ��Name$^ ParamsPaneStackPaanel��$��0,5,0,0q�$	��230��$	��Top=�.+�� -   $* ArcLengthGrid��$��54��$
��10,0q�$��Center=�.+��� $��Arc Length Multiplier��$
��Left=�$��0q�$��13.333(����	#FF3D3D3D$	��Top=�]� $��25��$��Bottom=�.+��� $��0��$��0q����	#FF1B76B1$��0,0,5,5q��� $��20��$��0q�$��Right=����	#FF1B76B1$��5,0,0,5q��� -   _ ��Name$&_  ArcLengthMultiplierSettingSlider��$6 20Q�` � SmallChange$` 1Q�a �� AutoToolTipPlacement$a TopLeft=�b � ToolTip$"b Change Arc Length Multiplier��$	��Top=�c �� IsSnapToTickEnabledc . $	��150��< �� *appConfig.arcLengthMultiplier+F� ����    �7��|���7�	���	#FFADADAD$��1��$��-12,4,-12,0q�d �� Stretch$
d Fill=����	#FFE0E0E0� -   $* ScaleFactorGrid��$��54��$��
10,10,10,0q�$��Center=�.+��� $��Scale Factor��$
��Left=�$��0q�$��13.333(����	#FF3D3D3D$	��Top=�]� $��25��$��Bottom=�.+��� $	��500��$��0q����	#FF1B76B1$��0,0,5,5q��� $
��1500��$��0q�$��Right=����	#FF1B76B1$��5,0,0,5q��� -   $_ ScaleFactorSettingSlider��$
6 1500Q�$	` 100Q�$a TopLeft=�$b Change Scale Factor��$	��Top=�$	; 100Q�$	7 500Q�e �� TickFrequency$	e 100Q�$	��150��c . < �� *appConfig.scaleFactor+F� ����    �7��|���7�	���	#FFADADAD$��1��$��-12,4,-12,0q�$
d Fill=����	#FFE0E0E0� -   $* ApproxPolyGrid_Copy��$��54��$��
10,10,10,0q�$��Center=�.+��� $��Approx Polygon Parameter��$
��Left=�$��0q�$��13.333(����	#FF3D3D3D$	��Top=�]� $��25��$��Bottom=�.+��� $
��0.03��$��0q����	#FF1B76B1$��0,0,5,5q��� $
��0.10��$��0q�$��Right=����	#FF1B76B1$��5,0,0,5q��� -   $_ ApproxPolygonParamSlider��$	6 0.1Q�$
` 0.01Q�$a TopLeft=�$%b Change Approx Polygon Parameter��$	��Top=�$
; 0.01Q�$
7 0.03Q�$
e 0.01Q�$	��150��c . f �� AutoToolTipPrecision$f 2��< �� *appConfig.approxPolyparameter+F� ����    �7��|���7�	���	#FFADADAD$��1��$��-12,4,-12,0q�$
d Fill=����	#FFE0E0E0� -   $* ApproxPolyGrid_Copy1��$��54��$��
10,10,10,0q�$��Center=�.+��� $��Background Threshold��$
��Left=�$��0q�$��13.333(����	#FF3D3D3D$	��Top=�]� $��25��$��Bottom=�.+��� $��1��$��0q����	#FF1B76B1$��0,0,5,5q��� $��50��$��0q�$��Right=����	#FF1B76B1$��5,0,0,5q��� -   $_ BackgroundThresholdSlider��$6 50Q�$` 1Q�$a TopLeft=�$+b %Change background Threshold Parameter��$	��Top=�$7 1Q�$	��150��c . < �� *appConfig.backgroundThreshold+F� ����    �7��|���7�	���	#FFADADAD$��1��$��-12,4,-12,0q�$
d Fill=����	#FFE0E0E0� -   $* NoiseReGrid��$��25��$��
10,10,10,0q�$��Center=�.+��� $��Noise Reduction��$
��Left=�$��0q�$��13.333(����	#FF3D3D3D$	��Top=��� -   g ��Name$g NoiseReductionCheckBox��$�� ��$��Right=�$��0q�$��0q�$b Noise Reduction?��h d� 	IsCheckedh �� *noiseReduction+F� ����    �7��|���7�	���	#FFADADAD$��1��$��-12,4,-12,0q�$
d Fill=����	#FFE0E0E0� -   $* 
Multi_Edge��$��25��$��
10,10,10,0q�$��Center=�.+��� $��Multiple Edge��$
��Left=�$��0q�$��13.333(����	#FF3D3D3D$	��Top=�$b Multiple Edge���� -   $g MultiEdgeParamCheckBox��$�� ��$��Right=�$��0q�$��0q�$b Multiple Edge Parameter��h �� *	multiEdge+F� ����    �7��|���7�	���	#FFADADAD$��1��$��-12,4,-12,0q�$
d Fill=����	#FFE0E0E0�� -   $��Button��$��60��$��0q�#���  $��Bottom=�$	��250��$��Bottom=�$y�Hidden=�� -   $* CenterRightGrid��$��0q����	#FFFDFDFD.+�� -    $* NextStepGrid��$��0q�{�.  $��Center=�$��Center=�.+��� ���	#FFABABAB$��1q�$��0q�$��Center=�$��Center=�$X 4��$# 10q�$
  0.62Q�._��� $��0q�$-��'/OS11;component/images/broken_image.png��$
��Fill=�$��Center=�$	��240��$	��184��$(b "Add Images to start reconstruction��$��Center=��� -!   $R 	InfoLabel��$(��"Add Images to start reconstruction��$��36��$��	0,250,0,0q�$��Center=�$��Center=����	#FF3F3F3F$��16(�� -"   $* LoadingGrid��$��Center=�$��0q�$��Center=�$y�Hidden=�.+��� -#   i ��Name$i loading_gif��"	  WpfAnimatedGif.ImageBehaviorj 	  AnimatedSource$"j ../images/mosaiq_loading.gif��$��Center=�$��Center=�$��74��$��0q��� -$   $R dynamicInfoLabel��$,��&Re-Constructing Image. Please wait ...��$��	0,110,0,0q�$��Center=�$��Center=�$��16(����	#FF505050� -%   $* 
OutputGrid��$��0q�}�. .+��� $��20q�$# 20q�._��� -&   $i OutputImage��k J� RenderTransformOrigin$k 0.5,0.5&�� $��52��$��0,0,0,4q�$��Bottom=�$��Center=�.+��� ���	#FF428BCA$��0,22,0,0q�
�������$��Center=�$	��160���� -'   $V rotateLeftBtn��$��Button��$��Center=�$��
-165,0,0,0q�#���  $��52��
��������� -(   $V rotateRightBtn��$��Button��$��Center=�$��
0,0,-165,0q�#���  $��52���� -)   $
_ zoom��$��0,23,0,0q�$	��100��$��Center=�$��Center=�$6 5Q�$7 1Q�$< 1Q�{�. 