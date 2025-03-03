using BingeBox_WPF.Classes;
using BingeBoxLib_WPF.Managers;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using Class = BingeBox_WPF.Classes;

namespace BingeBox_WPF.Controls
{
    /// <summary>
    /// Interaction logic for PlaybackControls.xaml
    /// </summary>
    public partial class PlaybackControls : UserControl
    {
        static MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;

        public Class.NativeMethods.RECT Bounds { get; set; }

        VLCControl _parent;
        DispatcherTimer fadeTimer;
        private int currentIndex = 0;

        //, Class.NativeMethods.RECT bounds
        public PlaybackControls(VLCControl Parent)
        {
            InitializeComponent();

            TrackBar.Value = 0;
            TrackBar.Maximum = 100;
            _parent = Parent;
            //Bounds = bounds;
        }
        public void PlayVideo(StackPanel controls)
        {
            //_parent.VideoPlayer.Content = _parent.ControlPanel;
            _parent.VideoPlayer.MediaPlayer.Hwnd = _parent.hwnd;
            _parent.VideoPlayer.MediaPlayer.Play();
        }

        public void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            _parent.VideoPlayer.MediaPlayer.Stop();
            currentIndex--;
            _parent.media = new Media(_parent.libVlc, new Uri(_parent.playlistManager.Playlist[currentIndex].EpisodePath).AbsoluteUri, FromType.FromLocation);
            _parent.VideoPlayer.MediaPlayer.Media = _parent.media;
            PlayVideo(ControlPanel);
        }

        public void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_parent.playlistManager == null)
            {
                Debug.WriteLine($"playlistManager is null(PlayBtn_Click): {this.GetHashCode()}");
                return;
            }
            if(_parent.FileManager.FullSeriesList.Count == 0)
            {
                Debug.WriteLine("there are no series in the list");
                return;
            }
            if (_parent.VideoPlayer.MediaPlayer.Media != null)
            {
                if (_parent.VideoPlayer.MediaPlayer != null && _parent.VideoPlayer.MediaPlayer.Media != null)
                {
                    if (_parent.VideoPlayer.MediaPlayer.IsPlaying)
                    {
                        _parent.VideoPlayer.MediaPlayer.Pause();
                    }
                    else
                    {
                        PlayVideo(ControlPanel);
                    }
                }
            }
            else
            {
                //SETS THE MEDIA FOR THE FIRST TIME DURING RUNTIME
                _parent.media = new Media(_parent.libVlc, new Uri(_parent.playlistManager.Playlist[currentIndex].EpisodePath).AbsoluteUri,
                    FromType.FromLocation);
                _parent.VideoPlayer.MediaPlayer.Media = _parent.media;
                _parent.VideoPlayer.MediaPlayer.Play();
                //StartTimer();

            }
        }

        public void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            _parent.VideoPlayer.MediaPlayer.Stop();
            currentIndex++;
            _parent.media = new Media(_parent.libVlc, new Uri(_parent.playlistManager.Playlist[currentIndex].EpisodePath).AbsoluteUri, FromType.FromLocation);
            _parent.VideoPlayer.MediaPlayer.Media = _parent.media;

            PlayVideo(ControlPanel);
        }

        public bool isFullScreen = false;

        private double previousPlayerHeight;
        private double previousPlayerWidth;

        public void FullScreenBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: i made a new grid over the whole application(OuterGrid). fullscreen will 
            //send the player to the top grid, stretch to fit, and make the window fullscreen
            //(crazy how much fanagaling this is taking)

            if (!isFullScreen)
            {
                previousPlayerHeight = _parent.PlayerGrid.Height;
                previousPlayerWidth = _parent.PlayerGrid.Width;



                //mainWindow.MakeFullscreen(ControlPanel, isFullScreen, _parent);
            }
            else
            {
                //mainWindow.RemoveFullscreen(_parent);
                _parent.PlayerGrid.Height = previousPlayerHeight;
                _parent.PlayerGrid.Width = previousPlayerWidth;
                this.Visibility = Visibility.Visible;
            }

            isFullScreen = !isFullScreen;
        }
    }
}
