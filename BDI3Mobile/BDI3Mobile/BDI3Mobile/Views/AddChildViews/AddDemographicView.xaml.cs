using System;
using System.Threading.Tasks;
using BDI3Mobile.Common;
using BDI3Mobile.IServices;
using BDI3Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDI3Mobile.Views.AddChildViews
{
    public partial class AddDemographicView
    {
        public Grid Grid;
        public Action SaveChildAction { get; set; }

        WeakReference _currentField;
        public AddDemographicView()
        {
            InitializeComponent();
            Grid = MainGrid;
            EligibilityIFS.MaximumDate = DateTime.Now;
            EligiblityIEP.MaximumDate = DateTime.Now;
            this.BindingContextChanged -= AddDemographicView_BindingContextChanged;
            this.BindingContextChanged += AddDemographicView_BindingContextChanged;

            IFSPInitialDateEntry.TextChanged += IFSPInitialDateEntry_TextChanged;
            IFSPInitialDateEntry.Focused += IFSPInitialDateEntry_Focused;
            IFSPInitialDateEntry.Unfocused += IFSPInitialDateEntry_Unfocused;
            IFSPExitDateEntry.TextChanged += IFSPExitDateEntry_TextChanged;
            IFSPExitDateEntry.Focused += IFSPExitDateEntry_Focused;
            IFSPExitDateEntry.Unfocused += IFSPExitDateEntry_Unfocused;

            IEPInitialDateEntry.TextChanged += IEPInitialDateEntry_TextChanged;
            IEPInitialDateEntry.Focused += IEPInitialDateEntry_Focused;
            IEPInitialDateEntry.Unfocused += IEPInitialDateEntry_Unfocused;
            IEPExitDateEntry.TextChanged += IEPExitDateEntry_TextChanged;
            IEPExitDateEntry.Focused += IEPExitDateEntry_Focused;
            IEPExitDateEntry.Unfocused += IEPExitDateEntry_Unfocused;

            MessagingCenter.Subscribe<string[]>(this, "EditData", (values) =>
            {
                IFSPInitialDateEntry.TextChanged -= IFSPInitialDateEntry_TextChanged;
                IFSPInitialDateEntry.Focused -= IFSPInitialDateEntry_Focused;
                IFSPInitialDateEntry.Unfocused -= IFSPInitialDateEntry_Unfocused;
                IFSPExitDateEntry.TextChanged -= IFSPExitDateEntry_TextChanged;
                IFSPExitDateEntry.Focused -= IFSPExitDateEntry_Focused;
                IFSPExitDateEntry.Unfocused -= IFSPExitDateEntry_Unfocused;

                IEPInitialDateEntry.TextChanged -= IEPInitialDateEntry_TextChanged;
                IEPInitialDateEntry.Focused -= IEPInitialDateEntry_Focused;
                IEPInitialDateEntry.Unfocused -= IEPInitialDateEntry_Unfocused;
                IEPExitDateEntry.TextChanged -= IEPExitDateEntry_TextChanged;
                IEPExitDateEntry.Focused -= IEPExitDateEntry_Focused;
                IEPExitDateEntry.Unfocused -= IEPExitDateEntry_Unfocused;



                IFSPInitialDateEntry.TextChanged += IFSPInitialDateEntry_TextChanged;
                IFSPInitialDateEntry.Focused += IFSPInitialDateEntry_Focused;
                IFSPInitialDateEntry.Unfocused += IFSPInitialDateEntry_Unfocused;
                IFSPExitDateEntry.TextChanged += IFSPExitDateEntry_TextChanged;
                IFSPExitDateEntry.Focused += IFSPExitDateEntry_Focused;
                IFSPExitDateEntry.Unfocused += IFSPExitDateEntry_Unfocused;

                IEPInitialDateEntry.TextChanged += IEPInitialDateEntry_TextChanged;
                IEPInitialDateEntry.Focused += IEPInitialDateEntry_Focused;
                IEPInitialDateEntry.Unfocused += IEPInitialDateEntry_Unfocused;
                IEPExitDateEntry.TextChanged += IEPExitDateEntry_TextChanged;
                IEPExitDateEntry.Focused += IEPExitDateEntry_Focused;
                IEPExitDateEntry.Unfocused += IEPExitDateEntry_Unfocused;

                IEPInitialDateEntry.Text = values[2];
                IEPExitDateEntry.Text = values[3];
                IFSPInitialDateEntry.Text = values[4];
                IFSPExitDateEntry.Text = values[5];
            });

            MessagingCenter.Subscribe<string>(this, "ClearErrorText1", (arg1) =>
            {
                IFSPInitialDateEntryStackLayout.BackgroundColor = IFSPInitialDateEntry.BackgroundColor = Color.White;
                IFSPInitialDateEntry.Text = "";
                if (((AddChildViewModel)this.BindingContext) != null)
                    ((AddChildViewModel)this.BindingContext).IFSPinitialdateofEligibility = null;
                IFSPInitialDateEntry.TextColor = Colors.LightGrayColor;
                IFSPInitialDateEntry.FontAttributes = FontAttributes.None;
                if (((AddChildViewModel)this.BindingContext) != null)
                    ((AddChildViewModel)this.BindingContext).IsDOBEmpty = false;
                IFSPInitialDateFrame.BorderColor = Color.FromHex("#898D8D");
                IFSPInitialDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                IFSPInitialDateImageFrame.BorderColor = Color.FromHex("#898D8D");

                IFSPExitDateEntryStackLayout.BackgroundColor = IFSPExitDateEntry.BackgroundColor = Color.White;
                IFSPExitDateEntry.Text = "";
                if (((AddChildViewModel)this.BindingContext) != null)
                    ((AddChildViewModel)this.BindingContext).IFSPExitdate = null;
                IFSPExitDateEntry.TextColor = Colors.LightGrayColor;
                IFSPExitDateEntry.FontAttributes = FontAttributes.None;
                if (((AddChildViewModel)this.BindingContext) != null)
                    ((AddChildViewModel)this.BindingContext).IsDOBEmpty = false;
                IFSPExitDateFrame.BorderColor = Color.FromHex("#898D8D");
                IFSPExitDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                IFSPExitDateImageFrame.BorderColor = Color.FromHex("#898D8D");
            });
            MessagingCenter.Subscribe<string>(this, "ClearErrorText2", (arg1) =>
            {
                IEPInitialDateEntryStackLayout.BackgroundColor = IEPInitialDateEntry.BackgroundColor = Color.White;
                IEPInitialDateEntry.Text = "";
                if (((AddChildViewModel)this.BindingContext) != null)
                    ((AddChildViewModel)this.BindingContext).IEPinitialdateofEligibility = null;
                IEPInitialDateEntry.TextColor = Colors.LightGrayColor;
                IEPInitialDateEntry.FontAttributes = FontAttributes.None;
                if (((AddChildViewModel)this.BindingContext) != null)
                    ((AddChildViewModel)this.BindingContext).IsDOBEmpty = false;
                IEPInitialDateFrame.BorderColor = Color.FromHex("#898D8D");
                IEPInitialDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                IEPInitialDateImageFrame.BorderColor = Color.FromHex("#898D8D");

                IEPExitDateEntryStackLayout.BackgroundColor = IEPExitDateEntry.BackgroundColor = Color.White;
                IEPExitDateEntry.Text = "";
                if (((AddChildViewModel)this.BindingContext) != null)
                    ((AddChildViewModel)this.BindingContext).IEPExitdate = null;
                IEPExitDateEntry.TextColor = Colors.LightGrayColor;
                IEPExitDateEntry.FontAttributes = FontAttributes.None;
                if (((AddChildViewModel)this.BindingContext) != null)
                    ((AddChildViewModel)this.BindingContext).IsDOBEmpty = false;
                IEPExitDateFrame.BorderColor = Color.FromHex("#898D8D");
                IEPExitDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                IEPExitDateImageFrame.BorderColor = Color.FromHex("#898D8D");
            });

            MessagingCenter.Subscribe<String, bool>(this, "Tab", (text, value) =>
            {
                (_currentField?.Target as Entry)?.Unfocus();
                IEPExitDateEntry.IsTabStop = value;
                IEPInitialDateEntry.IsTabStop = value;
                IFSPExitDateEntry.IsTabStop = value;
                IFSPInitialDateEntry.IsTabStop = value;
            });
        }

        private void IEPExitDateEntry_Unfocused(object sender, FocusEventArgs e)
        {
            IEPExitDateEntry_TextChanged(sender, null);
            var entryText = (sender as Entry)?.Text;
            if (!string.IsNullOrEmpty(entryText))
            {
                if (this.BindingContext == null)
                {
                    if (!string.IsNullOrEmpty(entryText) && entryText == "01/01/0001")
                    {
                        (sender as Entry).Text = "";
                    }
                    return;
                }
                if (!string.IsNullOrEmpty(entryText) && (entryText.Trim().Length == 10))
                {
                    return;
                }
                else
                {
                    if (this.BindingContext != null)
                    {
                        ((AddChildViewModel)BindingContext).IEPExitDateIsValid = true;
                        ((AddChildViewModel)BindingContext).iEPexitdate = null;
                    }
                    IEPExitDateEntryStackLayout.BackgroundColor = IEPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                    IEPExitDateEntry.Text = "";
                    IEPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                    IEPExitDateEntry.FontAttributes = FontAttributes.Bold;
                    IEPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                    IEPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    IEPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                }
            }
        }

        private void IEPExitDateEntry_Focused(object sender, FocusEventArgs e)
        {
            ((AddChildViewModel)BindingContext).IEPExitDateIsValid = false;
            IEPExitDateEntryStackLayout.BackgroundColor = IEPExitDateEntry.BackgroundColor = Color.White;
            IEPExitDateEntry.TextColor = Colors.LightGrayColor;
            IEPExitDateEntry.FontAttributes = FontAttributes.None;
            IEPExitDateFrame.BorderColor = Color.FromHex("#898D8D");
            IEPExitDateImageFrame.BackgroundColor = Color.FromHex("#147cbd");
            IEPExitDateImageFrame.BorderColor = Color.FromHex("#898D8D");

        }
        private void IEPExitDateEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentField = new WeakReference(sender);
            var entryText = (sender as Entry)?.Text;
            if (this.BindingContext == null)
            {
                if (!string.IsNullOrEmpty(entryText) && (entryText == "01/01/0001" || entryText == "1/1/1"))
                {
                    (sender as Entry).Text = "";
                }
                return;
            }
            if (!string.IsNullOrEmpty(entryText) && entryText.Trim().Length == 10)
            {
                var splitedText = entryText.Split('/');
                if (splitedText != null && splitedText.Length != 3)
                {
                    if (this.BindingContext != null)
                    {
                        ((AddChildViewModel)BindingContext).IEPExitDateIsValid = true;
                        ((AddChildViewModel)BindingContext).iEPexitdate = null;
                    }
                    IEPExitDateEntryStackLayout.BackgroundColor = IEPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                    IEPExitDateEntry.Text = "";
                    IEPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                    IEPExitDateEntry.FontAttributes = FontAttributes.Bold;
                    IEPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                    IEPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    IEPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                }
                else if (splitedText != null && splitedText.Length == 3)
                {
                    try
                    {
                        if (splitedText[0].Length == 2 && splitedText[1].Length == 2 && splitedText[2].Length == 4)
                        {

                            var year = Convert.ToInt32(splitedText[2]);
                            var month = Convert.ToInt32(splitedText[0]);
                            var day = Convert.ToInt32(splitedText[1]);

                            var date = new DateTime(year, month, day);
                            if (year <= 1918 || year > DateTime.MaxValue.Year || year == 1)
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IEPExitDateIsValid = true;
                                    ((AddChildViewModel)BindingContext).iEPexitdate = null;
                                }
                                IEPExitDateEntryStackLayout.BackgroundColor = IEPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                                IEPExitDateEntry.Text = "";
                                IEPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                                IEPExitDateEntry.FontAttributes = FontAttributes.Bold;
                                IEPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                                IEPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                                IEPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                            }
                            else
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IEPExitDateIsValid = false;
                                    ((AddChildViewModel)BindingContext).iEPexitdate = new DateTime(year, month, day);
                                    var month1 = ((AddChildViewModel)BindingContext).IEPExitdate.Value.Month < 10 ? "0" + ((AddChildViewModel)BindingContext).IEPExitdate.Value.Month : ((AddChildViewModel)BindingContext).IEPExitdate.Value.Month + "";
                                    var day1 = ((AddChildViewModel)BindingContext).IEPExitdate.Value.Day < 10 ? "0" + ((AddChildViewModel)BindingContext).IEPExitdate.Value.Day : ((AddChildViewModel)BindingContext).IEPExitdate.Value.Day + "";
                                    var year1 = ((AddChildViewModel)BindingContext).IEPExitdate.Value.Year;
                                    IEPExitDateEntry.Text = month1 + "/" + day1 + "/" + year1;

                                    IEPExitDateEntryStackLayout.BackgroundColor = IEPExitDateEntry.BackgroundColor = Color.White;
                                    IEPExitDateEntry.TextColor = Colors.LightGrayColor;
                                    IEPExitDateEntry.FontAttributes = FontAttributes.None;
                                    IEPExitDateFrame.BorderColor = Color.FromHex("#898D8D");
                                    IEPExitDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                                    IEPExitDateImageFrame.BorderColor = Color.FromHex("#898D8D");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (this.BindingContext != null)
                        {
                            ((AddChildViewModel)BindingContext).IEPExitDateIsValid = true;
                            ((AddChildViewModel)BindingContext).iEPexitdate = null;
                        }
                        IEPExitDateEntryStackLayout.BackgroundColor = IEPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                        IEPExitDateEntry.Text = "";
                        IEPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                        IEPExitDateEntry.FontAttributes = FontAttributes.Bold;
                        IEPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                        IEPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                        IEPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                    }
                    }
                
            }
            else if (!string.IsNullOrEmpty(entryText) && entryText.Trim().Length == 8)
            {
                var splitedText = entryText.Split('/');
                if (splitedText != null && splitedText.Length != 3)
                {
                    if (this.BindingContext != null)
                    {
                        ((AddChildViewModel)BindingContext).IEPExitDateIsValid = true;
                        ((AddChildViewModel)BindingContext).iEPexitdate = null;
                    }
                    IEPExitDateEntryStackLayout.BackgroundColor = IEPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                    IEPExitDateEntry.Text = "";
                    IEPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                    IEPExitDateEntry.FontAttributes = FontAttributes.Bold;
                    IEPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                    IEPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    IEPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                }
                else if (splitedText != null && splitedText.Length == 3)
                {
                    try
                    {
                        if (splitedText[0].Length == 1 && splitedText[1].Length == 1 && splitedText[2].Length == 4)
                        {

                            var year = Convert.ToInt32(splitedText[2]);
                            var month = Convert.ToInt32(splitedText[0]);
                            var day = Convert.ToInt32(splitedText[1]);

                            var date = new DateTime(year, month, day);
                            if (year <= 1918 || year > DateTime.MaxValue.Year || year == 1)
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IEPExitDateIsValid = true;
                                    ((AddChildViewModel)BindingContext).iEPexitdate = null;
                                }
                                IEPExitDateEntryStackLayout.BackgroundColor = IEPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                                IEPExitDateEntry.Text = "";
                                IEPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                                IEPExitDateEntry.FontAttributes = FontAttributes.Bold;
                                IEPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                                IEPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                                IEPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                            }
                            else
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IEPExitDateIsValid = false;
                                    ((AddChildViewModel)BindingContext).iEPexitdate = new DateTime(year, month, day);
                                    var month1 = ((AddChildViewModel)BindingContext).IEPExitdate.Value.Month < 10 ? "0" + ((AddChildViewModel)BindingContext).IEPExitdate.Value.Month : ((AddChildViewModel)BindingContext).IEPExitdate.Value.Month + "";
                                    var day1 = ((AddChildViewModel)BindingContext).IEPExitdate.Value.Day < 10 ? "0" + ((AddChildViewModel)BindingContext).IEPExitdate.Value.Day : ((AddChildViewModel)BindingContext).IEPExitdate.Value.Day + "";
                                    var year1 = ((AddChildViewModel)BindingContext).IEPExitdate.Value.Year;
                                    IEPExitDateEntry.Text = month1 + "/" + day1 + "/" + year1;
                                    IEPExitDateEntryStackLayout.BackgroundColor = IEPExitDateEntry.BackgroundColor = Color.White;
                                    IEPExitDateEntry.TextColor = Colors.LightGrayColor;
                                    IEPExitDateEntry.FontAttributes = FontAttributes.None;
                                    IEPExitDateFrame.BorderColor = Color.FromHex("#898D8D");
                                    IEPExitDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                                    IEPExitDateImageFrame.BorderColor = Color.FromHex("#898D8D");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (this.BindingContext != null)
                        {
                            ((AddChildViewModel)BindingContext).IEPExitDateIsValid = true;
                            ((AddChildViewModel)BindingContext).iEPexitdate = null;
                        }
                        IEPExitDateEntryStackLayout.BackgroundColor = IEPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                        IEPExitDateEntry.Text = "";
                        IEPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                        IEPExitDateEntry.FontAttributes = FontAttributes.Bold;
                        IEPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                        IEPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                        IEPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                    }
                }
            }
        }

        private void IEPInitialDateEntry_Unfocused(object sender, FocusEventArgs e)
        {
            IEPInitialDateEntry_TextChanged(sender, null);
            var entryText = (sender as Entry)?.Text;
            if (!string.IsNullOrEmpty(entryText))
            {
                if (!string.IsNullOrEmpty(entryText) && (entryText.Trim().Length == 10))
                {
                    return;
                }
                else
                {
                    if (this.BindingContext != null)
                    {
                        ((AddChildViewModel)BindingContext).IEPInitialDateIsValid = true;
                        ((AddChildViewModel)BindingContext).iEPinitialdateofEligibility = null;
                    }
                    IEPInitialDateEntryStackLayout.BackgroundColor = IEPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                    IEPInitialDateEntry.Text = "";
                    IEPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                    IEPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                    IEPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                    IEPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    IEPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                }
            }
        }

        private void IEPInitialDateEntry_Focused(object sender, FocusEventArgs e)
        {
            ((AddChildViewModel)BindingContext).IEPInitialDateIsValid = false;

            IEPInitialDateEntryStackLayout.BackgroundColor = IEPInitialDateEntry.BackgroundColor = Color.White;
            IEPInitialDateEntry.TextColor = Colors.LightGrayColor;
            IEPInitialDateEntry.FontAttributes = FontAttributes.None;
            ((AddChildViewModel)this.BindingContext).IsDOBEmpty = false;
            IEPInitialDateFrame.BorderColor = Color.FromHex("#898D8D");
            IEPInitialDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
            IEPInitialDateImageFrame.BorderColor = Color.FromHex("#898D8D");
            /*string entryText = (sender as Entry)?.Text;
            if (DateTime.TryParseExact(entryText, "MM/dd/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out _))
            {
                IEPInitialDateEntry.Text = entryText;
            }
            else
            {
                if (((AddChildViewModel)this.BindingContext).IEPInitialDateIsValid)
                {
                    IEPInitialDateEntry.Text = "";
                }
                ((AddChildViewModel)this.BindingContext).IEPInitialDateIsValid = false;
                IEPInitialDateEntryStackLayout.BackgroundColor = IEPInitialDateEntry.BackgroundColor = Color.White;
                IEPInitialDateEntry.TextColor = Colors.LightGrayColor;
                IEPInitialDateEntry.FontAttributes = FontAttributes.None;
                ((AddChildViewModel)this.BindingContext).IsDOBEmpty = false;
                IEPInitialDateFrame.BorderColor = Color.FromHex("#898D8D");
                IEPInitialDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                IEPInitialDateImageFrame.BorderColor = Color.FromHex("#898D8D");
            }*/
        }

        private void IEPInitialDateEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentField = new WeakReference(sender);
            var entryText = (sender as Entry)?.Text;
            if (this.BindingContext == null)
            {
                if (!string.IsNullOrEmpty(entryText) && (entryText == "01/01/0001" || entryText == "1/1/1"))
                {
                    (sender as Entry).Text = "";
                }
                return;
            }
            if (!string.IsNullOrEmpty(entryText) && entryText.Trim().Length == 10)
            {
                var splitedText = entryText.Split('/');
                if (splitedText != null && splitedText.Length != 3)
                {
                    if (this.BindingContext != null)
                    {
                        ((AddChildViewModel)BindingContext).IEPInitialDateIsValid = true;
                        ((AddChildViewModel)BindingContext).iEPinitialdateofEligibility = null;
                    }
                    IEPInitialDateEntryStackLayout.BackgroundColor = IEPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                    IEPInitialDateEntry.Text = "";
                    IEPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                    IEPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                    IEPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                    IEPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    IEPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                }
                else if (splitedText != null && splitedText.Length == 3)
                {
                    if (splitedText[0].Length == 2 && splitedText[1].Length == 2 && splitedText[2].Length == 4)
                    {
                        try
                        {
                            var year = Convert.ToInt32(splitedText[2]);
                            var month = Convert.ToInt32(splitedText[0]);
                            var day = Convert.ToInt32(splitedText[1]);

                            var date = new DateTime(year, month, day);
                            if (year <= 1918 || Convert.ToInt32(splitedText[2]) > DateTime.Now.Year
                            || year > DateTime.MaxValue.Year || year == 1)
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IEPInitialDateIsValid = true;
                                    ((AddChildViewModel)BindingContext).iEPinitialdateofEligibility = null;
                                }
                                IEPInitialDateEntryStackLayout.BackgroundColor = IEPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                                IEPInitialDateEntry.Text = "";
                                IEPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                                IEPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                                IEPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                                IEPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                                IEPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                            }
                            else
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IEPInitialDateIsValid = false;
                                    ((AddChildViewModel)BindingContext).iEPinitialdateofEligibility = new DateTime(year, month, day);
                                    var month1 = ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Month < 10 ? "0" + ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Month : ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Month + "";
                                    var day1 = ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Day < 10 ? "0" + ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Day : ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Day + "";
                                    var year1 = ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Year;
                                    IEPInitialDateEntry.Text = month1 + "/" + day1 + "/" + year1;

                                    IEPInitialDateEntryStackLayout.BackgroundColor = IEPInitialDateEntry.BackgroundColor = Color.White;
                                    IEPInitialDateEntry.TextColor = Colors.LightGrayColor;
                                    IEPInitialDateEntry.FontAttributes = FontAttributes.None;
                                    IEPInitialDateFrame.BorderColor = Color.FromHex("#898D8D");
                                    IEPInitialDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                                    IEPInitialDateImageFrame.BorderColor = Color.FromHex("#898D8D");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (this.BindingContext != null)
                            {
                                ((AddChildViewModel)BindingContext).IEPInitialDateIsValid = true;
                                ((AddChildViewModel)BindingContext).iEPinitialdateofEligibility = null;
                            }
                            IEPInitialDateEntryStackLayout.BackgroundColor = IEPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                            IEPInitialDateEntry.Text = "";
                            IEPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                            IEPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                            IEPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                            IEPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                            IEPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(entryText) && entryText.Trim().Length == 8)
            {
                var splitedText = entryText.Split('/');
                if (splitedText != null && splitedText.Length != 3)
                {
                    if (this.BindingContext != null)
                    {
                        ((AddChildViewModel)BindingContext).IEPInitialDateIsValid = true;
                        ((AddChildViewModel)BindingContext).iEPinitialdateofEligibility = null;
                    }
                    IEPInitialDateEntryStackLayout.BackgroundColor = IEPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                    IEPInitialDateEntry.Text = "";
                    IEPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                    IEPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                    IEPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                    IEPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    IEPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                }
                else if (splitedText != null && splitedText.Length == 3)
                {
                    if (splitedText[0].Length == 1 && splitedText[1].Length == 1 && splitedText[2].Length == 4)
                    {
                        try
                        {
                            var year = Convert.ToInt32(splitedText[2]);
                            var month = Convert.ToInt32(splitedText[0]);
                            var day = Convert.ToInt32(splitedText[1]);

                            var date = new DateTime(year, month, day);
                            if (year <= 1918 || Convert.ToInt32(splitedText[2]) > DateTime.Now.Year
                            || year > DateTime.MaxValue.Year || year == 1)
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IEPInitialDateIsValid = true;
                                    ((AddChildViewModel)BindingContext).iEPinitialdateofEligibility = null;
                                }
                                IEPInitialDateEntryStackLayout.BackgroundColor = IEPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                                IEPInitialDateEntry.Text = "";
                                IEPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                                IEPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                                IEPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                                IEPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                                IEPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                            }
                            else
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IEPInitialDateIsValid = false;
                                    ((AddChildViewModel)BindingContext).iEPinitialdateofEligibility = new DateTime(year, month, day);
                                    var month1 = ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Month < 10 ? "0" + ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Month : ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Month + "";
                                    var day1 = ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Day < 10 ? "0" + ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Day : ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Day + "";
                                    var year1 = ((AddChildViewModel)BindingContext).IEPinitialdateofEligibility.Value.Year;
                                    IEPInitialDateEntry.Text = month1 + "/" + day1 + "/" + year1;
                                    IEPInitialDateEntryStackLayout.BackgroundColor = IEPInitialDateEntry.BackgroundColor = Color.White;
                                    IEPInitialDateEntry.TextColor = Colors.LightGrayColor;
                                    IEPInitialDateEntry.FontAttributes = FontAttributes.None;
                                    IEPInitialDateFrame.BorderColor = Color.FromHex("#898D8D");
                                    IEPInitialDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                                    IEPInitialDateImageFrame.BorderColor = Color.FromHex("#898D8D");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (this.BindingContext != null)
                            {
                                ((AddChildViewModel)BindingContext).IEPInitialDateIsValid = true;
                                ((AddChildViewModel)BindingContext).iEPinitialdateofEligibility = null;
                            }
                            IEPInitialDateEntryStackLayout.BackgroundColor = IEPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                            IEPInitialDateEntry.Text = "";
                            IEPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                            IEPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                            IEPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                            IEPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                            IEPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                        }
                    }
                }
            }
        }

        private void IFSPInitialDateEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentField = new WeakReference(sender);
            var entryText = (sender as Entry)?.Text;
            if (this.BindingContext == null)
            {
                if (!string.IsNullOrEmpty(entryText) && (entryText == "01/01/0001" || entryText == "1/1/1"))
                {
                    (sender as Entry).Text = "";
                }
                return;
            }
            if (!string.IsNullOrEmpty(entryText) && entryText.Trim().Length == 10)
            {
                var splitedText = entryText.Split('/');
                if (splitedText != null && splitedText.Length != 3)
                {
                    if (this.BindingContext != null)
                    {
                        ((AddChildViewModel)BindingContext).IFSPInitialDateIsValid = true;
                        ((AddChildViewModel)BindingContext).iFSPinitialdateofEligibility = null;
                    }
                    IFSPInitialDateEntryStackLayout.BackgroundColor = IFSPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                    IFSPInitialDateEntry.Text = "";
                    IFSPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                    IFSPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                    IFSPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                    IFSPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    IFSPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                }
                else if (splitedText != null && splitedText.Length == 3)
                {
                    if (splitedText[0].Length == 2 && splitedText[1].Length == 2 && splitedText[2].Length == 4)
                    {
                        try
                        {
                            var year = Convert.ToInt32(splitedText[2]);
                            var month = Convert.ToInt32(splitedText[0]);
                            var day = Convert.ToInt32(splitedText[1]);

                            var date = new DateTime(year, month, day);
                            if (year <= 1918 || Convert.ToInt32(splitedText[2]) > DateTime.Now.Year
                            || year > DateTime.MaxValue.Year || year == 1)
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IFSPInitialDateIsValid = true;
                                    ((AddChildViewModel)BindingContext).iFSPinitialdateofEligibility = null;
                                }
                                IFSPInitialDateEntryStackLayout.BackgroundColor = IFSPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                                IFSPInitialDateEntry.Text = "";
                                IFSPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                                IFSPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                                IFSPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                                IFSPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                                IFSPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                            }
                            else
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IFSPInitialDateIsValid = false;
                                    ((AddChildViewModel)BindingContext).iFSPinitialdateofEligibility = new DateTime(year, month, day);
                                    var month1 = ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Month < 10 ? "0" + ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Month : ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Month + "";
                                    var day1 = ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Day < 10 ? "0" + ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Day : ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Day + "";
                                    var year1 = ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Year;
                                    IFSPInitialDateEntry.Text = month1 + "/" + day1 + "/" + year1;
                                    IFSPInitialDateEntryStackLayout.BackgroundColor = IFSPInitialDateEntry.BackgroundColor = Color.White;
                                    IFSPInitialDateEntry.TextColor = Colors.LightGrayColor;
                                    IFSPInitialDateEntry.FontAttributes = FontAttributes.None;
                                    IFSPInitialDateFrame.BorderColor = Color.FromHex("#898D8D");
                                    IFSPInitialDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                                    IFSPInitialDateImageFrame.BorderColor = Color.FromHex("#898D8D");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (this.BindingContext != null)
                            {
                                ((AddChildViewModel)BindingContext).IFSPInitialDateIsValid = true;
                                ((AddChildViewModel)BindingContext).iFSPinitialdateofEligibility = null;
                            }
                            IFSPInitialDateEntryStackLayout.BackgroundColor = IFSPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                            IFSPInitialDateEntry.Text = "";
                            IFSPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                            IFSPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                            IFSPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                            IFSPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                            IFSPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(entryText) && entryText.Trim().Length == 8)
            {
                var splitedText = entryText.Split('/');
                if (splitedText != null && splitedText.Length != 3)
                {
                    if (this.BindingContext != null)
                    {
                        ((AddChildViewModel)BindingContext).IFSPInitialDateIsValid = true;
                        ((AddChildViewModel)BindingContext).iFSPinitialdateofEligibility = null;
                    }
                    IFSPInitialDateEntryStackLayout.BackgroundColor = IFSPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                    IFSPInitialDateEntry.Text = "";
                    IFSPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                    IFSPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                    IFSPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                    IFSPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    IFSPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                }
                else if (splitedText != null && splitedText.Length == 3)
                {
                    if (splitedText[0].Length == 1 && splitedText[1].Length == 1 && splitedText[2].Length == 4)
                    {
                        try
                        {
                            var year = Convert.ToInt32(splitedText[2]);
                            var month = Convert.ToInt32(splitedText[0]);
                            var day = Convert.ToInt32(splitedText[1]);

                            var date = new DateTime(year, month, day);
                            if (year <= 1918 || Convert.ToInt32(splitedText[2]) > DateTime.Now.Year
                            || year > DateTime.MaxValue.Year || year == 1)
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IFSPInitialDateIsValid = true;
                                    ((AddChildViewModel)BindingContext).iFSPinitialdateofEligibility = null;
                                }
                                IFSPInitialDateEntryStackLayout.BackgroundColor = IFSPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                                IFSPInitialDateEntry.Text = "";
                                IFSPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                                IFSPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                                IFSPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                                IFSPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                                IFSPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                            }
                            else
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IFSPInitialDateIsValid = false;
                                    ((AddChildViewModel)BindingContext).iFSPinitialdateofEligibility = new DateTime(year, month, day);
                                    var month1 = ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Month < 10 ? "0" + ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Month : ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Month + "";
                                    var day1 = ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Day < 10 ? "0" + ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Day : ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Day + "";
                                    var year1 = ((AddChildViewModel)BindingContext).IFSPinitialdateofEligibility.Value.Year;
                                    IFSPInitialDateEntry.Text = month1 + "/" + day1 + "/" + year1;
                                    IFSPInitialDateEntryStackLayout.BackgroundColor = IFSPInitialDateEntry.BackgroundColor = Color.White;
                                    IFSPInitialDateEntry.TextColor = Colors.LightGrayColor;
                                    IFSPInitialDateEntry.FontAttributes = FontAttributes.None;
                                    IFSPInitialDateFrame.BorderColor = Color.FromHex("#898D8D");
                                    IFSPInitialDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                                    IFSPInitialDateImageFrame.BorderColor = Color.FromHex("#898D8D");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (this.BindingContext != null)
                            {
                                ((AddChildViewModel)BindingContext).IFSPInitialDateIsValid = true;
                                ((AddChildViewModel)BindingContext).iFSPinitialdateofEligibility = null;
                            }
                            IFSPInitialDateEntryStackLayout.BackgroundColor = IFSPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                            IFSPInitialDateEntry.Text = "";
                            IFSPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                            IFSPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                            IFSPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                            IFSPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                            IFSPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                        }
                    }
                }
            }
        }

        private void IFSPInitialDateEntry_Focused(object sender, FocusEventArgs e)
        {
            ((AddChildViewModel)BindingContext).IFSPInitialDateIsValid = false;

            IFSPInitialDateEntryStackLayout.BackgroundColor = IFSPInitialDateEntry.BackgroundColor = Color.White;
            IFSPInitialDateEntry.TextColor = Colors.LightGrayColor;
            IFSPInitialDateEntry.FontAttributes = FontAttributes.None;
            ((AddChildViewModel)this.BindingContext).IsDOBEmpty = false;
            IFSPInitialDateFrame.BorderColor = Color.FromHex("#898D8D");
            IFSPInitialDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
            IFSPInitialDateImageFrame.BorderColor = Color.FromHex("#898D8D");

            /*_currentField = new WeakReference(sender);
            var entryText = (sender as Entry)?.Text;
            if (DateTime.TryParseExact(entryText, "MM/dd/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out _))
            {
                IFSPInitialDateEntry.Text = entryText;
            }
            else
            {
                if (((AddChildViewModel)this.BindingContext).IFSPInitialDateIsValid)
                {
                    IFSPInitialDateEntry.Text = "";
                }
                ((AddChildViewModel)this.BindingContext).IFSPInitialDateIsValid = false;
                IFSPInitialDateEntryStackLayout.BackgroundColor = IFSPInitialDateEntry.BackgroundColor = Color.White;
                IFSPInitialDateEntry.TextColor = Colors.LightGrayColor;
                IFSPInitialDateEntry.FontAttributes = FontAttributes.None;
                ((AddChildViewModel)this.BindingContext).IsDOBEmpty = false;
                IFSPInitialDateFrame.BorderColor = Color.FromHex("#898D8D");
                IFSPInitialDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                IFSPInitialDateImageFrame.BorderColor = Color.FromHex("#898D8D");
            }*/
        }

        private void IFSPInitialDateEntry_Unfocused(object sender, FocusEventArgs e)
        {
            IFSPInitialDateEntry_TextChanged(sender, null);
            var entryText = (sender as Entry)?.Text;
            if (!string.IsNullOrEmpty(entryText))
            {
                if ((entryText.Trim().Length == 10))
                {
                    return;
                }
                else
                {
                    if (this.BindingContext != null)
                    {
                        ((AddChildViewModel)BindingContext).IFSPInitialDateIsValid = true;
                        ((AddChildViewModel)BindingContext).iFSPinitialdateofEligibility = null;
                    }
                    IFSPInitialDateEntryStackLayout.BackgroundColor = IFSPInitialDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                    IFSPInitialDateEntry.Text = "";
                    IFSPInitialDateEntry.TextColor = Color.FromHex("#CC1417");
                    IFSPInitialDateEntry.FontAttributes = FontAttributes.Bold;
                    IFSPInitialDateFrame.BorderColor = Color.FromHex("#CC1417");
                    IFSPInitialDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    IFSPInitialDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                }
            }
        }

        private void IFSPExitDateEntry_Unfocused(object sender, FocusEventArgs e)
        {
            IFSPExitDateEntry_TextChanged(sender, null);
            var entryText = (sender as Entry)?.Text;
            if (!string.IsNullOrEmpty(entryText))
            {
                if ((entryText.Trim().Length == 10))
                {
                    return;
                }
                else
                {
                    if (this.BindingContext != null)
                    {
                        ((AddChildViewModel)BindingContext).IFSPExitDateIsValid = true;
                        ((AddChildViewModel)BindingContext).iFSPexitdate = null;
                    }
                    IFSPExitDateEntryStackLayout.BackgroundColor = IFSPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                    IFSPExitDateEntry.Text = "";
                    IFSPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                    IFSPExitDateEntry.FontAttributes = FontAttributes.Bold;
                    IFSPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                    IFSPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    IFSPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                }
            
            }
        }

        private void IFSPExitDateEntry_Focused(object sender, FocusEventArgs e)
        {
            ((AddChildViewModel)BindingContext).IFSPExitDateIsValid = false;
            IFSPExitDateEntryStackLayout.BackgroundColor = IFSPExitDateEntry.BackgroundColor = Color.White;
            IFSPExitDateEntry.TextColor = Colors.LightGrayColor;
            IFSPExitDateEntry.FontAttributes = FontAttributes.None;
            ((AddChildViewModel)this.BindingContext).IsDOBEmpty = false;
            IFSPExitDateFrame.BorderColor = Color.FromHex("#898D8D");
            IFSPExitDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
            IFSPExitDateImageFrame.BorderColor = Color.FromHex("#898D8D");
            /*_currentField = new WeakReference(sender);
            var entryText = (sender as Entry)?.Text;
            if (DateTime.TryParseExact(entryText, "MM/dd/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out _))
            {
                IFSPExitDateEntry.Text = entryText;
            }
            else
            {
                if (((AddChildViewModel)this.BindingContext).IFSPExitDateIsValid)
                {
                    IFSPExitDateEntry.Text = "";
                }
                ((AddChildViewModel)this.BindingContext).IFSPExitDateIsValid = false;
                IFSPExitDateEntryStackLayout.BackgroundColor = IFSPExitDateEntry.BackgroundColor = Color.White;
                IFSPExitDateEntry.TextColor = Colors.LightGrayColor;
                IFSPExitDateEntry.FontAttributes = FontAttributes.None;
                ((AddChildViewModel)this.BindingContext).IsDOBEmpty = false;
                IFSPExitDateFrame.BorderColor = Color.FromHex("#898D8D");
                IFSPExitDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                IFSPExitDateImageFrame.BorderColor = Color.FromHex("#898D8D");
            }*/
        }

        private void IFSPExitDateEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentField = new WeakReference(sender);
            var entryText = (sender as Entry)?.Text;
            if (this.BindingContext == null)
            {
                if (!string.IsNullOrEmpty(entryText) && (entryText == "01/01/0001" || entryText == "1/1/1"))
                {
                    (sender as Entry).Text = "";
                }
                return;
            }
            if (!string.IsNullOrEmpty(entryText) && entryText.Trim().Length == 10)
            {
                var splitedText = entryText.Split('/');
                if (splitedText != null && splitedText.Length != 3)
                {
                    if (this.BindingContext != null)
                    {
                        ((AddChildViewModel)BindingContext).IFSPExitDateIsValid = true;
                        ((AddChildViewModel)BindingContext).iFSPexitdate = null;
                    }
                    IFSPExitDateEntryStackLayout.BackgroundColor = IFSPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                    IFSPExitDateEntry.Text = "";
                    IFSPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                    IFSPExitDateEntry.FontAttributes = FontAttributes.Bold;
                    IFSPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                    IFSPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    IFSPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                }
                else if (splitedText != null && splitedText.Length == 3)
                {
                    try
                    {
                        if (splitedText[0].Length == 2 && splitedText[1].Length == 2 && splitedText[2].Length == 4)
                        {

                            var year = Convert.ToInt32(splitedText[2]);
                            var month = Convert.ToInt32(splitedText[0]);
                            var day = Convert.ToInt32(splitedText[1]);

                            var date = new DateTime(year, month, day);
                            if (year <= 1918 || year > DateTime.MaxValue.Year || year == 1)
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IFSPExitDateIsValid = true;
                                    ((AddChildViewModel)BindingContext).iFSPexitdate = null;
                                }
                                IFSPExitDateEntryStackLayout.BackgroundColor = IFSPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                                IFSPExitDateEntry.Text = "";
                                IFSPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                                IFSPExitDateEntry.FontAttributes = FontAttributes.Bold;
                                IFSPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                                IFSPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                                IFSPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                            }
                            else
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IFSPExitDateIsValid = false;
                                    ((AddChildViewModel)BindingContext).iFSPexitdate = new DateTime(year, month, day);
                                    var month1 = ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Month < 10 ? "0" + ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Month : ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Month + "";
                                    var day1 = ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Day < 10 ? "0" + ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Day : ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Day + "";
                                    var year1 = ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Year;
                                    IFSPExitDateEntry.Text = month1 + "/" + day1 + "/" + year1;
                                    IFSPExitDateEntryStackLayout.BackgroundColor = IFSPExitDateEntry.BackgroundColor = Color.White;
                                    IFSPExitDateEntry.TextColor = Colors.LightGrayColor;
                                    IFSPExitDateEntry.FontAttributes = FontAttributes.None;
                                    IFSPExitDateFrame.BorderColor = Color.FromHex("#898D8D");
                                    IFSPExitDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                                    IFSPExitDateImageFrame.BorderColor = Color.FromHex("#898D8D");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (this.BindingContext != null)
                        {
                            ((AddChildViewModel)BindingContext).IFSPExitDateIsValid = true;
                            ((AddChildViewModel)BindingContext).iFSPexitdate = null;
                        }
                        IFSPExitDateEntryStackLayout.BackgroundColor = IFSPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                        IFSPExitDateEntry.Text = "";
                        IFSPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                        IFSPExitDateEntry.FontAttributes = FontAttributes.Bold;
                        IFSPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                        IFSPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                        IFSPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                    }
                }
            }
            else if (!string.IsNullOrEmpty(entryText) && entryText.Trim().Length == 8)
            {
                var splitedText = entryText.Split('/');
                if (splitedText != null && splitedText.Length != 3)
                {
                    if (this.BindingContext != null)
                    {
                        ((AddChildViewModel)BindingContext).IFSPExitDateIsValid = true;
                        ((AddChildViewModel)BindingContext).iFSPexitdate = null;
                    }
                    IFSPExitDateEntryStackLayout.BackgroundColor = IFSPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                    IFSPExitDateEntry.Text = "";
                    IFSPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                    IFSPExitDateEntry.FontAttributes = FontAttributes.Bold;
                    IFSPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                    IFSPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    IFSPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                }
                else if (splitedText != null && splitedText.Length == 3)
                {
                    try
                    {
                        if (splitedText[0].Length == 1 && splitedText[1].Length == 1 && splitedText[2].Length == 4)
                        {

                            var year = Convert.ToInt32(splitedText[2]);
                            var month = Convert.ToInt32(splitedText[0]);
                            var day = Convert.ToInt32(splitedText[1]);

                            var date = new DateTime(year, month, day);
                            if (year <= 1918 || year > DateTime.MaxValue.Year || year == 1)
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IFSPExitDateIsValid = true;
                                    ((AddChildViewModel)BindingContext).iFSPexitdate = null;
                                }
                                IFSPExitDateEntryStackLayout.BackgroundColor = IFSPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                                IFSPExitDateEntry.Text = "";
                                IFSPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                                IFSPExitDateEntry.FontAttributes = FontAttributes.Bold;
                                IFSPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                                IFSPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                                IFSPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                            }
                            else
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).IFSPExitDateIsValid = false;
                                    ((AddChildViewModel)BindingContext).iFSPexitdate = new DateTime(year, month, day);
                                    var month1 = ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Month < 10 ? "0" + ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Month : ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Month + "";
                                    var day1 = ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Day < 10 ? "0" + ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Day : ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Day + "";
                                    var year1 = ((AddChildViewModel)BindingContext).IFSPExitdate.Value.Year;
                                    IFSPExitDateEntry.Text = month1 + "/" + day1 + "/" + year1;
                                    IFSPExitDateEntryStackLayout.BackgroundColor = IFSPExitDateEntry.BackgroundColor = Color.White;
                                    IFSPExitDateEntry.TextColor = Colors.LightGrayColor;
                                    IFSPExitDateEntry.FontAttributes = FontAttributes.None;
                                    IFSPExitDateFrame.BorderColor = Color.FromHex("#898D8D");
                                    IFSPExitDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                                    IFSPExitDateImageFrame.BorderColor = Color.FromHex("#898D8D");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (this.BindingContext != null)
                        {
                            ((AddChildViewModel)BindingContext).IFSPExitDateIsValid = true;
                            ((AddChildViewModel)BindingContext).iFSPexitdate = null;
                        }
                        IFSPExitDateEntryStackLayout.BackgroundColor = IFSPExitDateEntry.BackgroundColor = Color.FromHex("#FFF1F1");
                        IFSPExitDateEntry.Text = "";
                        IFSPExitDateEntry.TextColor = Color.FromHex("#CC1417");
                        IFSPExitDateEntry.FontAttributes = FontAttributes.Bold;
                        IFSPExitDateFrame.BorderColor = Color.FromHex("#CC1417");
                        IFSPExitDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                        IFSPExitDateImageFrame.BorderColor = Color.FromHex("#CC1417");
                    }
                    }
            }
        }

        private void AddDemographicView_BindingContextChanged(object sender, EventArgs e)
        {
            if (this.BindingContext != null)
            {
                if (this.BindingContext is AddChildViewModel viewmodel)
                    viewmodel.ReloadDemographicView = () =>
                    {
                        IFSPExitDateEntry.Text = "";
                        IFSPInitialDateEntry.Text = "";
                        IEPInitialDateEntry.Text = "";
                        IEPExitDateEntry.Text = "";
                    };
            }
        }
        public void SaveChildTapped(object sender, EventArgs e)
        {
            SaveChildAction.Invoke();
        }
        void RaceItemTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as AddChildViewModel)?.RaceItemTapped(e.Item);
            ((ListView) sender).SelectedItem = null;
        }

        void FundingSourceTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as AddChildViewModel)?.FundingSourceTapped(e.Item);
            ((ListView) sender).SelectedItem = null;
        }

        void LanguageTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as AddChildViewModel)?.LanguageSelected(e.Item);
        }

        void PrimaryDiagnosesTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as AddChildViewModel)?.PrimarDiagnosesTapped(e.Item);
        }
        void SecondaryDiagnosesTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as AddChildViewModel)?.SecondaryDiagnosesTapped(e.Item);
        }

        private void EthencityTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as AddChildViewModel)?.EthencitySelected(e.Item);
        }

        void OpenEligibilityDP(object sender, System.EventArgs e)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                EligibilityIFS.ShowDatePicker();
            }
            else
            {
                EligibilityIFS.Focus();
            }
        }

        void OpenIFSCExitDP(object sender, System.EventArgs e)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                IFSPExitDP.ShowDatePicker();
            }
            else
            {
                IFSPExitDP.Focus();
            }
        }
        void OpenEligilityIEPDP(object sender, System.EventArgs e)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                EligiblityIEP.ShowDatePicker();
            }
            else
            {
                EligiblityIEP.Focus();
            }
        }

        void OpenIEPExitDP(object sender, System.EventArgs e)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                IEPExitDP.ShowDatePicker();
            }
            else
            {
                IEPExitDP.Focus();
            }

        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (height > width)
            {
                if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
                {
                    SecondaryDiagnosisEntry.Placeholder = "Select secondar...";
                    PrimatryDiagnosisEntry.Placeholder = "Select primar...";
                }
            }
            else
            {
                PrimatryDiagnosisEntry.Placeholder = "Select primary diagnosis";
                SecondaryDiagnosisEntry.Placeholder = "Select secondary diagnosis";

            }
            // IFSP Exit Date clear issue
                IFSPExitDateEntry.IsEnabled = false;
                IFSPExitDateEntry.HeightRequest = IFSPExitDateEntry.Height;
                Task.Delay(200).ConfigureAwait(true);
                IFSPExitDateEntry.IsEnabled = true;
            }

        protected override void OnAppearing()
        {
            try
            {
                if (this.BindingContext != null)
                {
                    var viewModel = (AddChildViewModel)this.BindingContext;
                    lblTitle.Text = viewModel.OfflineStudentID > 0 ? "EDIT CHILD INFORMATION" : "ADD CHILD INFORMATION";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected override void OnDisappearing()
        {
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                    DependencyService.Get<IKeyboardHelper>().HideKeyboard();
            }
            catch (Exception)
            {

            }
            base.OnDisappearing();            
            if (this.BindingContext != null)
            {
                ((AddChildViewModel)BindingContext).ShowRaces = false;
                ((AddChildViewModel)BindingContext).ShowLanguages = false;
                ((AddChildViewModel)BindingContext).ShowEthencity = false;
                ((AddChildViewModel)BindingContext).ShowFundingSources = false;
                ((AddChildViewModel)BindingContext)?.UnCoverCommand.Execute(new object());
            }
        }
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentField = new WeakReference(sender);
        }
    }
}
