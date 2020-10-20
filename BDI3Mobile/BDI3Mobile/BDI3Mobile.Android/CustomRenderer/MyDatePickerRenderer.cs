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

[assembly: ExportRenderer(typeof(MyDatePicker), typeof(MyDatePickerRenderer))]
[assembly: ExportRenderer(typeof(MyDatePicker2), typeof(MyDatePickerRenderer2))]
namespace BDI3Mobile.Droid.CustomRenderer
{
    public class MyDatePickerRenderer : DatePickerRenderer, DatePickerDialog.IOnDateSetListener
    {
        public MyDatePickerRenderer(Context context) : base(context)
        {

        }
        
        public void OnDateSet(Android.Widget.DatePicker view, int year, int month, int dayOfMonth)
        {
            ((MyDatePicker)Element).SelectedDate = (view.DateTime.Month < 10 ? "0" + view.DateTime.Month : view.DateTime.Month + "") + "/" + (view.DateTime.Day < 10 ? "0" + view.DateTime.Day : view.DateTime.Day + "") + "/" + view.DateTime.Year;
            ((MyDatePicker)Element).Unfocus();
        }

        protected override DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
        {
            var picker = base.CreateDatePickerDialog(year, month, day);
            picker.SetOnDateSetListener(this);
            picker.CancelEvent += Picker_CancelEvent;
            return picker;
        }

        private void Picker_CancelEvent(object sender, EventArgs e)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);
        }
    }

    public class MyDatePickerRenderer2 : DatePickerRenderer, DatePickerDialog.IOnDateSetListener
    {
        public MyDatePickerRenderer2(Context context) : base(context)
        {

        }

        public void OnDateSet(Android.Widget.DatePicker view, int year, int month, int dayOfMonth)
        {
            ((MyDatePicker2)Element).SelectedDate = (view.DateTime.Month < 10 ? "0" + view.DateTime.Month : view.DateTime.Month + "") + "/" + (view.DateTime.Day < 10 ? "0" + view.DateTime.Day : view.DateTime.Day + "") + "/" + view.DateTime.Year;
            ((MyDatePicker2)Element).Unfocus();
        }

        protected override DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
        {
            var picker = base.CreateDatePickerDialog(year, month, day);
            picker.SetOnDateSetListener(this);
            picker.CancelEvent += Picker_CancelEvent;
            return picker;
        }
      

        private void Picker_CancelEvent(object sender, EventArgs e)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);
            if (Control == null || e.NewElement == null)
                return;

            this.Control.Click += OnPickerClick;
            this.Control.KeyListener = null;
           
        }
        
        void OnPickerClick(object sender, EventArgs e)
        {
            ShowDatePicker();
        }

        private void ShowDatePicker()
        {
            CreateDatePickerDialog(this.Element.Date.Year, this.Element.Date.Month - 1, this.Element.Date.Day);
        }
        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                this.Control.Click -= OnPickerClick;
            }

            base.Dispose(disposing);
        }

    }

}