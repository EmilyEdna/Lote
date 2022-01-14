using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using XExten.Advance.InternalFramework.Securities.Common;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace Lote.Common
{
    public class ContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (parameter == null)
                return string.Empty;

            var Flag = parameter.AsString().AsInt();
            if (Flag == 1)
                return value.AsString().IsNullOrEmpty() ? null : $"\t{value.AsString().Replace("　", "\n\t")}";
            else if (Flag == 2)
                return value.AsString().IsNullOrEmpty() ? null : $"\t{value.AsString().Replace("\r\n", "\n\t")}";
            else if (Flag == 3)
                return $"歌曲总数:{value}";
            else
                return string.Join(",", (value as List<string>));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ButtonContent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Flag = parameter.AsString().AsInt();
            if (Flag == 1)
            {
                var Value = value.AsString().AsSpan();
                if (Value.Length > 12)
                    return Value[..12].ToString() + "...";
                else
                    return value;
            }
            else {
                var Value = value.AsString().AsSpan();
                if (Value.Length > 10)
                    return Value[..10].ToString() + "...";
                else
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class NovelVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.AsString().IsNullOrEmpty() ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
