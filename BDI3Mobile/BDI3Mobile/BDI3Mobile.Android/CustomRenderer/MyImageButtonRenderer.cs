using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyImageButton), typeof(MyImageButtonRenderer))]
namespace BDI3Mobile.Droid.CustomRenderer
{
    public class MyImageButtonRenderer : ImageButtonRenderer
    {
        public MyImageButtonRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ImageButton> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Element.BackgroundColor = Color.White;
            }
        }
    }
}