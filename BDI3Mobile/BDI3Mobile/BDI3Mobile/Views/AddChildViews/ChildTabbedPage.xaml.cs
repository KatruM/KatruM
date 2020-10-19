using BDI3Mobile.IServices;
using BDI3Mobile.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
namespace BDI3Mobile.Views.AddChildViews
{
    public partial class ChildTabbedPage : Xamarin.Forms.TabbedPage
    {
        ICommonDataService dataservice;
        AddGeneralInfoView generalInfo;
        AddDemographicView demographicInfo;
        AddUserFieldView adduserfileds;
        AddChildViewModel AddChildViewModel;
        public ChildTabbedPage(int offlineStudentId)
        {
            try
            {
                InitializeComponent();
                dataservice = DependencyService.Get<ICommonDataService>();
                NavigationPage.SetHasNavigationBar(this, false);
                generalInfo = new AddGeneralInfoView() { Title = "General Information", Icon = "star" };
                demographicInfo = new AddDemographicView() { Title = "Demographics/Programs", Icon = "star" };
                adduserfileds = new AddUserFieldView() { Title = "User Identified Fields", Icon = "star" };

                On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
                Children.Add(generalInfo);
                Children.Add(demographicInfo);
                Children.Add(adduserfileds);
                AddChildViewModel = new ViewModels.AddChildViewModel(offlineStudentId);
                if(AddChildViewModel != null && AddChildViewModel.FirstName == null)
                {
                    AddChildViewModel.ReloadCommand.Execute(new object());
                }
                foreach (var child in Children)
                {
                    child.BindingContext = AddChildViewModel;
                }
                MessagingCenter.Subscribe<string>(this, "Error", (arg1) => {
                    try
                    {
                        this.SelectedItem = this.Children[0];
                        this.CurrentPage = this.Children[0];
                    }
                    catch(Exception ex)
                    {

                    }
                    });

                var saveClicked = new Action(() =>
                {
                    (Children[0] as AddGeneralInfoView).SaveChildTapped(null, null);
                });

              
                (Children[1] as AddDemographicView).SaveChildAction = saveClicked;
                (Children[2] as AddUserFieldView).SaveChildAction = saveClicked;
            }
            catch (Exception ex)
            {
                var x = ex;
            }
            dataservice.ClearAddChildContent = null;
            dataservice.ClearAddChildContent = new Action(ClearChild);
        }

        void HandleAction(string arg1, string arg2)
        {
        }
        private void ClearChild()
        {
            try
            {
                generalInfo.Grid.Children.Clear();
                demographicInfo.Grid.Children.Clear();
                adduserfileds.Grid.Children.Clear();

                generalInfo.BindingContext = null;
                generalInfo.Content = null;

                demographicInfo.BindingContext = null;
                demographicInfo.Content = null;

                adduserfileds.BindingContext = null;
                adduserfileds.Content = null;

                this.Children.Clear();
                this.BindingContext = null;

                AddChildViewModel.BindingContext = null;
                AddChildViewModel = null;
            }
            catch(Exception ex)
            {

            }
            GC.Collect();
            GC.SuppressFinalize(this);
        }

    }
}
