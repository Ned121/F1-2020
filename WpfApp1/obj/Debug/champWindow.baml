   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.champWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  
Кубок��5   	   $	��450��6   $��909.091��6$    � Icon$ 
.\icon.ico��64   .��5	      � .+�5   
   �� -   5        $��	champList��5   8   $��10,200,10,10q�6I   $��2��6_    �� View 5      �� 5      .P�5      �� -   5        position�� �� Width$
 auto��5   f    �� DisplayMemberBinding 67   ��  �� Path$ position�.��6s   
Место6z   �� 5      -   5        	pilotTeam��$
 auto��5   i    69   �� $ 	pilotTeam�.��6v   Пилоты/команда6�   �� 5      -   5        win��$
 auto��5   \    62   �� $	 win�.��6i   
Побед6p   �� 5      -   5        poll��$
 auto��5   ^    63   �� $
 poll�.��6k   
Поул6q   �� 5      -   5        laps��$
 auto��5   ^    63   �� $
 laps�.��6k   Лучших кругов6z   �� 5      -   5        
points��$
 auto��5   b    65   �� $ points�.��6o   
Очки6u   �� 5      -   5        	photo��$	 170��5   4   .��6@   
Фото �� CellTemplate 5      �� 5      .R�5   "   ��  ��Name$ photos��6(   $	��170��68   $��70��6D   ��6P   �� $ photo�5      5      �� 5      -	   5        id��$ 0��5   [    62   �� $ id�6[   5      5       �� 5!   
   -
   5        $
��winC��5!   =    �� 	MaxLength$ 2��6/   $��	80,24,0,0q�6I   	 �� TextWrapping$
	 Wrap=�6\   $��23��6p   $	��Top=�6|   $
��Left=�6�   $	��120��6�   �� 5"   
   $��
Побед��6   
 J� RenderTransformOrigin$
 0.517,0.335&�6    $��	14,24,0,0q�6D   $��33��6W   $	��Top=�6c   $
��Left=�6{   $��61��6�   �� 5#   
   -   5        $��
Назад��5#      $
��Left=�6!   $��
487,24,0,0q�6<   $	��Top=�6P   $��75��6h   $��20��6�   �� 5$   
   -   5        $ ��Редактировать��5$      $��
487,63,0,0q�6)   $��20��6N   $	��Top=�6Z   $
��Left=�6r   $��75��6�   �� 5%   
   -   5        $��idCheck��5%      $y�Hidden=�6!   $
��Left=�65   $��23��6P   $��394,108,-150,0q�6\   $
	 Wrap=�6t   $	��Top=�6�   $	��120��6�   $��1��6�   �� 5&   
   -   5        $��poulC��5&      $ 2��6   $��	80,78,0,0q�6I   $
	 Wrap=�6\   $��23��6p   $	��Top=�6|   $
��Left=�6�   $	��120��6�   �� 5'   
   $��Поул��6   $��	17,78,0,0q�6   $��37��62   $	��Top=�6>   $
��Left=�6V   $��63��6q   �� 5(   
   $
��ЛК��6   $
��Left=�6   $��
17,120,0,0q�68   $	��Top=�6L   �� 5)   
   -   5        $��lapsC��5)      $ 2��6   $
��Left=�6I   $��23��6d   $��
80,124,0,0q�6p   $
	 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��120��6�   �� 5*   
   $��Очки��6   $
��Left=�6   $��
223,78,0,0q�6:   $	��Top=�6N   �� 5+   
   -   5        $��pointC��5+      $ 9��6<   $
��Left=�6K   $��23��6f   $��
279,78,0,0q�6r   $
	 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��120��6�   �� 5,   
   -   5        $��nameFrom��5,      $��16(�6   $�� ��6.   $
��Left=�69   $��
239,13,0,0q�6T   $	��Top=�6h   $	��211��6�   $��60��6�   �� 5-   
   -   5        $��image��5-      $
��Left=�6   $	��160��68   $��
596,24,0,0q�6E   $	��Top=�6Y   $	��283��6q   5.      5/      