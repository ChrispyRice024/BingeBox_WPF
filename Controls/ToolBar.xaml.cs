using System;
using System.Collections.Generic;
using System.Linq;
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
using BingeBox_WPF.Windows;
using BingeBoxLib_WPF.Managers;

namespace BingeBox_WPF.Controls
{
    /// <summary>
    /// Interaction logic for ToolBar.xaml
    /// </summary>
    public partial class ToolBar : WinControls.UserControl
    {

        private Window win;
        public FileManager _fileManager;
        public ToolBar()
        {
            InitializeComponent();
        }

        private void MenuItem_AddSeries_Click(object sender, RoutedEventArgs e)
        {
            win = new AddSeries(_fileManager);
            win.Show();
        }
    }
}
