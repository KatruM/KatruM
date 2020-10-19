using BDI3Mobile.IServices;
using BDI3Mobile.Services;
using BDI3Mobile.View;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using BDI3Mobile.Services.DigitalTestRecordService;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using System.Net.Http;
using BDI3Mobile.Helpers;
using Newtonsoft.Json;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BDI3Mobile
{
    public partial class App : Application
    {
        public static string SqlFilename = "bdi3.db";
        public static string SqlFilePath;
        public static Action<bool> LogoutAction;
        public App()
        {
            InitializeComponent();
            InitializeService();
            MainPage = new LoginView();
            LogoutAction = new Action<bool>(LoadLoginPage);

        }

        protected override void OnStart()
        {
            AppCenter.Start("uwp=3755695e-46f2-4256-9a76-817dea46d7dd;" +
                   "android=9707c3d9-704a-4892-8ba0-6c1867decfdd;" +
                   "ios=be282f2d-5631-45c3-80e0-13ffb2a06ec8;",
                   typeof(Analytics), typeof(Crashes));

            //Prod Keys
            //AppCenter.Start("uwp=e042561f-da05-4449-8651-3aedd18e649d;" +
            //       "android=5f89d17e-e122-4185-a462-0d384b137d3a;" +
            //       "ios=c5082712-3f57-4d63-ae79-72dd8e8c07bc;",
            //       typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            //CLINICAL-5199
            ////SessionManager.SessionManager.Instance.StartSessionTimer();
        }
        public TimeSpan SessionDuration;
        protected override void OnResume()
        {

        }

        public void LoadLoginPage(bool isSessionExpired)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                ClearModalStack();
                MainPage = new NavigationPage(new LoginView(isSessionExpired));
            });
        }

        public async void ClearModalStack()
        {
            while (App.Current.MainPage.Navigation.ModalStack.Count > 0)
            {
                await App.Current.MainPage.Navigation.PopModalAsync(false);
            }
        }

        public void InitializeService()
        {
            DependencyService.RegisterSingleton<ITokenService>(new TokenService());
            DependencyService.RegisterSingleton<IDBConnection>(new DBConnection());
            DependencyService.RegisterSingleton<ILocationService>(new LocationService());
            DependencyService.RegisterSingleton<IProductResearchCodesService>(new ProductResearchCodesService());
            DependencyService.RegisterSingleton<IProductResearchCodeValuesService>(new ProductResearchCodeValuesService());
            DependencyService.RegisterSingleton<IRolesService>(new RolesService());
            DependencyService.RegisterSingleton<IStudentFundingSourcesService>(new StudentFundingSourceService());
            DependencyService.RegisterSingleton<IStudentsRaceService>(new StudentRaceService());
            DependencyService.RegisterSingleton<IStudentsService>(new StudentsService());
            DependencyService.RegisterSingleton<IMembershipService>(new MembershipService());
            DependencyService.RegisterSingleton<IUsersService>(new UsersService());
            DependencyService.RegisterSingleton<IUsersPermissionService>(new UsersPermissionService());
            DependencyService.RegisterSingleton<IUserInRolesService>(new UserInRoleService());
            DependencyService.RegisterSingleton<IContentCategoryService>(new ContentCategoryService());
            DependencyService.RegisterSingleton<IProgramNoteService>(new ProgramNoteService());
            DependencyService.RegisterSingleton<IExaminerService>(new ExaminerService());
            DependencyService.RegisterSingleton<IProductService>(new ProductService());
            DependencyService.RegisterSingleton<IAssessmentsService>(new AssessmentsService());
            DependencyService.RegisterSingleton<IContentCategoryLevelsService>(new ContentCategoryLevelsService());
            DependencyService.RegisterSingleton<IContentItemsService>(new ContentItemsService());
            DependencyService.RegisterSingleton<IContentItemAttributesService>(new ContentItemAttributesService());
            DependencyService.RegisterSingleton<IContentRubricsService>(new ContentRubricsService());
            DependencyService.RegisterSingleton<IContentRubricPointsService>(new ContentRubricPointsService());
            DependencyService.RegisterSingleton<IContentItemTallyService>(new ContentItemTallyService());
            DependencyService.RegisterSingleton<IContentItemTalliesScoresService>(new ContentItemTalliesScoresService());
            DependencyService.RegisterSingleton<IContentGroupService>(new ContentGroupService());
            DependencyService.RegisterSingleton<IContentGroupItemsService>(new ContentGroupItemsService());
            DependencyService.RegisterSingleton<IContentCategoryItemsService>(new ContentCategoryItemsService());
            DependencyService.RegisterSingleton<IUserLastConnectionActivityService>(new UserLastConnectionActivityService());
            DependencyService.RegisterSingleton<IClinicalTestFormService>(new ClinicalTestFormService());
            DependencyService.RegisterSingleton<IStudentTestFormsService>(new StudentTestFormsService());
            DependencyService.RegisterSingleton<IStudentTestFormResponsesService>(new StudentTestFormResponsesService());
            DependencyService.RegisterSingleton<IOrgRecordFormService>(new OrgRecordFormService());
            DependencyService.RegisterSingleton<IContentBasalCeilingsService>(new ContentBasalCeilingsService());
            DependencyService.RegisterSingleton<ICommonDataService>(new CommonDataService());
            DependencyService.RegisterSingleton<IPermissionService>(new PermissionService());
            DependencyService.RegisterSingleton<IUserPermissionService>(new UserPermissionService());
            DependencyService.RegisterSingleton<IUserSyncService>(new UserSyncService());
            DependencyService.RegisterSingleton<IStudentCommonDataService>(new StudentCommonDataService());
            DependencyService.RegisterSingleton<HttpClient>(new HttpClient());
            DependencyService.RegisterSingleton<BDIWebServices>(new BDIWebServices());
            DependencyService.RegisterSingleton<INavigationService>(new NavigationService());

        }
    }
}
