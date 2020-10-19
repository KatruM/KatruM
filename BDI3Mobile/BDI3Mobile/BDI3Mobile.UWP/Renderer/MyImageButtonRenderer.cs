using BDI3Mobile.UWP.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using BDI3Mobile.CustomRenderer;
using Windows.UI.Xaml;

[assembly: ExportRenderer(typeof(MyImageButton), typeof(MyImageButtonRenderer))]
namespace BDI3Mobile.UWP.Renderer
{
    public class MyImageButtonRenderer : ImageButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ImageButton> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.RequestedTheme = ElementTheme.Dark;
            }
        }
    }
}
