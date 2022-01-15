using Lote.Common;
using Lote.CommonWindow.ViewMdeol;
using Lote.Core.Common;
using Lote.Override;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XExten.Advance.StaticFramework;

namespace Lote.CommonWindow
{
    /// <summary>
    /// MangaReaderWindows.xaml 的交互逻辑
    /// </summary>
    public partial class MangaReaderWindows : LoteWindow
    {
        public MangaReaderWindows()
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
                    SystemHelper.SystemGC();
                    this.Close();
                    break;
                case SysFuncEnum.Down:
                    Copy();
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

        private void ColorZoneMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Copy()
        {
            var vm = (this.DataContext as MangaReaderWindowsViewModel);
            var root = vm.Names;
            if (root != null)
            {
                FileInfo infomation = new FileInfo(root[0].ToString());
                var ChapterInfo = vm.Chapters.FirstOrDefault(t => t.TagKey == infomation.Directory.Name);
                var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "LoteDown", "Manga", ChapterInfo.Name, ChapterInfo.Title));
                foreach (var item in root)
                {
                    FileInfo info = new FileInfo(item.ToString());
                    var files = SyncStatic.CreateFile(Path.Combine(dir, $"{info.Name}.jpg"));
                    info.CopyTo(files, true);
                }
                Process.Start("explorer.exe", dir);
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
    }
}
