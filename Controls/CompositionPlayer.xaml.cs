using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VlcMediaPlayer = LibVLCSharp.Shared.MediaPlayer;
using Comp = Windows.UI.Composition;
using Windows.UI.Composition.Desktop;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using System.Windows.Interop;
using System.Diagnostics;
using BingeBox_WPF.Classes;

namespace BingeBox_WPF.Controls
{
    /// <summary>
    /// Interaction logic for CompositionPlayer.xaml
    /// </summary>
    public partial class CompositionPlayer : UserControl
    {
        static MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;

        private int currentIndex = 0;        

        public LibVLC libVlc;
        public VlcMediaPlayer mediaPlayer;
        public Media media;

        public Comp.Compositor compositor;
        public Comp.ContainerVisual rootVisual;
        public Comp.SpriteVisual videoVisual;
        public Comp.SpriteVisual controlVisual;

        private Comp.CompositionTarget windowTarget;

        private Dictionary<string, Rect> buttonBounds = new Dictionary<string, Rect>()
        {
            ["playBtn"] = new Rect(100, 200, 80, 40),
            ["nextBtn"] = new Rect(200, 200, 80, 40)
        };


        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public CompositionPlayer()
        {
            InitializeComponent();
            DispatcherHelper.EnsureispatcherQueue();

            this.Loaded += CompositionPlayer_Loaded;
        }

        private void CompositionPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeComposition();

            libVlc = new LibVLC();
            mediaPlayer = new VlcMediaPlayer(libVlc);

            var windowHandle = new WindowInteropHelper(mainWindow).Handle;
            mediaPlayer.EnableHardwareDecoding = true;
            mediaPlayer.Hwnd = windowHandle;
        }

        private void InitializeComposition()
        {
            compositor = new Comp.Compositor();
            var windowHandle = new WindowInteropHelper(mainWindow).Handle;

            rootVisual = compositor.CreateContainerVisual();

            videoVisual = compositor.CreateSpriteVisual();
            videoVisual.Size = new System.Numerics.Vector2((float)Width, (float)Height);
            rootVisual.Children.InsertAtBottom(videoVisual);

            controlVisual = compositor.CreateSpriteVisual();
            controlVisual.Size = new System.Numerics.Vector2((float)Width, (float)Height);
            controlVisual.Opacity = 1.0f;
            rootVisual.Children.InsertAtTop(controlVisual);

            windowTarget = compositor.CreateTargetForCurrentView();
            windowTarget.Root = rootVisual;

            var overlayContent = CreateOverlayContent();
            controlVisual.Children.InsertAtTop(overlayContent);
        }

        private Comp.ContainerVisual CreateOverlayContent()
        {
            var container = compositor.CreateContainerVisual();

            var playBtn = compositor.CreateSpriteVisual();
            playBtn.Size = new System.Numerics.Vector2(50, 50);
            playBtn.Offset = new System.Numerics.Vector3(10, 10, 0);

            var progressBar = compositor.CreateSpriteVisual();
            progressBar.Size = new System.Numerics.Vector2((float)Width, 10);
            progressBar.Offset = new System.Numerics.Vector3(0, (float)Height - 20, 0);

            container.Children.InsertAtTop(playBtn);
            container.Children.InsertAtTop(progressBar);

            //TODO: add back and skip buttons
            return container;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if(videoVisual != null && controlVisual != null)
            {
                var newSize = new System.Numerics.Vector2((float)sizeInfo.NewSize.Width, (float)sizeInfo.NewSize.Height);

                videoVisual.Size = newSize;
                controlVisual.Size = newSize;
            }
        }

        private void Container_LeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = e.GetPosition(InnerVideoGrid);

            foreach(var button in buttonBounds)
            {
                if (button.Value.Contains(clickPosition))
                {
                    HandleButtonClick(button.Key);
                }
            }
        }

        private void HandleButtonClick(string btnId)
        {
            switch (btnId)
            {
                case "playBtn":
                    PlayBtn_Click();
                    break;
                case "nextBtn":
                    NextBtn_Click();
                    break;
            }
        }

        public void PlayBtn_Click()
        {
            if (mainWindow._playlistManager == null)
            {
                Debug.WriteLine($"playlistManager is null(PlayBtn_Click): {this.GetHashCode()}");
                return;
            }
            if (mainWindow._fileManager.FullSeriesList.Count == 0)
            {
                Debug.WriteLine("there are no series in the list");
                return;
            }
            if (mediaPlayer.Media != null)
            {
                if (mediaPlayer != null && mediaPlayer.Media != null)
                {
                    if (mediaPlayer.IsPlaying)
                    {
                        mediaPlayer.Pause();
                    }
                    else
                    {
                        mediaPlayer.Play();
                    }
                }
            }
            else
            {
                //SETS THE MEDIA FOR THE FIRST TIME DURING RUNTIME
                media = new Media(libVlc, new Uri(mainWindow._playlistManager.Playlist[currentIndex].EpisodePath).AbsoluteUri,
                    FromType.FromLocation);
                mediaPlayer.Media = media;
                mediaPlayer.Play();
            }
        }

        public void NextBtn_Click()
        {
            mediaPlayer.Stop();
            currentIndex++;
            media = new Media(libVlc, new Uri(mainWindow._playlistManager.Playlist[currentIndex].EpisodePath).AbsoluteUri, FromType.FromLocation);
            mediaPlayer.Media = media;

            mediaPlayer.Play();
        }
    }
}
