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

namespace Lote.Views.WallpaperViews
{
    /// <summary>
    /// WallpaperView.xaml 的交互逻辑
    /// </summary>
    public partial class WallpaperView : UserControl
    {
        public WallpaperView()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var vm = (this.DataContext as WallpaperViewModel);
            vm.PageIndex = 1;
            vm.InitFavorite(string.Empty);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var vm = (this.DataContext as WallpaperViewModel);
            Dispatcher.Invoke(() =>
            {
                vm.InitAll();
            });
        }
    }
}
