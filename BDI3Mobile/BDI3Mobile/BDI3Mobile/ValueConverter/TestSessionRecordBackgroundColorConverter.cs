using System;
using System.Globalization;
using Xamarin.Forms;

namespace BDI3Mobile.ValueConverter
{
    public class TestSessionRecordBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (bool)value ? Color.White : Color.FromHex("#f7f7f8");

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
