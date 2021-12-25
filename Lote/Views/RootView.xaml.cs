using Lote.Common;
using Lote.Core.Common;
using Lote.Override;
using Lote.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XExten.Advance.LinqFramework;

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
            this.window.WindowState = WindowState.Minimized;
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
    }
}
