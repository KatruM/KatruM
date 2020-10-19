using BDI3Mobile.CustomRenderer;
using Windows.UI.Xaml;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomButtonUWP), typeof(BDI3Mobile.UWP.Renderer.CustomButtonRenderer))]
namespace BDI3Mobile.UWP.Renderer
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var buttonStyle = Application.Current.Resources["ButtonStyle"] as Style;
                Control.Style = buttonStyle;
            }
        }
    }
}
