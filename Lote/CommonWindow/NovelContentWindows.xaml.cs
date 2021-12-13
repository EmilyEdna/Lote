using Lote.CommonWindow.ViewMdeol;
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
    /// NovelContentWindows.xaml 的交互逻辑
    /// </summary>
    public partial class NovelContentWindows : HandyControl.Controls.Window
    {
        public NovelContentWindows()
        {
            InitializeComponent();
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void WindowColor(object sender, RoutedEventArgs e)
        {
            this.window.Background = (sender as Button).Background;
        }
    }
}
