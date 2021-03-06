using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lote.Override
{
    public class LoteButton : Button
    {
        public Geometry Svg
        {
            get { return (Geometry)GetValue(SvgProperty); }
            set { SetValue(SvgProperty, value); }
        }

        public static readonly DependencyProperty SvgProperty =
            DependencyProperty.Register("Svg", typeof(Geometry), typeof(LoteButton), new PropertyMetadata(null));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(LoteButton), new PropertyMetadata(null));
    }
}
