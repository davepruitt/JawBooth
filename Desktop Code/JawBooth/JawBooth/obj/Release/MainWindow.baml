   M S B A M L   `   `   `  ���� C  ?JawBooth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:JawBoothJawBooth      JawBooth.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:JawBooth     -     � Title$  
MainWindow��$	��550��$	��625��.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+���  �� Orientation$ 
Horizontal=�$��Center=�$��0��.+��� $ Vertical=�$��5q�.+��� $��Feeder��$��Center=��� $	��100��$	��100������  �� Path$ FeederColor� �� FallbackValue$	 Red���� $��Center=����� $ FeederCount�$ 0���� $ Vertical=�$��5q�.+��� $��Nosepoke��$��Center=��� $	��100��$	��100������ $ NosepokeColor�$	 Red���� $��Center=����� $ NosepokeCount�$ 0���� $ Vertical=�$��5q�.+��� $��
Stimulator��$��Center=��� $	��100��$	��100������ $ StimulatorColor�$	 Red���� $��Center=����� $ StimulatorCount�$ 0���� $��1��$ 
Horizontal=�$��Center=�.+��� $��Animal Name:��$��24(�$
��Bold"�$��5,5,5,5q��� $��24(�$
��Bold"�$	��300��$��5,5,5,5q����� $ RatName� �� Mode$ TwoWay=��� $��2��.+��� $��30��$	��500��$��18(�$��10q�$��Center=�$��Center=� �� SelectedIndex �� $ StimulationSelectedIndex�$ 0��@��� $��Stimulation OFF���� $��Stimulation ON���� $ 
Horizontal=�$��Center=�$��3��.+��� $��Booth Number:��$��24(�$
��Bold"�$��5,5,5,5q��� $��24(�
��� � �$
��Bold"�$	��300��$��5,5,5,5q����� $ BoothNumber�$ 1���� $��4��$��Center=�$ Vertical=�.+��� $��30(�$
��Bold"�$	��300��$��75��$��10q����� $ ButtonSessionContent�$ Start������ $ ButtonSessionColor�$ Black������ $ SessionButtonCommand��� $��Center=�
��� � �$
��Bold"����� $ SessionTimerText�$ SessionTimer���� $ 
Horizontal=�$��Center=�$��0,20,0,0q�.+��� $��Manual feed��$��0,0,50,0q�$	��100��$��30������ $ ManualFeedCommand��� $��Manual stim��$��50,0,0,0q�$	��100��$��30������ $ ManualStimCommand�