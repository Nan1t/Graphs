   M S B A M L   `   `   `  ����A  =Graphs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:GraphsGraphs  5   	       Graphs.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Graphs     5   	   -   5          �Name$
  Root��5	      $	��480��5   	   $	��640��6   $	��480��6"   $	��640��62    � Title$ Graphs��5	   	   .��5
      �  � RowDefinitions 5   
   �� 5      $��32 �6   6*   �� 5      6   .+�5   
   � $��0��6   
�������.+�5      ]� $
��Left=�6   .+�5      �� -   5        $
��Load��5   +   $��24��6:   $��2 0 2 0q�6F   �� 5      -   5        $
��Save��5   +   $��24��6:   $��2 0 2 0q�6F   �� 5      -   5        $	��DFS��5   %   $��24��63   $��2 0 2 0q�6?   �� 5      -   5        $	��BFS��5   %   $��24��63   $��2 0 2 0q�6?   �� 5      -   5        $��Search min path��5   /   $��24��6I   $��2 0 2 0q�6U   �� 5      -   5        $��SpanningTree��5   2   $��24��6I   $��2 0 2 0q�6U   �� 5      -   5        $��
Clear path��5   +   $��24��6@   $��2 0 2 0q�6L   �� 5      -	   5        $��Rand weights��5   -   $��24��6D   $��2 0 2 0q�6P   5      ]� 5      $��Right=�6   .+�5      �� -
   5        $
��Wire��5   ,   $��24��6;   $��2 0 2 0q�6G   �� 5      -   5        $��Unselect��5   /   $��24��6B   $��2 0 2 0q�6N   �� 5      -   5        $��Clear��5   ,   $��24��6<   $��2 0 2 0q�6H   5      5       �� 5"   
   -   5         ��Name$ 
CanvasGrid��5"      $��1��69   
�������.+�5#      �� 
������� K� ZIndex$ -1��6*    �� Bottom$ 5��6<    �� Left$ 5��6N   [RMB] - Create node�� 5%      [LMB] - Drag node�� 5'      [Delete] - Delete node�� 5)      *([L CTRL, L SHIFT] - Multiple node select�� 5+      [W] - Wire two nodes5-      5.      5/      50      