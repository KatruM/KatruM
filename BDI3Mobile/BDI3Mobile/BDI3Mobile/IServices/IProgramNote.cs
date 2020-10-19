using BDI3Mobile.Models.Responses;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    interface IProgramNoteService
    {
        void InsertAll(List<ProgramNoteModel> programNotelist);
        List<ProgramNoteModel> GetProgramNote();
        void DeleteAll();
        void DeleteByDownloadedBy(int downloadedby);
    }
}
