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

[assembly: ExportRenderer(typeof(MyFrame), typeof(MyFrameRenderer))]
namespace BDI3Mobile.Droid.CustomRenderer
{
    class MyFrameRenderer : FrameRenderer
    {
        public MyFrameRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && e.OldElement == null)
            {
                //this.SetBackgroundResource(Resource.Drawable.FrameRenderValue);
                //GradientDrawable drawable = (GradientDrawable)this.Background;
                //drawable.SetColor(Android.Graphics.Color.ParseColor("#F0F0F0"));

                //e.NewElement.BorderColor    = SolidBrush
            }
        }
        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);

            if (Element == null || Element.BorderColor.A <= 0)
            {
                return;
            }
            using (var paint = new Paint
            {
                AntiAlias = true
            })
            using (var path = new Path())
            using (Path.Direction direction = Path.Direction.Cw)
            using (Paint.Style style = Paint.Style.Stroke)
            using (var rect = new RectF(0, 0, canvas.Width, canvas.Height))
            {
                var raduis = Android.App.Application.Context.ToPixels(Element.CornerRadius);
                path.AddRoundRect(rect, raduis, raduis, direction);
                //paint.StrokeWidth = Context.Resources.DisplayMetrics.Density * 2;  
                paint.SetStyle(style);
                paint.Color = Element.BorderColor.ToAndroid();
                canvas.DrawPath(path, paint);
            }
        }
    }
}