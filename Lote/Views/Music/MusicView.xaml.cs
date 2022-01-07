using Lote.Core.Common;
using Lote.Core.Service.DTO;
using Lote.Override;
using Music.SDK.ViewModel.Response;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XExten.Advance.LinqFramework;

namespace Lote.Views.Music
{
    /// <summary>
    /// MusicView.xaml 的交互逻辑
    /// </summary>
    public partial class MusicView : UserControl
    {
        private MusicViewModel vm;
        private System.Timers.Timer timer;
        public MusicView()
        {
            InitializeComponent();
            InitPlayBox();
            timer = new System.Timers.Timer
            {
                AutoReset = true,
                Interval = 100,
                Enabled = true
            };
            timer.Elapsed += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (this.ProgressBars.Value == this.ProgressBars.Maximum)
                    {
                        InitPlayBox();
                        PlayingConditions();
                    }
                    else
                    {
                        if (this.MediaPlay.Source != null)
                        {
                            this.SongTimeLbl.Content = this.MediaPlay.Position.ToString().Split(".").FirstOrDefault();
                            this.ProgressBars.Value = this.MediaPlay.Position.TotalSeconds;
                        }

                    }
                });
            };
        }

        private void InitPlayBox()
        {
            this.MediaPlay.Source = null;
            this.UserPause.Visibility = Visibility.Collapsed;
            this.UserPlay.Visibility = Visibility.Visible;
            this.SongNameLbl.Content = "请选择要播放的歌曲...";
            this.SongTimeLbl.Content = "00:00:00";
            this.ProgressBars.IsEnabled = false;
        }

        /// <summary>
        /// 切换列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Tab = (sender as TabControl);
            if (Tab == null)
                return;
            var vm = (this.DataContext as MusicViewModel);
            vm.PageIndex = 1;
            Dispatcher.Invoke(() => vm.ShowSong(Tab.SelectedIndex + 1));

        }

        /// <summary>
        /// 歌单变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sheets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tabs.SelectedIndex = 2;
        }

        /// <summary>
        /// 显示专辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAlbumClick(object sender, RoutedEventArgs e)
        {
            Tabs.SelectedIndex = 3;
            var vm = (this.DataContext as MusicViewModel);
            var id = (sender as LoteButton).CommandParameter.ToString();
            Dispatcher.Invoke(() => vm.ShowAlbum(id));
        }

        private void SongItemChanged(object sender, ScrollChangedEventArgs e)
        {
            var vm = (this.DataContext as MusicViewModel);
            if (vm.PageIndex == 1 && e.VerticalOffset == 0)
                return;
            if (vm.PageIndex > 1 && e.VerticalChange >= -48 && e.VerticalOffset == 0)
            {
                Dispatcher.Invoke(() => vm.SongLoadMore(false));
                var source = (e.OriginalSource as HandyControl.Controls.ScrollViewer);
                source.ScrollToHome();
            }
            if (e.VerticalOffset >= 300 && e.VerticalChange.ToString().Contains("."))
            {
                Dispatcher.Invoke(() => vm.SongLoadMore(true));
                var source = (e.OriginalSource as HandyControl.Controls.ScrollViewer);
                source.ScrollToHome();
            }
        }

        /// <summary>
        /// 音量控制
        /// </summary>
        private int VolumeAnime = 0;
        private void VolumeClick(object sender, MouseButtonEventArgs e)
        {
            if (VolumeAnime == 0)
            {
                BeginStoryboard((Storyboard)FindResource("Open"));
                VolumeAnime = 1;
            }
            else
            {
                BeginStoryboard((Storyboard)FindResource("Close"));
                VolumeAnime = 0;
            }
        }

        /// <summary>
        /// 播放控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayHandleClick(object sender, RoutedEventArgs e)
        {
            vm = this.DataContext as MusicViewModel;
            var btn = (sender as Button);
            var type = (MusicPlayFuncEnum)btn.CommandParameter.AsString().AsInt();
            switch (type)
            {
                case MusicPlayFuncEnum.Play:
                    if (MediaPlay.NaturalDuration.HasTimeSpan)
                    {
                        MediaPlay.Play();
                        this.UserPause.Visibility = Visibility.Visible;
                        this.UserPlay.Visibility = Visibility.Collapsed;
                        this.timer.Start();
                    }
                    else
                        PlayingConditions();
                    break;
                case MusicPlayFuncEnum.Pause:
                    this.MediaPlay.Pause();
                    this.UserPause.Visibility = Visibility.Collapsed;
                    this.UserPlay.Visibility = Visibility.Visible;
                    this.timer.Stop();
                    break;
                case MusicPlayFuncEnum.SkipNext:
                    PlayingConditions();
                    break;
                case MusicPlayFuncEnum.SkipPrevious:
                    PlayingConditions();
                    break;
                default:
                    break;
            }
        }

        private void Playing(PlayListDTO input)
        {
            MediaPlay.Close();
            MediaPlay.Source = new Uri(input.CacheAddress, UriKind.Absolute);
            MediaPlay.Play();
            SongNameLbl.Content = input.SongName;
            LoadTime();
            UserPlay.Visibility = Visibility.Collapsed;
            UserPause.Visibility = Visibility.Visible;
        }

        private void LoadTime()
        {
            Dispatcher.Invoke(() =>
            {
                Thread.Sleep(200);
                var time = !MediaPlay.NaturalDuration.HasTimeSpan ? "" : MediaPlay.NaturalDuration.TimeSpan.ToString()?.Split(".")?.FirstOrDefault();
                if (string.IsNullOrEmpty(time))
                    LoadTime();
                else
                {
                    SongTimeLbl.Content = time;
                    this.ProgressBars.IsEnabled = true;
                    this.ProgressBars.Maximum = int.Parse(time.Substring(3, 2)) * 60 + int.Parse(time.Substring(6, 2));
                    this.ProgressBars.Value = 0;
                    return;
                }
            });
        }

        private void PlayingConditions()
        {
            if (PlayCondition.SelectedIndex == -1) { PlayCondition.SelectedIndex = 0; }//如果用户没有选择播放模式默认列表循环
            InitPlayBox();
            if (PlayCondition.SelectedIndex == 0)//列表循环
            {
                if (PlayList.SelectedIndex + 1 == PlayList.Items.Count)//判断是否到列表底部
                {
                    var PlayDto = vm.PlayLists.FirstOrDefault();
                    Playing(PlayDto);
                    PlayList.SelectedIndex = 0;//定位
                }
                else
                {
                    Playing(vm.PlayLists[PlayList.SelectedIndex + 1]);
                    PlayList.SelectedIndex += 1;//定位
                }
            }
            else if (PlayCondition.SelectedIndex == 1)//单曲循环
            {
                //Play_Misic(Number[主页的播放列表.SelectedIndex].ToString());//播放当前选中项
            }
            else if (PlayCondition.SelectedIndex == 2)//随机播放
            {
                if (PlayList.Items.Count == 1)
                {
                    //Play_Misic(Number[0].ToString());//播放歌曲
                    PlayList.SelectedIndex = 0;//定位
                }
                else
                {
                    int stc = 0;//循环条件
                    while (stc == 0)//循环
                    {
                        int i = new Random().Next(PlayList.Items.Count - 1);//随机范围在列表最大-1
                        if (PlayList.SelectedIndex != i)//排除当前正在播放的
                        {
                            //Play_Misic(Number[i].ToString());//播放歌曲
                            PlayList.SelectedIndex = i;//定位
                            stc = 1;//条件排除
                        }
                    }
                }
            }
            else if (PlayCondition.SelectedIndex == 3)//顺序播放
            {
                if (PlayList.SelectedIndex + 1 != PlayList.Items.Count)
                {
                    // Play_Misic(Number[主页的播放列表.SelectedIndex + 1].ToString());
                    PlayList.SelectedIndex += 1;
                }
            }
        }
    }
}
