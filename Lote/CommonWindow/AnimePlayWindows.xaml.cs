﻿using LibVLCSharp.Shared;
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


namespace Lote.CommonWindow
{
    /// <summary>
    /// AnimePlayWindows.xaml 的交互逻辑
    /// </summary>
    public partial class AnimePlayWindows : HandyControl.Controls.Window
    {

        public AnimePlayWindows()
        {
            InitializeComponent();
            LibVLCSharp.Shared.Core.Initialize(Environment.CurrentDirectory + @"\VLC\X64\");
            this.LibVlcs = new LibVLC();
            this.MediaPlayers = new LibVLCSharp.Shared.MediaPlayer(this.LibVlcs);
            this.videos.MediaPlayer = this.MediaPlayers;
            this.window.MaxWidth = SystemParameters.PrimaryScreenWidth;
            this.window.MaxHeight = SystemParameters.PrimaryScreenHeight;
        }

        #region Field
        public LibVLC LibVlcs;
        public LibVLCSharp.Shared.MediaPlayer MediaPlayers;
        public Media Medias;
        public AnimePlayWindowsViewModel ViewModel;
        #endregion


        #region Private
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (this.DataContext as AnimePlayWindowsViewModel);
        }

        private void BtnClick(object sender, RoutedEventArgs e)
        {
            var btn = (sender as LoteButton);
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
                case VLCFuncEnum.FullScreen:
                    break;
                case VLCFuncEnum.FullScreenExit:
                    break;
                default:
                    break;
            }

        }

        private void Play()
        {
            if (!this.videos.MediaPlayer.IsPlaying)
            {
                using (Medias = new Media(LibVlcs, new Uri(ViewModel.WatchRoute)))
                    this.videos.MediaPlayer.Play(Medias);

                this.videos.MediaPlayer.Playing += MediaPlayer_Playing;
                this.videos.MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            }
            else
            {
                this.videos.MediaPlayer.Play();
            }


        }

        private void MediaPlayer_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var play = this.videos.MediaPlayer;
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
                var play = this.videos.MediaPlayer;
                Rate.Maximum = play.Length / 1000;
                RateTotal.Text = "/" + TimeSpan.FromSeconds(play.Length / 1000).ToString();
            });
        }

        private void Pause()
        {
            this.videos.MediaPlayer.Pause();
        }

        private void Stop()
        {
            this.videos.MediaPlayer.Stop();
        }

        private void VoiceChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.videos.MediaPlayer == null) return;
            this.videos.MediaPlayer.Volume = Convert.ToInt32(e.NewValue) * 10;
        }

        private void RateDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if (this.videos.MediaPlayer == null) return;
            var position = (float)(Rate.Value / Rate.Maximum);
            if (position == 1)
            {
                position = 0.99f;
            }
            this.videos.MediaPlayer.Position = position;
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

        private void Max()
        {
            this.window.WindowState = this.window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Min()
        {
            this.window.WindowState = WindowState.Minimized;
        }
        #endregion

        public void CloseBase()
        {
            this.MediaPlayers.Stop();
            this.MediaPlayers.Dispose();
            this.LibVlcs.Dispose();
            this.Close();
        }
    }
}
