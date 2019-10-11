using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;

namespace Graphs {
    class WeightPosConverter : IMultiValueConverter {

        private Line line;
        private bool isX = false;

        public WeightPosConverter(Line line, bool isX) {
            this.line = line;
            this.isX = isX;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue) {
                return DependencyProperty.UnsetValue;
            }

            if (isX) {
                return (line.X1 + line.X2) / 2;
            }

            return (line.Y1 + line.Y2) / 2;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        /*public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (isX) {
                return (line.X1 + line.X2) / 2;
            }
            return (line.Y1 + line.Y2) / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }*/
    }
}
