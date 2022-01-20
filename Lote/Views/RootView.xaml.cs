using Lote.Common;
using Lote.Core.Common;
using Lote.Override;
using Lote.ViewModels;
using Lote.Views.MusicViews;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace Lote.Views
{
    public partial class RootView : LoteWindow
    {
        public RootView()
        {
            InitializeComponent();
        }
        public Color color;
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
                    if (Contents != null)
                    {
                        if (Contents.Content is MusicView)
                        {
                            var view = (Contents.Content as MusicView);
                            view.timer.Close();
                            view.lyrictimer.Close();
                        }
                    }
                    this.Close();
                    Application.Current.Shutdown();
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
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Visibility = Visibility.Collapsed;
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

            //设置字体
            if (Navs != null)
            {
                UiElementHelper.FindVisualChild<LoteButton>(Navs).ForEach(item =>
                {
                    item.Foreground = new SolidColorBrush(color);
                });
            }
            if (Contents != null)
            {
                if (Contents.FindName("播放器背景") != null)
                    ((Rectangle)Contents.FindName("播放器背景")).Fill = Zone.Background;
            }
        }

        private void ColorZoneMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
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

        private void UserCenterClick(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as RootViewModel).Redirect("8");
        }

        private void ProcessClick(object sender, RoutedEventArgs e)
        {
            var Icon = (TaskIcoFuncEnum)(sender as MenuItem).CommandParameter.AsString().AsInt();
            string dir = string.Empty;
            switch (Icon)
            {
                case TaskIcoFuncEnum.Manga:
                    dir = SyncStatic.CreateDir(System.IO.Path.Combine(Environment.CurrentDirectory, "LoteDown", "Manga"));
                    break;
                case TaskIcoFuncEnum.Music:
                    dir = SyncStatic.CreateDir(System.IO.Path.Combine(Environment.CurrentDirectory, "LoteDown", "Music"));
                    break;
                case TaskIcoFuncEnum.Wallpaper:
                    dir = SyncStatic.CreateDir(System.IO.Path.Combine(Environment.CurrentDirectory, "LoteDown", "Wallpaper"));
                    break;
                case TaskIcoFuncEnum.Novel:
                    dir = SyncStatic.CreateDir(System.IO.Path.Combine(Environment.CurrentDirectory, "LoteDown", "LightNovel"));
                    break;
                default:
                    if (Contents != null)
                    {
                        if (Contents.Content is MusicView)
                        {
                            var view = (Contents.Content as MusicView);
                            view.timer.Close();
                            view.lyrictimer.Close();
                        }
                    }
                    this.Close();
                    Application.Current.Shutdown();
                    break;
            }
            if(!dir.IsNullOrEmpty())
                Process.Start("explorer.exe", dir);
        }
    }
}
