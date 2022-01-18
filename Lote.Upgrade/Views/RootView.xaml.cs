using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Lote.Upgrade.Views
{
    public partial class RootView : Window
    {
        public RootView()
        {
            InitializeComponent();
         
        }

        private void ColorZoneMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpgradeClick(object sender, RoutedEventArgs e)
        {
            GetUpgradeFile();
        }
    }
}
