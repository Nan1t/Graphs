using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Graphs {
    class PosConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return ((double)value) + 16;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }
    }
}
