   M S B A M L   `   `   `  ���� E  AJSCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:JSCompiler
JSCompiler       JSCompiler.EmployeesWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:JSCompiler       � Title$  EmployeesWindow��$	��450��$	��800��.��� .+��� -   $
��Exit��$��
Выйти��$
��Left=�$	��Top=�$��75��$��29���� -   $
��Back��$��
Назад��$
��Left=�$	��Top=�$��75��$��29��$��75,0,0,0q��� -   $	��Add��$8��2Добавить нового сотрудника��$��20��$��520,115,102,278q��� -   $��Update��$��Обновить��$��20��$��520,147,102,246q��� $��Имя��$��10,215,679,180q� �� TextAlignment$ Center=�$��15(��� $��Фамилия��$��120,215,494,180q�$ Center=�$��15(��� $��Отчество��$��250,215,288,180q�$ Center=�$��15(��� $��Должность��$��461,215,131,180q�$ Center=�$��15(��� $ ��Редактировать��$��613,215,10,180q�$ Center=�$��15(��� -    ��Name$ listview��$��Stretch=�$��10,245,9.6,10q�$��Stretch=� � 	Resources  �� 
TargetType �� Triggers P� Property P� Value �� Property	 �� Value��%l   '��       �� " �� P�  �@J�IsKeyboardFocusWithin . . ���  �@��
IsSelected	 . .�-   ���� .R�� 
 � ColumnDefinitions
 �� $	��150 ��� $	��150 ��� $	��150 ��� $	��150 ��� $��80 �.+��� $��0��$��12,6,0,0q�����  �� Path$
 Name��� $��1��$��12,6,0,0q����� $ Surname��� $��2��$��12,6,0,0q����� $ 
Patronymic��� $��3��$��12,6,0,0q����� $ Post_ID��� $ ��Редактировать��-   $��4��$
��Edit��$��80��$��40���� $2��,Сортировка и фильтрация��$��40,88,458,307q�$��15(�$ Center=��� $#��Фильтр по ролям:��$ Center=�$��
10,119,0,0q�$��15(�$
��Left=�$	��144��$��27��$	��Top=��� $!��Отсортировать:��$ Center=�$��
25,146,0,0q�$��15(�$
��Left=�$	��129��$��27��$	��Top=��� $��Поиск:��$ Center=�$��
85,178,0,0q�$��15(�$
��Left=�$��69��$��27��$	��Top=� J� RenderTransformOrigin$ 
0.465,0.84&��� -    ��Name$ RoleCB��$
��Left=�$��154,115,0,0q�$	��Top=�$	��146��$��27�� �� SelectedIndex$ 0��.@��� $��Все роли���� $��Директор���� $��Программист���� -	   $ SortCB��$
��Left=�$��154,147,0,0q�$	��Top=�$	��146��$��27��$ 0��.@��� $��-���� $��Имя���� $��Фамилия���� $��Отчество���� $��Должность���� -
   $��Poisk��$
��Left=�$��27��$��154,179,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��146��$
��Left=�$��Center=�.��    	HelpBrush j� TileMode �� Opacity j� Stretch j� 
AlignmentX j� 
AlignmentY !� Visual��%n   &	        !� $
 None=�$	 0.4Q�$
 None=�$
 Left=�$ Center=� �� $��Normal$�$��
Поиск��.������ " �� P�  � �� Z� . ���  � ��#	 [  P�  � ��$  ��. ���  � ��#	 [  .��