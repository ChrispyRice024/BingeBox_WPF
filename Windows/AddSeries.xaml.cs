using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using BingeBoxLib_WPF;
using BingeBoxLib_WPF.Models;
using BingeBoxLib_WPF.Managers;
using Microsoft.Win32;
using System.Diagnostics;

namespace BingeBox_WPF.Windows
{
    /// <summary>
    /// Interaction logic for AddSeries.xaml
    /// </summary>
    public partial class AddSeries : Window, INotifyPropertyChanged
    {

        //TODO: add loading bar/some kind of feedback(loading wheel?)
        public event PropertyChangedEventHandler? PropertyChanged;

        private OpenFolderDialog folderDialog;
        private FileManager _fileManager;

        private Series newSeries;
        public Series NewSeries
        {
            get { return newSeries; }
            set
            {
                newSeries = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(newSeries.SeriesName));
            }
        }

        private bool isEpisodic;
        public bool IsEpisodic
        {
            get { return isEpisodic; }
            set
            {
                isEpisodic = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEpisodic"));
            }
        }

        private string seriesName;
        public string SeriesName
        {
            get { return seriesName; }
            set
            {
                seriesName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesName"));
            }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Location"));
            }
        }
        public AddSeries(FileManager fileManager)
        {
            DataContext = this;
            InitializeComponent();
            _fileManager = fileManager;
        }
        
        private async void AddSeriesBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    newSeries = new Series(seriesName, isEpisodic, location);
                });
                Debug.WriteLine($"Series Name: {newSeries.SeriesName}");
                _fileManager.SaveToFile(newSeries, FileManager.seriesFile);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"New Exception When Creating Series: {ex}");
            }   
        }
        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            folderDialog = new OpenFolderDialog();
            folderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            folderDialog.ShowDialog();

            string SelectedFolder = folderDialog.FolderName;

            Location = SelectedFolder;
        }
    }
}
