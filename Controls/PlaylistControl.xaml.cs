using BingeBoxLib_WPF.Managers;
using BingeBoxLib_WPF.Models;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinControls = System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BingeBox_WPF.Controls
{
    /// <summary>
    /// Interaction logic for PlaylistControl.xaml
    /// </summary>
    public partial class PlaylistControl : WinControls.UserControl
    {
        //TODO: should be able to select an episode from the playlist view

        public PlaylistManager _playlistManager;
        public FileManager _fileManager;
        public MainWindow ParentWindow;
        public ObservableCollection<Episode> Playlist;

        public PlaylistControl()
        {
            InitializeComponent();
            if(ParentWindow != null)
            {
                this.DataContext = ParentWindow;
            }
            else
            {
                Debug.WriteLine("Parent Window is null in PlaylistControl");
            }
        }

        private void ShuffleBtn_Click(object sender, RoutedEventArgs e)
        {
            _playlistManager.PopulatePlaylist();
        }

        private void PlaylistLst_Loaded(object sender, RoutedEventArgs e)
        {
            _playlistManager.PopulatePlaylist();
        }
    }
}
