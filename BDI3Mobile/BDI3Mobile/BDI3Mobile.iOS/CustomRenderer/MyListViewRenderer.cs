using System;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyListView), typeof(MyListViewRenderer))]
namespace BDI3Mobile.iOS.CustomRenderer
{
    public class MyListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SeparatorInset = UIEdgeInsets.Zero;
            }
        }
    }
}
