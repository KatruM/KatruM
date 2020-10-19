using BDI3Mobile.CustomRenderer;
using BDI3Mobile.UWP.Renderer;
using Windows.UI.Xaml;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MyCheckBox), typeof(CheckboxViewRenderer))]
namespace BDI3Mobile.UWP.Renderer
{
    public class CheckboxViewRenderer : ViewRenderer<MyCheckBox, FrameworkElement>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<MyCheckBox> e)
        {
            base.OnElementChanged(e);
            if(e.NewElement != null)
            {
                if (Control == null)
                {
                    PointerEntered += Control_PointerEntered;
                    PointerExited += Control_PointerExited;
                }
                 ((MyCheckBox)Element).OutlineColor = Common.Colors.chekboxColor;
            }
            
        }       

        private void Control_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ((MyCheckBox)Element).OutlineColor = Common.Colors.chekboxColor;
            ((MyCheckBox)Element).IsChecked = !((MyCheckBox)Element).IsChecked;
            ((MyCheckBox)Element).IsChecked = !((MyCheckBox)Element).IsChecked;

        }

        private void Control_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ((MyCheckBox)Element).OutlineColor = Common.Colors.BlueColor;
            ((MyCheckBox)Element).IsChecked = !((MyCheckBox)Element).IsChecked;
            ((MyCheckBox)Element).IsChecked = !((MyCheckBox)Element).IsChecked;
            //MessagingCenter.Send<string>("", "MouseEntered");
        }

    }
}
