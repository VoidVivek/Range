   M S B A M L   `   `   `  ����M  IAveva.CounterRange, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Aveva.CounterRange.ViewsAveva.CounterRange.Views  5      1    +Aveva.CounterRange.Views.RangeConfigControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:Aveva.CounterRange.Views     5      .��5	      �   � RowDefinitions  5
   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   .+�5      �� .��6   Name6   �� 5      $	��120��6   $��2 0 0 0q�6"   ��63   �� *Name+�� 5      $��10 0 0 0q�6   .��6&   Class6-   �� 5      $	��120��6   $��2 0 0 0q�6"   ��63   �� *Class+�� 5      $��25��6   $��2 0 0 0q�6    .��61   ...66   5      �� 5   
   $��1��6   $ 
Horizontal=�6"   $��5q�6;   .+�5      �� $��Add range rule��6   $��5q�6M   ��6.   �� *
AddCommand+6M   �� 5      $ ��Remove selected range rule��6   $��5q�6\   ��6:   �� *RemoveCommand+6\   �� 5      $��Copy selected range rule��6   $��5q�6X   ��68   �� *CopyCommand+6X   �� 5      $!��Move selected range rule Up��6   $��5q�6]   ��6;   �� *MoveUpCommand+6]   �� 5      $#��Move selected range rule Down��6   $��5q�6a   ��6=   �� *MoveDownCommand+6a   5      �� 5   
   $��2��6   ��6   �� *RangeRuleControlViewModels+��5      �� 5       �� 
TargetType" ��6   .�5       ��  �� Property �@��
IsSelected �� Value 63   �� *
IsSelected+5!      ��5#      �� 5$      .R�5%      /   )Aveva.CounterRange.Views.RangeRuleControl  5&      5(      5)      5*      