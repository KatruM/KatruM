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
using BDI3Mobile.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(BorderlessEditorRenderer))]
namespace BDI3Mobile.Droid.CustomRenderer
{
    public class BorderlessEditorRenderer : EditorRenderer
    {
        public BorderlessEditorRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Control.Background = null;
            }
        }
    }
}