using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.iOS.CustomRenderer;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyDatePicker), typeof(MyDatePickerRenderer))]
[assembly: ExportRenderer(typeof(MyDatePicker2), typeof(MyDatePickerRenderer2))]

namespace BDI3Mobile.iOS.CustomRenderer
{
    public class MyDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                if (Control != null)
                {
                    var toolbar = (UIToolbar)Control.InputAccessoryView;
                    var doneBtn = toolbar.Items[1];

                    doneBtn.Clicked -= DoneBtn_Clicked;
                }

            }

            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    var toolbar = (UIToolbar)Control.InputAccessoryView;
                    var doneBtn = toolbar.Items[1];

                    doneBtn.Clicked += DoneBtn_Clicked;
                }
            }
        }

        void DoneBtn_Clicked(object sender, EventArgs e)
        {
            ((MyDatePicker)Element).SelectedDate = Element.Date.GetDateTimeFormats()[3]; //Element.Date.ToShortDateString();
        }
    }
    public class MyDatePickerRenderer2 : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                if(Control != null)
                {
                    var toolbar = (UIToolbar)Control.InputAccessoryView;
                    var doneBtn = toolbar.Items[1];

                    doneBtn.Clicked -= DoneBtn_Clicked;
                }
                
            }

            if (e.NewElement != null)
            {
                if(Control != null)
                {
                    var toolbar = (UIToolbar)Control.InputAccessoryView;
                    var doneBtn = toolbar.Items[1];

                    doneBtn.Clicked += DoneBtn_Clicked;
                }
            }
        }

        void DoneBtn_Clicked(object sender, EventArgs e)
        {
            ((MyDatePicker2)Element).SelectedDate = Element.Date.GetDateTimeFormats()[3];
        }
    }

}