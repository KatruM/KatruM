using BDI3Mobile.CustomRenderer;
using BDI3Mobile.UWP.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace BDI3Mobile.UWP.Renderer
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.BorderThickness = new Windows.UI.Xaml.Thickness(0);
                Control.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            }
        }
    }
}
