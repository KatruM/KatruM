using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MySwitch), typeof(MySwitchRenderer))]
namespace BDI3Mobile.Droid.CustomRenderer
{
    public class MySwitchRenderer : SwitchRenderer
    {
        private Android.Graphics.Color greyColor = new Android.Graphics.Color(215, 218, 220);
        private Android.Graphics.Color greenColor = new Android.Graphics.Color(32, 156, 68);

        protected override void Dispose(bool disposing)
        {
            this.Control.CheckedChange -= this.OnCheckedChange;
            base.Dispose(disposing);
        }

        public MySwitchRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Switch> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null)
            {
                if (this.Control.Checked)
                {
                    this.Control.ThumbDrawable.SetColorFilter(greyColor, PorterDuff.Mode.SrcAtop);
                    this.Control.TrackDrawable.SetColorFilter(Android.Graphics.Color.Rgb(76, 217, 100), PorterDuff.Mode.Screen);
                }
                else
                {
                    this.Control.ThumbDrawable.SetColorFilter(greyColor, PorterDuff.Mode.SrcAtop);
                    this.Control.TrackDrawable.SetColorFilter(greyColor, PorterDuff.Mode.SrcAtop);
                }

                this.Control.CheckedChange += this.OnCheckedChange;
            }
        }

        private void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (this.Control.Checked)
            {
                this.Control.ThumbDrawable.SetColorFilter(greyColor, PorterDuff.Mode.SrcAtop);
                this.Control.TrackDrawable.SetColorFilter(Android.Graphics.Color.Rgb(76,217,100), PorterDuff.Mode.Screen);
            }
            else
            {
                this.Control.ThumbDrawable.SetColorFilter(greyColor, PorterDuff.Mode.SrcAtop);
                this.Control.TrackDrawable.SetColorFilter(greyColor, PorterDuff.Mode.SrcAtop);
            }
        }

    }
}