#region Using
using System;
using System.Globalization;
using System.Windows.Data;
#endregion

namespace Spawn.InputOverlay.UI.Converters
{
    public class FloatConverter : IValueConverter
    {
        #region Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strRet = value.ToString();

            if (value is float fValue)
                strRet = Math.Round(fValue, 3).ToString(CultureInfo.InvariantCulture);

            return strRet;
        }
        #endregion

        #region ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float fRet = 0f;

            if (targetType.Equals(typeof(float)))
                fRet = System.Convert.ToSingle(value);

            return fRet;
        }
        #endregion
    }
}