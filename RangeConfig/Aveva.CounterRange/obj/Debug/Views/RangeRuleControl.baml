   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      M IAveva.CounterRange, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Aveva.CounterRangeAveva.CounterRange 5      M+clr-namespace:Aveva.CounterRange.ConvertersAveva.CounterRange.Converters 5      C&clr-namespace:Aveva.CounterRange.EnumsAveva.CounterRange.Enums 5
      /   )Aveva.CounterRange.Views.RangeRuleControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9system&clr-namespace:System;assembly=mscorlib     5      2local clr-namespace:Aveva.CounterRange     5      B
converters+clr-namespace:Aveva.CounterRange.Converters     5      8enums&clr-namespace:Aveva.CounterRange.Enums     5
        � 	Resources  5      8  2Aveva.CounterRange.Converters.IndexToBoolConverter   indexToBoolConverter6 0Aveva.CounterRange.Converters.GroupTypeConverter  groupTypeConverter �� MergedDictionaries �� Source�� 5   
    5      �� 5      $6 0/Aveva.CounterRange;component/Styles/Styles.xaml?�6%   %:   &	        &	        5      6.     5      6,   5      .��5      � $��5q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��4q�6;   .+�5      �� .��6   	Min value6   �� 5      $��50��6   ��6!   �� *
MinValue+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� �� ValidatesOnDataErrors . 6�   �� 5      .��6   	Max value6   �� 5       $��50��6   ��6!   �� *
MaxValue+$ TwoWay=�$ PropertyChanged=� . 6�   �� 5!      ��6   �� *
Sequence+64   5#      �� 5$   
   -   5         ��Name$ lvConditions��5$      $��0q�6   $��1��64   ��6A   �� 	 �� Path$	 
Conditions�
 �� View
 5%      �� 5&      .P�5'      �� $�� ��6%    �� Width$
 Auto��6/    �� CellTemplate 5(      �� 5)      .R�5*   "   �� $ 
Horizontal=�6-   .+�5+   &   �� $��+��6-    � ToolTip$ Insert new Condition��5,   -   
��� � �
������ 
������ $
��Bold"�53   -   $��17(�54   -   ��5-   -   ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" >�$	 DataContext.AddCommand���5/   -   �� *.+54   -   �� 55   &   $��X��6-   $ Remove this Condition��56   -   
���  ��
������ 
������ $
��Bold"�5=   -   $��2 0 0 0q�5>   -   ��57   -   ��  �� *FindAncestor+" >�$	 DataContext.RemoveCommand���59   -   �� *.+5>   -   5?   #   5@      5B      �� 5C      $�� ��6%   $
 Auto��6/    �� HeaderTemplate 5D      �� 5E      .R�5F   "   �� $	��[ ]��6)   $ Group selected Conditions��5G   )   $��Center=�5H   )   $��30��5I   )   
���   �
������ 
������ $
��Bold"�5P   )   ��5J   )   ��  �� *FindAncestor+" >�$(	 "DataContext.GroupConditionsCommand���5L   )   �� *.+5P   )   5R       5T      �� 5U      .R�5V   "   �� $ 
Horizontal=�6-   .+�5W   &   ��  d� 	IsChecked 6/   �� *
IsSelected+�� 5X   &   $��Right=�6,   $��16��6H   $��0q�6T   $��Center=�5Z   ,   $��16��6G   $
��Fill=�6R   ��5Y   ,   �� *StatusInGroup+ �� 	Converter# [ 5Z   R   5[   #   5\      5^      �� 5_      $��And/Or��6%   $
 Auto��65    5`      �� 5a      .R�5b   "   �� $��50��6+    �� SelectedValuePath$	 Tag��6V    �� SelectedValue 66   �� *AndOr+.@�5c   &   �� $	��And��63   
 � Tag 5d   *   0  *Aveva.CounterRange.Enums.ConditionComposer 5e   .   And=�6K   5g   '   �� 5h   &   $��Or��63    5i   *    5j   .   Or=�6J   5l   '   {�5n   &   `� 5o   *    `� 	Converter# [  67   1�5p   .   �� 5q   2    6:   �� *FindAncestor+$ ListViewL��� 5r   2    6:   �� *FindAncestor+$ ListViewItemL�5t   +   5w   #   5x      5z      �� 5{      $��	Attribute��6%   $
 Auto��68    5|      �� 5}      .R�5~   "   �� $ 
Horizontal=�6-   .+�5   &   �� $	��120��6.   ��6:   �� *	Attribute+$ TwoWay=�$ PropertyChanged=� . �� 5�   &   $��25��6-   $��2 0 0 0q�68   .��6I   ...6N   5�   #   5�      5�      �� 5�      $��
Comparison��6%   $
 Auto��69    5�      �� 5�      .R�5�   "   ��  �� DisplayMemberPath$ DisplayName��5�   +   $ Value��5�   +   ��5�   +   *  $Aveva.CounterRange.EnumToItemsSource  *M� *enums:ComparisonOperator++ 5�   +   �� $	 ComparisonOperator�5�      5�      �� 5�      $��Value��6%   $
 Auto��64    5�      �� 5�      .R�5�   "   �� $	��150��6*   ��66   �� *Value+$ TwoWay=�$ PropertyChanged=� . 5�      5�      5�      5�      5�      5�      