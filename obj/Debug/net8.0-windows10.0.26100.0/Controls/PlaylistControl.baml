   M S B A M L   `   `   `  ����G  CBingeBox_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:BingeBox_WPF.ControlsBingeBox_WPF.Controls  5      +    %BingeBox_WPF.Controls.PlaylistControl   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:BingeBox_WPF.Controls     5      .��5	      �   � RowDefinitions  5
   
   �� 5      �� 5      $
��.08* �6   .+�5   
   �� -   5         ��Name$ PlaylistLst��5      $��0��6)   $
��True��6W   ��66   �� *
Playlist+ �� View 5      �� 5      .P�5      ��  �� Width$
 Auto��6%   $��Series��62    �� DisplayMemberBinding 6B   �� *ParentSeries+5      �� 5      $
 Auto��6%   $��Title��62    6A   �� *EpisodeTitle+5      5      5      �� 5   
   -   5         ��Name$ 
ShuffleBtn��5      $��1��6%   $��Shuffle Playlist��62   6M   5      5      