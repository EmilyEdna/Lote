using Lote.Override;
using Lote.ViewModels;
using System.Windows;
using System.Windows.Controls;
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

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
