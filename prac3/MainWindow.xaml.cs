using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace prac3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Boolean PlayerBtn = true;
        List<string> songs = new List<string>();
        int songamount = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void FolderPick_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                string[] files = Directory.GetFiles(dialog.FileName);
                List<string> strings = new List<string>();
                SongList.ItemsSource = null;
                SongList.ItemsSource = strings;
                songs = files.ToList();
                foreach (string s in songs)
                {
                    FileInfo info = new FileInfo(s);
                    strings.Add(info.Name);
                }
                SongList.SelectedIndex = 0;
                SongPlayer.Source = new Uri(files[SongList.SelectedIndex]);
                SongPlayer.Play();
                PlayStopBtn.Content = "꠱";
                songamount = files.Length;
            }
        }

        private void PlayStopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PlayerBtn == true)
            {
                SongPlayer.Stop();
                PlayStopBtn.Content = "▶";
                PlayerBtn = false;
            }
            else
            {
                SongPlayer.Play();
                PlayStopBtn.Content = "꠱";
                PlayerBtn = true;
            }

        }

        private void SongPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            SngSlider.Maximum = SongPlayer.NaturalDuration.TimeSpan.Ticks;
        }

        private void SngSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SongPlayer.Position = new TimeSpan(Convert.ToInt64(SngSlider.Value));
        }

        private void PrevSngBtn_Click(object sender, RoutedEventArgs e)
        {

            SongList.SelectedIndex -= 1;
            if (SongList.SelectedIndex < 0)
            {
                SongList.SelectedIndex = 0;
            }

        }

        private void NxtSngBtn_Click(object sender, RoutedEventArgs e)
        {
            SongList.SelectedIndex += 1;
            if (SongList.SelectedIndex > songamount)
            {
                SongList.SelectedIndex = songamount;
            }

        }
        private void SongChange()
        {

        }

        private void SongList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SongPlayer.Stop();
            SongPlayer.Source = new Uri(songs[SongList.SelectedIndex]);
            SongPlayer.Play();
        }
    }
}
