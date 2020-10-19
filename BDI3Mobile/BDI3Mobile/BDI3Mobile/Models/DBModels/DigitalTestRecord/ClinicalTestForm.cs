using BDI3Mobile.Common;
using Newtonsoft.Json;
using SQLite;
using System;

namespace BDI3Mobile.Models.DBModels.DigitalTestRecord
{
    public class StudentTestFormOverview
    {
        [PrimaryKey,AutoIncrement]
        public int LocalTestRecodId { get; set; }
        public string notes { get; set; }
        public string additionalNotes { get; set; }
        public int assessmentId { get; set; }
        public int LocalStudentId { get; set; }
        public int createdByUserId { get; set; }
        public string formParameters { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public bool IsFormSaved { get; set; }
        public string FormStatus { get; set; }
        public int SyncStausCode { get; set; } = -1;
        [Ignore]
        public string SyncStausDesc
        {
            get
            {
                return SyncStausCode == 1 || SyncStausCode == -1 ? "" : "Failed";
            }
        }
    }
    public class FormParamterClass
    {
        public bool? AllStandard { get; set; }
        public bool? ValidRepresentation { get; set; }
        public bool? HasGlasses { get; set; }
        public bool? GlassesUsed { get; set; }
        public bool? HasHearingAid { get; set; }
        public bool? HearingAidUsed { get; set; }
        public int? ProgramLabelId { get; set; }
        public DateTime? TestDate { get; set; }
        public DateTime? CommitDate { get; set; }
        
    }
    public class MenuItem
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("textValue")]
        public string TextValue { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("selected")]
        public bool Selected { get; set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; set; }
    }

    public class JsonEncryptedValueConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return KeyEncryption.DecryptAsInt((string)reader.Value);
            }

            if (reader.ValueType == typeof(long))
            {
                return Convert.ToInt32(reader.Value);
            }

            return reader.Value;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(string) || objectType == typeof(int));
        }
    }
}
