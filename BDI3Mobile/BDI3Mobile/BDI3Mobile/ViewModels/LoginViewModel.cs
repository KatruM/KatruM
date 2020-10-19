using System;
using System.Collections.Generic;
using System.Windows.Input;
using BDI3Mobile.View;
using Xamarin.Forms;
using BDI3Mobile.Helpers;
using Xamarin.Essentials;
using BDI3Mobile.Common;
using Acr.UserDialogs;
using System.Threading.Tasks;
using BDI3Mobile.IServices;
using System.Linq;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.Models;
using Rg.Plugins.Popup.Services;
using BDI3Mobile.Models.Requests;
using BDI3Mobile.Models.Responses;
using System.IO;
using BDI3Mobile.Views.PopupViews;
using Newtonsoft.Json.Linq;
using PCLStorage;
using System.Reflection;
using FFImageLoading;
using System.IO.Compression;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BDI3Mobile.ViewModels
{
    public class LoginViewModel : BaseclassViewModel
    {
        private readonly BDIWebServices bDIWebServices;
        private readonly IStudentCommonDataService studentCommonDataService = DependencyService.Get<IStudentCommonDataService>();
        private readonly IUserSyncService userSyncService = DependencyService.Get<IUserSyncService>();
        private readonly ICreateHtmlFiles createHtmlFiles = DependencyService.Get<ICreateHtmlFiles>();
        private readonly ICommonDataService commonDataService;
        private readonly IContentBasalCeilingsService contentBasalCeilingsService;
        private readonly IProductResearchCodeValuesService _productResearchCodeValuesService;
        private readonly IProductResearchCodesService _productResearchCodesService;
        private readonly IUsersService _userService;
        private readonly IMembershipService _membershipService;
        private readonly ITokenService _tokenService;
        private readonly IStudentsService _studentService;
        private readonly IProductService _productService;
        private readonly IAssessmentsService _assessmentService;
        private readonly IContentCategoryLevelsService _contentCategoryLevelsService;
        private readonly IContentCategoryService _contentCategoryService;
        private readonly IContentCategoryItemsService _contentCategoryItemsService;
        private readonly IContentItemsService _contentItemService;
        private readonly IContentItemAttributesService _contentItemAttributeService;
        private readonly IContentRubricsService _contentRubricsService;
        private readonly IContentRubricPointsService _contentRubicPointsService;
        private readonly IContentItemTallyService _contentItemTalliesService;
        private readonly IContentItemTalliesScoresService _contentItemTalliesScoresService;
        private readonly IContentGroupService _contentGroupService;
        private readonly IContentGroupItemsService _contentGroupItemsService;
        private readonly ILocationService _locationService;
        private readonly IProgramNoteService _programNoteService;
        private readonly IExaminerService _examinerService;
        private readonly IUserLastConnectionActivityService _userConnectionLogService;
        private readonly IOrgRecordFormService _orgRecordFormService;
        private readonly IPermissionService _permissionService;
        private readonly IUserPermissionService _userPermissionService;

        private bool isCheckedChanged;
        public bool IsCheckedChanged
        {
            get
            {
                return isCheckedChanged;
            }
            set
            {
                isCheckedChanged = value;
                RememberMeChanged();
                OnPropertyChanged(nameof(IsCheckedChanged));
            }


        }
        private string userid;
        public string UserID
        {
            get
            {
                return userid;
            }
            set
            {
                userid = value;
                if (!string.IsNullOrEmpty(value) && value.Trim().Length > 0)
                {
                    UserNameBorderColor = Color.FromHex("#6A6D6D");
                    UserNamePlaceHolderColor = Color.FromHex("#262626");
                    UserNameErrorText = "";
                }
                OnPropertyChanged(nameof(UserID));
            }
        }
        private string password;
        public string PassID
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                if (!string.IsNullOrEmpty(value) && value.Trim().Length > 0 && !string.IsNullOrEmpty(PasswordErrorText) && PasswordErrorText.Length > 0)
                {
                    PasswordPlaceHolderColor = Color.FromHex("#262626");
                    PasswordBorderColor = Color.FromHex("#6A6D6D");
                }
                if (!string.IsNullOrEmpty(PassID) && PassID.Trim().Length > 0)
                    PasswordErrorText = "";
                OnPropertyChanged(nameof(PassID));

            }

        }
        private bool inActivityErrorMessageVisible = false;
        public bool InActivityErrorMessageVisible
        {
            get { return inActivityErrorMessageVisible; }
            set
            {
                if (value == true) { ShowSessionLogoutPopup(); }
                inActivityErrorMessageVisible = value;
                OnPropertyChanged(nameof(InActivityErrorMessageVisible));
            }
        }
        public ICommand LoginUserCommand { get; set; }
        private bool userTappedForgotPassword;
        public Action ClearContent { get; set; }
        public LoginViewModel()
        {
            bDIWebServices = DependencyService.Get<BDIWebServices>();
            commonDataService = DependencyService.Get<ICommonDataService>();
            _locationService = DependencyService.Get<ILocationService>();
            _userService = DependencyService.Get<IUsersService>();
            _membershipService = DependencyService.Get<IMembershipService>();
            _tokenService = DependencyService.Get<ITokenService>();
            _studentService = DependencyService.Get<IStudentsService>();
            _productService = DependencyService.Get<IProductService>();
            _assessmentService = DependencyService.Get<IAssessmentsService>();
            _contentCategoryLevelsService = DependencyService.Get<IContentCategoryLevelsService>();
            _productResearchCodesService = DependencyService.Get<IProductResearchCodesService>();
            _productResearchCodeValuesService = DependencyService.Get<IProductResearchCodeValuesService>();
            _contentCategoryService = DependencyService.Get<IContentCategoryService>();
            _contentCategoryItemsService = DependencyService.Get<IContentCategoryItemsService>();
            _programNoteService = DependencyService.Get<IProgramNoteService>();
            _examinerService = DependencyService.Get<IExaminerService>();
            _contentItemService = DependencyService.Get<IContentItemsService>();
            _contentItemAttributeService = DependencyService.Get<IContentItemAttributesService>();
            _contentRubricsService = DependencyService.Get<IContentRubricsService>();
            _contentRubicPointsService = DependencyService.Get<IContentRubricPointsService>();
            _contentItemTalliesService = DependencyService.Get<IContentItemTallyService>();
            _contentItemTalliesScoresService = DependencyService.Get<IContentItemTalliesScoresService>();
            _contentGroupService = DependencyService.Get<IContentGroupService>();
            _contentGroupItemsService = DependencyService.Get<IContentGroupItemsService>();
            _userConnectionLogService = DependencyService.Get<IUserLastConnectionActivityService>();
            _orgRecordFormService = DependencyService.Get<IOrgRecordFormService>();
            contentBasalCeilingsService = DependencyService.Get<IContentBasalCeilingsService>();
            _permissionService = DependencyService.Get<IPermissionService>();
            _userPermissionService = DependencyService.Get<IUserPermissionService>();

            if (Application.Current.Properties.ContainsKey("Name"))
            {
                if (Application.Current.Properties["Name"] != null)
                {
                    UserID = Application.Current.Properties["Name"].ToString();
                }
            }

            if (Application.Current.Properties.ContainsKey("Password"))
            {
                if (Application.Current.Properties["Password"] != null)
                {
                    PassID = Application.Current.Properties["Password"].ToString();
                }
            }

            if (Application.Current.Properties.ContainsKey("RememberMe"))
            {
                var isChecked = Application.Current.Properties["RememberMe"].ToString();
                if (isChecked == "True")
                {
                    IsCheckedChanged = true;
                }
                else if (isChecked == "False")
                {
                    isCheckedChanged = false;
                }
            }
            LoginUserCommand = new Command(LoginUser);

            async void LoginUser()
            {
                try
                {
                    DateTime d1 = DateTime.Now;
                    //Microsoft.AppCenter.Crashes.ErrorReport crashReport = await Microsoft.AppCenter.Crashes.Crashes.GetLastSessionCrashReportAsync();
                    // var cra = Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                    //ErrorReport crashReport = await Crashes.GetLastSessionCrashReportAsync();
                    UserNameErrorText = "";
                    PasswordErrorText = "";
                    if (string.IsNullOrEmpty(UserID) || UserID.Trim().Length == 0)
                    {
                        UserNameBorderColor = Color.FromHex("#cc1416");
                        UserNamePlaceHolderColor = Color.FromHex("#ba0e10");
                        UserNameErrorText = "The Username field is required.";
                    }

                    if (string.IsNullOrEmpty(PassID) || PassID.Trim().Length == 0)
                    {
                        PasswordBorderColor = Color.FromHex("#cc1416");
                        PasswordPlaceHolderColor = Color.FromHex("#ba0e10");
                        PasswordErrorText = "The Password field is required.";
                    }

                    if ((!string.IsNullOrEmpty(UserNameErrorText)) || (!string.IsNullOrEmpty(PasswordErrorText)))
                    {
                        return;
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.ShowLoading("Authenticating...", Acr.UserDialogs.MaskType.Black);
                    });
                    //

                    if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    {
                        var userName = UserID;
                        var password = PassID;
                        bool userFound = false;

                        var users = _userService.GetUsers();
                        if (users.Count > 0)
                        {

                            var user = users.FirstOrDefault(s => s.UserName.ToLower() == userName.ToLower());
                            if (user != null)
                            {
                                var memberships = _membershipService.GetMemberships();
                                var membershipForUser = memberships.FirstOrDefault(s => s.UserId == user.UserId);
                                if (membershipForUser != null)
                                {
                                    var decryptedPassword = CryptographicHelper.Decrypt(membershipForUser.Password, EncryptionKeys.GUID);
                                    if (decryptedPassword == password)
                                    {
                                        Application.Current.Properties["PassID"] = PassID;
                                        Application.Current.Properties["UserName"] = UserID;
                                        Application.Current.Properties["FirstName"] = user.FirstName;
                                        Application.Current.Properties["LastName"] = user.LastName;
                                        Application.Current.Properties["UserID"] = user.UserId;
                                        Application.Current.Properties["OrgnazationID"] = user.OrganizationID;
                                        Application.Current.Properties["Email"] = user.EmailAddress;
                                        Application.Current.Properties["UserTypeID"] = user.UserTypeID;
                                        //If we call the below method above the property assigning, it would throw an exeception in the OrgID somewhere it need for and it won't able login in offline.
                                        await GenerateDataAndMenu();

                                        var data = studentCommonDataService.GetStudentCommonData();
                                        BDI3Mobile.Models.Common.CommonData commonData = new BDI3Mobile.Models.Common.CommonData();
                                        if (data != null && data.Any())
                                        {
                                            commonData.FundingSources = data.Where(p => p.Type == 0).ToList();
                                            commonData.Races = data.Where(p => p.Type == 1).ToList();
                                            commonData.Languages = data.Where(p => p.Type == 2).ToList();
                                            commonData.Diagnoses = data.Where(p => p.Type == 3).ToList();
                                        }
                                        commonDataService.Races = new List<Race>();
                                        commonDataService.fundingSources = new List<FundingSource>();
                                        commonDataService.PrimaryDiagnostics = new List<Diagnostics>();
                                        commonDataService.SecondaryDiagnostics = new List<Diagnostics>();
                                        commonDataService.Languages = new List<Language>();

                                        if (commonData.Races != null && commonData.Races.Any())
                                        {
                                            var races = new List<Race>();
                                            foreach (var item in commonData.Races)
                                            {
                                                races.Add(new Race() { text = item.Text, value = item.Value });
                                            }
                                            commonDataService.Races = races;
                                        }
                                        if (commonData.FundingSources != null && commonData.FundingSources.Any())
                                        {
                                            var fundingSources = new List<FundingSource>();
                                            foreach (var item in commonData.FundingSources)
                                            {
                                                fundingSources.Add(new FundingSource() { text = item.Text, value = item.Value });
                                            }
                                            commonDataService.fundingSources = fundingSources;
                                        }
                                        if (commonData.Diagnoses != null && commonData.Diagnoses.Any())
                                        {
                                            var pDiagnoStics = new List<Diagnostics>();
                                            foreach (var item in commonData.Diagnoses)
                                            {
                                                pDiagnoStics.Add(new Diagnostics() { text = item.Text, value = item.Value });
                                            }
                                            commonDataService.PrimaryDiagnostics = pDiagnoStics;
                                        }
                                        if (commonData.Diagnoses != null && commonData.Diagnoses.Any())
                                        {
                                            var sDiagnoStics = new List<Diagnostics>();
                                            foreach (var item in commonData.Diagnoses)
                                            {
                                                sDiagnoStics.Add(new Diagnostics() { text = item.Text, value = item.Value });
                                            }
                                            commonDataService.SecondaryDiagnostics = sDiagnoStics;
                                        }
                                        if (commonData.Languages != null && commonData.Languages.Any())
                                        {
                                            var Languages = new List<Language>();
                                            foreach (var item in commonData.Languages)
                                            {
                                                Languages.Add(new Language() { text = item.Text, value = item.Value });
                                            }
                                            commonDataService.Languages = Languages;
                                        }
                                        userFound = true;
                                        await CreateHtmlFolders();
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            UserDialogs.Instance.HideLoading();
                                        });
                                        ClearContent?.Invoke();
                                        Application.Current.MainPage = new Views.DashboardHomeView();
                                    }
                                }
                            }
                        }
                        if (!userFound)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                UserDialogs.Instance.HideLoading();
                            });

                            PasswordErrorText = ErrorMessages.NetworkErrorMessage;
                            UserNameErrorText = "";
                            return;
                        }
                    }
                    else
                    {
                        var response = await bDIWebServices.LoginUser(new { username = UserID, password = PassID });
                        if (response != null)
                        {
                            if (!string.IsNullOrEmpty(response.StatusCode))
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    UserDialogs.Instance.HideLoading();
                                });

                                UserID = "";
                                PassID = "";
                                UserNameBorderColor = Color.FromHex("#cc1416");
                                UserNamePlaceHolderColor = Color.FromHex("#ba0e10");
                                PasswordBorderColor = Color.FromHex("#cc1416");
                                PasswordPlaceHolderColor = Color.FromHex("#ba0e10");
                                PasswordErrorText = "Username and/or Password is incorrect";
                                return;
                            }
                            else
                            {

                                var userResponse = _tokenService.GetTokenResposne();
                                int uID, organizationID;
                                bool isSuccess = int.TryParse(userResponse.UserID, out uID);
                                bool isOrganizationIDSuccess = int.TryParse(userResponse.OrganizationID, out organizationID);
                                if (isSuccess)
                                {
                                    Application.Current.Properties["PassID"] = PassID;
                                    Application.Current.Properties["UserName"] = UserID;
                                    Application.Current.Properties["UserID"] = uID;
                                    Application.Current.Properties["OrgnazationID"] = organizationID;
                                    Application.Current.Properties["Email"] = userResponse.Email;
                                    Application.Current.Properties["FirstName"] = userResponse.FirstName;
                                    Application.Current.Properties["LastName"] = userResponse.LastName;
                                    Application.Current.Properties["UserTypeID"] = userResponse.utid;

                                    var offlineUser = _userService.GetUserByUserName(UserID);
                                    if (offlineUser == null)
                                    {
                                        UserDialogs.Instance.HideLoading();
                                        var downloadSuccess = await CheckFirstTimeAndDownload();
                                        if (!downloadSuccess)
                                        {
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        await GenerateDataAndMenu();
                                        var data = studentCommonDataService.GetStudentCommonData();
                                        BDI3Mobile.Models.Common.CommonData commonData = new BDI3Mobile.Models.Common.CommonData();
                                        if (data != null && data.Any())
                                        {
                                            commonData.FundingSources = data.Where(p => p.Type == 0).ToList();
                                            commonData.Races = data.Where(p => p.Type == 1).ToList();
                                            commonData.Languages = data.Where(p => p.Type == 2).ToList();
                                            commonData.Diagnoses = data.Where(p => p.Type == 3).ToList();
                                        }
                                        commonDataService.Races = new List<Race>();
                                        commonDataService.fundingSources = new List<FundingSource>();
                                        commonDataService.PrimaryDiagnostics = new List<Diagnostics>();
                                        commonDataService.SecondaryDiagnostics = new List<Diagnostics>();
                                        commonDataService.Languages = new List<Language>();

                                        if (commonData.Races != null && commonData.Races.Any())
                                        {
                                            var races = new List<Race>();
                                            foreach (var item in commonData.Races)
                                            {
                                                races.Add(new Race() { text = item.Text, value = item.Value });
                                            }
                                            commonDataService.Races = races;
                                        }
                                        if (commonData.FundingSources != null && commonData.FundingSources.Any())
                                        {
                                            var fundingSources = new List<FundingSource>();
                                            foreach (var item in commonData.FundingSources)
                                            {
                                                fundingSources.Add(new FundingSource() { text = item.Text, value = item.Value });
                                            }
                                            commonDataService.fundingSources = fundingSources;
                                        }
                                        if (commonData.Diagnoses != null && commonData.Diagnoses.Any())
                                        {
                                            var pDiagnoStics = new List<Diagnostics>();
                                            foreach (var item in commonData.Diagnoses)
                                            {
                                                pDiagnoStics.Add(new Diagnostics() { text = item.Text, value = item.Value });
                                            }
                                            commonDataService.PrimaryDiagnostics = pDiagnoStics;
                                        }
                                        if (commonData.Diagnoses != null && commonData.Diagnoses.Any())
                                        {
                                            var sDiagnoStics = new List<Diagnostics>();
                                            foreach (var item in commonData.Diagnoses)
                                            {
                                                sDiagnoStics.Add(new Diagnostics() { text = item.Text, value = item.Value });
                                            }
                                            commonDataService.SecondaryDiagnostics = sDiagnoStics;
                                        }
                                        if (commonData.Languages != null && commonData.Languages.Any())
                                        {
                                            var Languages = new List<Language>();
                                            foreach (var item in commonData.Languages)
                                            {
                                                Languages.Add(new Language() { text = item.Text, value = item.Value });
                                            }
                                            commonDataService.Languages = Languages;
                                        }
                                    }
                                    var user = new Models.DBModels.Users()
                                    {
                                        UserId = uID,
                                        UserName = userResponse.UserName,
                                        FirstName = userResponse.FirstName,
                                        LastName = userResponse.LastName,
                                        EmailAddress = userResponse.Email,
                                        OrganizationID = organizationID,
                                        Phone = userResponse.Phone,
                                        UserTypeID = Convert.ToInt32(userResponse.utid)
                                    };
                                    _userService.InsertOrUpdate(user);
                                    var queryUser = _userService.GetUserByID(user.UserId);
                                    var encryptedPassword = CryptographicHelper.Encrypt(PassID, EncryptionKeys.GUID);
                                    _membershipService.InsertorUpdate(new Models.DBModels.Membership()
                                    {
                                        UserId = uID,
                                        Password = encryptedPassword,
                                    });
                                }
                                if (!isCheckedChanged)
                                {
                                    Application.Current.Properties["Name"] = "";
                                    Application.Current.Properties["Password"] = "";
                                }
                                else
                                {
                                    Application.Current.Properties["Name"] = UserID;
                                    Application.Current.Properties["Password"] = PassID;
                                }
                                await CreateHtmlFolders();
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    UserDialogs.Instance.HideLoading();
                                    try
                                    {
                                        await PopupNavigation.Instance.PopAllAsync();
                                    }
                                    catch (Exception)
                                    {
                                    }
                                });
                                DateTime d2 = DateTime.Now;
                                TimeSpan d3 = d2.Subtract(d1);
                                Debug.WriteLine("Login time:" + d3);
                                ClearContent?.Invoke();
                                Application.Current.MainPage = new Views.DashboardHomeView();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        UserDialogs.Instance.HideLoading();
                        try
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                        catch (Exception)
                        {
                        }
                    });


                    Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                }
            }


            forgot = new Command(forg);

            void forg()
            {
                if (userTappedForgotPassword)
                    return;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var forgetPasswordView = new ForgotPasswordView();
                    forgetPasswordView.UName = UserID;
                    if (isCheckedChanged)
                    {
                        Application.Current.Properties["Name"] = UserID;
                        Application.Current.Properties["Password"] = PassID;
                    }
                    else
                    {
                        Application.Current.Properties["Name"] = UserID;
                        Application.Current.Properties["Password"] = "";
                    }

                    UserNameBorderColor = Color.FromHex("#898d8d");
                    UserNamePlaceHolderColor = Color.FromHex("#262626");
                    PasswordBorderColor = Color.FromHex("#898d8d");
                    PasswordPlaceHolderColor = Color.FromHex("#262626");
                    UserNameErrorText = "";
                    PasswordErrorText = "";
                    UserID = "";
                    PassID = "";

                    userTappedForgotPassword = true;
                    await App.Current.MainPage.Navigation.PushModalAsync(forgetPasswordView);
                    userTappedForgotPassword = false;
                });

            }
        }

        public List<BDI3Mobile.Models.DBModels.Location> GenerateSubLocations(IEnumerable<LocationResponseModel> subItems, List<Models.DBModels.Location> locations, int parentlocationid, int uid)
        {
            if (subItems != null && subItems.Any())
            {
                foreach (var item in subItems)
                {
                    var subLocation = new Models.DBModels.Location();
                    subLocation.isDeleted = item.isDeleted;
                    subLocation.updatedOn = item.updatedOn;
                    subLocation.DownloadedBy = uid;
                    subLocation.LocationId = item.value;
                    subLocation.LocationName = item.text;
                    subLocation.IsEnabled = item.enabled;
                    subLocation.ParentLocationId = item.parentLocationID;
                    subLocation.UserId = int.Parse(Application.Current.Properties["UserID"].ToString());
                    locations.Add(subLocation);
                    GenerateSubLocations(item.subItems, locations, item.value, uid);
                }
            }
            return null;
        }
        public ICommand remember { get; set; }
        public ICommand forgot { get; set; }

        public ICommand help => new Command<string>((url) => { Xamarin.Essentials.Launcher.TryOpenAsync(new System.Uri(url)); });
        public ICommand SearchErrorContinueCommand => new Command(async () => await PopupNavigation.Instance.PopAllAsync(false));

        #region ErrorMessages
        private Color userNameBorderColor = Color.FromHex("#898d8d");
        public Color UserNameBorderColor
        {
            get { return userNameBorderColor; }
            set
            {
                userNameBorderColor = value;
                OnPropertyChanged(nameof(UserNameBorderColor));
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

        private string userNameErrorText;
        public string UserNameErrorText
        {
            get { return userNameErrorText; }
            set
            {
                userNameErrorText = value;
                OnPropertyChanged(nameof(UserNameErrorText));
            }
        }

        private string passwordErrorText;
        public string PasswordErrorText
        {
            get { return passwordErrorText; }
            set
            {
                passwordErrorText = value;
                OnPropertyChanged(nameof(PasswordErrorText));
            }
        }

        private Color passwordBorderColor = Color.FromHex("#898d8d");
        public Color PasswordBorderColor
        {
            get { return passwordBorderColor; }
            set
            {
                passwordBorderColor = value;
                OnPropertyChanged(nameof(PasswordBorderColor));
            }
        }
        private Color passwordPlaceHolderColor = Color.FromHex("#262626");
        public Color PasswordPlaceHolderColor
        {
            get { return passwordPlaceHolderColor; }
            set
            {
                passwordPlaceHolderColor = value;
                OnPropertyChanged(nameof(PasswordPlaceHolderColor));
            }
        }
        #endregion
        private async void ShowSessionLogoutPopup()
        {
            var popup = new Views.PopupViews.CustomPopupView(new Views.PopupViews.CustomPopUpDetails() { Header = "Session Expired", Message = "You have been logged out due to inactivity.", Height = 211, Width = 450 });
            popup.BindingContext = this;
            popup.CloseWhenBackgroundIsClicked = false;
            await PopupNavigation.Instance.PushAsync(popup);
        }
        private void RememberMeChanged()
        {
            if (isCheckedChanged)
            {
                Application.Current.Properties["RememberMe"] = "True";
                if (UserID != null)
                {
                    Application.Current.Properties["Name"] = UserID;
                    Application.Current.Properties["Password"] = PassID;
                }
            }
            else
            {
                Application.Current.Properties["RememberMe"] = "False";
                Application.Current.Properties["Name"] = "";
                Application.Current.Properties["Password"] = "";
            }
        }

        private DateTime CovertToDateFromString(string obj)
        {
            if (obj == null)
            {
                return default(DateTime);
            }
            else
            {
                DateTime dateTime;
                var splittedDate = obj.Split('/');
                if (splittedDate != null && splittedDate.Any())
                {
                    return new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));
                }
                bool isSucess = DateTime.TryParse(obj, out dateTime);
                if (isSucess)
                {
                    return dateTime;
                }
            }
            return default(DateTime);
        }

        #region FirstTime and Download
        private async Task DeleteFolder(string foldername)
        {
            return;
            PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
            try
            {
                var itemsfolder = await rootFolder.GetFolderAsync("ItemsFolder");
                await itemsfolder.DeleteAsync();
            }
            catch (Exception)
            {
            }
        }
        public async Task CreateRubricHtmlFiles(List<ContentRubric> contentRubric)
        {
            PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
            if (contentRubric != null && contentRubric.Any())
            {
                var itemsfolder = await rootFolder.CreateFolderAsync("ContentRubric", PCLStorage.CreationCollisionOption.OpenIfExists);
                foreach (var item in contentRubric)
                {
                    if (string.IsNullOrEmpty(item.notes))
                    {
                        continue;
                    }
                    var itemfile = await itemsfolder.CreateFileAsync("rubric" + item.contentRubricId + ".html", PCLStorage.CreationCollisionOption.OpenIfExists);
                    var htmlstring = "";
                    var htmlSource = item.notes;
                    htmlSource = "<span>" + htmlSource + "</span>";
                    if (htmlSource.Contains("math"))
                    {
                        htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta http-equiv='Content-Type' charset='UTF-8' content='text/html;charset = UTF-8' name='viewport' content='width=device-width,initial-scale=1,maximum-scale=1'/></head>";
                        htmlstring += "<body><script type='text/javascript' src='es5/tex-mml-chtml.js' defer></script> <link rel='stylesheet' type='text/css' href='styles/record_forms.css'>";
                    }
                    else
                    {
                        htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta http-equiv='Content-Type' charset='UTF-8' content='text/html;charset = UTF-8' name='viewport' content='width=device-width,initial-scale=1,maximum-scale=1'/></head>";
                        htmlstring += "<body><link rel='stylesheet' type='text/css' href='styles/record_forms.css'>";
                    }
                    var finalString = htmlstring + htmlSource + "</body></html>";
                    File.WriteAllText(System.IO.Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, itemsfolder.Name, itemfile.Name), finalString);
                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        item.HtmlFilePath = "file://" + Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, itemsfolder.Name, itemfile.Name);
                    }
                    else if (DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        item.HtmlFilePath = itemsfolder.Name + "/" + itemfile.Name;
                    }
                    else
                    {
                        item.HtmlFilePath = "ms-appdata:///local/" + itemsfolder.Name + "/" + itemfile.Name;
                    }
                }
                _contentRubricsService.UpdateAll(contentRubric);
            }
        }
        public async Task CreateScoringHtmlFiles(List<ContentRubricPoint> contentRubricPoints)
        {
            PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
            await DeleteFolder("ScoringFolder");
            if (contentRubricPoints != null && contentRubricPoints.Any())
            {
                var itemsfolder = await rootFolder.CreateFolderAsync("ScoringFolder", CreationCollisionOption.OpenIfExists);
                foreach (var item in contentRubricPoints)
                {
                    var itemfile = await itemsfolder.CreateFileAsync("scoringContent" + item.contentRubricPointsId + ".html", PCLStorage.CreationCollisionOption.OpenIfExists);
                    var htmlstring = "";
                    var htmlSource = item.description;
                    htmlSource = "<span>" + htmlSource + "</span>";
                    if (htmlSource.Contains("math"))
                    {
                        htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta  name='viewport' charset='UTF-8' content='width=device-width,initial-scale=1,maximum-scale=1' http-equiv = 'content-type' content = 'text/html; charset=UTF-8'></head>";
                        htmlstring += "<body><script type='text/javascript' src='es5/tex-mml-chtml.js' defer></script> <link rel='stylesheet' type='text/css' href='styles/record_forms.css'>";
                    }
                    else
                    {
                        htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta  name='viewport' charset='UTF-8' content='width=device-width,initial-scale=1,maximum-scale=1' http-equiv = 'content-type' content = 'text/html; charset=UTF-8'></head>";
                        htmlstring += "<body><link rel='stylesheet' type='text/css' href='styles/record_forms.css'>";
                    }
                    var finalString = htmlstring + htmlSource + "</body></html>";
                    File.WriteAllText(Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, itemsfolder.Name, itemfile.Name), finalString);
                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        item.HtmlFilePath = "file://" + Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, itemsfolder.Name, itemfile.Name);
                    }
                    else if (DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        item.HtmlFilePath = itemsfolder.Name + "/" + itemfile.Name;
                    }

                    else
                    {
                        item.HtmlFilePath = "ms-appdata:///local/" + itemsfolder.Name + "/" + itemfile.Name;
                    }

                }
                _contentRubicPointsService.UpdateAll(contentRubricPoints);
            }
        }
        public async Task CreateItemHtmlFiles(List<ContentItem> lstContentItems)
        {
            if (lstContentItems != null && lstContentItems.Any())
            {
                PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                await DeleteFolder("ItemsFolder");
                try
                {
                    var itemsfolder = await rootFolder.CreateFolderAsync("ItemsFolder", PCLStorage.CreationCollisionOption.OpenIfExists);
                    foreach (var item in lstContentItems)
                    {
                        var itemfile = await itemsfolder.CreateFileAsync("itemcontent_" + item.contentItemId + ".html", PCLStorage.CreationCollisionOption.OpenIfExists);

                        var htmlstring = "";
                        var htmlSource = item.itemText;
                        htmlSource = "<span>" + htmlSource + "</span>";
                        if (htmlSource.Contains("math"))
                        {
                            htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta  name='viewport' charset='UTF-8' content='width=device-width,initial-scale=1,maximum-scale=1'/></head>";
                            htmlstring += "<body><script type='text/javascript' src='es5/tex-mml-chtml.js' defer></script> <link rel='stylesheet' type='text/css' href='styles/record_forms.css'>";
                        }
                        else
                        {
                            htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta  name='viewport' charset='UTF-8' content='width=device-width,initial-scale=1,maximum-scale=1'/></head>";
                            htmlstring += "<body><link rel='stylesheet' type='text/css' href='styles/record_forms.css'>";
                        }
                        var finalString = htmlstring + htmlSource + "</body></html>";
                        File.WriteAllText(Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, itemsfolder.Name, itemfile.Name), finalString);
                        if (DeviceInfo.Platform == DevicePlatform.Android)
                        {
                            item.HtmlFilePath = "file://" + Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, itemsfolder.Name, itemfile.Name);
                        }
                        else if (DeviceInfo.Platform == DevicePlatform.iOS)
                        {
                            item.HtmlFilePath = itemsfolder.Name + "/" + itemfile.Name;
                        }
                        else
                        {
                            item.HtmlFilePath = "ms-appdata:///local/" + itemsfolder.Name + "/" + itemfile.Name;
                        }
                    }
                    _contentItemService.UpdateAll(lstContentItems);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }

            }
        }
        public async Task CreateItemAttributeHtmlFiles(List<ContentItemAttribute> lstContentItemAttribute)
        {
            if (lstContentItemAttribute != null && lstContentItemAttribute.Any())
            {
                PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                await DeleteFolder("ContentItemAttribute");
                try
                {
                    var itemsfolder = await rootFolder.CreateFolderAsync("ContentItemAttribute", PCLStorage.CreationCollisionOption.OpenIfExists);
                    foreach (var item in lstContentItemAttribute)
                    {
                        if (item.name == "Image")
                        {
                            continue;
                        }
                        var itemfile = await itemsfolder.CreateFileAsync("itemcontentattribute_" + item.contentItemAttributeId + ".html", PCLStorage.CreationCollisionOption.OpenIfExists);

                        var htmlstring = "";
                        var htmlSource = item.value;
                        htmlSource = "<span>" + htmlSource + "</span>";
                        if (htmlSource.Contains("math"))
                        {
                            htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta name='viewport' content='initial-scale=1.0'/></head>";
                            htmlstring += "<body><script type='text/javascript' src='es5/tex-mml-chtml.js' defer></script> <link rel='stylesheet' type='text/css' href='styles/record_forms.css'>";
                        }
                        else
                        {
                            htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta name='viewport' content='initial-scale=1.0'/></head>";
                            htmlstring += "<body><link rel='stylesheet' type='text/css' href='styles/record_forms.css'>";
                        }
                        var finalString = htmlstring + htmlSource + "</body></html>";
                        File.WriteAllText(System.IO.Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, itemsfolder.Name, itemfile.Name), finalString);
                        if (DeviceInfo.Platform == DevicePlatform.Android)
                        {
                            item.HtmlFilePath = "file://" + Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, itemsfolder.Name, itemfile.Name);
                        }
                        else if (DeviceInfo.Platform == DevicePlatform.iOS)
                        {
                            item.HtmlFilePath = itemsfolder.Name + "/" + itemfile.Name;
                        }
                        else
                        {
                            item.HtmlFilePath = "ms-appdata:///local/" + itemsfolder.Name + "/" + itemfile.Name;
                        }
                    }
                    _contentItemAttributeService.UpdateAll(lstContentItemAttribute);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }

            }
        }
        private string downloadProgressMessage;
        public string DownloadProgressMessage
        {
            get
            {
                return downloadProgressMessage;
            }
            set
            {
                downloadProgressMessage = value;
                OnPropertyChanged(nameof(DownloadProgressMessage));
            }
        }
        public async Task<bool> CheckFirstTimeAndDownload()
        {
            DateTime d1 = DateTime.Now;
            UserDialogs.Instance.HideLoading();
            await PopupNavigation.Instance.PushAsync(new SyncingPopupView() { BindingContext = this });
            double totalTaskCount = 8;
            var percentageCompleted = 100;
            DownloadProgressMessage = "0%";
            var completedTaskCount = 0;
            var userResponse = _tokenService.GetTokenResposne();
            int uID, organizationID;
            int.TryParse(userResponse.UserID, out uID);
            int.TryParse(userResponse.OrganizationID, out organizationID);

            #region StudentDownload
            _studentService.DeleteByDownloadedBy(uID);
            var childRecords = await bDIWebServices.GetChildRecords(deleteType: 0);
            var offlineStudentRecords = _studentService.GetStudents();
            if (offlineStudentRecords != null && offlineStudentRecords.Any())
            {
                offlineStudentRecords = offlineStudentRecords.Where(p => p.OrgId == Convert.ToInt32(Application.Current.Properties["OrgnazationID"]) && p.DownloadedBy == uID).ToList();
            }
            if (childRecords != null)
            {
                if (childRecords.ResearchCodes != null && childRecords.ResearchCodes.Any())
                {
                    _productResearchCodesService.DeleteAll(organizationID);
                    if (childRecords.ResearchCodes != null && childRecords.ResearchCodes.Any())
                    {
                        _productResearchCodesService.InsertAll(childRecords.ResearchCodes);
                    }
                }
                var studentsToInsert = new List<Students>();
                var studentsToUpdate = new List<Students>();
                if (childRecords.Childrens != null && childRecords.Childrens.Any())
                {
                    try
                    {
                        GenerateStudentList(childRecords.Childrens, studentsToInsert, uID, offlineStudentRecords, studentsToUpdate);
                    }
                    catch (Exception ex)
                    {
                    }

                }
                if (studentsToInsert != null && studentsToInsert.Any())
                {
                    _studentService.InsertAll(studentsToInsert);
                    var lstResearchCodeValues = new List<ProductResearchCodeValues>();
                    foreach (var childItem in childRecords.Childrens)
                    {
                        if (childItem.ResearchCodes != null && childItem.ResearchCodes.Any())
                        {
                            foreach (var innerItem in childItem.ResearchCodes)
                            {
                                var ProductResearchCodeValues = new ProductResearchCodeValues();
                                ProductResearchCodeValues.ModifiedDate = innerItem.ModifiedDate;
                                ProductResearchCodeValues.DownloadedBy = uID;
                                ProductResearchCodeValues.OrganizationId = organizationID;
                                ProductResearchCodeValues.value = innerItem.value;
                                ProductResearchCodeValues.ResearchCodeValueId = innerItem.ResearchCodeValueId;
                                ProductResearchCodeValues.ResearchCodeId = innerItem.ResearchCodeId;
                                ProductResearchCodeValues.OfflineStudentID = studentsToInsert.FirstOrDefault(p => p.UserId == childItem.ChildUserID.ToString()).OfflineStudentID;
                                lstResearchCodeValues.Add(ProductResearchCodeValues);
                            }
                        }
                    }
                    if (lstResearchCodeValues != null && lstResearchCodeValues.Any())
                    {
                        _productResearchCodeValuesService.InsertAll(lstResearchCodeValues);
                    }
                }
            }
            else if (childRecords != null && childRecords.StatusCode != 0)
            {
                await PopupNavigation.Instance.PopAllAsync();
                await UserDialogs.Instance.AlertAsync("Download Failed!");

                return false;
            }
            completedTaskCount += 1;
            DownloadProgressMessage = Convert.ToInt32(percentageCompleted / (totalTaskCount - completedTaskCount)) + "%";
            #endregion

            #region Locations
            _locationService.DeleteByDownloadedBy(uID);
            var locations = await bDIWebServices.GetLocationRequestModel(uID);
            if (locations == null || !locations.Any())
            {
                await PopupNavigation.Instance.PopAllAsync();
                await UserDialogs.Instance.AlertAsync("Download Failed!");
                return false;
            }
            else
            {
                if (locations != null && locations.Any())
                {
                    var totalLocations = new List<Models.DBModels.Location>();
                    foreach (var item in locations)
                    {
                        var location = new Models.DBModels.Location();
                        location.isDeleted = item.isDeleted;
                        location.updatedOn = item.updatedOn;
                        location.DownloadedBy = uID;
                        location.LocationId = item.value;
                        location.LocationName = item.text;
                        location.IsEnabled = item.enabled;
                        location.ParentLocationId = item.parentLocationID;
                        location.UserId = uID;
                        totalLocations.Add(location);
                        GenerateSubLocations(item.subItems, totalLocations, 0, uID);
                    }
                    _locationService.InsertAll(totalLocations);
                }
            }

            completedTaskCount += 1;
            DownloadProgressMessage = Convert.ToInt32(percentageCompleted / (totalTaskCount - completedTaskCount)) + "%";
            #endregion

            #region OrgForms
            _orgRecordFormService.DeleteCurrentOrgRecordForm(organizationID);
            try
            {
                var organizationRecordForms = await bDIWebServices.GetOrgRecordForms();
                if (organizationRecordForms != null && organizationRecordForms.Any())
                {
                    organizationRecordForms.ForEach((item) =>
                    {
                        item.DownloadedBy = uID;
                        item.OrganizationId = organizationID;
                    });
                    _orgRecordFormService.Insert(organizationRecordForms);
                }
            }
            catch (Exception ex)
            {
                await PopupNavigation.Instance.PopAllAsync();
                await UserDialogs.Instance.AlertAsync("Download Failed!");
                return false;
            }
            completedTaskCount += 1;
            DownloadProgressMessage = Convert.ToInt32(percentageCompleted / (totalTaskCount - completedTaskCount)) + "%";
            #endregion

            #region Examiner
            _examinerService.DeleteByDownloadedBy(uID);
            var examiners = await bDIWebServices.GetExaminer(new Models.Requests.StaffRequestModel());
            if (examiners == null || !examiners.Any())
            {
                await PopupNavigation.Instance.PopAllAsync();
                await UserDialogs.Instance.AlertAsync("Download Failed!");
                return false;
            }
            else
            {
                if (examiners != null && examiners.Any())
                {
                    var orgId = Convert.ToInt32(Application.Current.Properties["OrgnazationID"].ToString());
                    foreach (var item in examiners)
                    {
                        item.UserID = KeyEncryption.Decrypt(item.UserID);
                        item.OrganizationId = orgId;
                        item.DownloadedBy = uID;
                    }
                    _examinerService.InsertAll(examiners);
                }
            }
            completedTaskCount += 1;
            DownloadProgressMessage = Convert.ToInt32(percentageCompleted / (totalTaskCount - completedTaskCount)) + "%";
            #endregion

            #region CommonData
            await _permissionService.DeleteAllAsync();
            var commonData = await bDIWebServices.GetCommonData();
            if (commonData == null)
            {
                await PopupNavigation.Instance.PopAllAsync();
                await UserDialogs.Instance.AlertAsync("Download Failed!");
                return false;
            }
            else
            {
                studentCommonDataService.DeleteAll();
                if (commonData.FundingSources != null && commonData.FundingSources.Any())
                {
                    commonData.FundingSources.ForEach(p => p.Type = 0);
                    studentCommonDataService.InertAll(commonData.FundingSources);
                }
                if (commonData.Races != null && commonData.Races.Any())
                {
                    commonData.Races.ForEach(p => p.Type = 1);
                    studentCommonDataService.InertAll(commonData.Races);
                }
                if (commonData.Languages != null && commonData.Languages.Any())
                {
                    commonData.Languages.ForEach(p => p.Type = 2);
                    studentCommonDataService.InertAll(commonData.Languages);
                }
                if (commonData.Diagnoses != null && commonData.Diagnoses.Any())
                {
                    commonData.Diagnoses.ForEach(p => p.Type = 3);
                    studentCommonDataService.InertAll(commonData.Diagnoses);
                }
                if (commonData.Permissions != null && commonData.Permissions.Any())
                {
                    var allPermissionList = new List<Models.DBModels.Permissions>();
                    foreach (var item in commonData.Permissions)
                    {
                        var permission = new Models.DBModels.Permissions()
                        {
                            PermissionId = item.Value,
                            Description = item.Text,
                            Selected = item.Selected,
                            Disabled = item.Disabled
                        };
                        allPermissionList.Add(permission);
                    }
                    await _permissionService.InsertAllAsync(allPermissionList);
                }
                var userpermissions = default(List<string>);
                if (JObject.Parse(_tokenService.GetTokenResposne().Perms).ToObject<Dictionary<int, string>>().ContainsKey(10))
                {
                    userpermissions = JObject.Parse(_tokenService.GetTokenResposne().Perms).ToObject<Dictionary<int, string>>().Where(p => p.Key == 10).Select(p => p.Value).ToList();
                    foreach (var item in userpermissions)
                    {
                        await _userPermissionService.DeleteAllAsync();
                        var allUserPermissionList = new List<Models.DBModels.UserPermissions>();
                        var splitids = item.Split(',').ToList();
                        foreach (var perId in splitids)
                        {
                            var userPermissions = new Models.DBModels.UserPermissions()
                            {
                                UserId = uID,
                                PermissionId = Convert.ToInt32(perId)
                            };
                            allUserPermissionList.Add(userPermissions);
                        }
                        await _userPermissionService.InsertAllAsync(allUserPermissionList);
                    }
                }
            }
            completedTaskCount += 1;
            DownloadProgressMessage = Convert.ToInt32(percentageCompleted / (totalTaskCount - completedTaskCount)) + "%";
            #endregion

            #region ProgramNotes
            _programNoteService.DeleteByDownloadedBy(uID);
            var programNotes = await bDIWebServices.GetProgramNote(organizationID);
            if (programNotes == null)
            {
                await PopupNavigation.Instance.PopAllAsync();
                await UserDialogs.Instance.AlertAsync("Download Failed!");
                return false;
            }
            else
            {
                if (programNotes != null && programNotes.Any())
                {
                    var totalProgram = new List<ProgramNoteModel>();
                    foreach (var item in programNotes)
                    {
                        var programNote = new ProgramNoteModel();
                        programNote.LabelId = item.LabelId;
                        programNote.LabelName = item.LabelName;
                        programNote.DeleteType = item.DeleteType;
                        programNote.DownLoadedBy = uID;
                        programNote.updatedOn = item.updatedOn;
                        programNote.OrganizationId = item.OrganizationId;
                        totalProgram.Add(programNote);
                    }
                    _programNoteService.InsertAll(totalProgram);
                }
            }
            completedTaskCount += 1;
            DownloadProgressMessage = Convert.ToInt32(percentageCompleted / (totalTaskCount - completedTaskCount)) + "%";
            #endregion

            #region ContentData
            var contentSyncData = userSyncService.GetContentSyncData(ContentTypes.Content.ToString());
            if (contentSyncData == null)
            {
                var contentcategory = await bDIWebServices.GetDomainRequestModel(uID);
                if (contentcategory != null && contentcategory.StatusCode != 0)
                {
                    await PopupNavigation.Instance.PopAllAsync();
                    await UserDialogs.Instance.AlertAsync("Download Failed!");
                    return false;
                }
                else
                {
                    _productService.DeleteAll();
                    _productService.InserAll(contentcategory);

                    _assessmentService.DeleteAll();
                    _assessmentService.InserAll(contentcategory);

                    _contentCategoryLevelsService.DeleteAll();
                    _contentCategoryLevelsService.InserAll(contentcategory);

                    _contentCategoryService.DeleteAll();
                    _contentCategoryService.InserAll(contentcategory);

                    _contentCategoryItemsService.DeleteAll();
                    _contentCategoryItemsService.InserAll(contentcategory);

                    _contentItemService.DeleteAll();
                    if (contentcategory.contentItems != null && contentcategory.contentItems.Any())
                    {
                        var contentItemStyle = contentcategory.contentItems.Where(m => !string.IsNullOrEmpty(m.itemText) && m.contentItemId >= 516);
                        if (contentItemStyle != null && contentItemStyle.Any())
                        {
                            foreach (var val in contentItemStyle)
                            {
                                val.itemText = val.itemText.Replace("&nbsp;", " ")
                                    .Replace("<span style=\"font-weight:bold\"><span style=\"color:#007E94\">", "<span><span><b>")
                                    .Replace("<span style=\"font-weight:bold\"> <span style=\"color:#007E94\">", "<span> <span><b>")
                                    .Replace("</span></span>", "</b></span></span>")
                                    .Replace("</span>.</span>", "</b></span>.</span>")
                                    .Replace("</span><span style=\"color:#007E94\">", "</b></span><span>")
                                    .Replace(" style=\"line - height: 1.5;\"", "")
                                    .Replace("&ldquo;", "&quot;")
                                    .Replace("&rdquo;", "&quot;")
                                    .Replace("&rsquo;", "&apos;")
                                    .Replace("&middot;", "&#183;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 2 </mo> </mfrac></math>", "&#189;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 3 </mo> </mfrac></math>", "&#8531;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 4 </mo> </mfrac></math>", "&#188;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 8 </mo> </mfrac></math>", "&#8539;");
                            }
                        }

                        var contentItemMath = contentcategory.contentItems.Where(m => !string.IsNullOrEmpty(m.itemText) && m.itemText.Contains("<math>"));
                        if (contentItemMath != null && contentItemMath.Any())
                        {
                            foreach (var val in contentItemMath)
                            {
                                val.itemText = val.itemText.Replace("<math> <mfrac> <mo> 1 </mo> <mo> 2 </mo> </mfrac></math>", "&#189;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 3 </mo> </mfrac></math>", "&#8531;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 4 </mo> </mfrac></math>", "&#188;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 8 </mo> </mfrac></math>", "&#8539;")
                                    .Replace("&middot;", "&#183;");
                            }
                        }
                    }
                    _contentItemService.InserAll(contentcategory);

                    _contentItemAttributeService.DeleteAll();
                    if (contentcategory.contentItemAttributes != null && contentcategory.contentItemAttributes.Any())
                    {
                        var itemsAttributeHasMath = contentcategory.contentItemAttributes.Where(m => !string.IsNullOrEmpty(m.value) && m.value.Contains("<math>"));
                        if (itemsAttributeHasMath != null && itemsAttributeHasMath.Any())
                        {
                            foreach (var val in itemsAttributeHasMath)
                            {
                                val.value = val.value.Replace("<math> <mfrac> <mo> 1 </mo> <mo> 2 </mo> </mfrac></math>", "&#189;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 3 </mo> </mfrac></math>", "&#8531;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 4 </mo> </mfrac></math>", "&#188;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 8 </mo> </mfrac></math>", "&#8539;")
                                    .Replace("&middot;", "&#183;");
                            }
                        }
                    }
                    _contentItemAttributeService.InserAll(contentcategory);

                    _contentRubricsService.DeleteAll();
                    if (contentcategory.contentRubrics != null && contentcategory.contentRubrics.Any())
                    {
                        var itemsHasMath = contentcategory.contentRubrics.Where(m => !string.IsNullOrEmpty(m.notes));
                        if (itemsHasMath != null && itemsHasMath.Any())
                        {
                            foreach (var val in itemsHasMath)
                            {
                                val.notes = val.notes.Replace("<math> <mfrac> <mo> 1 </mo> <mo> 2 </mo> </mfrac></math>", "&#189;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 3 </mo> </mfrac></math>", "&#8531;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 4 </mo> </mfrac></math>", "&#188;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 8 </mo> </mfrac></math>", "&#8539;")
                                    .Replace("&nbsp;", "&#160;")
                                    .Replace("background-color:white;", "")
                                    .Replace("<td colspan=\"2\" style=\"width:100%;overflow:auto;white-space:normal\">", "")
                                    .Replace("&ldquo;", "&quot;")
                                    .Replace("&rdquo;", "&quot;")
                                    .Replace("&rsquo;", "&apos;")
                                    .Replace("<td>", "")
                                    .Replace("&bull;", "&#8226;");
                            }
                        }
                        var itemsScoringHasMath = contentcategory.contentRubrics.Where(m => !string.IsNullOrEmpty(m.pointsDesc) && m.pointsDesc.Contains("<math>"));
                        if (itemsScoringHasMath != null && itemsScoringHasMath.Any())
                        {
                            foreach (var val in itemsScoringHasMath)
                            {
                                val.pointsDesc = val.pointsDesc.Replace("<math> <mfrac> <mo> 1 </mo> <mo> 2 </mo> </mfrac></math>", "&#189;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 3 </mo> </mfrac></math>", "&#8531;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 4 </mo> </mfrac></math>", "&#188;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 8 </mo> </mfrac></math>", "&#8539;");
                            }
                        }
                    }
                    _contentRubricsService.InserAll(contentcategory);

                    _contentRubicPointsService.DeleteAll();
                    if (contentcategory.contentRubrics != null && contentcategory.contentRubrics.Any())
                    {
                        var itemsRubricPointsHasMath = contentcategory.contentRubricPoints.Where(m => !string.IsNullOrEmpty(m.description) && m.description.Contains("<math>"));
                        if (itemsRubricPointsHasMath != null && itemsRubricPointsHasMath.Any())
                        {
                            foreach (var val in itemsRubricPointsHasMath)
                            {
                                val.description = val.description.Replace("<math> <mfrac> <mo> 1 </mo> <mo> 2 </mo> </mfrac></math>", "&#189;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 3 </mo> </mfrac></math>", "&#8531;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 4 </mo> </mfrac></math>", "&#188;")
                                    .Replace("<math> <mfrac> <mo> 1 </mo> <mo> 8 </mo> </mfrac></math>", "&#8539;");
                            }
                        }
                    }
                    _contentRubicPointsService.InserAll(contentcategory);

                    _contentItemTalliesService.DeleteAll();
                    _contentItemTalliesService.InserAll(contentcategory);

                    _contentItemTalliesScoresService.DeleteAll();
                    _contentItemTalliesScoresService.InserAll(contentcategory);

                    _contentGroupService.DeleteAll();
                    _contentGroupService.InserAll(contentcategory);

                    _contentGroupItemsService.DeleteAll();
                    _contentGroupItemsService.InserAll(contentcategory);

                    contentBasalCeilingsService.DeleteAll();
                    contentBasalCeilingsService.InserAll(contentcategory.contentBasalCeilings);

                    commonDataService.TotalCategories = new List<Models.DBModels.ContentCategory>(contentcategory.contentCategories);
                    commonDataService.ContentCategoryItems = new List<ContentCategoryItem>(contentcategory.contentCategoryItems);
                    commonDataService.ContentBasalCeilings = new List<ContentBasalCeilings>(contentcategory.contentBasalCeilings);
                    commonDataService.ContentGroupItems = new List<ContentGroupItem>(contentcategory.contentGroupItems);
                    commonDataService.ContentGroups = new List<ContentGroup>(contentcategory.contentGroups);
                    commonDataService.ContentItemAttributes = new List<ContentItemAttribute>(contentcategory.contentItemAttributes);
                    commonDataService.ContentItems = new List<ContentItem>(contentcategory.contentItems);
                    commonDataService.ContentItemTallies = new List<ContentItemTally>(contentcategory.contentItemTallies);
                    commonDataService.ContentItemTallyScores = new List<ContentItemTallyScore>(contentcategory.contentItemTallyScores);
                    commonDataService.ContentRubricPoints = new List<ContentRubricPoint>(contentcategory.contentRubricPoints);
                    commonDataService.ContentRubrics = new List<ContentRubric>(contentcategory.contentRubrics);
                    commonDataService.ContentCategoryLevels = new List<Models.DBModels.ContentCategoryLevel>(contentcategory.contentCategoryLevels);

                    var task1 = CreateItemHtmlFiles(contentcategory.contentItems);
                    var task2 = CreateItemAttributeHtmlFiles(contentcategory.contentItemAttributes);
                    var task3 = CreateRubricHtmlFiles(contentcategory.contentRubrics);
                    var task4 = CreateScoringHtmlFiles(contentcategory.contentRubricPoints);
                    await Task.WhenAll(new List<Task> { task1, task2, task3, task4 });

                    task1.Dispose(); task1 = null; task2.Dispose(); task2 = null; task3.Dispose(); task3 = null; task4.Dispose(); task4 = null;
                    var dataTask = commonDataService.GenerateData();
                    var menuTask1 = commonDataService.GenerateAcademicMentList();
                    var menuTask2 = commonDataService.GenerateBattleMenuList();
                    var menuTask3 = commonDataService.GenerateScreenerMenuList();
                    await Task.WhenAll(new List<Task> { menuTask1, menuTask2, menuTask3, dataTask });
                    menuTask1.Dispose(); menuTask1 = null; menuTask2.Dispose(); menuTask2 = null; menuTask3.Dispose(); dataTask.Dispose(); dataTask = null;
                    userSyncService.DeleteSyncData();
                    userSyncService.Insert(new Models.Common.ContentSyncData() { ContentSynced = true, ContentType = ContentTypes.Content.ToString() });
                }
            }
            else
            {
                await GenerateDataAndMenu();
            }
            commonDataService.Races = new List<Race>();
            commonDataService.fundingSources = new List<FundingSource>();
            commonDataService.PrimaryDiagnostics = new List<Diagnostics>();
            commonDataService.SecondaryDiagnostics = new List<Diagnostics>();
            commonDataService.Languages = new List<Language>();

            if (commonData.Races != null && commonData.Races.Any())
            {
                var races = new List<Race>();
                foreach (var item in commonData.Races)
                {
                    races.Add(new Race() { text = item.Text, value = item.Value });
                }
                commonDataService.Races = races;
            }
            if (commonData.FundingSources != null && commonData.FundingSources.Any())
            {
                var fundingSources = new List<FundingSource>();
                foreach (var item in commonData.FundingSources)
                {
                    fundingSources.Add(new FundingSource() { text = item.Text, value = item.Value });
                }
                commonDataService.fundingSources = fundingSources;
            }
            if (commonData.Diagnoses != null && commonData.Diagnoses.Any())
            {
                var pDiagnoStics = new List<Diagnostics>();
                foreach (var item in commonData.Diagnoses)
                {
                    pDiagnoStics.Add(new Diagnostics() { text = item.Text, value = item.Value });
                }
                commonDataService.PrimaryDiagnostics = pDiagnoStics;
            }
            if (commonData.Diagnoses != null && commonData.Diagnoses.Any())
            {
                var sDiagnoStics = new List<Diagnostics>();
                foreach (var item in commonData.Diagnoses)
                {
                    sDiagnoStics.Add(new Diagnostics() { text = item.Text, value = item.Value });
                }
                commonDataService.SecondaryDiagnostics = sDiagnoStics;
            }
            if (commonData.Languages != null && commonData.Languages.Any())
            {
                var Languages = new List<Language>();
                foreach (var item in commonData.Languages)
                {
                    Languages.Add(new Language() { text = item.Text, value = item.Value });
                }
                commonDataService.Languages = Languages;
            }
            completedTaskCount += 1;
            DownloadProgressMessage = Convert.ToInt32(percentageCompleted / (totalTaskCount - completedTaskCount)) + "%";
            #endregion

            #region ImageDownload
            var contentImageSyncData = userSyncService.GetContentSyncData(ContentTypes.Images.ToString());
            if (contentImageSyncData == null)
            {
                PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                var file = default(PCLStorage.IFile);
                try
                {
                    var fileExists = await rootFolder.CheckExistsAsync("Product_10.zip");
                    if (fileExists != PCLStorage.ExistenceCheckResult.NotFound)
                    {
                        file = await rootFolder.GetFileAsync("Product_10.zip");
                        await file.DeleteAsync();
                        file = default(PCLStorage.IFile);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                var fileDownloadSuccess = false;
                if (file == null)
                {
                    fileDownloadSuccess = await bDIWebServices.DownloadImageFileAsync();
                }
                if (!fileDownloadSuccess)
                {
                    await PopupNavigation.Instance.PopAllAsync();
                    await UserDialogs.Instance.AlertAsync("Download Failed!");
                    return false;
                }
                userSyncService.Insert(new Models.Common.ContentSyncData() { ContentSynced = true, ContentType = ContentTypes.Images.ToString() });
            }
            completedTaskCount += 1;
            DownloadProgressMessage = Convert.ToInt32(percentageCompleted / (totalTaskCount - completedTaskCount == 0 ? 1 : totalTaskCount - completedTaskCount)) + "%";
            #endregion

            var date = DateTime.Now.ToUniversalTime().ToString("s") + "Z";
            userSyncService.InsertUserSync(new Models.Common.UserSyncTable() { DownLoadedBy = uID, LastSyncDatetime = date });
            DateTime d2 = DateTime.Now;
            TimeSpan d3 = d2 - d1;
            Debug.WriteLine("Download Time:" + d3);
            return true;
        }

        public void GenerateStudentList(List<Child> childRecords, List<Students> students, int addedBy, List<Students> offlineStudentRecords, List<Students> studentstoUpdate)
        {
            foreach (var childRecord in childRecords)
            {
                int gender = 0, language = 0;
                if (childRecord.Language != null && childRecord.Language.Count() > 0)
                {
                    language = childRecord.Language.FirstOrDefault().value;
                }
                if (childRecord.Gender != null && childRecord.Gender.Count() > 0)
                {
                    gender = childRecord.Gender.FirstOrDefault().value;
                }
                var selectedEthnicityIds = "";
                if (childRecord.Ethnicity != null && childRecord.Ethnicity.Any())
                {
                    selectedEthnicityIds = string.Join(",", childRecord.Ethnicity.Select(p => p.value));
                }
                var selectedFundingSourceIds = "";
                if (childRecord.FundingSources != null && childRecord.FundingSources.Any())
                {
                    selectedFundingSourceIds = string.Join(",", childRecord.FundingSources.Select(p => p.value));
                }
                var selectedRaceIds = "";
                if (childRecord.Race != null && childRecord.Race.Any())
                {
                    selectedRaceIds = string.Join(",", childRecord.Race.Select(p => p.value));
                }
                var selectedPrimaryDiagnosesIds = "";
                if (childRecord.Diagnoses != null && childRecord.Diagnoses.Any())
                {
                    selectedPrimaryDiagnosesIds = string.Join(",", childRecord.Diagnoses.Select(p => p.value));
                }
                var selectedSecondaryDiagnosesIds = "";
                if (childRecord.SecondaryDiagnoses != null && childRecord.SecondaryDiagnoses.Any())
                {
                    selectedSecondaryDiagnosesIds = string.Join(",", childRecord.SecondaryDiagnoses.Select(p => p.value));
                }
                var selectedLanguageIds = "";
                if (childRecord.Language != null && childRecord.Language.Any())
                {
                    selectedLanguageIds = string.Join(",", childRecord.Language.Select(p => p.value));
                }
                int? locationID = null;
                if (childRecord.Location != null && childRecord.Location.Any())
                {
                    locationID = childRecord.Location.FirstOrDefault().value;
                }
                var childToAdd = new Models.DBModels.Students()
                {
                    updatedOn = childRecord.updatedOn,
                    updatedOnUTC = childRecord.updatedOnUTC,
                    DownloadedBy = addedBy,
                    isDeleteStatus = childRecord.isDeleteStatus,
                    SelectedEthnictyIds = selectedEthnicityIds,
                    SelectedFundingSourceIds = selectedFundingSourceIds,
                    SelectedLanguageIds = selectedLanguageIds,
                    SelectedPrimaryDiagnosesIds = selectedPrimaryDiagnosesIds,
                    SelectedRaceIds = selectedRaceIds,
                    SelectedSecondaryDiagnosesIds = selectedSecondaryDiagnosesIds,
                    StudentID = childRecord.ChildUserID.ToString(),
                    FirstName = childRecord.FirstName,
                    LastName = childRecord.LastName,
                    MiddleName = childRecord.MiddleName,
                    Gender = gender,
                    ChildID = childRecord.ChildId,
                    Birthdate = CovertToDateFromString(childRecord.DOB),
                    EnrollmentDate = CovertToDateFromString(childRecord.EnrollmentDate),
                    UserId = childRecord.ChildUserID.ToString(),
                    AddedBy = childRecord.AddedBy,
                    ParentGuardian1 = childRecord.Parent1Name,
                    ParentGuardian2 = childRecord.Parent2Name,
                    ParentEmailAddress1 = childRecord.Parent1Email,
                    ParentEmailAddress2 = childRecord.Parent2Email,
                    IsIFSP = Convert.ToByte(childRecord.IFSP),
                    IsIEP = Convert.ToByte(childRecord.IEP),
                    IEPEligibilityDate = CovertToDateFromString(childRecord.IEPInitialDate),
                    IEPExitDate = CovertToDateFromString(childRecord.IEPExitDate),
                    IFSPEligibilityDate = CovertToDateFromString(childRecord.IFSPInitialDate),
                    IFSPExitDate = CovertToDateFromString(childRecord.IFSPExitDate),
                    IsFreeLunch = Convert.ToByte(childRecord.FreeLunch),
                    IsSynced = true,
                    SelectedLocationId = locationID,
                    OrgId = Convert.ToInt32(Application.Current.Properties["OrgnazationID"].ToString())
                };
                if (childRecord.ChildUserID != null)
                {
                    if (offlineStudentRecords != null && offlineStudentRecords.Any())
                    {
                        var existedRecord = offlineStudentRecords.FirstOrDefault(p => p.StudentID == childRecord.ChildUserID.ToString());
                        if (existedRecord == null)
                        {
                            students.Add(childToAdd);
                        }
                    }
                    else
                    {
                        students.Add(childToAdd);
                    }
                }
            }
        }
        private async Task GenerateDataAndMenu()
        {
            var task1 = _contentCategoryService.GetItemsAsync();
            var task2 = _contentCategoryItemsService.GetItemsAsync();
            var task3 = contentBasalCeilingsService.GetItemsAsync();
            var task4 = _contentGroupItemsService.GetItemsAsync();
            var task5 = _contentGroupService.GetItemsAsync();
            var task6 = _contentItemAttributeService.GetItemsAsyns();
            var task7 = _contentItemService.GetItemsAsync();
            var task8 = _contentItemTalliesService.GetItemsAsync();
            var task9 = _contentItemTalliesScoresService.GetItemsAsync();
            var task10 = _contentRubicPointsService.GetItemsAsync();
            var task11 = _contentRubricsService.GetItemsAsync();
            var task12 = _contentCategoryLevelsService.GetItemsAsync();

            await Task.WhenAll(new List<Task>() { task1, task2,task3,task4,task5, task6,
                                        task7, task8, task9, task10, task11, task12});

            commonDataService.TotalCategories = new List<Models.DBModels.ContentCategory>(await task1);
            commonDataService.ContentCategoryItems = new List<ContentCategoryItem>(await task2);
            commonDataService.ContentBasalCeilings = new List<ContentBasalCeilings>(await task3);
            commonDataService.ContentGroupItems = new List<ContentGroupItem>(await task4);
            commonDataService.ContentGroups = new List<ContentGroup>(await task5);
            commonDataService.ContentItemAttributes = new List<ContentItemAttribute>(await task6);
            commonDataService.ContentItems = new List<ContentItem>(await task7);
            commonDataService.ContentItemTallies = new List<ContentItemTally>(await task8);
            commonDataService.ContentItemTallyScores = new List<ContentItemTallyScore>(await task9);
            commonDataService.ContentRubricPoints = new List<ContentRubricPoint>(await task10);
            commonDataService.ContentRubrics = new List<ContentRubric>(await task11);
            commonDataService.ContentCategoryLevels = new List<Models.DBModels.ContentCategoryLevel>(await task12);


            task1.Dispose(); task1 = null; task2.Dispose(); task2 = null; task3.Dispose(); task3 = null; task4.Dispose(); task4 = null;
            task5.Dispose(); task5 = null; task6.Dispose(); task6 = null; task7.Dispose(); task7 = null; task8.Dispose(); task8 = null;
            task9.Dispose(); task9 = null; task10.Dispose(); task10 = null; task11.Dispose(); task11 = null; task12.Dispose(); task12 = null;


            var taskdata = commonDataService.GenerateData();
            var menuTask1 = commonDataService.GenerateAcademicMentList();
            var menuTask2 = commonDataService.GenerateBattleMenuList();
            var menuTask3 = commonDataService.GenerateScreenerMenuList();
            await Task.WhenAll(new List<Task>() { menuTask1, menuTask2, menuTask3, taskdata });
            taskdata.Dispose(); taskdata = null; menuTask1.Dispose(); menuTask1 = null; menuTask2.Dispose(); menuTask2 = null; menuTask3.Dispose(); menuTask3 = null;
        }
        #endregion


        public async Task CreateHtmlFolders()
        {
            var tempFolder = PCLStorage.FileSystem.Current.LocalStorage;
            var materialContentfolder = await tempFolder.CreateFolderAsync("contenthtml", CreationCollisionOption.OpenIfExists);
            await materialContentfolder.CreateFileAsync("materialcontent.html", CreationCollisionOption.ReplaceExisting);
            await materialContentfolder.CreateFileAsync("behaviourcontent.html", CreationCollisionOption.ReplaceExisting);
            await materialContentfolder.CreateFileAsync("capturecontent.html", CreationCollisionOption.ReplaceExisting);
            await materialContentfolder.CreateFileAsync("Descriptioncontent.html", CreationCollisionOption.ReplaceExisting);
            await materialContentfolder.CreateFileAsync("ItemDescriptioncontent.html", CreationCollisionOption.ReplaceExisting);

            var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            var assembly = typeof(LoginViewModel).Assembly;
            var jsstream = assembly.GetManifestResourceStream("BDI3Mobile.es5.tex-mml-chtml.js");
            var bytArray = jsstream.ToByteArray();



            var contentFolder = await tempFolder.CreateFolderAsync("contenthtml", CreationCollisionOption.OpenIfExists);
            var jsFolder = await contentFolder.CreateFolderAsync("es5", CreationCollisionOption.OpenIfExists);
            var file = await jsFolder.CreateFileAsync("tex-mml-chtml.js", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(file.Path, bytArray);


            var stylestream = assembly.GetManifestResourceStream("BDI3Mobile.Styles.record_forms.css");
            var recordStleByte = stylestream.ToByteArray();
            var stylesFolder = await contentFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var stylesheetfile = await stylesFolder.CreateFileAsync("record_forms.css", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(stylesheetfile.Path, recordStleByte);


            var assessmentStylesfileStream = assembly.GetManifestResourceStream("BDI3Mobile.Styles.assessment_form.css");
            var asseementByteArray = assessmentStylesfileStream.ToByteArray();
            var assessmentStylesFolder = await contentFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var assessmentStylesheetfile = await assessmentStylesFolder.CreateFileAsync("assessment_form.css", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(assessmentStylesheetfile.Path, asseementByteArray);


            var itemFolderFolder = await tempFolder.CreateFolderAsync("ItemsFolder", CreationCollisionOption.OpenIfExists);
            var itemsjsFolder = await itemFolderFolder.CreateFolderAsync("es5", CreationCollisionOption.OpenIfExists);
            var itemjsfile = await itemsjsFolder.CreateFileAsync("tex-mml-chtml.js", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(itemjsfile.Path, bytArray);

            var itemsstylesFolder = await itemFolderFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var itemstylesheetfile = await itemsstylesFolder.CreateFileAsync("record_forms.css", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(itemstylesheetfile.Path, recordStleByte);

            var assessmentitemsstylesFolder = await itemFolderFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var assessmentitemstylesheetfile = await assessmentitemsstylesFolder.CreateFileAsync("assessment_form.css", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(assessmentitemstylesheetfile.Path, asseementByteArray);

            var scoringFolder = await tempFolder.CreateFolderAsync("ScoringFolder", CreationCollisionOption.OpenIfExists);
            var scoringjsFolder = await scoringFolder.CreateFolderAsync("es5", CreationCollisionOption.OpenIfExists);
            var scoringjsjsfile = await scoringjsFolder.CreateFileAsync("tex-mml-chtml.js", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(scoringjsjsfile.Path, bytArray);

            var scoringstylesFolder = await scoringFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var scoringstylesheetfile = await scoringstylesFolder.CreateFileAsync("record_forms.css", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(scoringstylesheetfile.Path, recordStleByte);

            var assessmentscoringstylesFolder = await scoringFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var assesmentscoringstylesheetfile = await assessmentscoringstylesFolder.CreateFileAsync("assessment_form.css", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(assesmentscoringstylesheetfile.Path, asseementByteArray);


            var ContentRubricFolder = await tempFolder.CreateFolderAsync("ContentRubric", CreationCollisionOption.OpenIfExists);
            var ContentRubricJSFolder = await ContentRubricFolder.CreateFolderAsync("es5", CreationCollisionOption.OpenIfExists);
            var ContentRubricJSfile = await ContentRubricJSFolder.CreateFileAsync("tex-mml-chtml.js", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(ContentRubricJSfile.Path, bytArray);

            var ContentRubricstylesFolder = await ContentRubricFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var ContentRubricstylesheetfile = await ContentRubricstylesFolder.CreateFileAsync("record_forms.css", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(ContentRubricstylesheetfile.Path, recordStleByte);

            var contentItemAttributeFolder = await tempFolder.CreateFolderAsync("ContentItemAttribute", CreationCollisionOption.OpenIfExists);
            var contentItemAttributejsFolder = await contentItemAttributeFolder.CreateFolderAsync("es5", CreationCollisionOption.OpenIfExists);
            var contentItemAttributejsfile = await contentItemAttributejsFolder.CreateFileAsync("tex-mml-chtml.js", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(contentItemAttributejsfile.Path, bytArray);

            var contentItemAttributeStylesFolder = await contentItemAttributeFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var contentItemAttributeStylesSheeet = await contentItemAttributeStylesFolder.CreateFileAsync("record_forms.css", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(contentItemAttributeStylesSheeet.Path, recordStleByte);

            var assessmentContentItemAttributeStylesFolder = await contentItemAttributeFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var assessmentContentItemAttributeStylesSheeet = await assessmentContentItemAttributeStylesFolder.CreateFileAsync("assessment_form.css", CreationCollisionOption.ReplaceExisting);
            System.IO.File.WriteAllBytes(assessmentContentItemAttributeStylesSheeet.Path, asseementByteArray);

            var helpFolder = await tempFolder.CreateFileAsync("Help.zip", CreationCollisionOption.OpenIfExists);
            var helpStream = assembly.GetManifestResourceStream("BDI3Mobile.BDI3_Mobile.zip");
            if (helpStream != null)
            {
                var helpStreamBytes = helpStream.ToByteArray();
                System.IO.File.WriteAllBytes(helpFolder.Path, helpStreamBytes);
                ExistenceCheckResult folderexist = await tempFolder.CheckExistsAsync("BDI3_Mobile");
                if (folderexist == ExistenceCheckResult.FolderExists)
                {
                    var helpFileFolder = await tempFolder.GetFolderAsync("BDI3_Mobile");
                    await helpFileFolder.DeleteAsync();
                }
                ZipFile.ExtractToDirectory(helpFolder.Path, tempFolder.Path);
                helpStream.Dispose();
                helpStream = null;
                helpStreamBytes = null;
            }
            assessmentStylesfileStream.Dispose();
            assessmentStylesfileStream = null;
            stylestream.Dispose();
            stylestream = null;
            jsstream.Dispose(); recordStleByte = null; bytArray = null;
            jsstream = null; asseementByteArray = null;
        }
    }
}
