using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        Boolean ShuffleInfo = true;
        Boolean PlayerBtn = true;
        List<string> songs = new List<string>();
        int songamount = 0;
        List<string> ShuffleList = new List<string>();
        Boolean RepeatStatus = false;
        Boolean RepeatStatusControler = false;

        public MainWindow()
        {
            InitializeComponent();
            ThreadShit();
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
                    if(s.EndsWith("mp3") || s.EndsWith("m4a") || s.EndsWith("waw"))
                    {
                        FileInfo info = new FileInfo(s);
                        strings.Add(info.Name);
                    }
                    
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
                SongPlayer.Pause();
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
            if (RepeatStatus == true && SngSlider.Maximum == SngSlider.Value)
            {
                SngSlider.Value = 0;
                RepeatStatusControler = true;
            }
            else
            {
                if (SngSlider.Maximum == SngSlider.Value)
                {
                    SongList.SelectedIndex += 1;
                    RepeatStatusControler = false;
                }
            }

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

        private void SongList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SngSlider.Value = 0;
            SongPlayer.Stop();

            if (SongList.SelectedIndex != -1)
            {
                SongPlayer.Source = new Uri(songs[SongList.SelectedIndex]);
                SongPlayer.Play();
            }
        }

        private void ShuffleBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ShuffleInfo == true)
            {
                SongList.ItemsSource = null;
                SongList.SelectedIndex = 0;
                ShuffleMethod();
                ShuffleInfo = false;
            }
            else
            {
                SongList.ItemsSource = null;
                SongList.ItemsSource = songs;
                SongList.SelectedIndex = 0;
                ShuffleInfo = true;
            }

        }
        private void ShuffleMethod()
        {
            Random random = new Random();
            List<string> ShuffleList = new List<string>();
            List<int> value = Enumerable.Range(0, songamount).ToList();
            for (int i = 0; i < songs.Count; i++)
            {
                int index = random.Next(0, value.Count);
                ShuffleList.Add(songs[value[index]]);
                value.RemoveAt(index);
            }
            SongList.ItemsSource = ShuffleList;
            SongPlayer.Source = new Uri(ShuffleList[SongList.SelectedIndex]);
        }
        public void ThreadShit()
        {
            Thread thread = new Thread(_ =>
            {

                while (true)
                {
                    Thread.Sleep(1);
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        SngSlider.Value = SongPlayer.Position.Ticks;
                        SongPosition.Text = Convert.ToString(SongPlayer.Position);
                        SongDuration.Text = Convert.ToString(SongPlayer.NaturalDuration);
                    }));
                }

            });
            thread.Start();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void RepeateBtn_Click(object sender, RoutedEventArgs e)
        {
            RepeatStatus = true;
            if (RepeatStatusControler == true)
            {
                RepeatStatus = false;
            }
        }
    }
}