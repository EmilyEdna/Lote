﻿using Lote.Override;
using Music.SDK.ViewModel.Response;
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
            Dispatcher.Invoke(() =>
            {
                vm.ShowSong(Tab.SelectedIndex + 1);
            });

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
            Dispatcher.Invoke(() =>
            {
                vm.ShowAlbum(id);
            });
        }
    }
}
