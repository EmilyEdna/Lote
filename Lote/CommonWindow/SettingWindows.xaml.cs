using Lote.CommonWindow.ViewMdeol;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XExten.Advance.LinqFramework;

namespace Lote.CommonWindow
{
    /// <summary>
    /// SettingWindows.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindows : LoteWindow
    {
        private string OpenWindow = "OpenWindow";
        private string GiftClose = "GiftClose";
        private string GiftOpen = "GiftOpen";
        public SettingWindows()
        {
            InitializeComponent();
            BeginAnime(OpenWindow);
        }

        private void BeginAnime(string key)
        {
            BeginStoryboard((Storyboard)FindResource(key));
        }

        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void GiftCloseClick(object sender, RoutedEventArgs e)
        {
            BeginAnime(GiftClose);
        }

        private void GiftOpenClick(object sender, MouseButtonEventArgs e)
        {
            GiftContent.Visibility = Visibility.Visible;
            BeginAnime(GiftOpen);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PlayBoxChecked(object sender, RoutedEventArgs e)
        {
            var rb = (sender as RadioButton);
            var vm = (this.DataContext as SettingWindowsViewModel);
            vm.Root.PlayBox = rb.CommandParameter.ToString().AsInt();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            var vm = (this.DataContext as SettingWindowsViewModel);
            vm.Root = vm.optionService.AddOrUpdate(vm.Root);
            this.Close();
        }
    }
}
