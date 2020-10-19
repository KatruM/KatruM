using System;

namespace BDI3Mobile.Models.Common
{
    public class NoteModel
    {
        public string Notes { get; set; }
        public bool IsItemLevelNote { get; set; }
        public bool IsSubDomainNote { get; set; }
        public bool IsFormNote { get; set; }
        public int ContentCategoryId { get; set; }
        public int LocalInstaceId { get; set; }
        public string noteHeaderPrefix { get; set; }
        public string noteHeaderSufix { get; set; }
        public Action<NoteModel> SaveNotes { get; set; }
    }
}
