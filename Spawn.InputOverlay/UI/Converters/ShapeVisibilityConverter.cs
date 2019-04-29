#region Using
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
#endregion

namespace Spawn.InputOverlay.UI.Converters
{
    public class ShapeVisibilityConverter : IMultiValueConverter
    {
        #region Convert
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility retVal = Visibility.Collapsed;

            if (values.Length == 2
                && values[0] is OverlayShape selectedShape && values[1] is OverlayShape currentShape
                && selectedShape == currentShape)
            {
                retVal = Visibility.Visible;
            }

            return retVal;
        }
        #endregion

        #region ConvertBack
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
        #endregion
    }
}