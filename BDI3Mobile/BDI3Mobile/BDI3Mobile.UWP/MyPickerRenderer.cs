using BDI3Mobile.CustomRenderer;
using BDI3Mobile.UWP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MyPicker), typeof(MyPickerRenderer))]
namespace BDI3Mobile.UWP
{
    class MyPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // TODO: Focus() doesn't open date picker popup on UWP, it's known issue 
                // on Xamarin.Forms and should be fixed in 2.5. Had to open it manually.
                var flyout = new PickerFlyout() { Placement = FlyoutPlacementMode.Right };
                flyout.Confirmed += (s, args) =>
                {
                    //Control.Date = args.NewDate;

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
