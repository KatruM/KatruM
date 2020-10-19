using BDI3Mobile.Models.Common;
using BDI3Mobile.Views.PopupViews;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace BDI3Mobile.Behaviors
{
    class NumericValidator : Behavior<Entry>
    {
        bool isPopUpOpen = false;
        bool isAdaptive = false;
        bool isAcademic = false;
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            entry.Focused += Entry_Focused;
            entry.Unfocused += Entry_Unfocused;

            base.OnAttachedTo(entry);
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            if (isAdaptive)
                ((Entry)sender).Text = "";
            else if (isAcademic)
                ((Entry)sender).Text = "";
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            if (isAdaptive)
                ((Entry)sender).Text = "";
            else if (isAcademic)
                ((Entry)sender).Text = "";
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            entry.Focused -= Entry_Focused;
            entry.Unfocused -= Entry_Unfocused;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(args.NewTextValue))
            {
				char[] charArray = (sender as Entry).Text.ToCharArray();
                Regex reg = null;
                reg = new System.Text.RegularExpressions.Regex("^[0-9][0-9]*$");
                bool isValid = args.NewTextValue.ToCharArray().All(x => char.IsDigit(x)); //Make sure all characters are numbers
                bool isValidInput = reg.IsMatch(((Entry)sender).Text);

                if ((sender as Entry).Text.Length == 2 && charArray[0] == '0')
                {
                    bool firstCharZero = charArray[0].Equals('0');
                    ((Entry)sender).Text = firstCharZero ? args.NewTextValue.Replace(args.NewTextValue, charArray[1].ToString()): args.NewTextValue;
                }
                else
                {
                    ((Entry)sender).Text = (isValid && isValidInput) ? args.NewTextValue : args.NewTextValue.Remove(args.NewTextValue.Length - 1);
                }

                if (isValid && isValidInput)
                {
                    Entry text = (Entry)sender;
                    TestSessionModel context = text.BindingContext as TestSessionModel;

                    string subDomain = null;
                    string parentDomainCode = null;
                    int input = 0, range = 0;

                    var entryText = ((Entry)sender).Text;
                    if (entryText != string.Empty)
                    {
                        input = Int32.Parse(entryText);
                        if (context != null)
                        {
                            subDomain = context.Domain.ToString();
                            parentDomainCode = context.ParentDomainCode.ToString();
                        }
                        if (subDomain == "Self-Care")
                        {
                            range = 74;
                            ((Entry)sender).Text = ValidateRange(range, input);
                        }
                        else if (subDomain == "Personal Responsibility" || subDomain == "Peer Interaction" || subDomain == "Perceptual Motor")
                        {
                            range = 48;

                            ((Entry)sender).Text = ValidateRange(range, input);
                        }
                        else if (subDomain == "Adult Interaction")
                        {
                            range = 54;
                            ((Entry)sender).Text = ValidateRange(range, input);
                        }
                        else if (subDomain == "Self-Concept and Social Role" || subDomain == "Reasoning and Academic Skills" || subDomain == "Perception and Concepts")
                        {
                            range = 66;
                            ((Entry)sender).Text = ValidateRange(range, input);
                        }
                        else if (subDomain == "Receptive Communication")
                        {
                            range = 68;
                            ((Entry)sender).Text = ValidateRange(range, input);
                        }
                        else if (subDomain == "Expressive Communication")
                        {
                            range = 82;
                            ((Entry)sender).Text = ValidateRange(range, input);
                        }
                        else if (subDomain == "Gross Motor")
                        {
                            range = 90;
                            var result = ValidateRange(range, input);
                            if (result != null)
                            {
                                isAcademic = false;
                                ((Entry)sender).Text = result.ToString();
                            }
                            else
                            {
                                isAcademic = true;
                                ((Entry)sender).Unfocus();
                                ((Entry)sender).Focus();
                            }
                        }
                        else if (subDomain == "Fluency")
                        {
                            range = 90;
                            if (Device.RuntimePlatform != Device.iOS)
                            {
                                var result = ValidateRange(range, input);
                                if (result != null)
                                {
                                    isAcademic = false;
                                    ((Entry)sender).Text = result.ToString();
                                }
                                else
                                {
                                    isAcademic = true;
                                    ((Entry)sender).Unfocus();
                                    ((Entry)sender).Focus();
                                }
                            }
                            else
                            {
                                ((Entry)sender).Text = ValidateRange(range, input);
                            }
                        }
                        else if (subDomain == "Fine Motor" || subDomain == "Attention and Memory")
                        {
                            range = 60;
                            ((Entry)sender).Text = ValidateRange(range, input);
                        }
                        else if (subDomain == "Social-Emotional" || subDomain == "Communication" || subDomain == "Motor" || subDomain == "Cognitive")
                        {
                            range = 40;
                            ((Entry)sender).Text = ValidateRange(range, input);
                        }
                        else if (subDomain == "Print Concepts" || subDomain == "Letter-Sound Correspondence")
                        {
                            range = 12;

                            if (Device.RuntimePlatform != Device.iOS)
                            {
                                var result = ValidateRange(range, input);
                                if (result != null)
                                {
                                    isAcademic = false;
                                    ((Entry)sender).Text = result.ToString();
                                }
                                else
                                {
                                    isAcademic = true;
                                    ((Entry)sender).Unfocus();
                                    ((Entry)sender).Focus();
                                }
                            }
                            else
                            {
                                ((Entry)sender).Text = ValidateRange(range, input);
                            }
                        }
                        else if
                            (subDomain == "Rhyming" || subDomain == "Syllables" || subDomain == "Onset Rime" || subDomain == "Phoneme Identification" || subDomain == "Phoneme Blending and Segmenting" || subDomain == "Phoneme Manipulation" || subDomain == "Early Decoding" ||
                            subDomain == "Sight Words" || subDomain == "Nonsense Words" || subDomain == "Long Vowel Patterns" || subDomain == "Inflectional Endings" ||
                            subDomain == "Geometry" || subDomain == "Operations and Algebraic Thinking" || subDomain == "Measurement and Data")
                        {
                            range = 10;
                            if (Device.RuntimePlatform != Device.iOS)
                            {

                                var result = ValidateRange(range, input);
                                if (result != null)
                                {
                                    isAcademic = false;
                                    ((Entry)sender).Text = result.ToString();
                                }
                                else
                                {
                                    isAcademic = true;
                                    ((Entry)sender).Unfocus();
                                    ((Entry)sender).Focus();
                                }
                            }
                            else
                            {
                                ((Entry)sender).Text = ValidateRange(range, input);
                            }
                        }
                        else if (subDomain == "Letter Identification")
                        {
                            range = 52;
                            if (Device.RuntimePlatform != Device.iOS)
                            {
                                var result = ValidateRange(range, input);
                                if (result != null)
                                {
                                    isAcademic = false;
                                    ((Entry)sender).Text = result.ToString();
                                }
                                else
                                {
                                    isAcademic = true;
                                    ((Entry)sender).Unfocus();
                                    ((Entry)sender).Focus();
                                }
                            }
                            else
                            {
                                ((Entry)sender).Text = ValidateRange(range, input);
                            }
                        }
                        else if (subDomain == "Listening Comprehension")
                        {
                            range = 14;
                            if (Device.RuntimePlatform != Device.iOS)
                            {
                                var result = ValidateRange(range, input);
                                if (result != null)
                                {
                                    isAcademic = false;
                                    ((Entry)sender).Text = result.ToString();
                                }
                                else
                                {
                                    isAcademic = true;
                                    ((Entry)sender).Unfocus();
                                    ((Entry)sender).Focus();
                                }
                            }
                            else
                            {
                                ((Entry)sender).Text = ValidateRange(range, input);
                            }
                        }
                        else if (subDomain == "Numbers, Counting, and Sets")
                        {
                            range = 19;
                            if (Device.RuntimePlatform != Device.iOS)
                            {
                                var result = ValidateRange(range, input);
                                if (result != null)
                                {
                                    isAcademic = false;
                                    ((Entry)sender).Text = result.ToString();
                                }
                                else
                                {
                                    isAcademic = true;
                                    ((Entry)sender).Unfocus();
                                    ((Entry)sender).Focus();
                                }
                            }
                            else
                            {
                                ((Entry)sender).Text = ValidateRange(range, input);
                            }
                        }
                        else if (parentDomainCode == "ADP")
                        {
                            range = 40;
                            var result = ValidateRange(range, input);
                            if (result != null)
                            {
                                isAdaptive = false;
                                ((Entry)sender).Text = result.ToString();
                            }
                            else
                            {
                                isAdaptive = true;
                                ((Entry)sender).Unfocus();
                                ((Entry)sender).Focus();
                            }
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }

        private string ValidateRange(int range, int input)
        {
            var errorMessage = "";
            if (input <= range)
            {
                return input.ToString();
            }
            else
            {
                errorMessage = "Please enter a score in the range of 0 to " + range + " for this subdomain.";
                var popup = new InvalidRawScorePopUp(new PopUpDetails() { Message = errorMessage });
                if (PopupNavigation.Instance.PopupStack.Count <= 1)
                {
                    PopupNavigation.Instance.PushAsync(popup);
                }
                return null;
            }
        }
    }
    public class TimerNumbericValidator : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += Entry_TextChanged;
            base.OnAttachedTo(entry);
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                char[] enteredText = e.NewTextValue.ToCharArray();
                char lastChar = enteredText[enteredText.Length - 1];
                if (!char.IsDigit(lastChar) && lastChar != ':')
                {
                    ((Entry)sender).Text = e.NewTextValue.Remove(e.NewTextValue.Length - 1);
                }
            }
            else if (e.NewTextValue == " ")
            {
                ((Entry)sender).Text = e.NewTextValue.Remove(e.NewTextValue.Length - 1);
            }

        }
        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= Entry_TextChanged;
            base.OnDetachingFrom(entry);
        }
    }

    public class NumbericDateValidator : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += Entry_TextChanged;
            base.OnAttachedTo(entry);
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                char[] enteredText = e.NewTextValue.ToCharArray();
                char lastChar = enteredText[enteredText.Length - 1];
                if (!char.IsDigit(lastChar) && lastChar != ':')
                {
                    ((Entry)sender).Text = e.NewTextValue.Remove(e.NewTextValue.Length - 1);
                }
            }
            else if (e.NewTextValue == " ")
            {
                ((Entry)sender).Text = e.NewTextValue.Remove(e.NewTextValue.Length - 1);
            }

        }
        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= Entry_TextChanged;
            base.OnDetachingFrom(entry);
        }
    }


}
