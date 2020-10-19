using System.Collections.Generic;

namespace BDI3Mobile.Models.DBModels.DigitalTestRecord
{
    public class FormJsonClass
    {
        public int sectionId { get; set; }
        public List<ItemInfo> items { get; set; }
    }
    public class ItemInfo
    {
        public int ContentRubricPointId { get; set; }
        public int itemId { get; set; }
        public string captureMode { get; set; }
        public int? itemScore { get; set; }
        public string itemNotes { get; set; }
        public List<TallyItemInfo> tallyItems { get; set; }
    }
    public class TallyItemInfo
    {
        public int itemTallyId { get; set; }
        public int tallyScore { get; set; }
    }
}
