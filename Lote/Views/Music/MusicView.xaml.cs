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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XExten.Advance.LinqFramework;

namespace Lote.Views.Music
{
    /// <summary>
    /// MusicView.xaml 的交互逻辑
    /// </summary>
    public partial class MusicView : UserControl
    {
        public MusicView()
        {
            InitializeComponent();
        }
        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Tab = (sender as TabControl);
            if (Tab == null)
                return;
            var vm = (this.DataContext as MusicViewModel);
            vm.PageIndex = 1;
            Dispatcher.Invoke(() => vm.ShowSong(Tab.SelectedIndex + 1));

        }

        private void Sheets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tabs.SelectedIndex = 2;
        }

        private void LoteButton_Click(object sender, RoutedEventArgs e)
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
            if (vm.PageIndex > 1 && e.VerticalChange >=-48&&e.VerticalOffset==0)
            {
                Dispatcher.Invoke(() => vm.SongLoadMore(false));
                var source = (e.OriginalSource as HandyControl.Controls.ScrollViewer);
                source.ScrollToHome();
            }
            if (e.VerticalOffset >= 300&&e.VerticalChange.ToString().Contains("."))
            {
                Dispatcher.Invoke(() => vm.SongLoadMore(true));
                var source = (e.OriginalSource as HandyControl.Controls.ScrollViewer);
                source.ScrollToHome();
            }
        }
    }
}
