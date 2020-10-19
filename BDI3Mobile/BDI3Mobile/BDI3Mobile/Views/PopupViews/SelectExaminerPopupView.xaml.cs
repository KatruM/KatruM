using BDI3Mobile.Models.Responses;
using BDI3Mobile.ViewModels;
using BDI3Mobile.ViewModels.AssessmentViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace BDI3Mobile.Views.PopupViews
{
    public partial class SelectExaminerPopupView : PopupPage
    {
        TestSessionOverViewModel _viewModel;
        AssessmentConfigPopupViewModel _assessmentConfigPopupViewModel;
        public bool isPopupViewOpen;

        public SelectExaminerPopupView(TestSessionOverViewModel vM, Thickness margin, AssessmentConfigPopupViewModel assessmentConfigPopupViewModel = null)
        {
            InitializeComponent();
            if (vM != null)
            {
                _viewModel = vM;
                BindingContext = vM;
                ExaminerPopupView.Margin = margin;
            }
            else
            {
                _assessmentConfigPopupViewModel = assessmentConfigPopupViewModel;
                BindingContext = assessmentConfigPopupViewModel;
                ExaminerPopupView.Margin = margin;
            }

            SetExaminerItemSelection();
        }
        /// <summary>
        /// Set a check mark image on selected examiner.
        /// </summary>
        private void SetExaminerItemSelection()
        {
            if (_viewModel != null)
            {
                string examinerItem = "";
                if (_viewModel.TestSessionRecords.Count > 0)
                {
                    foreach (var record in _viewModel.TestSessionRecords)
                    {
                        if (record.DomainCode == _viewModel.SelectedItemDomainCode)
                        {
                            examinerItem = record.Examiner;
                        }
                    }
                }
                else if (_viewModel.Records.Count > 0)
                {
                    foreach (var record in _viewModel.Records)
                    {
                        if (record.DomainCode == _viewModel.SelectedItemDomainCode)
                        {
                            examinerItem = record.Examiner;
                        }
                    }

                }
                else if (_viewModel.AcademicTestSessionRecords.Count > 0)
                {
                    foreach (var record in _viewModel.AcademicTestSessionRecords)
                    {
                        if (record.DomainCode == _viewModel.SelectedItemDomainCode)
                        {
                            examinerItem = record.Examiner;
                        }
                    }

                }
                foreach (var item in _viewModel.ExaminerList)
                {
                    if (item.text == examinerItem)
                    {
                        item.selected = true;
                    }
                    else
                    {
                        item.selected = false;
                    }
                }
            }
            else
            {
                if(_assessmentConfigPopupViewModel.Examiner != "")
                {

                }
            }
        }

        private void Examiner_Tapped(object sender, ItemTappedEventArgs e)
        {
            //var selectedExaminer = (((StackLayout)sender).Children[0] as Label).Text;
            //await PopupNavigation.Instance.PopAsync();
            //isPopupViewOpen = false;
            // Bug Fix: CLINICAL 4055
            if (_viewModel != null)
            {
                var selectedExaminer = e.Item as Examiner;
                foreach (var item in _viewModel.ExaminerList)
                {
                    if (item.text == selectedExaminer.text)
                    {
                        if (!item.selected)
                        {
                            item.selected = true;
                            _viewModel.UpdateSelectedExaminer(selectedExaminer.text);
                        }
                    }
                    else
                    {
                        item.selected = false;
                    }
                }
            }
            else
            {
                var examiner = e.Item as Examiner;
                foreach (var item in _assessmentConfigPopupViewModel.ExaminerList)
                {
                    if (item.text == examiner.text)
                    {
                        if (item.selected)
                        {
                            item.selected = !item.selected;
                            _assessmentConfigPopupViewModel.Examiner = _assessmentConfigPopupViewModel.DefaultExaminer;
                        }
                        else
                        {
                            item.selected = !item.selected;
                            _assessmentConfigPopupViewModel.Examiner = examiner.text.ToString();
                            ExaminerListview.SelectedItem = examiner.text;
                        }
                    }
                    else
                    {
                        item.selected = false;
                    }
                }
            }
        }
    }
}
