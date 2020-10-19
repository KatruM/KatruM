using BDI3Mobile.Common;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using BDI3Mobile.ViewModels.AcademicSurveyLiteracyViewModel;
using BDI3Mobile.ViewModels.AdministrationViewModels;
using BDI3Mobile.Views.ItemAdministrationView;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.RecordToolsForms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesMenu 
    {
        private readonly ICommonDataService commonDataService = DependencyService.Get<ICommonDataService>();
        public AdministrationViewModel administrationViewModel { get; set; }
        public AcademicSurveyLiteracyViewModel academicSurveyLiteracyViewModel { get; set; }

        public NotesMenu(AdministrationViewModel adminViewModel)
        {
            InitializeComponent();
            administrationViewModel = adminViewModel;
            if (!adminViewModel.IsBattelleDevelopmentCompleteChecked)
                SDgrid.Height = 0;
            else
                SDgrid.Height = 45;
        }

        public NotesMenu(AcademicSurveyLiteracyViewModel academicSurveyViewModel)
        {
            InitializeComponent();
            academicSurveyLiteracyViewModel = academicSurveyViewModel;
            SDgrid.Height = 45;
        }

        private async void BackToRecordFormTool(object sender, EventArgs e)
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAllAsync(false);
            }

            if (administrationViewModel != null)
            {
                await PopupNavigation.Instance.PushAsync(new RecordToolsPOP(administrationViewModel));
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new RecordToolsPOP(academicSurveyLiteracyViewModel));
            }
        }

        private async void OpenItemLevelNote(object sender, EventArgs e)
        {
            string noteHeaderPrefix ="";
            string[] noteHeaderSuffixCollection;
            string noteHeaderSuffix = " ";
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAllAsync(false);
            }
            if(sender != null)
            {
                if((sender is Label))
                {
                    noteHeaderPrefix = (sender as Label).Text + ": ";
                }
                if(sender is Image)
                {
                    noteHeaderPrefix = (((sender as Image).Parent as Grid).Children[0] as Label).Text + ": ";
                }
                if (sender is Grid)
                {
                    noteHeaderPrefix = ((sender as Grid).Children[0] as Label).Text + ": ";
                }
                if (administrationViewModel != null)
                {
                    noteHeaderSuffixCollection = administrationViewModel.AdministrationHeader.Split('(');
                    noteHeaderSuffix = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(noteHeaderSuffixCollection[0].ToLower()) + administrationViewModel.ItemNumber;
                }
                else if(academicSurveyLiteracyViewModel != null)
                {
                    noteHeaderSuffixCollection = academicSurveyLiteracyViewModel.AdministrationHeader.Split('(');
                    noteHeaderSuffix = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(noteHeaderSuffixCollection[0].ToLower()) + academicSurveyLiteracyViewModel.CurrentAcademicContentModel?.AcademicItemModel?.FirstOrDefault(p => p.IsSelected).ItemNumber;
                }
            }
            if (administrationViewModel != null && administrationViewModel.CaptureMode != null)
            {
                var model = new NoteModel();

                model.noteHeaderPrefix = noteHeaderPrefix;
                model.noteHeaderSufix = " " +  noteHeaderSuffix;
                model.ContentCategoryId = administrationViewModel.CaptureMode.DomainCategoryId;
                model.IsItemLevelNote = true;
                model.Notes = administrationViewModel.CaptureMode.Notes;
                model.LocalInstaceId = administrationViewModel.LocaInstanceID;
                model.SaveNotes = administrationViewModel.NotesSaveAction;
                await PopupNavigation.Instance.PushAsync(new ItemLevelNotePopup(model));
            }
            else
            {
                var model = new NoteModel();

                if ((sender is Label))
                {
                    noteHeaderPrefix = (sender as Label).Text + ": ";
                }
                model.noteHeaderPrefix = noteHeaderPrefix;
                model.noteHeaderSufix = noteHeaderSuffix;
                model.ContentCategoryId = academicSurveyLiteracyViewModel.CurrentAcademicContentModel.DomainCategoryId;
                model.IsItemLevelNote = true;
                model.Notes = academicSurveyLiteracyViewModel.CurrentAcademicContentModel.AcademicItemModel?.FirstOrDefault(p => p.IsSelected)?.Notes;
                model.LocalInstaceId = academicSurveyLiteracyViewModel.LocaInstanceID;
                model.SaveNotes = academicSurveyLiteracyViewModel.NotesSaveAction;
                await PopupNavigation.Instance.PushAsync(new ItemLevelNotePopup(model));
            }
            
        }

        private async void OpenRecordFormNote(object sender, EventArgs e)
        {
            string noteHeaderPrefix = "";
            string[] noteHeaderSuffixCollection;

            string noteHeaderSuffix = " ";
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAllAsync(false);
            }
            if (sender != null)
            {
                if ((sender is Label))
                {
                    noteHeaderPrefix = (sender as Label).Text + ": ";
                }
                if (sender is Image)
                {
                    noteHeaderPrefix = (((sender as Image).Parent as Grid).Children[0] as Label).Text + ": ";
                }
                if (sender is Grid)
                {
                    noteHeaderPrefix = ((sender as Grid).Children[0] as Label).Text + ": ";
                }
                if (administrationViewModel != null)
                {
                    noteHeaderSuffix = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(administrationViewModel.AssessmentType.ToLower());
                }
                else if(academicSurveyLiteracyViewModel != null)
                {
                    noteHeaderSuffix = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(AssignmentTypes.BattelleEarlyAcademicSurveyString);
                }
            }


            if (administrationViewModel != null)
            {
                var model = new NoteModel();
                model.noteHeaderPrefix = noteHeaderPrefix;
                model.noteHeaderSufix = noteHeaderSuffix;
                model.ContentCategoryId = administrationViewModel.CaptureMode.DomainCategoryId;
                model.IsFormNote = true;
                model.Notes = administrationViewModel.FormNotes;
                model.LocalInstaceId = administrationViewModel.LocaInstanceID;
                model.SaveNotes = administrationViewModel.NotesSaveAction;
                await PopupNavigation.Instance.PushAsync(new ItemLevelNotePopup(model));
            }
            else  if(academicSurveyLiteracyViewModel != null)
            {
                var model = new NoteModel();
                model.noteHeaderPrefix = noteHeaderPrefix;
                model.noteHeaderSufix = noteHeaderSuffix;
                model.ContentCategoryId = academicSurveyLiteracyViewModel.CurrentAcademicContentModel.DomainCategoryId;
                model.IsFormNote = true;
                model.Notes = academicSurveyLiteracyViewModel.FormNotes;
                model.LocalInstaceId = academicSurveyLiteracyViewModel.LocaInstanceID;
                model.SaveNotes = academicSurveyLiteracyViewModel.NotesSaveAction;
                await PopupNavigation.Instance.PushAsync(new ItemLevelNotePopup(model));
            }
        }
        private async void OpenSubdomainNote(object sender, EventArgs e)
        {
            string noteHeaderPrefix = "";
            string[] noteHeaderSuffixCollection;
            string noteHeaderSuffix = " ";
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAllAsync(false);
            }
            if (sender != null)
            {
                if ((sender is Label))
                {
                    noteHeaderPrefix = (sender as Label).Text.Split(' ')[0] + "/Area Note: ";
                }
                if (sender is Image)
                {
                    noteHeaderPrefix = (((sender as Image).Parent as Grid).Children[0] as Label).Text.Split(' ')[0] + "/Area Note: ";
                }
                if(sender is Grid)
                {
                    noteHeaderPrefix = ((sender as Grid).Children[0] as Label).Text.Split(' ')[0] + "/Area Note: ";
                }

                if (administrationViewModel != null)
                {
                    noteHeaderSuffixCollection = administrationViewModel.AdministrationHeader.Split(' ');
                    noteHeaderSuffix = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(administrationViewModel.AdministrationHeader.Split(':')[1].Split('(')[0].Trim().ToLower());
                }
                else if (academicSurveyLiteracyViewModel != null)
                {
                    noteHeaderSuffixCollection = academicSurveyLiteracyViewModel.AdministrationHeader.Split(' ');
                    noteHeaderSuffix = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(academicSurveyLiteracyViewModel.AdministrationHeader.Split(':')[1].Split('(')[0].Trim().ToLower());
                }
                //noteHeaderSuffix = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(noteHeaderSuffixCollection[1].ToLower());
                //noteHeaderSuffix = noteHeaderSuffix.Trim(':');
            }
            if (administrationViewModel != null)
            {
                var model = new NoteModel();
                model.noteHeaderPrefix = noteHeaderPrefix;
                model.noteHeaderSufix = noteHeaderSuffix;
                model.ContentCategoryId = administrationViewModel.CaptureMode.DomainCategoryId;
                model.IsSubDomainNote = true;
                model.Notes = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == model.ContentCategoryId).Notes;
                model.LocalInstaceId = administrationViewModel.LocaInstanceID;
                model.SaveNotes = administrationViewModel.NotesSaveAction;
                await PopupNavigation.Instance.PushAsync(new ItemLevelNotePopup(model));
            }
            else if (academicSurveyLiteracyViewModel != null)
            {
                    var model = new NoteModel();
                    if ((sender is Label))
                    {
                        noteHeaderPrefix = (sender as Label).Text.Split(' ')[0] + "/Area Note: ";
                    }
                    model.noteHeaderPrefix = noteHeaderPrefix;
                    model.noteHeaderSufix = noteHeaderSuffix;
                    if (academicSurveyLiteracyViewModel.CurrentAcademicContentModel.AreaId != 0)
                    {
                        model.ContentCategoryId = academicSurveyLiteracyViewModel.CurrentAcademicContentModel.AreaId;
                    }
                    else
                    {
                        model.ContentCategoryId = academicSurveyLiteracyViewModel.CurrentAcademicContentModel.SubDomainCategoryId;
                    }
                    model.IsSubDomainNote = true;
                    model.Notes = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == model.ContentCategoryId).Notes;
                    model.LocalInstaceId = academicSurveyLiteracyViewModel.LocaInstanceID;
                    model.SaveNotes = academicSurveyLiteracyViewModel.NotesSaveAction;
                    await PopupNavigation.Instance.PushAsync(new ItemLevelNotePopup(model));
                }
            }
        }
    }
