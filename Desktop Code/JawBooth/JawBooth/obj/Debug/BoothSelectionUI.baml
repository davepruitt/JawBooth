   M S B A M L   `   `   `  ����C  ?JawBooth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:JawBoothJawBooth  5   	       JawBooth.BoothSelectionUI   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:JawBooth     5   	     � Title$  Please select a booth��5   	    � SizeToContent$ Height=�5	   	    � 
ResizeMode$ NoResize=�5
   	    � WindowStartupLocation$ CenterOwner=�5   	   $	��350��5   	   .��5      ��  �� Orientation$ Vertical=�6   .+�5   
   �� $��10q�6o   $��0q�5      ��5      ��  �� Path$ AvailablePorts� �� SelectedIndex 6>   �� $ SelectedPortIndex���5      �� 5      .R�5      �� $ Vertical=�6!   .+�5      �� $��24(�6Y   ��6$   �� $ 	BoothName� �� FallbackValue$ Booth��6Y   �� 5      $ 
Horizontal=�6%   .+�5      �� $��Last updated: ��6(   $��10(�6>   �� 5      $��10(�6l   ��6(   �� $ LastUpdatedInformation�$ Unknown��6l   5      �� 5      $ 
Horizontal=�6%   .+�5      �� $��16��5   %   $��16��60   $	��2,0q�6<   y�5   %   �� $ ConnectionBadVisibility�$ 	Collapsed��.+�5   "   �� 	 �� X1$ 4��6'   		 �� X2$	 12��6.   	
 �� Y1$
 4��66   	 �� Y2$ 12��6=   $��2��6E   
���  ��6Y   �� 5   "   $ 4��6'   $	 12��6.   $
 12��66   $ 4��6>   $��2��6E   
���  ��6Y   5      �� 5       $��16��5!   %   $��16��60   $	��2,0q�6<   y�5    %   �� $ ConnectionGoodVisibility�$ Visible��.+�5"   "   �� $ 4��6'   $
 8��6.   $	 6��65   $ 12��6<   $��2��6D   
��� � �6X   �� 5#   "   $ 6��6'   $
 12��6.   $	 12��66   $ 4��6>   $��2��6E   
��� � �6Y   5$      �� 5%      $��10(�6g   ��6(   �� $ DeviceInformation�$ Arduino��6g   5&      5'      5(      5*      �� 5,   
   $��14(�6   $
��Bold"�5-      $��Center=�5/      ��5.      �� $ NoBoothsDetectedText�$
 None��y�50      �� $ NoBoothsTextVisibility�$ 	Collapsed���� 53   
   $ 
Horizontal=�6   $��Center=�6.   $��10q�6K   .+�54      �� $	��120��6   $��40��55      $��5,0,0,5q�56      $��Bottom=�58      ��57      �� $ RefreshCommand���59      �� 5:      .+�5;      F� 
��� � �$��3��6.   ��5<      B� 5=   "   .��5>   &   E�  E� 
StartPoint$
 10,0&�61   .��5?   *   ��  �� 
IsLargeArc .  �� SweepDirection$ 	Clockwise=�6G    �� Size$ 10,10��6b    �� Point$
 9,-5&�6o   5@   '   5A   #   5C      � 5D       � Points' �   ���  ��� 	= �����ا ��v�
��� � �
��� � �6K   5E      5G      �� 5H      -   5        $��Center=�5J      $��Bottom=�5K      $	��120��5L      $��40��5M      $��5,0,0,5q�5N      $��18(�5O      $
��Bold"�5P      ��5H      �� $ SelectBoothButtonContent�$ OK��{�5I      �� $ SelectBoothButtonEnabled�$
 True��5Q      5S      5U      5V      