using BDI3Mobile.CustomRenderer;
using BDI3Mobile.UWP.Renderer;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MyImage), typeof(TooltipImageRenderer))]
namespace BDI3Mobile.UWP.Renderer
{
    public class TooltipImageRenderer : ImageRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Image> e)
        {
            base.OnElementChanged(e);

            if (Element == null)
                return;

            if (!string.IsNullOrEmpty(Element.ClassId))
            {
                if (Element.ClassId == "Edit")
                {
                    ToolTip toolTip = new ToolTip();
                    toolTip.Content = "Edit";
                    ToolTipService.SetToolTip(this, toolTip);
                }
                else if (Element.ClassId == "Add Record")
                {
                    ToolTip toolTip = new ToolTip();
                    toolTip.Content = "Add Record";
                    ToolTipService.SetToolTip(this, toolTip);
                }
            }
        }
    }
}
