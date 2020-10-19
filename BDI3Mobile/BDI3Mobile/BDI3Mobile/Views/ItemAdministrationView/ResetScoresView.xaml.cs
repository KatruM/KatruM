using System;
using System.Collections.Generic;
using BDI3Mobile.Models.BL;
using BDI3Mobile.ViewModels.AdministrationViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.ItemAdministrationView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetScoresView
    {
        ResetScoresViewModel viewModel;
        Action BackAction { get; set; }
        public ResetScoresView(Action<List<int>> ResetAction, bool isbatteldevelopment, Action OnBackAction, bool isAcademicForm = false)
        {
            InitializeComponent();
            BackAction = OnBackAction;
            if(isAcademicForm)
                viewModel = new ResetScoresViewModel(isbatteldevelopment, isAcademicForm);
            else
                viewModel = new ResetScoresViewModel(isbatteldevelopment);

            if (ResetAction != null)
                viewModel.ResetAction = ResetAction;
            BindingContext = viewModel;
        }
        
        private async void BackgroundClicked_Popup(object sender, EventArgs e)
        {
            //CLINICAL-4217: Just pop both record tools and reset score page.
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAllAsync();
            }
        }
        private async void BackToRecordFormTool(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            BackAction?.Invoke();
        }
        private void OnDomainListViewTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.ItemIndex > 0)//if we mark cv.SelectedItem = null this will hit back;
            {
                // Deselect the item to remove the highlighted element.
                if (sender is ListView cv) 
                    cv.SelectedItem = null;

                var selectedItem = e.Item as ResetScore;
                selectedItem.IsSelected = !selectedItem.IsSelected;
            }
        }

        private void OnAreaListViewTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.ItemIndex >= 0)
            {
                // Deselect the item to remove the highlighted element.
                if (sender is ListView cv)
                    cv.SelectedItem = null;

                var selectedItem = e.Item as ResetScore;
                selectedItem.IsSelected = !selectedItem.IsSelected;
            }
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.ItemIndex >= 0)
            {
                if (sender is ListView cv)
                    cv.SelectedItem = null;

                var selectedItem = e.Item as ResetScore;
                selectedItem.IsSelected = !selectedItem.IsSelected;
            }
        }
    }
}
