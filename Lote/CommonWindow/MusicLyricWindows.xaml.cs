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
    /// MusicLyricWindows.xaml 的交互逻辑
    /// </summary>
    public partial class MusicLyricWindows : Window
    {
        public MusicLyricWindows()
        {
            InitializeComponent();
        }

        private void LyricMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
