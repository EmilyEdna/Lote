using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Lote.Override
{
    public class LoteComboBoxItem: ComboBoxItem
    {

        public int SeleteType
        {
            get { return (int)GetValue(SeleteTypeProperty); }
            set { SetValue(SeleteTypeProperty, value); }
        }

        public static readonly DependencyProperty SeleteTypeProperty =
            DependencyProperty.Register("SeleteType", typeof(int), typeof(LoteComboBoxItem), new PropertyMetadata(0));
    }
}
