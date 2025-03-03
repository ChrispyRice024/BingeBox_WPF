using System;
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
using System.Windows.Shapes;
using LibVLCSharp.WPF;
using LibVLCSharp.Shared;
using BingeBox_WPF.Controls;
using System.Windows.Interop;

namespace BingeBox_WPF.Windows
{
    /// <summary>
    /// Interaction logic for FloatingWindow.xaml
    /// </summary>
    public partial class FloatingWindow : Window
    {
        public IntPtr hwnd;

        public VLCControl _parentControl;
        public VideoView FSPlayer;

        public FloatingWindow()
        {
            InitializeComponent();
        }
        public FloatingWindow(VLCControl parent)
        {
            this.Loaded += FloatingWindow_Loaded;
            _parentControl = parent;
            FSPlayer = _parentControl.VideoPlayer;
            hwnd = new WindowInteropHelper(this).Handle;
            FSPlayer.MediaPlayer.EnableHardwareDecoding = true;
            FSPlayer.MediaPlayer.Hwnd = hwnd;

        }

        private void FloatingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeComposition();
            FSPlayer = _parentControl.VideoPlayer;
        }

        private void InitializeComposition()
        {

        }
    }
}
