global using System.Collections.ObjectModel;
global using Microsoft.UI.Composition;
global using System;
using BingeBox_WPF.Classes;
using BingeBox_WPF.Controls;
using BingeBoxLib_WPF.Managers;
using BingeBoxLib_WPF.Models;
using LibVLCSharp.WPF;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Windows.Media;
using Input = System.Windows.Input;
using WinCon = System.Windows.Controls;
using BingeBox_WPF.Windows;
using System.Runtime.InteropServices;



namespace BingeBox_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        //TODO: !!!refactor and reorganize!!!
        //TODO: Add Delete page/function
        public event PropertyChangedEventHandler? PropertyChanged;

        private IntPtr hwnd;

        private DpiScale dpiScale;
        public NativeMethods.RECT currentMonitor;

        public FileManager _fileManager;
        public PlaylistManager _playlistManager;
        
        private ObservableCollection<Episode> playlist;
        public ObservableCollection<Episode> Playlist
        {
            get { return playlist; }
            set
            {
                playlist = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Playlist)));
            }
        }

        public FloatingWindow FSControls;

        public MainWindow()
        {
            _fileManager = new FileManager();
            _playlistManager = new PlaylistManager(_fileManager);

            Playlist = _playlistManager.Playlist;

            _playlistManager.PropertyChanged += (s, e) =>
            {
                Playlist = _playlistManager.Playlist;
            };

            this.Closing += MainWindow_Closing;
            this.LocationChanged += (s, e) =>
            {
                dpiScale = VisualTreeHelper.GetDpi(this);
                currentMonitor = MonitorHelper.GetMonitorDetails(hwnd);
            };
            this.SizeChanged += (s, e) =>
            {
                //VideoControl.InvalidateArrange();
            };

            InitializeComponent();

            hwnd = new WindowInteropHelper(this).Handle;
            currentMonitor = MonitorHelper.GetMonitorDetails(hwnd);

            fadeTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            fadeTimer.Tick += FadeTimer_Tick;

            DataContext = this;
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            //ShowTaskbar();
        }

        public void FadeTimer_Tick(object sender, EventArgs e)
        {
            ControlPanel.Visibility = Visibility.Collapsed;

            fadeTimer.Stop();
        }

        private void _playlistControl_Loaded(object sender, RoutedEventArgs e)
        {
            _playlistControl.DataContext = this;

            _playlistControl._playlistManager = _playlistManager;
            _playlistControl._fileManager = _fileManager;

            _playlistControl.ParentWindow = this;

            _playlistManager.PopulatePlaylist();
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            _toolBar._fileManager = _fileManager;
        }

        private void VideoControl_Loaded(object sender, RoutedEventArgs e)
        {
            //VideoControl.playlistManager = _playlistManager;
            //VideoControl.FileManager = _fileManager;
        }
        DispatcherTimer fadeTimer = new DispatcherTimer();
        public void VideoPlayer_MouseMove(object sender, Input.MouseEventArgs e)
        {
            ControlPanel.Visibility = Visibility.Visible;
            fadeTimer.Start();
        }

        //private void ReplacePlayerEvents(VideoView player)
        //{
        //    player.MouseMove += VideoControl.VideoPlayer_MouseMove;
        //    player.MediaPlayer.MediaChanged += VideoControl.VideoPlayer_MediaChanged;
        //}
        //private void ReplaceButtonEvents(StackPanel panel)
        //{
        //    var buttonEvents = new Dictionary<string, RoutedEventHandler>
        //    {
        //        //{"PlayBtn", VideoControl.PlayBtn_Click },
        //        //{"NextBtn", VideoControl.NextBtn_Click },
        //        //{"PrevBtn", VideoControl.PrevBtn_Click },
        //        //{"FullScreenBtn", VideoControl.FullScreenBtn_Click }
        //    };

        //    foreach(var child in panel.Children)
        //    {
        //        if(child is WinCon.Button btn && buttonEvents.TryGetValue(btn.Name, out var handler))
        //        {
        //            btn.Click -= handler;
        //            btn.Click += handler;
        //        }
        //    }
        //}

        //private void BringToTop()
        //{
        //    if (FSControls == null)
        //        return;

        //    if (FSControls.IsVisible)
        //    {
        //        var FSControlHwnd = new WindowInteropHelper(FSControls).Handle;
        //        Debug.WriteLine($"FSControlHwnd: {FSControlHwnd}");
        //        NativeMethods.SetWindowPos(
        //            FSControlHwnd,
        //            hwnd,
        //            0, 0, 0, 0,
        //            NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOACTIVE);
        //    }
        //}

        //public PlaybackControls FSWindow;
        //private void CreateFSWindow(bool isFullscreen)
        //{
        //    if (FSControls == null)
        //        return;

        //    if (!isFullscreen)
        //    {
        //        FSControls = new FloatingWindow(VideoControl);
        //        FSControls.Left = this.Left;
        //        FSControls.Top = this.Top;
        //        FSControls.Width = this.Width;
        //        FSControls.Height = this.Height;
        //        BringToTop();
        //        FSControls.Show();
        //    }
        //    else
        //    {
        //        FSControls.Hide();
        //        FSControls = null;
        //    }
        //}

        //[DllImport("user32.dll", SetLastError = true)]
        //private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);
        //[DllImport("user32.dll", SetLastError = true)]
        //private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //private const int SW_HIDE = 0;
        //private const int SW_SHOW = 5;
        //private void HideTaskbar()
        //{
        //    IntPtr taskbarHwnd = FindWindow("Shell_TrayWnd", null);
        //    ShowWindow(taskbarHwnd, SW_HIDE);
        //}
        //private void ShowTaskbar()
        //{
        //    IntPtr taskbarHwnd = FindWindow("Shell_TrayWnd", null);
        //    ShowWindow(taskbarHwnd, SW_SHOW);
        //}

        //public void MakeFullscreen(StackPanel controlPanel, bool isFullscreen, VLCControl control)
        //{
        //    currentMonitor = MonitorHelper.GetMonitorDetails(hwnd);
        //    float screenWidth = currentMonitor.Width;

        //    Debug.WriteLine($"screen width: {screenWidth}");
        //    Debug.WriteLine($"screen width: {currentMonitor.Width}");
        //    Debug.WriteLine($"screen Height: {currentMonitor.Height}");

        //    if(this.Left >= screenWidth)
        //    {
        //        this.Left = screenWidth;
        //        this.Top = currentMonitor.Top;
        //        this.Width = currentMonitor.Right;
        //        this.Height = currentMonitor.Bottom;
        //        //this.Topmost = true;

                
        //        HideTaskbar();
        //        this.UpdateLayout();
        //        //CreateFSWindow(isFullscreen);
        //    }
        //    else
        //    {
        //        this.Left = currentMonitor.Left;
        //        this.Top = currentMonitor.Top;
        //        this.Width = currentMonitor.Width / dpiScale.DpiScaleX;
        //        this.Height = currentMonitor.Height / dpiScale.DpiScaleY;
        //        HideTaskbar();
        //    }
        //    Grid.SetRowSpan(control.VideoPlayer, 2);
            
        //    Debug.WriteLine($"window Left: {this.Left}");
        //    Debug.WriteLine($"window Top: {this.Top}");
        //    Debug.WriteLine($"window width: {this.Width}");
        //    Debug.WriteLine($"window height: {this.Height}");
        //    //CreateFSWindow(currentMonitor);
        //    //VideoControl.ControlPanel.Visibility = Visibility.Collapsed;
        //    VideoControlWrapper.Children.Remove(VideoControl);

        //    control.PlayerGrid.UpdateLayout();
            

        //    //WinCon.Panel.SetZIndex(OuterGrid, 3);
        //    OuterGrid.Children.Add(VideoControl);
            
        //    ReplacePlayerEvents(VideoControl.VideoPlayer);

        //    OuterGrid.UpdateLayout();
        //}
        //public void RemoveFullscreen(VLCControl control)
        //{
        //    currentMonitor = MonitorHelper.GetMonitorDetails(hwnd);
        //    //here i need to use the name of the control on the XAML
        //    //once the control is in the OuterGrid VLCControl has no access to it
        //    if (OuterGrid.Children.Count > 0)
        //    {

        //        //this.WindowState = WindowState.Normal;
        //        //this.WindowStyle = WindowStyle.SingleBorderWindow;
        //        VideoControl.ControlPanel.Visibility = Visibility.Visible;
        //        var controlGrid = VideoControl.ControlPanel;
        //        OuterGrid.Background = System.Windows.Media.Brushes.Black;
        //        OuterGrid.Children.Remove(VideoControl);
        //        VideoControlWrapper.Children.Add(VideoControl);
        //    }
        //    this.UpdateLayout();
        //}
    }
}