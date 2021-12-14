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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lote.Views.Option
{
    /// <summary>
    /// OptionView.xaml 的交互逻辑
    /// </summary>
    public partial class OptionView : UserControl
    {
        public OptionView()
        {
            InitializeComponent();
        }

        private void WkPwd(object sender, RoutedEventArgs e)
        {
            var dc = (this.DataContext as OptionViewModel).Root;
            dc.WkPwd = (sender as PasswordBox).Password;
        }

        private void ProxyPwd(object sender, RoutedEventArgs e)
        {
            var dc = (this.DataContext as OptionViewModel).Root;
            dc.ProxyPwd = (sender as PasswordBox).Password;
        }
    }
}
