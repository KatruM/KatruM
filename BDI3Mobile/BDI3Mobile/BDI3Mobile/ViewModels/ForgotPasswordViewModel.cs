using Acr.UserDialogs;
using BDI3Mobile.Common;
using BDI3Mobile.Helpers;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDI3Mobile.ViewModels
{
    public class ForgotPasswordViewModel : BaseclassViewModel
    {
        #region Properties
        private BDIWebServices services;
        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                if (!string.IsNullOrEmpty(value) && value.Trim().Length > 0 && !string.IsNullOrEmpty(ErrorText) && ErrorText.Length > 0)
                {
                    UserNameBorderColor = Color.FromHex("#898d8d");
                    UserNamePlaceHolderColor = Color.FromHex("#262626");
                    UserNameColor = Color.FromHex("#262626");
                    Backcolor = Color.White;
                    ErrorText = "";
                    backcolor = Color.White;
                }
                OnPropertyChanged(nameof(UserName));
            }
        }

        public Color userNameColor;
        public Color UserNameColor
        {
            get
            {
                return userNameColor;
            }
            set
            {
                userNameColor = value;
                OnPropertyChanged(nameof(UserNameColor));
            }
        }

        private Color userNameBorderColor = Color.FromHex("#898d8d");
        public Color UserNameBorderColor
        {
            get { return userNameBorderColor; }
            set
            {
                if (value == Color.FromHex("#898d8d"))
                {
                    ErroAttribute = FontAttributes.None;
                    IsEnableSendEmail = true;
                    MailBtnColor = Common.Colors.BlueColor;
                }
                else
                {
                    ErroAttribute = FontAttributes.Bold;
                    IsEnableSendEmail = false;
                    MailBtnColor = Common.Colors.Silver;
                }
                userNameBorderColor = value;
                OnPropertyChanged(nameof(UserNameBorderColor));
            }
        }
        private Color backcolor = Color.White;
        public Color Backcolor
        {
            get { return backcolor; }
            set
            {
                backcolor = value;
                OnPropertyChanged(nameof(Backcolor));
            }
        }

        private Color userNamePlaceHolderColor = Color.FromHex("#262626");
        public Color UserNamePlaceHolderColor
        {
            get { return userNamePlaceHolderColor; }
            set
            {
                userNamePlaceHolderColor = value;
                OnPropertyChanged(nameof(UserNamePlaceHolderColor));
            }
        }

        private string errorText;
        public string ErrorText
        {
            get { return errorText; }
            set
            {
                errorText = value;
                OnPropertyChanged(nameof(ErrorText));
            }
        }
        private bool isEnableSendEmail = true;
        public bool IsEnableSendEmail
        {
            get { return isEnableSendEmail; }
            set
            {
                isEnableSendEmail = value;
                OnPropertyChanged(nameof(IsEnableSendEmail));
            }
        }
        private Color mailBtnColor = Common.Colors.BlueColor;
        public Color MailBtnColor
        {
            get { return mailBtnColor; }
            set
            {
                mailBtnColor = value;
                OnPropertyChanged(nameof(MailBtnColor));
            }
        }

        private FontAttributes errorAttribute;
        public FontAttributes ErroAttribute
        {
            get { return errorAttribute; }
            set
            {
                errorAttribute = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Ctor
        public ForgotPasswordViewModel()
        {
            services = new BDIWebServices();
            UserNameColor = Color.FromHex("#262626");
        }
        #endregion

        #region Commands
        public ICommand CancelCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PopModalAsync();
                });
            }
        }
        public ICommand SendEmailCommand
        {
            get
            {
                return new Command(async () =>
                {
                    ErrorText = "";
                    if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    {
                        ErrorText = ErrorMessages.NetworkErrorMessage;
                        return;
                    }
                    if (string.IsNullOrEmpty(UserName) || UserName.Trim().Length == 0)
                    {
                        UserNameBorderColor = Color.FromHex("#cc1416");
                        UserNamePlaceHolderColor = Color.FromHex("#ba0e10");
                        ErrorText = "The Username field is required.";
                        Backcolor = Color.FromHex("#FFF1F1");
                        return;
                    }

                    UserDialogs.Instance.ShowLoading("Sending Email...", Acr.UserDialogs.MaskType.Black);
                    var response = await services.ForGotPassWord(new ForgotPassModel() { username = UserName, productId = 10 });
                    UserDialogs.Instance.HideLoading();
                    if (response != "Success")
                    {
                        UserNameColor = Color.FromHex("#ba0e10");
                        UserNameBorderColor = Color.FromHex("#cc1416");
                        UserNamePlaceHolderColor = Color.FromHex("#ba0e10");
                        Backcolor = Color.FromHex("#FFF1F1");
                    }
                    var message = response == "Success" ? "Email Sent!" : "Unable to send password.";
                    ErrorText = message;
                    return;
                });
            }
        }
        #endregion
    }
    public class ForgotPassModel
    {
        public string username { get; set; }
        public int productId { get; set; }
    }
}
