#region Using
using System;
using System.Globalization;
using System.Windows.Data;
#endregion

namespace Spawn.InputOverlay.UI.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        #region Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }
        #endregion

        #region ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
        #endregion
    }
}