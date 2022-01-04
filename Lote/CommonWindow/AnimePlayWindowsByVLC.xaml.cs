using LibVLCSharp.Shared;
using Lote.CommonWindow.ViewMdeol;
using Lote.Core.Common;
using Lote.Override;
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
using System.Windows.Threading;

namespace Lote.CommonWindow
{
    /// <summary>
    /// AnimePlayWindows.xaml 的交互逻辑
    /// </summary>
    public partial class AnimePlayWindowsByVLC : LoteWindow
    {

        public AnimePlayWindowsByVLC()
        {
            InitializeComponent();
            LibVLCSharp.Shared.Core.Initialize(Environment.CurrentDirectory + @"\Plugins\VLC_X64\");
            this.LibVlcs = new LibVLC();
            this.MediaPlayers = new LibVLCSharp.Shared.MediaPlayer(this.LibVlcs);
            this.Videos.MediaPlayer = this.MediaPlayers;
            this.window.MaxWidth = SystemParameters.PrimaryScreenWidth;
            this.window.MaxHeight = SystemParameters.PrimaryScreenHeight;
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        #region Field
        private DispatcherTimer Timer;
        private LibVLC LibVlcs;
        private Color color;
        private LibVLCSharp.Shared.MediaPlayer MediaPlayers;
        private Media Medias;
        private AnimePlayWindowsVLCViewModel ViewModel;
        #endregion

        #region Private

        private void Play()
        {
            if (!this.Videos.MediaPlayer.IsPlaying)
            {
                using (Medias = new Media(LibVlcs, new Uri(ViewModel.WatchRoute)))
                    this.Videos.MediaPlayer.Play(Medias);

                this.Videos.MediaPlayer.Playing += MediaPlayer_Playing;
                this.Videos.MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            }
            else
            {
                this.Videos.MediaPlayer.Play();
            }
        }

        private void Pause()
        {
            this.Videos.MediaPlayer.Pause();
        }

        private void Stop()
        {
            this.RatePlay.Text = "00:00:00";
            this.RateTotal.Text= "00:00:00";
            this.Videos.MediaPlayer.Stop();
        }

        private void Max()
        {
            this.window.WindowState = this.window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Min()
        {
            this.window.WindowState = WindowState.Minimized;
        }
        #endregion

        #region Event
        private void VoiceChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.Videos.MediaPlayer == null) return;
            this.Videos.MediaPlayer.Volume = Convert.ToInt32(e.NewValue) * 10;
        }

        private void RateDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if (this.Videos.MediaPlayer == null) return;
            var position = (float)(Rate.Value / Rate.Maximum);
            if (position == 1)
            {
                position = 0.99f;
            }
            this.Videos.MediaPlayer.Position = position;
        }

        private void SysClick(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Button);
            var SysFunc = Enum.Parse<SysFuncEnum>(btn.CommandParameter.ToString());
            switch (SysFunc)
            {
                case SysFuncEnum.Min:
                    Min();
                    break;
                case SysFuncEnum.Max:
                    Max();
                    break;
                case SysFuncEnum.Close:
                    CloseBase();
                    break;
                default:
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (this.DataContext as AnimePlayWindowsVLCViewModel);
        }

        private void BtnClick(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Button);
            var VlcFunc = Enum.Parse<VLCFuncEnum>(btn.CommandParameter.ToString());
            switch (VlcFunc)
            {
                case VLCFuncEnum.Pause:
                    Pause();
                    break;
                case VLCFuncEnum.Play:
                    Play();
                    break;
                case VLCFuncEnum.Stop:
                    Stop();
                    break;
                default:
                    break;
            }

        }

        private void MediaPlayer_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var play = this.Videos.MediaPlayer;
                Rate.Value = play.Time / 1000;
                RatePlay.Text = TimeSpan.FromSeconds(play.Time / 1000).ToString();
                if (Rate.Value % 60 == 0)
                {

                }
            });
        }

        private void MediaPlayer_Playing(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var play = this.Videos.MediaPlayer;
                Rate.Maximum = play.Length / 1000;
                RateTotal.Text = "/" + TimeSpan.FromSeconds(play.Length / 1000).ToString();
            });
        }

        private void ColorZoneMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ThemeSelect(object sender, SelectionChangedEventArgs e)
        {
            var select = (sender as ComboBox);

            var item = (select.Items[select.SelectedIndex] as LoteComboBoxItem);

            if (item.SeleteType == 0)
                color = (Color)ColorConverter.ConvertFromString("#FF2CCFA0");
            else if (item.SeleteType == 1)
                color = (Color)ColorConverter.ConvertFromString("#FFFF9999");
            else if (item.SeleteType == 2)
                color = (Color)ColorConverter.ConvertFromString("#FF10AEC2");
            else if (item.SeleteType == 3)
                color = (Color)ColorConverter.ConvertFromString("#FFED556A");
            else
                color = (Color)ColorConverter.ConvertFromString("#FF000000");
            //设置背景
            Zone.Background = new SolidColorBrush(color);
        }

        private void BackgroudSelect(object sender, SelectionChangedEventArgs e)
        {
            var select = (sender as ComboBox);

            var item = (select.Items[select.SelectedIndex] as LoteComboBoxItem);

            if (item.SeleteType == 0)
            {
                this.Source = new BitmapImage(new Uri("/Resource/Assets/Backgroud1.jpg", UriKind.Relative));
            }
            else if (item.SeleteType == 1)
            {
                this.Source = new BitmapImage(new Uri("/Resource/Assets/Backgroud2.jpg", UriKind.Relative));
            }
            else if (item.SeleteType == 2)
            {
                this.Source = new BitmapImage(new Uri("/Resource/Assets/Backgroud3.jpg", UriKind.Relative));
            }
            else if (item.SeleteType == 3)
            {
                this.Source = new BitmapImage(new Uri("/Resource/Assets/Backgroud4.jpg", UriKind.Relative));
            }
        }

        private void WindowMouseEnter(object sender, MouseEventArgs e)
        {
            VideoHandle.Visibility = Visibility.Visible;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.Videos != null && this.Videos.MediaPlayer != null && this.Videos.MediaPlayer.IsPlaying && VideoHandle.Visibility == Visibility.Visible)
            {
                VideoHandle.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        public void CloseBase()
        {
            this.MediaPlayers.Dispose();
            this.LibVlcs.Dispose();
            this.Timer.Stop();
            this.Close();
        }
    }
}
