using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using System.Threading.Tasks;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(MyListView), typeof(MyLIstviewRenderer))]
namespace BDI3Mobile.Droid.CustomRenderer
{
    public class MyLIstviewRenderer : ListViewRenderer
    {
        public MyLIstviewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.SmoothScrollbarEnabled = true;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == MyListView.ItemsSourceProperty.PropertyName)
            {
                var adapter = Control.Adapter;
                Control.Adapter = null;
                Control.Adapter = adapter;
            }
        }
    }
}