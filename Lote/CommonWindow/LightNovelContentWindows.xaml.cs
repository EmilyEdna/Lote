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
    /// LightNovelContentWindows.xaml 的交互逻辑
    /// </summary>
    public partial class LightNovelContentWindows : Lote.Override.LoteWindow
    {
        public LightNovelContentWindows()
        {
            InitializeComponent();
        }

        public Color color;

        private void WindowColor(object sender, RoutedEventArgs e)
        {
            this.window.Background = (sender as Button).Background;
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
                    this.Close();
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
            else
                color = (Color)ColorConverter.ConvertFromString("#FFED556A");
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
