   M S B A M L   `   `   `  ����M  IAveva.CounterRange, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Aveva.CounterRange.ViewsAveva.CounterRange.Views  5      2    ,Aveva.CounterRange.Views.RangeConfigsControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:Aveva.CounterRange.Views     5      .��5	      �   � RowDefinitions  5
   
   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   6.   �� 5      $��* �6   6+   .+�5   
   ]� $��0��6   ��. .+�5      ��  �� Orientation$ 
Horizontal=�6   $��Bottom=�62   $��10q�6J   .+�5      �� $��2 0 0 0q�6   ��6*   �� *
AddCommand+.��6I   Add6N   �� 5      $��2 0 0 0q�6   ��6*   �� *RemoveCommand+.��6L   Remove6T   �� 5      $��2 0 0 0q�6   ��6*   �� *CopyCommand+.��6J   Copy6P   5      �� 5      -   5        $��	lvConfigs��5      $ �� IsSynchronizedWithCurrentItem$
 TrueY�6]   $��2q�6�   ��6(   �� *RangeConfigControlViewModels+��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property �@��
IsSelected �� Value 67   �� *
IsSelected+5      ��5      �� 5      .R�5      �� $��2q�6$   ��60   �� *Name+5       5"      5#      1   +Aveva.CounterRange.Views.RangeConfigControl  5$   
   $��1��6#    � DataContext 63   ��  �� ElementName$ 	lvConfigs��	 �� Path$	 SelectedItem�5%      5&      5'      