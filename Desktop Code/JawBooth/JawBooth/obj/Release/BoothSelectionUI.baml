   M S B A M L   `   `   `  ���� C  ?JawBooth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:JawBoothJawBooth      JawBooth.BoothSelectionUI   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:JawBooth       � Title$  Please select a booth�� � SizeToContent$ Height=� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterOwner=�$	��350��.����  �� Orientation$ Vertical=�.+��� $��10q�$��0q�����  �� Path$ AvailablePorts� �� SelectedIndex �� $ SelectedPortIndex����� .R��� $ Vertical=�.+��� $��24(����� $ 	BoothName� �� FallbackValue$ Booth���� $ 
Horizontal=�.+��� $��Last updated: ��$��10(��� $��10(����� $ LastUpdatedInformation�$ Unknown���� $ 
Horizontal=�.+��� $��16��$��16��$	��2,0q�y��� $ ConnectionBadVisibility�$ 	Collapsed��.+��� 	 �� X1$ 4��		 �� X2$	 12��	
 �� Y1$
 4��	 �� Y2$ 12��$��2��
���  ���� $ 4��$	 12��$
 12��$ 4��$��2��
���  ���� $��16��$��16��$	��2,0q�y��� $ ConnectionGoodVisibility�$ Visible��.+��� $ 4��$
 8��$	 6��$ 12��$��2��
��� � ��� $ 6��$
 12��$	 12��$ 4��$��2��
��� � ��� $��10(����� $ DeviceInformation�$ Arduino���� $��14(�$
��Bold"�$��Center=����� $ NoBoothsDetectedText�$
 None��y��� $ NoBoothsTextVisibility�$ 	Collapsed���� $ 
Horizontal=�$��Center=�$��10q�.+��� $	��120��$��40��$��5,0,0,5q�$��Bottom=����� $ RefreshCommand����� .+�F� 
��� � �$��3����B� .��E�  E� 
StartPoint$
 10,0&�.����  �� 
IsLargeArc .  �� SweepDirection$ 	Clockwise=� �� Size$ 10,10�� �� Point$
 9,-5&��  � Points' �   ���  ��� 	= �����ا ��v�
��� � �
��� � ��� -   $��Center=�$��Bottom=�$	��120��$��40��$��5,0,0,5q�$��18(�$
��Bold"����� $ SelectBoothButtonContent�$ OK��{��� $ SelectBoothButtonEnabled�$
 True��