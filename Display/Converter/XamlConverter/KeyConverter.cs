using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace Arstive.Display.Converter.XamlConverter
{
    public class KeyConverter : IValueConverter
    {
        /// <summary>
        /// Convert key to string
        /// </summary>
        /// <param name="value">Key instance</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>String pattern of key</returns>
        public object Convert(object? value, Type targetType,
            object? parameter, CultureInfo culture) =>
            new System.Windows.Input.KeyConverter().ConvertToString((Key)value!)!;

        /// <summary>
        /// Convert string back to key
        /// </summary>
        /// <param name="value">String instance</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>Key pattern of string</returns>
        public object ConvertBack(object? value, Type targetType,
            object? parameter, CultureInfo culture) =>
            new System.Windows.Input.KeyConverter().ConvertFrom((string)value!)!;
    }
}
