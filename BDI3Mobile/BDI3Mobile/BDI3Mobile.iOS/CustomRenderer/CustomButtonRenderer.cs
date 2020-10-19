using System;
using System.ComponentModel;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]

namespace BDI3Mobile.iOS.CustomRenderer
{
    public class CustomButtonRenderer: ButtonRenderer
    {
         protected override void OnElementChanged(ElementChangedEventArgs<Button> args)
        {
            base.OnElementChanged(args);
            if (Control != null) SetColors();
            
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);
            if (args.PropertyName == nameof(Button.IsEnabled)) SetColors();
        }

        private void SetColors()
        {
            Control.SetTitleColor(Element.IsEnabled ? UIColor.White : UIColor.White, Element.IsEnabled ? UIControlState.Normal : UIControlState.Disabled);
            Control.BackgroundColor = Element.IsEnabled ? UIColor.FromRGB(9, 109, 169) : UIColor.Gray;
            
        }
    }
}
