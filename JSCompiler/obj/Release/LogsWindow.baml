   M S B A M L   `   `   `  ���� E  AJSCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:JSCompiler
JSCompiler      JSCompiler.LogsWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:JSCompiler       � Title$  
LogsWindow��$	��450��$	��900��.��� .+��� -   $
��Exit��$��
Выйти��$
��Left=�$	��Top=�$��75��$��29���� -   $
��Back��$��
Назад��$
��Left=�$	��Top=�$��75��$��29��$��75,0,0,0q��� -   $��Update��$��Обновить��$��80��$	��Top=�$��30��$��762,10,51.6,0q��� $��Путь��$	��Top=�$��20��$	��300�� �� TextAlignment$ Center=�$��15(�$��10,62,583.6,0q��� $��Код��$	��Top=�$��20��$	��300��$ Center=�$��15(�$��291,62,302.6,0q��� $��Результат��$	��Top=�$��20��$	��140��$ Center=�$��15(�$��589,62,164.6,0q��� $%��Время выполнения��$	��Top=�$��20��$	��140��$ Center=�$��15(�$��740,62,13.6,0q��� -    ��Name$ listview��$��Stretch=�$��10,87,10,10q�$��Stretch=� � 	Resources  �� 
TargetType �� Triggers P� Property P� Value �� Property	 �� Value��%l   '��       �� " �� P�  �@J�IsKeyboardFocusWithin . . ���  �@��
IsSelected	 . .�-   ���� .R�� 
 � ColumnDefinitions
 �� $	��300 ��� $	��300 ��� $	��100 ��� $	��130 �.+��� $��0��$��1q�
���   �._��� $
��Wrap=�����  �� Path$
 Path��� $��1��$��1q�
���   �._��� $
��Wrap=����� $
 Code��� $��2��$��1q�
���   �._��� $
��Wrap=����� $ Result��� $��3��$��1q�
���   �._��� $��3��$
��Wrap=����� $ Execution_Time��� $!��Отсортировать:��$ Center=�$��
246,11,0,0q�$��15(�$
��Left=�$	��112��$��27��$	��Top=��� $��Поиск:��$ Center=�$��
520,15,0,0q�$��15(�$
��Left=�$��69��$��20��$	��Top=��� -    ��Name$ SortCB��$
��Left=�$��
360,10,0,0q�$	��Top=�$	��146��$��27�� �� SelectedIndex$ 0��.@��� $��-���� $��Путь���� $��Код���� $��Результат���� $%��Время выполнения���� -   $��Poisk��$
��Left=�$��27��$��
589,10,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��146��$
��Left=�$��Center=�.��    	HelpBrush j� TileMode �� Opacity j� Stretch j� 
AlignmentX j� 
AlignmentY !� Visual��%n   &	        !� $
 None=�$	 0.4Q�$
 None=�$
 Left=�$ Center=� �� $��Normal$�$��
Поиск��.������ " �� P�  � �� Z� . ���  � ��#	 [  P�  � ��$  ��. ���  � ��#	 [  .��