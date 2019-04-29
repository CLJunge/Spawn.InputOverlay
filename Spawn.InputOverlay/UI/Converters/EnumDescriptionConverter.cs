#region Using
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
#endregion

namespace Spawn.InputOverlay.UI.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        #region Convert
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object objRet = DependencyProperty.UnsetValue;

            if (value != null && value is Enum enumValue)
                objRet = GetDescription(enumValue);

            return objRet;
        }
        #endregion

        #region ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => throw new NotSupportedException();
        #endregion

        #region [STATIC] GetDescription
        public static string GetDescription(Enum enumValue)
        {
            string strRet = enumValue.ToString();

            Type type = enumValue.GetType();
            MemberInfo[] memInfo = type.GetMember(strRet);

            if (memInfo?.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs?.Length > 0)
                    strRet = ((DescriptionAttribute)attrs[0]).Description;
            }
            return strRet;
        }
        #endregion
    }
}