using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Gui.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        private Visibility GetVisibility(bool isVisible, bool inverse)
        {
            if (isVisible)
            {
                return inverse ? Visibility.Collapsed : Visibility.Visible;
            }

            return inverse ? Visibility.Visible : Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetVisibility(value is bool a ? a : false, parameter is bool b ? b : false);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
