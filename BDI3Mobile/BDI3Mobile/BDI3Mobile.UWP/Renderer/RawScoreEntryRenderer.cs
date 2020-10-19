using BDI3Mobile.CustomRenderer;
using BDI3Mobile.UWP;
using BDI3Mobile.Views.PopupViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MyEntry), typeof(RawScoreEntryRenderer))]
namespace BDI3Mobile.UWP
{
    class RawScoreEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.TextAlignment = Windows.UI.Xaml.TextAlignment.Center;
                Control.BorderThickness = new Windows.UI.Xaml.Thickness(1);
                Control.CornerRadius = new Windows.UI.Xaml.CornerRadius(4.0);
                Control.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 191, 192, 192));

                e.NewElement.Focused += (sender, evt) =>
                {
                    Control.BorderThickness = new Windows.UI.Xaml.Thickness(2);
                };

                e.NewElement.Unfocused += (sender, evt) =>
                {
                    Control.BorderThickness = new Windows.UI.Xaml.Thickness(1);
                };
            }
        }      
    }
}

