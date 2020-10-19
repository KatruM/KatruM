using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.UWP;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.UWP;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using System.ComponentModel;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(MyDatePicker), typeof(MyDatePickerRenderer))]
namespace BDI3Mobile.UWP
{
    class MyDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // TODO: Focus() doesn't open date picker popup on UWP, it's known issue 
                // on Xamarin.Forms and should be fixed in 2.5. Had to open it manually.
                var flyout = new DatePickerFlyout() { Placement = FlyoutPlacementMode.Top }; 
                flyout.DatePicked += (s, args) =>
                {
                    Control.Date = args.NewDate;

                };
                FlyoutBase.SetAttachedFlyout(Control, flyout);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == VisualElement.IsFocusedProperty.PropertyName)
            {   
                if (Element.IsFocused)
                {
                    FlyoutBase.ShowAttachedFlyout(Control);
                }
            }
        }
    }
}
