using BDI3Mobile.Common;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace BDI3Mobile.ValueConverter
{
    public partial class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isSelected = (bool)value;
            if (!isSelected)
            {
                return "#ffffff";
            }
            else
            {
                return "#2196F3";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class CheckboxFillColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isSelected = (bool)value;
            if (!isSelected)
            {
                return Color.Transparent;
            }
            else
            {
                return Colors.LightBlueColor;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public partial class CheckboxOutlineColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isSelected = (bool)value;
            if (!isSelected)
            {
                return Color.Gray;
            }
            else
            {
                return Colors.LightBlueColor;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class SyncImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                bool? syncState = value as bool?;

                if (syncState != null)
                {
                    if (syncState.Value) return ConvertToImageSource("iconASyncsa.png");
                    else return ConvertToImageSource("iconSynca.png");
                }
            }
            return ConvertToImageSource("ic_idle");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;        
        }

        private ImageSource ConvertToImageSource(string fileName)
        {
            return ImageSource.FromFile(fileName);
        }
    }
    public class DeleteImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                bool? syncState = value as bool?;

                if (syncState != null)
                {
                    if (syncState.Value) return ConvertToImageSource("icondeletesa.png");
                    else return ConvertToImageSource("icondeletesa.png");
                }
            }
            return ConvertToImageSource("ic_idle");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        private ImageSource ConvertToImageSource(string fileName)
        {
            return ImageSource.FromFile(fileName);
        }
    }

}
