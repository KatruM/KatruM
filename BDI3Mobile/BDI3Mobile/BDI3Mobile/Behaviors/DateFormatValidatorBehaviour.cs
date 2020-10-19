using System;
using Xamarin.Forms;

namespace BDI3Mobile.Behaviors
{
    public class DateFormatValidatorBehaviour :Behavior<Entry>
    {
        public DateFormatValidatorBehaviour()
        {
        }

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);
            (bindable as Entry).TextChanged += DateFormatValidatorBehaviour_TextChanged;
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            base.OnDetachingFrom(bindable);
            (bindable as Entry).TextChanged -= DateFormatValidatorBehaviour_TextChanged;
        }

        private void DateFormatValidatorBehaviour_TextChanged(object sender, TextChangedEventArgs e)
        {
            char lastChar = char.MinValue;
            string entryText = (sender as Entry).Text;
            if (entryText.Length > 0)
                lastChar = entryText[entryText.Length - 1];

            var defaultDate = default(DateTime);
            var defaultDateStr = defaultDate.Month + "/" + defaultDate.Day + "/" + defaultDate.Year;
            if (entryText == defaultDateStr)
            {
                entryText = "";
                (sender as Entry).Text = entryText;
            }

            if (entryText != "mm/dd/yyyy" && entryText.Length > 0 && (!char.IsDigit(lastChar) && lastChar != '/'))
            {
                entryText = entryText.Remove(entryText.Length - 1);
                (sender as Entry).Text = entryText;
            }
        }

       
    }
}
