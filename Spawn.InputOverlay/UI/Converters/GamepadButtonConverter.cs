#region Using
using SharpDX.XInput;
using System;
using System.Globalization;
using System.Windows.Data;
#endregion

namespace Spawn.InputOverlay.UI.Converters
{
    public class GamepadButtonConverter : IValueConverter
    {
        #region Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strRet = value.ToString();

            if (strRet.Equals(GamepadButtonFlags.LeftThumb.ToString()))
                strRet = "Left Trigger";
            else if (strRet.Equals(GamepadButtonFlags.RightThumb.ToString()))
                strRet = "Right Trigger";
            else if (strRet.Equals(GamepadButtonFlags.LeftShoulder.ToString()))
                strRet = "Left Shoulder";
            else if (strRet.Equals(GamepadButtonFlags.RightShoulder.ToString()))
                strRet = "Right Shoulder";

            return strRet;
        }
        #endregion

        #region ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
        #endregion
    }
}