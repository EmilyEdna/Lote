using Lote.CommonWindow;
using Lote.CommonWindow.ViewMdeol;
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
using XExten.Advance.StaticFramework;

namespace Lote.Views.MusicViews
{
    /// <summary>
    /// MusicView.xaml 的交互逻辑
    /// </summary>
    public partial class MusicView : UserControl
    {
        private MusicViewModel vm;
        public System.Timers.Timer timer;
        public System.Timers.Timer lyrictimer;
        private int PlayState = -1;
        private IDictionary<string, LotePlayListDTO> CurrentPlay = null;
        private IDictionary<string, MusicLyricWindows> windows = null;
        public MusicView()
        {
            InitializeComponent();
            CurrentPlay = new Dictionary<string, LotePlayListDTO>();
            windows = new Dictionary<string, MusicLyricWindows>();
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
                        if (this.MediaPlay.Source != null && PlayState != -1)
                        {
                            this.SongTimeLbl.Content = this.MediaPlay.Position.ToString().Split(".").FirstOrDefault();
                            this.ProgressBars.Value = this.MediaPlay.Position.TotalSeconds;
                        }

                    }
                });
            };

            lyrictimer = new System.Timers.Timer
            {
                AutoReset = true,
                Interval = 100,
                Enabled = true
            };
            InitPlayBox();
        }

        private void InitPlayBox()
        {
            this.timer.Close();
            this.MediaPlay.Source = null;
            this.MediaPlay.Stop();
            this.MediaPlay.Close();
            this.UserPause.Visibility = Visibility.Collapsed;
            this.UserPlay.Visibility = Visibility.Visible;
            this.SongNameLbl.Content = "请选择要播放的歌曲...";
            this.SongTimeLbl.Content = "00:00:00";
            this.ProgressBars.Value = 0;
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
                        this.lyrictimer.Start();
                        PlayState = 1;
                    }
                    else
                        PlayingConditions();
                    break;
                case MusicPlayFuncEnum.Pause:
                    this.MediaPlay.Pause();
                    this.UserPause.Visibility = Visibility.Collapsed;
                    this.UserPlay.Visibility = Visibility.Visible;
                    this.timer.Stop();
                    this.lyrictimer.Stop();
                    PlayState = -1;
                    break;
                case MusicPlayFuncEnum.SkipNext:
                    Dispatcher.Invoke(() => InitPlayBox());
                    if (PlayCondition.SelectedIndex != -1 && vm.PlayLists.Count >= 1)
                    {
                        if (PlayCondition.SelectedIndex == 0 || PlayCondition.SelectedIndex == 1 || PlayCondition.SelectedIndex == 3)
                        {
                            if (PlayList.SelectedIndex + 1 == PlayList.Items.Count)
                            {
                                Playing(vm.PlayLists[0]);
                                PlayList.SelectedIndex = 0;
                                SetSelect();
                            }
                            else
                            {
                                Playing(vm.PlayLists[PlayList.SelectedIndex + 1]);
                                PlayList.SelectedIndex += 1;
                                SetSelect();
                            }
                        }
                        else if (PlayCondition.SelectedIndex == 2) { PlayingConditions(); }
                    }
                    else
                    {
                        if (PlayList.Items.Count != 0)
                        {
                            PlayCondition.SelectedIndex = 0;
                            PlayHandleClick(btn, e);
                        }
                        else
                            HandyControl.Controls.MessageBox.Info("木有任何歌曲（；´д｀）ゞ", "提示");
                    }
                    LyricState = 0;
                    SongLyricClick(null, null);
                    break;
                case MusicPlayFuncEnum.SkipPrevious:
                    Dispatcher.Invoke(() => InitPlayBox());
                    if (PlayCondition.SelectedIndex != -1 && vm.PlayLists.Count >= 1)
                    {
                        if (PlayCondition.SelectedIndex == 0 || PlayCondition.SelectedIndex == 1 || PlayCondition.SelectedIndex == 3)
                        {
                            if (PlayList.SelectedIndex == 0)//从最底部播放
                            {
                                Playing(vm.PlayLists[PlayList.Items.Count - 1]);
                                PlayList.SelectedIndex = PlayList.Items.Count - 1;
                                SetSelect();
                            }
                            else//正常上一曲
                            {
                                if (PlayList.SelectedIndex == -1)
                                {
                                    Playing(vm.PlayLists[PlayList.Items.Count - 1]);
                                    PlayList.SelectedIndex = PlayList.Items.Count - 1;
                                    SetSelect();
                                }
                                else
                                {
                                    Playing(vm.PlayLists[PlayList.SelectedIndex - 1]);
                                    PlayList.SelectedIndex -= 1;
                                    SetSelect();
                                }
                            }
                        }
                        else if (PlayCondition.SelectedIndex == 2) { PlayingConditions(); }
                    }
                    else
                    {
                        if (PlayList.Items.Count != 0)
                        {
                            PlayCondition.SelectedIndex = 0;
                            PlayHandleClick(btn, e);
                        }
                        else
                            HandyControl.Controls.MessageBox.Info("木有任何歌曲（；´д｀）ゞ", "提示");
                    }
                    LyricState = 0;
                    SongLyricClick(null, null);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="input"></param>
        private void Playing(LotePlayListDTO input)
        {
            CurrentPlay.Clear();
            CurrentPlay.Add(nameof(LotePlayListDTO), input);
            SongNameLbl.Content = input.SongName;
            MediaPlay.Close();
            MediaPlay.Source = new Uri(input.CacheAddress, UriKind.Absolute);
            MediaPlay.Play();
            LoadTime();
            UserPlay.Visibility = Visibility.Collapsed;
            UserPause.Visibility = Visibility.Visible;
            PlayState = 1;
            timer.Start();
        }

        /// <summary>
        /// 加载音频的时常
        /// </summary>
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

        /// <summary>
        /// 播放条件
        /// </summary>
        private void PlayingConditions()
        {
            if (PlayList.Items.Count <= 0)
                return;

            if (PlayCondition.SelectedIndex == -1) { PlayCondition.SelectedIndex = 0; }//如果用户没有选择播放模式默认列表循环
            InitPlayBox();
            if (PlayCondition.SelectedIndex == 0)//列表循环
            {
                if (PlayList.SelectedIndex + 1 == PlayList.Items.Count)//判断是否到列表底部
                {
                    var PlayDto = vm.PlayLists.FirstOrDefault();
                    Playing(PlayDto);
                    PlayList.SelectedIndex = 0;//定位
                    SetSelect();
                }
                else
                {
                    Playing(vm.PlayLists[PlayList.SelectedIndex + 1]);
                    PlayList.SelectedIndex += 1;//定位
                    SetSelect();
                }
            }
            else if (PlayCondition.SelectedIndex == 1)//单曲循环
            {
                var index = PlayList.SelectedIndex == -1 ? 0 : PlayList.SelectedIndex;
                Playing(vm.PlayLists[index]);
                PlayList.SelectedIndex = index;
                SetSelect();
            }
            else if (PlayCondition.SelectedIndex == 2)//随机播放
            {
                if (PlayList.Items.Count == 1)
                {
                    Playing(vm.PlayLists[0]);
                    PlayList.SelectedIndex = 0;//定位
                    SetSelect();
                }
                else
                {
                    int stc = 0;//循环条件
                    while (stc == 0)//循环
                    {
                        int i = new Random().Next(PlayList.Items.Count - 1);//随机范围在列表最大-1
                        if (PlayList.SelectedIndex != i)//排除当前正在播放的
                        {
                            Playing(vm.PlayLists[i]);//播放歌曲
                            PlayList.SelectedIndex = i;//定位
                            stc = 1;//条件排除
                            SetSelect();
                        }
                    }
                }
            }
            else if (PlayCondition.SelectedIndex == 3)//顺序播放
            {
                if (PlayList.SelectedIndex + 1 != PlayList.Items.Count)
                {
                    Playing(vm.PlayLists[PlayList.SelectedIndex + 1]);
                    PlayList.SelectedIndex += 1;
                    SetSelect();
                }
            }
        }

        /// <summary>
        /// 音量条离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolumeSettingMouseLeave(object sender, MouseEventArgs e)
        {
            BeginStoryboard((Storyboard)FindResource("Close"));
            VolumeAnime = 0;
        }

        /// <summary>
        /// 音量改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = (sender as Slider);
            VolumeShow.Content = (int)slider.Value + "%";
            MediaPlay.Volume = slider.Value / 100;
        }
        private void SetSelect()
        {
            for (int index = 0; index < this.PlayList.Items.Count; index++)
            {
                var item = (this.PlayList.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem);

                if (index == this.PlayList.SelectedIndex)
                {
                    item.IsSelected = true;
                    item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF9999"));
                }
                else
                {
                    item.IsSelected = false;
                    item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"));
                }

            }
        }

        private int LyricState = 0;
        private void SongLyricClick(object sender, MouseButtonEventArgs e)
        {

            if (this.MediaPlay.Source != null && LyricState == 0)
            {
                MusicLyricResult result = this.vm.LoadLyric(CurrentPlay.Values.FirstOrDefault());
                if (result == null && result.Lyrics == null)
                    return;
                LyricState = 1;
                MusicLyricWindows win = new MusicLyricWindows();
                win.DataContext = vm.GetContainer<MusicLyricWindowsViewModel>();

                lyrictimer.Elapsed += (s, e) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        var Seconds = this.MediaPlay.Position.ToString().Split(".").FirstOrDefault();
                        if (result.Lyrics != null)
                        {
                            foreach (var item in result.Lyrics)
                            {
                                var Target = "00:" + item.Time.Split(".").FirstOrDefault();
                                if (Target == Seconds)
                                {
                                    (win.DataContext as MusicLyricWindowsViewModel).Lyric = item.Lyric;
                                }
                            }
                        }

                    });
                };

                win.WindowStartupLocation = WindowStartupLocation.Manual;
                win.Top = (SystemParameters.PrimaryScreenHeight / 10) * 7.5;
                win.Left = (SystemParameters.PrimaryScreenWidth / 10) * 1.9;
                win.Topmost = true;
                win.Show();
                if (windows.Count != 0)
                {
                    windows.Values.FirstOrDefault().Close();
                    windows.Clear();
                }
                windows.Add(nameof(MusicLyricWindows), win);
                lyrictimer.Start();
                return;
            }
            if (this.MediaPlay.Source == null || LyricState == 1)
            {
                var win = windows.Values.FirstOrDefault();
                if (win == null)
                    return;
                (win.DataContext as MusicLyricWindowsViewModel).Lyric = String.Empty;
                win.Close();
                lyrictimer.Close();
                LyricState = 0;
            }
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            var mitem = (sender as MenuItem);
            var Route = vm.GetPlayRoute(Guid.Parse(mitem.CommandParameter.ToString()));
            if (this.MediaPlay.Source != null)
            {
                if (this.MediaPlay.Source.LocalPath.Equals(Route))
                {
                    InitPlayBox();
                    SyncStatic.DeleteFile(Route);
                }
            }

        }
    }
}
