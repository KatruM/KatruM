using System;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.UWP.Renderer;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MyDatePicker), typeof(MyDatePickerRenderer))]
[assembly: ExportRenderer(typeof(MyDatePicker2), typeof(MyDatePickerRenderer2))]
namespace BDI3Mobile.UWP.Renderer
{
    class MyDatePickerRenderer : DatePickerRenderer
    {
        DatePickerFlyout flyout;
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                ((MyDatePicker)Element).ShowDatePicker = new Action(() =>
                {
                    if (((MyDatePicker)Element).ManualMinDate)
                    {
                        flyout.MinYear = DateTime.SpecifyKind(((MyDatePicker)Element).MinimumDate, DateTimeKind.Unspecified);
                    }
                    if (((MyDatePicker)Element).ManualMaxDate)
                    {
                        flyout.MaxYear = DateTime.SpecifyKind(((MyDatePicker)Element).MaximumDate, DateTimeKind.Unspecified);
                    }
                    flyout.DatePicked -= Flyout_DatePicked;
                    flyout.Date = Control.Date;
                    flyout.DatePicked += Flyout_DatePicked;
                    FlyoutBase.ShowAttachedFlyout(Control);
                }); 
                var minYear = DateTime.SpecifyKind(new DateTime(1919, 01, 01), DateTimeKind.Unspecified);
                if (((MyDatePicker)Element).ManualMinDate)
                {
                    minYear = DateTime.SpecifyKind(((MyDatePicker)Element).MinimumDate, DateTimeKind.Unspecified);
                }
                var maxYear = DateTime.SpecifyKind(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), DateTimeKind.Unspecified);
                if (((MyDatePicker)Element).ManualMaxDate)
                {
                    maxYear = DateTime.SpecifyKind(((MyDatePicker)Element).MaximumDate, DateTimeKind.Unspecified);
                }
                // TODO: Focus() doesn't open date picker popup on UWP, it's known issue 
                // on Xamarin.Forms and should be fixed in 2.5. Had to open it manually.
                flyout = new DatePickerFlyout() { Placement = FlyoutPlacementMode.Bottom, MaxYear = maxYear, MinYear = minYear };
                flyout.DatePicked += Flyout_DatePicked;
                FlyoutBase.SetAttachedFlyout(Control, flyout);
            }
        }

        private void Flyout_DatePicked(DatePickerFlyout sender, DatePickedEventArgs args)
        {
            if (args.NewDate.Date > Element.MaximumDate)
            {

            }
            else
            {
                Control.Date = args.NewDate;
                ((MyDatePicker)Element).SelectedDate = (Control.Date.Month < 10 ? "0" + Control.Date.Month : Control.Date.Month + "") + "/" + (Control.Date.Day < 10 ? "0" + Control.Date.Day : Control.Date.Day + "") + "/" + Control.Date.Year;

            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }

    class MyDatePickerRenderer2 : DatePickerRenderer
    {
        DatePickerFlyout flyout;
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                ((MyDatePicker2)Element).ShowDatePicker = new Action(() =>
                {
                    if (((MyDatePicker2)Element).ManualMinDate)
                    {
                        flyout.MinYear = DateTime.SpecifyKind(((MyDatePicker2)Element).MinimumDate, DateTimeKind.Unspecified);
                    }
                    if (((MyDatePicker2)Element).ManualMaxDate)
                    {
                        flyout.MaxYear = DateTime.SpecifyKind(((MyDatePicker2)Element).MaximumDate, DateTimeKind.Unspecified);
                    }

                    flyout.DatePicked -= Flyout_DatePicked;
                    flyout.Date = Control.Date;
                    flyout.DatePicked += Flyout_DatePicked;
                    FlyoutBase.ShowAttachedFlyout(Control);
                });
                var minYear = DateTime.SpecifyKind(new DateTime(1919, 01, 01), DateTimeKind.Unspecified);
                if (((MyDatePicker2)Element).ManualMinDate)
                {
                    minYear = DateTime.SpecifyKind(((MyDatePicker2)Element).MinimumDate, DateTimeKind.Unspecified);
                }
                var maxYear = DateTime.SpecifyKind(new DateTime(DateTime.MaxValue.Year, DateTime.MaxValue.Month, DateTime.MaxValue.Day), DateTimeKind.Unspecified);
                if (((MyDatePicker2)Element).ManualMaxDate)
                {
                    maxYear = DateTime.SpecifyKind(((MyDatePicker2)Element).MaximumDate, DateTimeKind.Unspecified);
                }
                // TODO: Focus() doesn't open date picker popup on UWP, it's known issue 
                // on Xamarin.Forms and should be fixed in 2.5. Had to open it manually.
                flyout = new DatePickerFlyout() { Placement = FlyoutPlacementMode.Bottom, MaxYear = maxYear, MinYear = minYear };
                flyout.DatePicked += Flyout_DatePicked;
                FlyoutBase.SetAttachedFlyout(Control, flyout);
            }
        }
        private void Flyout_DatePicked(DatePickerFlyout sender, DatePickedEventArgs args)
        {
            if (args.NewDate.Date > Element.MaximumDate)
            {

            }
            else
            {
                Control.Date = args.NewDate;
                ((MyDatePicker2)Element).SelectedDate = (Control.Date.Month < 10 ? "0" + Control.Date.Month : Control.Date.Month + "") + "/" + (Control.Date.Day < 10 ? "0" + Control.Date.Day : Control.Date.Day + "") + "/" + Control.Date.Year;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }
}
