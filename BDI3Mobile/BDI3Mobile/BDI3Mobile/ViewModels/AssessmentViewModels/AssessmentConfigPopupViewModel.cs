using Acr.UserDialogs;
using BDI3Mobile.Common;
using BDI3Mobile.Helpers;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using BDI3Mobile.Models.Responses;
using BDI3Mobile.Services.DigitalTestRecordService;
using BDI3Mobile.Views.AcademicSurvey;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDI3Mobile.ViewModels.AssessmentViewModels
{
    public class AssessmentConfigPopupViewModel : BaseclassViewModel
    {
        #region Properties
        private readonly ICommonDataService _commonDataService;
        public IStudentTestFormsService studentTestFormsService;
        private readonly IClinicalTestFormService clinicalTestFormService;
        private readonly IStudentsService _studentService;
        private readonly IOrgRecordFormService _orgRecordFormService;
        #endregion       

        #region Ctor
        public AssessmentConfigPopupViewModel(int offlineStudentId)
        {
            _commonDataService = DependencyService.Get<ICommonDataService>();
            studentTestFormsService = DependencyService.Get<IStudentTestFormsService>();
            clinicalTestFormService = DependencyService.Get<IClinicalTestFormService>();
            _studentService = DependencyService.Get<IStudentsService>();
            _orgRecordFormService = DependencyService.Get<IOrgRecordFormService>();
            OfflineStudentId = offlineStudentId;
            StartAssessmentCommand = new Command(StartAssessmentClicked);
        }
        public async void LoadData()
        {
            await Task.Delay(0);
            var offlineData = _studentService.GetStudentById(OfflineStudentId);
            if (offlineData != null)
            {
                FullName = offlineData.FirstName + " " + offlineData.LastName;
                var year = offlineData.Birthdate.Year;
                var month = offlineData.Birthdate.Month < 10 ? "0" + offlineData.Birthdate.Month : "" + offlineData.Birthdate.Month;
                var day = offlineData.Birthdate.Day < 10 ? "0" + offlineData.Birthdate.Day : offlineData.Birthdate.Day + "";
                DOB = month + "/" + day + "/" + year;
            }
            GetProgrameNoteandExaminer();
            GetOrgRecordForms();
        }
        #endregion

        #region Properties
        private bool isAgeRestricted = false;
        public bool IsAgeRestricted
        {
            get { return isAgeRestricted; }
            set
            {
                isAgeRestricted = value;
                OnPropertyChanged(nameof(IsAgeRestricted));
            }
        }
        private bool _IsStartEnabled;
        public bool IsStartEnabled
        {
            get { return _IsStartEnabled; }
            set
            {
                _IsStartEnabled = value;
                OnPropertyChanged(nameof(IsStartEnabled));
            }
        }

        private string fullName;
        public string FullName
        {
            get
            {
                return fullName;
            }
            set
            {
                fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string dOB;
        public string DOB
        {
            get { return dOB; }
            set
            {
                dOB = value;
                OnPropertyChanged(nameof(DOB));
            }
        }

        private string testDate;
        public string TestDate
        {
            get { return testDate; }
            set
            {
                testDate = value;
                var age = HelperMethods.CalculateAge(DOB, testDate);
                if (age != null && ((age[0] > 7) || ((age[0] == 7) && (age[1] > 11)) || ((age[0] >= 7) && (age[1] >= 11) && (age[2] > 0))))
                {
                    IsAgeRestricted = true;
                }

                OnPropertyChanged(nameof(TestDate));
            }
        }


        private bool isBattelleDevelopmentalCompleteChecked;
        public bool IsBattelleDevelopmentalCompleteChecked
        {
            get
            {
                return isBattelleDevelopmentalCompleteChecked;
            }
            set
            {
                isBattelleDevelopmentalCompleteChecked = value;
                OnPropertyChanged(nameof(IsBattelleDevelopmentalCompleteChecked));
            }
        }
        private bool isBattelleDevelopmentalScreenerChecked;
        public bool IsBattelleDevelopmentalScreenerChecked
        {
            get
            {
                return isBattelleDevelopmentalScreenerChecked;
            }
            set
            {
                isBattelleDevelopmentalScreenerChecked = value;
                OnPropertyChanged(nameof(IsBattelleDevelopmentalScreenerChecked));
            }
        }
        private bool isBattelleEarlyAcademicSurveyChecked;
        public bool IsBattelleEarlyAcademicSurveyChecked
        {
            get
            {
                return isBattelleEarlyAcademicSurveyChecked;
            }
            set
            {
                isBattelleEarlyAcademicSurveyChecked = value;
                OnPropertyChanged(nameof(IsBattelleEarlyAcademicSurveyChecked));
            }
        }
        private string _programNote = "Select a program label";
        public string ProgramNote
        {
            get { return _programNote; }
            set
            {
                _programNote = value;
                OnPropertyChanged(nameof(ProgramNote));
            }
        }
        private string _examiner;
        public string Examiner
        {
            get { return _examiner; }
            set
            {
                _examiner = value;
                OnPropertyChanged(nameof(Examiner));
            }
        }
        private string _defaultExaminer;
        public string DefaultExaminer
        {
            get { return _defaultExaminer; }
            set
            {
                _defaultExaminer = value;
                OnPropertyChanged(nameof(DefaultExaminer));
            }
        }

        public int LocalTestFormID { get; set; }
        private List<Examiner> _examinerList;
        public List<Examiner> ExaminerList
        {
            get { return _examinerList; }
            set
            {
                _examinerList = value;
                OnPropertyChanged(nameof(ExaminerList));
            }
        }
        private List<ProgramNote> _programNoteList;
        public List<ProgramNote> ProgramNoteList
        {
            get { return _programNoteList; }
            set
            {
                _programNoteList = value;
                OnPropertyChanged(nameof(ProgramNoteList));
            }
        }
        public int OfflineStudentId { get; set; }

        private List<OrgRecordForms> _orgRecordFormList;
        public List<OrgRecordForms> OrgRecordFormList
        {
            get { return _orgRecordFormList; }
            set
            {
                _orgRecordFormList = value;
                OnPropertyChanged(nameof(OrgRecordFormList));
            }
        }
        private OrgRecordForms _selectedItem;
        public OrgRecordForms SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem != value)
                {
                    if (_selectedItem != null)
                        _selectedItem.IsChecked = false;
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                    _selectedItem.IsChecked = true;
                }
            }
        }
        
        #endregion

        #region Actions
        public Action SetExaminer { get; set; }
        public Action SetProgramNotes { get; set; }
        #endregion

        #region ICommands
        public ICommand AddNewRecordCommand { get; set; }
        public ICommand CancelClickedCommand
        {
            get
            {
                return new Command(async () => await PopupNavigation.Instance.PopAsync());
            }
        }
        public ICommand StartAssessmentCommand { get; set; }
        #endregion

        #region Methods
        private void GetProgrameNoteandExaminer()
        {
            ProgramNoteList = new List<ProgramNote>();
            ExaminerList = new List<Examiner>();

            if (_commonDataService.ProgramNoteModels != null && _commonDataService.ProgramNoteModels.Any())
            {
                foreach (var item in _commonDataService.ProgramNoteModels)
                {
                    ProgramNoteList.Add(new ProgramNote() { text = item.LabelName, selected = false });
                }
                SetProgramNotes?.Invoke();
            }
            if (_commonDataService.SearchStaffResponseModel != null && _commonDataService.SearchStaffResponseModel.Any())
            {
                DefaultExaminer = _commonDataService.SearchStaffResponseModel.FirstOrDefault(p => p.UserID == Application.Current.Properties["UserID"].ToString()).FirstNameLastName;
                foreach (var item in _commonDataService.SearchStaffResponseModel)
                {
                    ExaminerList.Add(new Examiner() { text = item.FirstNameLastName, selected = false });
                }
                SetExaminer?.Invoke();
            }
        }
        /// <summary>
        /// Fetches the purchased record forms for an Organisation.
        /// </summary>
        private void GetOrgRecordForms()
        {
            OrgRecordFormList = new List<OrgRecordForms>();
            if (_commonDataService.OrgRecordFormList != null && _commonDataService.OrgRecordFormList.Any())
            {
                foreach (var item in _commonDataService.OrgRecordFormList)
                {
                    OrgRecordForms organizationRecordForms = new OrgRecordForms();
                    organizationRecordForms.AssesmentID = item.AssessmentID;
                    organizationRecordForms.Description = item.Description;
                    organizationRecordForms.IsChecked = item.IsChecked;
                    OrgRecordFormList.Add(organizationRecordForms);
                }
            }
        }
        private async void StartAssessmentClicked()
        {
            _commonDataService.DOB = DOB;
            _commonDataService.IsAcademicForm = false;
            _commonDataService.IsScreenerForm = false;
            _commonDataService.IsCompleteForm = false;
            if (PopupNavigation.Instance.PopupStack != null && PopupNavigation.Instance.PopupStack.Count == 1)
            {
                await PopupNavigation.Instance.PopAsync(false);
            }
            UserDialogs.Instance.ShowLoading("Loading...");
            await Task.Delay(300);
            var clinicalTestForm = new StudentTestFormOverview
            {
                LocalStudentId = OfflineStudentId,
                assessmentId = IsBattelleDevelopmentalCompleteChecked ? AssignmentTypes.BattelleDevelopmentalCompleteID : IsBattelleDevelopmentalScreenerChecked ? AssignmentTypes.BattelleDevelopmentalScreenerID : AssignmentTypes.BattelleDevelopmentalAcademicSurveyID
            };
            int.TryParse(Application.Current.Properties["UserID"].ToString(), out var addedBy);
            clinicalTestForm.createdByUserId = addedBy;
            clinicalTestForm.createDate = DateTime.Now;
            var label = ProgramNote == "Select a program label" ? null : ProgramNote;
            var labelId = label != null ? _commonDataService.ProgramNoteModels.FirstOrDefault(p => p.LabelName == ProgramNote)?.LabelId : null;
            clinicalTestForm.FormStatus = "Not started";
            DateTime dateTime;
            DateTime.TryParse(TestDate, out dateTime);
            clinicalTestForm.formParameters = JsonConvert.SerializeObject(
                new FormParamterClass()
                {
                    ProgramLabelId = labelId,
                    TestDate = dateTime,
                });
            clinicalTestFormService.InsertTestForm(clinicalTestForm);
            _commonDataService.StudentTestFormOverview = clinicalTestForm;
            LocalTestFormID = clinicalTestForm.LocalTestRecodId;
            var contentBaseCeiling = _commonDataService.ContentBasalCeilings;
            if (IsBattelleEarlyAcademicSurveyChecked)
            {
                var academicCategories = _commonDataService.AcademicCategories;
                if (academicCategories != null && academicCategories.Any())
                {
                    var lstStudentTestForms = new List<StudentTestForms>();
                    var parentCategories = _commonDataService.AcademicCategories.Where(p => p.parentContentCategoryId == 0).ToList();
                    foreach (var parentCategory in parentCategories)
                    {
                        var childCategory = _commonDataService.AcademicCategories.Where(p => p.parentContentCategoryId == parentCategory.contentCategoryId);
                        if (childCategory != null && childCategory.Any())
                        {
                            foreach (var item in childCategory)
                            {
                                var studentTestForms = new StudentTestForms
                                {
                                    TSOStatus = "Not Started",
                                    LocalformInstanceId = LocalTestFormID,
                                    testDate = TestDate,
                                    examinerId = Convert.ToInt32(_commonDataService.SearchStaffResponseModel.FirstOrDefault(p => p.FirstNameLastName == Examiner)?.UserID),
                                    contentCategoryId = item.contentCategoryId,
                                    createDate = DateTime.Now,
                                    BaselCeilingReached = contentBaseCeiling.FirstOrDefault(p => p.contentCategoryId == item.contentCategoryId) == null,
                                    IsBaselCeilingApplied = contentBaseCeiling.FirstOrDefault(p => p.contentCategoryId == item.contentCategoryId) != null,
                                };
                                var childchildCategory = _commonDataService.AcademicCategories.Where(p => p.parentContentCategoryId == item.contentCategoryId);
                                if (childchildCategory != null && childchildCategory.Any())
                                {
                                    foreach (var childitem in childchildCategory)
                                    {
                                        var childitemstudentTestForms = new StudentTestForms
                                        {
                                            TSOStatus = "Not Started",
                                            LocalformInstanceId = LocalTestFormID,
                                            testDate = TestDate,
                                            examinerId = Convert.ToInt32(_commonDataService.SearchStaffResponseModel.FirstOrDefault(p => p.FirstNameLastName == Examiner)?.UserID),
                                            contentCategoryId = childitem.contentCategoryId,
                                            createDate = DateTime.Now,
                                            BaselCeilingReached = contentBaseCeiling.FirstOrDefault(p => p.contentCategoryId == childitem.contentCategoryId) == null,
                                            IsBaselCeilingApplied = contentBaseCeiling.FirstOrDefault(p => p.contentCategoryId == childitem.contentCategoryId) != null,
                                        };
                                        lstStudentTestForms.Add(childitemstudentTestForms);
                                    }
                                }
                                else
                                {
                                    lstStudentTestForms.Add(studentTestForms);
                                }
                            }
                        }
                        else
                        {
                            var studentTestForms = new StudentTestForms
                            {
                                TSOStatus = "Not Started",
                                LocalformInstanceId = LocalTestFormID,
                                testDate = TestDate,
                                examinerId = Convert.ToInt32(_commonDataService.SearchStaffResponseModel.FirstOrDefault(p => p.FirstNameLastName == Examiner)?.UserID),
                                contentCategoryId = parentCategory.contentCategoryId,
                                createDate = DateTime.Now
                            };
                            lstStudentTestForms.Add(studentTestForms);
                        }
                    }
                    studentTestFormsService.DeleteAll(LocalTestFormID);
                    studentTestFormsService.InsertAll(lstStudentTestForms);
                    _commonDataService.LocaInstanceID = LocalTestFormID;
                    _commonDataService.StudentTestForms = new List<StudentTestForms>(lstStudentTestForms);
                    _commonDataService.IsAcademicForm = true;

                    var navigationParams = new AdminstrationNavigationParams
                    {
                        LocalInstanceID = LocalTestFormID,
                        DOB = DOB,
                        TestDate = TestDate,
                        FullName = FullName,
                        OfflineStudentID = OfflineStudentId,
                        IsDevelopmentCompleteForm = false
                    };
                    await Application.Current.MainPage.Navigation.PushModalAsync(new AcademicformWithMatAndItems(navigationParams));
                    _commonDataService.ClearAddChildContent?.Invoke();
                }
            }

            if ((IsBattelleDevelopmentalCompleteChecked && !string.IsNullOrEmpty(Examiner)) || (IsBattelleDevelopmentalScreenerChecked && !string.IsNullOrEmpty(Examiner)))
            {
                var totalCategories = IsBattelleDevelopmentalScreenerChecked ? _commonDataService.ScreenerCategories : _commonDataService.BattleCategories.Where(p => p.contentCategoryLevelId == 2).ToList();
                if (totalCategories != null && totalCategories.Any())
                {
                    var lstStudentTestForms = new List<StudentTestForms>();
                    foreach (var item in totalCategories)
                    {
                        if (IsBattelleDevelopmentalScreenerChecked)
                        {
                            _commonDataService.IsScreenerForm = true;
                        }
                        else if (IsBattelleDevelopmentalCompleteChecked)
                        {
                            _commonDataService.IsCompleteForm = true;
                        }
                        var studentTestForms = new StudentTestForms
                        {
                            TSOStatus = "Not Started",
                            LocalformInstanceId = LocalTestFormID,
                            testDate = TestDate,
                            examinerId = Convert.ToInt32(_commonDataService.SearchStaffResponseModel
                                    .FirstOrDefault(p => p.FirstNameLastName == Examiner)?.UserID),
                            contentCategoryId = item.contentCategoryId,
                            createDate = DateTime.Now,
                            BaselCeilingReached = contentBaseCeiling.FirstOrDefault(p => p.contentCategoryId == item.contentCategoryId) == null,
                            IsBaselCeilingApplied = true // contentBaseCeiling.FirstOrDefault(p => p.contentCategoryId == item.contentCategoryId) != null,
                        };
                        lstStudentTestForms.Add(studentTestForms);
                    }
                    studentTestFormsService.DeleteAll(LocalTestFormID);
                    studentTestFormsService.InsertAll(lstStudentTestForms);
                    _commonDataService.LocaInstanceID = LocalTestFormID;
                    _commonDataService.StudentTestForms = new List<StudentTestForms>(lstStudentTestForms);
                }

                var navigationParams = new AdminstrationNavigationParams
                {
                    LocalInstanceID = LocalTestFormID,
                    DOB = DOB,
                    TestDate = TestDate,
                    FullName = FullName,
                    OfflineStudentID = OfflineStudentId,
                    IsDevelopmentCompleteForm = IsBattelleDevelopmentalCompleteChecked
                };
                _commonDataService.ClearAddChildContent?.Invoke();
                await Application.Current.MainPage.Navigation.PushModalAsync(new AdministrationView(navigationParams));
            }
        }

        #endregion
    }
    /// <summary>
    /// ViewModel class for Collection View ItemsSource Binding
    /// </summary>
    public class OrgRecordForms : BaseclassViewModel
    {
        public int AssesmentID { get; set; }
        private String _description;
        public String Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        private Boolean _checked;
        public Boolean IsChecked
        {
            get => _checked;
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    OnPropertyChanged();
                }
            }

        }
    }

}
