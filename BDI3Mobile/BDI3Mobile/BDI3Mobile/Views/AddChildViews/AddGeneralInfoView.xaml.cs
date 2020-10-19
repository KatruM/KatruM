using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BDI3Mobile.Common;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.ViewModels;
using Xamarin.Forms;

namespace BDI3Mobile.Views.AddChildViews
{
    public partial class AddGeneralInfoView
    {
        private WeakReference _currentField;
        bool isInvalid = false;
        public Grid Grid;

        public AddGeneralInfoView()
        {
            InitializeComponent();
            Grid = MainGrid;
            BindingContextChanged -= AddGeneralInfoView_BindingContextChanged;
            BindingContextChanged += AddGeneralInfoView_BindingContextChanged;
            parent1name.TextChanged += Parentfield_TextChanged;
            parent1email.TextChanged += Parentfield_TextChanged;
            parent2name.TextChanged += Parentfield_TextChanged;
            parent2email.TextChanged += Parentfield_TextChanged;
            locationListview.ItemSelected += LocationListview_ItemSelected;

            MessagingCenter.Subscribe<string[]>(this, "EditData", values =>
            {
                DOB.Text = values[0];

                if (values[1] == "1/1/1")
                {
                    EnrollmentDate.Text = "";
                }
                else
                {
                    EnrollmentDate.Text = values[1];
                }

                EnrollmentStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.White;
                EnrollmentDate.TextColor = Colors.LightGrayColor;
                EnrollmentDate.FontAttributes = FontAttributes.None;
                EnrollmentFrame.BorderColor = Color.FromHex("#898D8D");
                EnrollmentImageFrame.BackgroundColor = Color.FromHex("147cbd");
                EnrollmentImageFrame.BorderColor = Color.FromHex("#898D8D");

                if (this.BindingContext != null)
                {
                    ((AddChildViewModel)BindingContext).EnrollmentDateIsInvalid = false;
                    ((AddChildViewModel)BindingContext).ParentEmailerror = false;
                }
                Parent1EmailErrorLabel.IsVisible = false;
                Parent1EmailFrame.BorderColor = Colors.BorderColor;
                Parent2EmailErrorLabel.IsVisible = false;
                Parent2EmailFrame.BorderColor = Colors.BorderColor;
            });

            Parent1EmailErrorLabel.IsVisible = false;
            DOB.Unfocused += DOB_Unfocused;
            DOB.Focused += DOB_Focused;
            DOB.TextChanged += DOB_TextChanged;
            EnrollmentDate.Unfocused += EnrollmentDate_Unfocused;
            EnrollmentDate.Focused += EnrollmentDate_Focused;
            EnrollmentDate.TextChanged += EnrollmentDate_TextChanged;
            MessagingCenter.Subscribe<String, bool>(this, "Tab", (text, value) => { if (_currentField != null && value == false) (_currentField.Target as Entry)?.Unfocus(); });
            MessagingCenter.Subscribe<String, bool>(this, "Tab", (text, value) =>
            {
                if (_currentField != null && value == false)
                    (_currentField.Target as Entry)?.Unfocus();
                EnrollmentDate.IsTabStop = value;
                DOB.IsTabStop = value;
                parent1name.IsTabStop = value;
                parent1email.IsTabStop = value;
                parent2name.IsTabStop = value;
                parent2email.IsTabStop = value;
                GenderImageBtn.IsTabStop = value;
                GenderImageBtn.IsTabStop = value;
            });

            GenderEntry.TextColor = Colors.LightGrayColor;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var viewModel = (AddChildViewModel)BindingContext;
                if (viewModel != null && viewModel.OfflineStudentID > 0)
                    lblTitle.Text = "EDIT CHILD INFORMATION";
                else
                    lblTitle.Text = "ADD CHILD INFORMATION";
                //Workaround: UI Image buttons rearrangement on keyboard show/hide
                if (Device.RuntimePlatform == Device.Android)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        ChildFirstName.Focus();
                        //ChildMiddleName.Focus();
                        await Task.Delay(20);
                        ChildFirstName.IsEnabled = false;
                        ChildFirstName.IsEnabled = true;
                        ChildMiddleName.IsEnabled = false;
                        ChildMiddleName.IsEnabled = true;
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void AddGeneralInfoView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ShowLocation" && ((AddChildViewModel)BindingContext).ShowLocation)
            {
                LoadTree();
            }
        }

        private void LoadTree()
        {
            locationListview.BindingContext = BindingContext as AddChildViewModel;
        }

        private void Parentfield_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                _currentField = new WeakReference(sender);
                if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                {
                    if (sender is BorderlessEntry)
                    {
                        ((Entry)sender).FontSize = ((Entry)sender).Text.Length > 0 ? 14 : 12;
                    }
                }
            }
            catch(Exception ex)
            {

            }

        }

        #region EventHandlers
        private void EnrollmentDate_TextChanged(object sender, TextChangedEventArgs e)
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
                        ((AddChildViewModel)BindingContext).EnrollmentDateIsInvalid = true;
                        ((AddChildViewModel)BindingContext).EnrollDate = null;
                        ((AddChildViewModel)BindingContext).IsEnrollmentEmpty = false;
                    }
                    EnrollmentStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.FromHex("#FFF1F1");
                    EnrollmentDate.Text = "";
                    EnrollmentDate.TextColor = Color.FromHex("CC1417");
                    EnrollmentDate.FontAttributes = FontAttributes.Bold;
                    EnrollmentFrame.BorderColor = Color.FromHex("#CC1417");
                    EnrollmentImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    EnrollmentImageFrame.BorderColor = Color.FromHex("#CC1417");
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
                            if (year <= 1918 || year > DateTime.MaxValue.Year || year == 1)
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).EnrollmentDateIsInvalid = true;
                                    ((AddChildViewModel)BindingContext).EnrollDate = null;
                                    ((AddChildViewModel)BindingContext).IsEnrollmentEmpty = false;
                                }
                                EnrollmentStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.FromHex("#FFF1F1");
                                EnrollmentDate.Text = "";
                                EnrollmentDate.TextColor = Color.FromHex("CC1417");
                                EnrollmentDate.FontAttributes = FontAttributes.Bold;
                                EnrollmentFrame.BorderColor = Color.FromHex("#CC1417");
                                EnrollmentImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                                EnrollmentImageFrame.BorderColor = Color.FromHex("#CC1417");
                            }
                            else
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).EnrollmentDateIsInvalid = false;
                                    ((AddChildViewModel)BindingContext).IsEnrollmentEmpty = false;
                                    ((AddChildViewModel)BindingContext).EnrollDate = new DateTime(year, month, day);
                                    var month1 = ((AddChildViewModel)BindingContext).EnrollDate.Value.Month < 10 ? "0" + ((AddChildViewModel)BindingContext).EnrollDate.Value.Month : ((AddChildViewModel)BindingContext).EnrollDate.Value.Month + "";
                                    var day1 = ((AddChildViewModel)BindingContext).EnrollDate.Value.Day < 10 ? "0" + ((AddChildViewModel)BindingContext).EnrollDate.Value.Day : ((AddChildViewModel)BindingContext).EnrollDate.Value.Day + "";
                                    var year1 = ((AddChildViewModel)BindingContext).EnrollDate.Value.Year;
                                    EnrollmentDate.Text = month1 + "/" + day1 + "/" + year1;

                                    EnrollmentStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.White;
                                    EnrollmentDate.TextColor = Colors.LightGrayColor;
                                    EnrollmentDate.FontAttributes = FontAttributes.None;
                                    EnrollmentFrame.BorderColor = Color.FromHex("#898D8D");
                                    EnrollmentImageFrame.BackgroundColor = Color.FromHex("147cbd");
                                    EnrollmentImageFrame.BorderColor = Color.FromHex("#898D8D");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (this.BindingContext != null)
                            {
                                ((AddChildViewModel)BindingContext).EnrollmentDateIsInvalid = true;
                                ((AddChildViewModel)BindingContext).EnrollDate = null;
                                ((AddChildViewModel)BindingContext).IsEnrollmentEmpty = false;
                            }
                            EnrollmentStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.FromHex("#FFF1F1");
                            EnrollmentDate.Text = "";
                            EnrollmentDate.TextColor = Color.FromHex("CC1417");
                            EnrollmentDate.FontAttributes = FontAttributes.Bold;
                            EnrollmentFrame.BorderColor = Color.FromHex("#CC1417");
                            EnrollmentImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                            EnrollmentImageFrame.BorderColor = Color.FromHex("#CC1417");
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
                        ((AddChildViewModel)BindingContext).EnrollmentDateIsInvalid = true;
                        ((AddChildViewModel)BindingContext).EnrollDate = null;
                        ((AddChildViewModel)BindingContext).IsEnrollmentEmpty = false;
                    }
                    EnrollmentStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.FromHex("#FFF1F1");
                    EnrollmentDate.Text = "";
                    EnrollmentDate.TextColor = Color.FromHex("CC1417");
                    EnrollmentDate.FontAttributes = FontAttributes.Bold;
                    EnrollmentFrame.BorderColor = Color.FromHex("#CC1417");
                    EnrollmentImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    EnrollmentImageFrame.BorderColor = Color.FromHex("#CC1417");
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
                            if (year <= 1918 || year > DateTime.MaxValue.Year || year == 1)
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).EnrollmentDateIsInvalid = true;
                                    ((AddChildViewModel)BindingContext).EnrollDate = null;
                                    ((AddChildViewModel)BindingContext).IsEnrollmentEmpty = false;
                                }
                                EnrollmentStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.FromHex("#FFF1F1");
                                EnrollmentDate.Text = "";
                                EnrollmentDate.TextColor = Color.FromHex("CC1417");
                                EnrollmentDate.FontAttributes = FontAttributes.Bold;
                                EnrollmentFrame.BorderColor = Color.FromHex("#CC1417");
                                EnrollmentImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                                EnrollmentImageFrame.BorderColor = Color.FromHex("#CC1417");
                            }
                            else
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).EnrollmentDateIsInvalid = false;
                                    ((AddChildViewModel)BindingContext).IsEnrollmentEmpty = false;
                                    ((AddChildViewModel)BindingContext).EnrollDate = new DateTime(year, month, day);
                                    var month1 = ((AddChildViewModel)BindingContext).EnrollDate.Value.Month < 10 ? "0" + ((AddChildViewModel)BindingContext).EnrollDate.Value.Month : ((AddChildViewModel)BindingContext).EnrollDate.Value.Month + "";
                                    var day1 = ((AddChildViewModel)BindingContext).EnrollDate.Value.Day < 10 ? "0" + ((AddChildViewModel)BindingContext).EnrollDate.Value.Day : ((AddChildViewModel)BindingContext).EnrollDate.Value.Day + "";
                                    var year1 = ((AddChildViewModel)BindingContext).EnrollDate.Value.Year;
                                    EnrollmentDate.Text = month1 + "/" + day1 + "/" + year1;

                                    EnrollmentStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.White;
                                    EnrollmentDate.TextColor = Colors.LightGrayColor;
                                    EnrollmentDate.FontAttributes = FontAttributes.None;
                                    EnrollmentFrame.BorderColor = Color.FromHex("#898D8D");
                                    EnrollmentImageFrame.BackgroundColor = Color.FromHex("147cbd");
                                    EnrollmentImageFrame.BorderColor = Color.FromHex("#898D8D");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (this.BindingContext != null)
                            {
                                ((AddChildViewModel)BindingContext).EnrollmentDateIsInvalid = true;
                                ((AddChildViewModel)BindingContext).EnrollDate = null;
                                ((AddChildViewModel)BindingContext).IsEnrollmentEmpty = false;
                            }
                            EnrollmentStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.FromHex("#FFF1F1");
                            EnrollmentDate.Text = "";
                            EnrollmentDate.TextColor = Color.FromHex("CC1417");
                            EnrollmentDate.FontAttributes = FontAttributes.Bold;
                            EnrollmentFrame.BorderColor = Color.FromHex("#CC1417");
                            EnrollmentImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                            EnrollmentImageFrame.BorderColor = Color.FromHex("#CC1417");
                        }
                    }
                }
            }
        }

        private void EnrollmentDate_Focused(object sender, FocusEventArgs e)
        {
            EnrollmentDate_TextChanged(sender, null);
        }

        private void EnrollmentDate_Unfocused(object sender, FocusEventArgs e)
        {
            EnrollmentDate_TextChanged(sender, null);
            var entryText = (sender as Entry)?.Text;
            if (!string.IsNullOrEmpty(entryText) && entryText.Trim().Length < 10)
            {
                if (this.BindingContext != null)
                {
                    ((AddChildViewModel)BindingContext).EnrollmentDateIsInvalid = true;
                    ((AddChildViewModel)BindingContext).IsEnrollmentEmpty = false;
                    ((AddChildViewModel)BindingContext).EnrollDate = null;
                }
                EnrollmentStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.FromHex("#FFF1F1");
                EnrollmentDate.Text = "";
                EnrollmentDate.TextColor = Color.FromHex("CC1417");
                EnrollmentDate.FontAttributes = FontAttributes.Bold;
                EnrollmentFrame.BorderColor = Color.FromHex("#CC1417");
                EnrollmentImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                EnrollmentImageFrame.BorderColor = Color.FromHex("#CC1417");
            }
        }

        private void DOB_TextChanged(object sender, TextChangedEventArgs e)
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
                        ((AddChildViewModel)BindingContext).DOBIsInvalid = true;
                        ((AddChildViewModel)BindingContext).IsDOBEmpty = false;
                    }
                    DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.FromHex("#FFF1F1");
                    DOB.Text = "";
                    DOB.TextColor = Color.FromHex("CC1417");
                    DOB.FontAttributes = FontAttributes.Bold;
                    DOBFrame.BorderColor = Color.FromHex("#CC1417");
                    DOBImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    DOBImageFrame.BorderColor = Color.FromHex("#CC1417");
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
                                    ((AddChildViewModel)BindingContext).DOBIsInvalid = true;
                                    ((AddChildViewModel)BindingContext).IsDOBEmpty = false;
                                }
                                DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.FromHex("#FFF1F1");
                                DOB.Text = "";
                                DOB.TextColor = Color.FromHex("CC1417");
                                DOB.FontAttributes = FontAttributes.Bold;
                                DOBFrame.BorderColor = Color.FromHex("#CC1417");
                                DOBImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                                DOBImageFrame.BorderColor = Color.FromHex("#CC1417");
                            }
                            else
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).DOBIsInvalid = false;
                                    ((AddChildViewModel)BindingContext).IsDOBEmpty = false;
                                    var month1 = month < 10 ? ("0" + month) : (month + "");
                                    var day1 = day < 10 ? "0" + day : day + "";
                                    var year1 = year;
                                    DOB.Text = month1 + "/" + day1 + "/" + year1;

                                    DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.White;
                                    DOB.TextColor = Colors.LightGrayColor;
                                    DOB.FontAttributes = FontAttributes.None;
                                    DOBFrame.BorderColor = Color.FromHex("#898D8D");
                                    DOBImageFrame.BackgroundColor = Color.FromHex("147cbd");
                                    DOBImageFrame.BorderColor = Color.FromHex("#898D8D");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (this.BindingContext != null)
                            {
                                ((AddChildViewModel)BindingContext).DOBIsInvalid = true;
                                ((AddChildViewModel)BindingContext).IsDOBEmpty = false;
                            }
                            DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.FromHex("#FFF1F1");
                            DOB.Text = "";
                            DOB.TextColor = Color.FromHex("CC1417");
                            DOB.FontAttributes = FontAttributes.Bold;
                            DOBFrame.BorderColor = Color.FromHex("#CC1417");
                            DOBImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                            DOBImageFrame.BorderColor = Color.FromHex("#CC1417");
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
                        ((AddChildViewModel)BindingContext).DOBIsInvalid = true;
                        ((AddChildViewModel)BindingContext).IsDOBEmpty = true;
                    }
                    DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.FromHex("#FFF1F1");
                    DOB.Text = "";
                    DOB.TextColor = Color.FromHex("CC1417");
                    DOB.FontAttributes = FontAttributes.Bold;
                    DOBFrame.BorderColor = Color.FromHex("#CC1417");
                    DOBImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                    DOBImageFrame.BorderColor = Color.FromHex("#CC1417");
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
                                    ((AddChildViewModel)BindingContext).DOBIsInvalid = true;
                                    ((AddChildViewModel)BindingContext).IsDOBEmpty = true;
                                }
                                DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.FromHex("#FFF1F1");
                                DOB.Text = "";
                                DOB.TextColor = Color.FromHex("CC1417");
                                DOB.FontAttributes = FontAttributes.Bold;
                                DOBFrame.BorderColor = Color.FromHex("#CC1417");
                                DOBImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                                DOBImageFrame.BorderColor = Color.FromHex("#CC1417");
                            }
                            else
                            {
                                if (this.BindingContext != null)
                                {
                                    ((AddChildViewModel)BindingContext).DOBIsInvalid = false;
                                    ((AddChildViewModel)BindingContext).IsDOBEmpty = false;
                                    var month1 = month < 10 ? ("0" + month) : (month + "");
                                    var day1 = day < 10 ? "0" + day : day + "";
                                    var year1 = year;
                                    DOB.Text = month1 + "/" + day1 + "/" + year1;

                                    DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.White;
                                    DOB.TextColor = Colors.LightGrayColor;
                                    DOB.FontAttributes = FontAttributes.None;
                                    DOBFrame.BorderColor = Color.FromHex("#898D8D");
                                    DOBImageFrame.BackgroundColor = Color.FromHex("147cbd");
                                    DOBImageFrame.BorderColor = Color.FromHex("#898D8D");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (this.BindingContext != null)
                            {
                                ((AddChildViewModel)BindingContext).DOBIsInvalid = true;
                                ((AddChildViewModel)BindingContext).IsDOBEmpty = true;
                            }
                            DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.FromHex("#FFF1F1");
                            DOB.Text = "";
                            DOB.TextColor = Color.FromHex("CC1417");
                            DOB.FontAttributes = FontAttributes.Bold;
                            DOBFrame.BorderColor = Color.FromHex("#CC1417");
                            DOBImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                            DOBImageFrame.BorderColor = Color.FromHex("#CC1417");
                        }
                    }
                }
            }
        }

        private void DOB_Focused(object sender, FocusEventArgs e)
        {

        }
        private void DOB_Unfocused(object sender, FocusEventArgs e)
        {
            DOB_TextChanged(sender, null);
            var entryText = (sender as Entry)?.Text;
            if (!string.IsNullOrEmpty(entryText) && entryText.Trim().Length < 10)
            {
                if (this.BindingContext != null)
                {
                    ((AddChildViewModel)BindingContext).DOBIsInvalid = true;
                    ((AddChildViewModel)BindingContext).IsDOBEmpty = false;
                }
                DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.FromHex("#FFF1F1");
                DOB.Text = "";
                DOB.TextColor = Color.FromHex("CC1417");
                DOB.FontAttributes = FontAttributes.Bold;
                DOBFrame.BorderColor = Color.FromHex("#CC1417");
                DOBImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                DOBImageFrame.BorderColor = Color.FromHex("#CC1417");
            }
        }
        private void AddGeneralInfoView_BindingContextChanged(object sender, EventArgs e)
        {
            if (BindingContext != null)
            {
                var viewmodel = (BindingContext as AddChildViewModel);
                ((AddChildViewModel)BindingContext).PropertyChanged -= AddGeneralInfoView_PropertyChanged;
                ((AddChildViewModel)BindingContext).PropertyChanged += AddGeneralInfoView_PropertyChanged;
                if (viewmodel != null)
                    viewmodel.ReloadAction = () =>
                    {
                        DOB.Text = "";
                        EnrollmentDate.Text = "";
                        Parent1EmailErrorLabel.IsVisible = false;
                        Parent1EmailFrame.BorderColor = Colors.BorderColor;
                        Parent2EmailErrorLabel.IsVisible = false;
                        Parent2EmailFrame.BorderColor = Colors.BorderColor;
                        DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.White;
                        DOB.Text = "";
                        DOB.TextColor = Colors.LightGrayColor;
                        DOB.FontAttributes = FontAttributes.None;

                        DOBFrame.BorderColor = Color.FromHex("#898D8D");
                        DOBImageFrame.BackgroundColor = Color.FromHex("147cbd");
                        DOBImageFrame.BorderColor = Color.FromHex("#898D8D");
                        ((AddChildViewModel)BindingContext).IsDOBEmpty = false;
                        ((AddChildViewModel)BindingContext).DOBIsInvalid = false;
                        ((AddChildViewModel)BindingContext).EnrollmentDateIsInvalid = false;
                        ((AddChildViewModel)BindingContext).IsEnrollmentEmpty = false;
                        ((AddChildViewModel)BindingContext).IsEnrollmentEmpty = false;
                        EnrollmentStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.White;
                        EnrollmentDate.Text = "";
                        EnrollmentDate.TextColor = Colors.LightGrayColor;
                        EnrollmentDate.FontAttributes = FontAttributes.None;
                        EnrollmentFrame.BorderColor = Color.FromHex("#898D8D");
                        EnrollmentImageFrame.BackgroundColor = Color.FromHex("147cbd");
                        EnrollmentImageFrame.BorderColor = Color.FromHex("#898D8D");
                    };
            }
        }
        private void Parent2EmailField_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (BorderlessEntry)sender;

            var viewModel = (AddChildViewModel)BindingContext;
            if (entry != null && !string.IsNullOrEmpty(entry.Text))
            {
                try
                {
                    if (!RegexUtilities.IsValidEmail(entry.Text)) throw new Exception();
                    Parent2EmailErrorLabel.IsVisible = false;
                    Parent2EmailFrame.BorderColor = Colors.BorderColor;
                }
                catch
                {
                    Parent2EmailFrame.BorderColor = Colors.RedColor;
                    Parent2EmailErrorLabel.IsVisible = true;
                    Parent2EmailErrorLabel.Text = "Invalid e-mail address. Please reenter.";
                }
            }
            else
            {
                Parent2EmailErrorLabel.IsVisible = false;
                Parent2EmailFrame.BorderColor = Colors.BorderColor;
            }
            if (viewModel.ParentEmailerror)
            {
                Parent1EmailErrorLabel.IsVisible = true;
                Parent1EmailErrorLabel.Text = "Parent 1 Email and Parent 2 Email should not be the same.";
            }
            else
            {
                if (isInvalid)
                {
                    Parent1EmailFrame.BorderColor = Colors.RedColor;
                    Parent1EmailErrorLabel.IsVisible = true;
                    Parent1EmailErrorLabel.Text = "Invalid e-mail address. Please reenter.";
                }
                else
                {
                    if (!isInvalid)
                    {
                        Parent1EmailErrorLabel.IsVisible = false;
                        Parent1EmailFrame.BorderColor = Colors.BorderColor;
                    }
                    else
                        Parent1EmailErrorLabel.IsVisible = true;
                }

            }

        }
        private void Parent1EmailField_Unfocused(object sender, FocusEventArgs e)
        {

            var entry = (BorderlessEntry)sender;
            var viewModel = (AddChildViewModel)BindingContext;
            if (entry != null && !string.IsNullOrEmpty(entry.Text))
            {
                try
                {
                    if (!RegexUtilities.IsValidEmail(entry.Text)) throw new Exception();
                    Parent1EmailErrorLabel.Text = "Parent 1 Email and Parent 2 Email should not be the same.";
                    viewModel.Parent1EmailBorderColor = Colors.BorderColor;
                    viewModel.Parent1EmailTextColor = Colors.BorderColor;
                    Parent1EmailErrorLabel.IsVisible = false;
                    isInvalid = false;
                }
                catch
                {
                    viewModel.Parent1EmailBorderColor = Colors.RedColor;
                    viewModel.Parent1EmailTextColor = Colors.RedColor;
                    Parent1EmailErrorLabel.IsVisible = true;
                    Parent1EmailErrorLabel.Text = "Invalid e-mail address. Please reenter.";
                    isInvalid = true;
                }
            }
            else
            {
                Parent1EmailErrorLabel.IsVisible = false;
                Parent1EmailFrame.BorderColor = Colors.BorderColor;
                isInvalid = false;
            }
            if (viewModel.ParentEmailerror)
            {
                Parent1EmailErrorLabel.IsVisible = true;
                Parent1EmailErrorLabel.Text = "Parent 1 Email and Parent 2 Email should not be the same.";
                isInvalid = false;
            }
            else
            {
                if (!isInvalid)
                {
                    Parent1EmailErrorLabel.IsVisible = false;
                    Parent1EmailFrame.BorderColor = Colors.BorderColor;
                }
                else
                {
                    Parent1EmailErrorLabel.IsVisible = true;
                }

            }

        }
        private void GenderTapped(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as AddChildViewModel)?.GenderSelectionCommand.Execute(e.Item);
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentField = new WeakReference(sender);
        }
        #endregion

        #region GestureHandlers

        void OpenDob(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                DOBpicker.ShowDatePicker();
            }
            else
            {
                DOBpicker.Focus();
            }
        }
        public void SelectGender_Clicked(object sender, EventArgs e)
        {
            (BindingContext as AddChildViewModel)?.SelectGenderCommand.Execute(new object());
        }
        void OpenEnrollment(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                EnrollmentPicker.ShowDatePicker();
            }
            else
            {
                EnrollmentPicker.Focus();
            }
        }
        #endregion

        public void SaveChildTapped(object sender, EventArgs e)
        {
            string dob = null, enrollmentDate = null;

            var viewModel = (AddChildViewModel)BindingContext;
            if (viewModel.ParentEmailerror || Parent1EmailErrorLabel.IsVisible) return;

            if (!string.IsNullOrEmpty(DOB.Text) && !((AddChildViewModel)BindingContext).DOBIsInvalid)
            {
                dob = DOB.Text;
            }
            if (!string.IsNullOrEmpty(EnrollmentDate.Text) && !((AddChildViewModel)BindingContext).EnrollmentDateIsInvalid)
            {
                enrollmentDate = EnrollmentDate.Text;
            }
            viewModel.SaveChildClicked(dob, enrollmentDate);
        }

        #region OverriderMethods
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (height > width)
            {
                if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                {
                    parent1name.FontSize = parent1email.FontSize = parent2name.FontSize = parent2email.FontSize = 12;
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                {
                    parent1name.FontSize = parent1email.FontSize = parent2name.FontSize = parent2email.FontSize = 13.5;
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (this.BindingContext != null)
            {
                ((AddChildViewModel)BindingContext).ShowGender = false;
                ((AddChildViewModel)BindingContext).ShowLocation = false;
                ((AddChildViewModel)BindingContext)?.UnCoverCommand.Execute(new object());
            }
        }

        #endregion

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            (BindingContext as AddChildViewModel)?.SelectLocationCommand.Execute(new object());
        }

        private void Level1ImageButton_Clicked(object sender, EventArgs e)
        {

            var clickedCell = (sender as ImageButton)?.BindingContext as LocationNew;
            var locations = (BindingContext as AddChildViewModel)?.LocationsObservableCollection;
            var subLocations = ((sender as ImageButton)?.BindingContext as LocationNew)?.SubLocations.OrderByDescending(l => l.LocationName);

            if (subLocations == null) return;
            foreach (var item in subLocations)
            {
                if (locations != null && !locations.Contains(item))
                {
                    var index = locations.IndexOf(clickedCell) + 1;
                    if (clickedCell != null) clickedCell.IsExpanded = true;
                    (BindingContext as AddChildViewModel)?.LocationsObservableCollection.Insert(index, item);
                }
                else
                {
                    if (item.SubLocations != null)
                    {
                        foreach (var subItem in item.SubLocations)
                        {
                            if (subItem.SubLocations != null)
                            {
                                foreach (var subItem2 in subItem.SubLocations)
                                {
                                    if (subItem2.SubLocations != null)
                                    {
                                        foreach (var subItem3 in subItem2.SubLocations)
                                        {
                                            (BindingContext as AddChildViewModel)?.LocationsObservableCollection.Remove(subItem3);
                                        }
                                    }
                                    subItem2.IsExpanded = false;
                                    (BindingContext as AddChildViewModel)?.LocationsObservableCollection.Remove(subItem2);
                                }
                            }
                            subItem.IsExpanded = false;
                            (BindingContext as AddChildViewModel)?.LocationsObservableCollection.Remove(subItem);
                        }
                    }
                    item.IsExpanded = false;
                    if (clickedCell != null) clickedCell.IsExpanded = false;
                    (BindingContext as AddChildViewModel)?.LocationsObservableCollection.Remove(item);

                }
            }
        }

        private void LocationListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (LocationNew)e.SelectedItem;
            var locations = (BindingContext as AddChildViewModel)?.LocationsObservableCollection;
            if (item != null)
            {
                if (!item.IsSelected)
                    if (locations != null)
                        foreach (var selectedItem in locations)
                        {
                            selectedItem.IsSelected = false;
                        }

                if (locations != null && locations.Contains(item))
                {
                    item.IsSelected = !item.IsSelected;
                    if (item.IsSelected)
                    {
                        LocationEntry.Text = item.LocationName;
                        ((AddChildViewModel)BindingContext).SelectedLocation = new Location
                        {
                            LocationId = item.LocationId,
                            LocationName = item.LocationName
                        };
                        (BindingContext as AddChildViewModel)?.SelectLocationCommand.Execute(new object());
                    }
                    else
                    {
                        LocationEntry.Text = "Select location";
                        ((AddChildViewModel)BindingContext).SelectedLocation = null;
                    }
                }
            }
            locationListview.SelectedItem = null;
        }
    }
}
