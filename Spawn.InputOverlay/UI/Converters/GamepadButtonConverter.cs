#region Using
using Spawn.InputOverlay.Input;
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

            if (strRet.Equals(GamepadButtons.LeftTrigger.ToString()))
                strRet = "Left Trigger";
            else if (strRet.Equals(GamepadButtons.RightTrigger.ToString()))
                strRet = "Right Trigger";
            else if (strRet.Equals(GamepadButtons.LeftShoulder.ToString()))
                strRet = "Left Shoulder";
            else if (strRet.Equals(GamepadButtons.RightShoulder.ToString()))
                strRet = "Right Shoulder";

            return strRet;
        }
        #endregion

        #region ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
        #endregion
    }
}