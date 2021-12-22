using Lote.Core.Common;
using Lote.Override;
using Lote.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using XExten.Advance.LinqFramework;

namespace Lote.Views
{
    public partial class RootView : HandyControl.Controls.Window
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
            {
                color = (Color)ColorConverter.ConvertFromString("#FF2CCFA0");
                Zone.Background = new SolidColorBrush(color);
            }
            else if(item.SeleteType == 1)
            {
                color = (Color)ColorConverter.ConvertFromString("#FFFF9999");
                Zone.Background = new SolidColorBrush(color);
            }
            else if (item.SeleteType == 2)
            {
                color = (Color)ColorConverter.ConvertFromString("#FF10AEC2");
                Zone.Background = new SolidColorBrush(color);
            }
            else if (item.SeleteType == 3)
            {
                color = (Color)ColorConverter.ConvertFromString("#FFED556A");
                Zone.Background = new SolidColorBrush(color);
            }
        }

        private void ColorZoneMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
