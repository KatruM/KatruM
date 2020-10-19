using Xamarin.Forms;

namespace BDI3Mobile.Behaviors
{
    public class EntryLengthValidatorBehaviour : Behavior<Entry>
    {
        public int MaxLength { get; set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            if(entry != null && entry.Text != null && entry.Text !="")
            {
                if (entry.Text.Length > this.MaxLength)
                {
                    string entryText = entry.Text;

                    entryText = entryText.Remove(entryText.Length - 1); // remove last char

                    entry.Text = entryText; 
                }
            }
            // if Entry text is longer then valid length
            
        }
    }
}
