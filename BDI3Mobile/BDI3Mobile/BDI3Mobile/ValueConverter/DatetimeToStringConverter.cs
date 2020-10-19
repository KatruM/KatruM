using BDI3Mobile.Common;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace BDI3Mobile.ValueConverter
{
    public class DatetimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is DateTime)
            {
                var convertedValue = (System.Convert.ToDateTime(value)).ToString("MM/dd/yyyy");
                if (convertedValue == DateTime.MinValue.ToString("MM/dd/yyyy") || convertedValue == new DateTime(1900,1,1).ToString("MM/dd/yyyy"))
                {
                    return null;
                }
                else
                {
                    return convertedValue;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valueAsString = System.Convert.ToBoolean(value);
            return valueAsString ? Colors.RubicPointSelectionBckgrd : Colors.RubicPointDefaultBckgrd;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BoolToItemSelectionBckgrdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valueAsString = System.Convert.ToBoolean(value);
            return valueAsString ? Colors.WhiteColor : Colors.GreyedItemBgColor;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BoolToItemSelectionTabConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valueAsString = System.Convert.ToBoolean(value);
            return valueAsString ? Color.DarkCyan : Colors.GreyedItemBgColor;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BoolToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valueAsString = System.Convert.ToBoolean(value);
            return valueAsString ? new Thickness(3) : new Thickness(0);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BoolToFontAttributeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valueAsString = System.Convert.ToBoolean(value);
            return valueAsString ? FontAttributes.Bold : FontAttributes.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            var convertedValue = value as string;
            return !string.IsNullOrEmpty(convertedValue) && convertedValue.Length > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ReverseVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            return !(System.Convert.ToBoolean(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valueAsString = System.Convert.ToBoolean(value);
            return valueAsString ? Color.Red : Colors.BorderColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class SyncConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                bool? syncState = value as bool?;

                if (syncState != null)
                {
                    if (syncState.Value) return ConvertToImageSource("BDI3Mobile.CollpsedGlyph.png");
                    else return ConvertToImageSource("BDI3Mobile.OpenGlyph.png");
                }
            }
            return ConvertToImageSource("ic_idle");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private ImageSource ConvertToImageSource(string fileName)
        {
            return ImageSource.FromFile(fileName);
        }
    }

    public class TextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valueAsString = System.Convert.ToBoolean(value);
            return valueAsString ? Colors.BlackColor : Colors.MediumGrayColor;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
