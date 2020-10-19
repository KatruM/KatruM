using BDI3Mobile.Models.Common;
using BDI3Mobile.ViewModels.Record_Tools_ViewModel;
using System;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.RecordToolsForms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemLevelNotePopup
    {
        public NotesPopupViewModel MyViewModel;
        public ItemLevelNotePopup(NoteModel model)
        {
            InitializeComponent();
            MyViewModel = new NotesPopupViewModel();
            MyViewModel.Model = model;
            MyViewModel.NoteHeaderTextPrefix = model.noteHeaderPrefix;
            MyViewModel.NoteHeaderTextsufix = model.noteHeaderSufix;
            MyViewModel.Notes = model.Notes;
            this.BindingContext = MyViewModel;
        }

        private void Page_BackgroundClicked(object sender, EventArgs e)
        {

        }
    }
}