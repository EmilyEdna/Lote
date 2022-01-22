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

namespace Lote.CommonWindow
{
    /// <summary>
    /// SettingWindows.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindows : LoteWindow
    {
        public SettingWindows()
        {
            InitializeComponent();
            BeginStoryboard((Storyboard)FindResource("OpenWindow"));
        }

        private void MoveWIndow(object sender, MouseButtonEventArgs e)
        {

        }

        private void MoveWIndow(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
