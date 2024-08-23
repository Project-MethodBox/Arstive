﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Arstive.Display.Converter
{
    public class KeyConverter : IValueConverter
    {
        /// <summary>
        /// Convert key to string
        /// </summary>
        /// <param name="value">Key instance</param>
        /// <returns>String pattern of key</returns>
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture) =>
            new System.Windows.Input.KeyConverter().ConvertToString((Key)value)!;

        /// <summary>
        /// Convert string back to key
        /// </summary>
        /// <param name="value">String instance</param>
        /// <returns>Key pattern of string</returns>
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture) =>
            new System.Windows.Input.KeyConverter().ConvertFrom((string)value)!;
    }
}