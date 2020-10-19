using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using BDI3Mobile.Services.DigitalTestRecordService;
using Rg.Plugins.Popup.Services;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDI3Mobile.ViewModels.Record_Tools_ViewModel
{
    public class NotesPopupViewModel : BaseclassViewModel
    {
        private readonly ICommonDataService commonDataService = DependencyService.Get<ICommonDataService>();
        private readonly IStudentTestFormsService studentTestFormsService;
        private readonly IClinicalTestFormService clinicalTestFormService;
        public NoteModel Model { get; set; }
        private string notes;
        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }
        private string noteHeaderTextPrefix;
        public string NoteHeaderTextPrefix
        {
            get { return noteHeaderTextPrefix; }
            set
            {
                noteHeaderTextPrefix = value;
                OnPropertyChanged(nameof(NoteHeaderTextPrefix));
            }
        }

        private string noteHeaderTextsufix;
        public string NoteHeaderTextsufix
        {
            get { return noteHeaderTextsufix; }
            set
            {
                noteHeaderTextsufix = value;
                OnPropertyChanged(nameof(NoteHeaderTextsufix));
            }
        }
        public ICommand CloseNotePopup => new Command(async() =>
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                 await PopupNavigation.Instance.PopAllAsync(false);
            }
        });
        public ICommand SaveNoteUp => new Command(async () =>
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAllAsync(false);
            }
            Model.Notes = Notes;
            if (Model.IsSubDomainNote)
            {
                commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == Model.ContentCategoryId).Notes = Notes;
                //studentTestFormsService.UpdateTestFormNotes(Model.LocalInstaceId, Model.ContentCategoryId, Notes);
            }
            else if(Model.IsFormNote)
            {
                // clinicalTestFormService.UpdateTestFormNote(Notes, Model.LocalInstaceId);
            }
            Model.SaveNotes?.Invoke(Model);
        });
        public NotesPopupViewModel()
        {
            studentTestFormsService = DependencyService.Get<IStudentTestFormsService>();
            clinicalTestFormService = DependencyService.Get<IClinicalTestFormService>();
        }
    }
}
