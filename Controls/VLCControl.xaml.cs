using LibVLCSharp.WPF;
using System;
using System.IO;
using System.Collections.Generic;
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
using LibVLCSharp.Shared;
using UserControl = System.Windows.Controls.UserControl;
using VlcMediaPlayer = LibVLCSharp.Shared.MediaPlayer;
using WinCon = System.Windows.Controls;
using Input = System.Windows.Input;
using Class = BingeBox_WPF.Classes;
using BingeBoxLib_WPF.Managers;
using System.ComponentModel;
using BingeBoxLib_WPF.Models;
using System.Windows.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using BingeBox_WPF.Classes;
using BingeBox_WPF.Windows;

namespace BingeBox_WPF.Controls
{
    /// <summary>
    /// Interaction logic for VLCControl.xaml
    /// </summary>
    /// 
    //TODO: the TrackBar thumb stays at the end of the track
    //TODO: no volume control(is initialized to 100)
    public partial class VLCControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        

        static MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
        public IntPtr hwnd = new WindowInteropHelper(mainWindow).Handle;
        public IntPtr CurrentScreen;
        private static Class.NativeMethods.RECT bounds;

        readonly PlaybackControls _controls;

        public VlcMediaPlayer mediaPlayer;
        public LibVLC libVlc;
        public Media media;

        private DispatcherTimer fadeTimer;

        private int currentIndex = 0;
        public MainWindow parentWindow;
        public PlaylistManager playlistManager;

        private PlaybackControls controlPanel;
        public PlaybackControls ControlPanel
        {
            get { return controlPanel; }
            set 
            {
                controlPanel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(controlPanel)));
            }
        }

        private FileManager _fileManager;
        public FileManager FileManager
        {
            get { return _fileManager; }
            set
            {
                _fileManager = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileManager)));
            }
        }
        private FloatingWindow floatingWindow;
        public VLCControl()
        {
            InitializeComponent();

            InitializeLibVlc();

            //floatingWindow = new FloatingWindow(VideoPlayer, this);
            //floatingWindow.Height = this.Height;
            //floatingWindow.Width = this.Width;
            //floatingWindow.Background = Brushes.Transparent;
            //floatingWindow.Show();

            ControlPanel = new PlaybackControls(this);
            Grid.SetRow(ControlPanel, 1);
            PlayerGrid.Children.Add(ControlPanel);

            //this.LayoutUpdated += (s, e) =>
            //{
            //    floatingWindow = new FloatingWindow(VideoPlayer, this);
            //};

            //if(Environment.OSVersion.Version.Major >= 6)
            //{
            //    floatingWindow.WindowStyle = WindowStyle.None;
            //    var hwnd = new WindowInteropHelper(this.floatingWindow).Handle;
            //    var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            //}
        }

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x20;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int index, int newStyle);

        private void InitializeLibVlc()
        {
            var vlcLibDir = System.IO.Path.Combine(FileManager.projectRoot, "Debug\\net8.0 - windows10.0.26100.0");
            Debug.WriteLine($"vlcLibDir: {vlcLibDir}");
            Core.Initialize();

            libVlc = new LibVLC("--verbose=2");
            mediaPlayer = new VlcMediaPlayer(libVlc);

            VideoPlayer.MediaPlayer = mediaPlayer;
            VideoPlayer.MediaPlayer.Volume = 100;
            VideoPlayer.MediaPlayer.MediaChanged += VideoPlayer_MediaChanged;
            

            fadeTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            fadeTimer.Tick += (s, e) =>
            {
                if (ControlPanel.isFullScreen)
                {
                    return;
                }
                
                ControlPanel.Visibility = Visibility.Collapsed;
                fadeTimer.Stop();
                
            };

            VideoPlayer.MediaPlayer.EncounteredError += (sender, eventLog) =>
            {
                Debug.WriteLine($"Encountred error in MediaPlayer: {eventLog}");
            };
            //VideoPlayer.MediaPlayer.TimeChanged += VideoPlayer_TimeChanged;
        }

        //internal static class NativeMethods
        //{
        //    public const Int32 MONITOR_DEFAULTTOPRIMARY = 0x00000001;
        //    public const Int32 MONITOR_DEFAULTTONEAREST = 0x00000002;

        //    [DllImport("user32.dll")]
        //    public static extern IntPtr MonitorFromWindow(IntPtr handle, Int32 flags);
        //}

        //private void VideoPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        //{
        //    Application.Current.Dispatcher.BeginInvoke(() =>
        //    {
        //        TrackBar.Value = e.Time;
        //    });

        //}



        public void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            playlistManager.MarkAsRerun(VideoPlayer.MediaPlayer, playlistManager.Playlist[currentIndex]);
            
            media = new Media(libVlc, new Uri(playlistManager.Playlist[currentIndex].EpisodePath).AbsoluteUri, FromType.FromLocation);

            VideoPlayer.MediaPlayer.Play();
            //StartTimer();
        }


        //bool isSeeking = false;
        //private void TrackBar_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        //{
        //    isSeeking = true;
        //}

        //private void TrackBar_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        //{
        //    if(VideoPlayer.MediaPlayer != null)
        //    {
        //        VideoPlayer.MediaPlayer.Time = (long)TrackBar.Value;
        //    }
        //    isSeeking = false;
        //}

        //private void TrackBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    if(!isSeeking && VideoPlayer.MediaPlayer != null)
        //    {
        //        VideoPlayer.MediaPlayer.Time = (long)TrackBar.Value;
        //    }
        //}
        //private DispatcherTimer timer;
        //private void StartTimer()
        //{
        //    if (!isSeeking && VideoPlayer.MediaPlayer != null)
        //    {
        //        timer = new DispatcherTimer(); timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
                    
        //        timer.Tick += (s, e) =>
        //        {
        //            TrackBar.Value = (float)VideoPlayer.MediaPlayer.Time;
        //        };
        //        timer.Start();
        //    }
        //}

        public void VideoPlayer_MediaChanged(object? sender, MediaPlayerMediaChangedEventArgs e)
        {
            if (VideoPlayer.MediaPlayer.Media != null)
            {
                //TrackBar.Value = (float)VideoPlayer.MediaPlayer.Time;
                //TrackBar.Maximum = (float)VideoPlayer.MediaPlayer.Length;

                //Debug.WriteLine($"TrackBar Value: {TrackBar.Value}");
                //Debug.WriteLine($"TrackBar Max: {TrackBar.Maximum}");
                //VideoPlayer.Content = ControlPanel;
                VideoPlayer.MediaPlayer.Play();
            }
        }
        
        public void VideoPlayer_MouseMove(object sender, Input.MouseEventArgs e)
        {
            if (!ControlPanel.isFullScreen)
            {
                ControlPanel.Visibility = Visibility.Visible;
                fadeTimer.Start();
            }
        }
    }
}
