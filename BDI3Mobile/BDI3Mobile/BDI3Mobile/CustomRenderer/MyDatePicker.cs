using System;
using Xamarin.Forms;

namespace BDI3Mobile.CustomRenderer
{
    public class MyDatePicker : DatePicker
    {
        public bool ManualMinDate { get; set; }
        public bool ManualMaxDate { get; set; }
        public Action ShowDatePicker { get; set; }
        public static readonly BindableProperty IsShowPickerProperty =
            BindableProperty.Create(nameof(IsShowPicker), typeof(bool), typeof(MyDatePicker), false, BindingMode.TwoWay);

        public bool IsShowPicker
        {
            get { return (bool)GetValue(IsShowPickerProperty); }
            set
            {
                SetValue(IsShowPickerProperty, value);
            }
        }

        public static readonly BindableProperty CurrentDateProperty =
            BindableProperty.Create(nameof(SelectedDate), typeof(string), typeof(MyDatePicker), null, BindingMode.TwoWay);

        public string SelectedDate
        {
            get
            {
                var valueofPicker = (string)GetValue(CurrentDateProperty);
                if (!string.IsNullOrEmpty(valueofPicker) && valueofPicker ==  (DateTime.MinValue.Month + "/" + DateTime.MinValue.Day + "/" + DateTime.MinValue.Year))
                {
                    return "";
                }
                else
                {
                    return valueofPicker;
                }
            }
                    
            set
            {
                if (!string.IsNullOrEmpty(value) && value == (DateTime.MinValue.Month + "/" + DateTime.MinValue.Day + "/" + DateTime.MinValue.Year))
                {
                    SetValue(CurrentDateProperty, "");
                }
                else
                {
                    SetValue(CurrentDateProperty, value);
                }
            }
        }

        public MyDatePicker()
        {
            DateSelected += MyDatePicker_DateSelected;
        }

        private void MyDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
    }

    public class MyDatePicker2 : DatePicker
    {
        public bool ManualMinDate { get; set; }
        public bool ManualMaxDate { get; set; }
        public Action ShowDatePicker { get; set; }
        public static readonly BindableProperty IsShowPickerProperty =
            BindableProperty.Create(nameof(IsShowPicker), typeof(bool), typeof(MyDatePicker2), false, BindingMode.TwoWay);

        public bool IsShowPicker
        {
            get { return (bool)GetValue(IsShowPickerProperty); }
            set
            {
                SetValue(IsShowPickerProperty, value);
            }
        }

        public static readonly BindableProperty CurrentDateProperty =
            BindableProperty.Create(nameof(SelectedDate), typeof(string), typeof(MyDatePicker2), null, BindingMode.TwoWay);

        public string SelectedDate
        {
            get
            {
                var valueofPicker = (string)GetValue(CurrentDateProperty);
                if (!string.IsNullOrEmpty(valueofPicker) && valueofPicker == (DateTime.MinValue.Month + "/" + DateTime.MinValue.Day + "/" + DateTime.MinValue.Year))
                {
                    return "";
                }
                else
                {
                    return valueofPicker;
                }
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && value == (DateTime.MinValue.Month + "/" + DateTime.MinValue.Day + "/" + DateTime.MinValue.Year))
                {
                    SetValue(CurrentDateProperty, "");
                }
                else
                {
                    SetValue(CurrentDateProperty, value);
                }
            }
        }

        public MyDatePicker2()
        {
            DateSelected += MyDatePicker_DateSelected;
        }

        private void MyDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
    }

}

