   M S B A M L   `   `   `  ����C  ?JawBooth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:JawBoothJawBooth  5   	       JawBooth.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:JawBooth     5   	   -   5          � Title$  
MainWindow��5   	   $	��550��6   $	��625��6)   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��Center=�5      $��0��5      .+�5      �� $ Vertical=�6   $��5q�60   .+�5      �� $��Feeder��6   $��Center=�5      �� 5      $	��100��5      $	��100��5      ��5      ��  �� Path$ FeederColor� �� FallbackValue$	 Red��5      �� 5!      $��Center=�5"      ��5!      �� $ FeederCount�$ 0��5"      5$      �� 5&      $ Vertical=�6   $��5q�60   .+�5'      �� $��Nosepoke��6   $��Center=�5(      �� 5*      $	��100��5+      $	��100��5,      ��5*      �� $ NosepokeColor�$	 Red��5,      �� 5.      $��Center=�5/      ��5.      �� $ NosepokeCount�$ 0��5/      51      �� 53      $ Vertical=�6   $��5q�60   .+�54      �� $��
Stimulator��6   $��Center=�55      �� 57      $	��100��58      $	��100��59      ��57      �� $ StimulatorColor�$	 Red��59      �� 5;      $��Center=�5<      ��5;      �� $ StimulatorCount�$ 0��5<      5>      5@      �� 5B   
   $��1��6   $ 
Horizontal=�5C      $��Center=�5D      .+�5E      �� $��Animal Name:��6   $��24(�5F      $
��Bold"�5G      $��5,5,5,5q�5H      �� 5J      $��24(�5K      $
��Bold"�5L      $	��300��5M      $��5,5,5,5q�5N      ��5J      �� $ RatName� �� Mode$ TwoWay=�5N      5P      �� 5R   
   $��2��6   .+�5S      �� $��30��5T      $	��500��6#   $��18(�6/   $��10q�6=   $��Center=�5U      $��Center=�5V       �� SelectedIndex 5S      �� $ StimulationSelectedIndex�$ 0��@�5W      �� 5X      $��Stimulation OFF��6#   �� 5Y      $��Stimulation ON��6#   5[      5\      �� 5^   
   $ 
Horizontal=�6   $��Center=�6.   $��3��6K   .+�5_      �� $��Booth Number:��6   $��24(�5`      $
��Bold"�5a      $��5,5,5,5q�5b      �� 5d      $��24(�5e      
��� � �$
��Bold"�5g      $	��300��5h      $��5,5,5,5q�5i      ��5d      �� $ BoothNumber�$ 1��5i      5k      �� 5m   
   $��4��6   $��Center=�5n      $ Vertical=�5o      .+�5p      �� $��30(�5r      $
��Bold"�5s      $	��300��5t      $��75��5u      $��10q�5v      ��5p      �� $ ButtonSessionContent�$ Start����5q      �� $ ButtonSessionColor�$ Black����5w      �� $ SessionButtonCommand��� 5z      $��Center=�5{      
��� � �$
��Bold"�5}      ��5z      �� $ SessionTimerText�$ SessionTimer��5}      �� 5�      $ 
Horizontal=�6   $��Center=�62   $��0,20,0,0q�6O   .+�5�      �� $��Manual feed��6   $��0,0,50,0q�5�      $	��100��5�      $��30��5�      ��5�      �� $ ManualFeedCommand��� 5�      $��Manual stim��6   $��50,0,0,0q�5�      $	��100��5�      $��30��5�      ��5�      �� $ ManualStimCommand�5�      5�      5�      5�      