using BDI3Mobile.Models.DBModels;
using BDI3Mobile.Models.Requests;
using BDI3Mobile.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BDI3Mobile.Models
{
    public class Child
    {
        [JsonProperty("forceSave")]
        public bool ForceSave { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("middleName")]
        public string MiddleName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("dob")]
        public string DOB { get; set; }
        [JsonProperty("childId")]
        public string ChildId { get; set; }
        [JsonProperty("enrollmentDate")]
        public string EnrollmentDate { get; set; }
        [JsonProperty("parent1Name")]
        public string Parent1Name { get; set; }
        [JsonProperty("parent2Name")]
        public string Parent2Name { get; set; }
        [JsonProperty("parent1Email")]
        public string Parent1Email { get; set; }
        [JsonProperty("parent2Email")]
        public string Parent2Email { get; set; }
        [JsonProperty("ifsp")]
        public int? IFSP { get; set; }
        [JsonProperty("iep")]
        public int? IEP { get; set; }
        [JsonProperty("ifspInitialDate")]
        public string IFSPInitialDate { get; set; }
        [JsonProperty("iepInitialDate")]
        public string IEPInitialDate { get; set; }
        [JsonProperty("ifspExitDate")]
        public string IFSPExitDate { get; set; }
        [JsonProperty("iepExitDate")]
        public string IEPExitDate { get; set; }
        [JsonProperty("freeLunch")]
        public int? FreeLunch { get; set; }
        [JsonProperty("fundingSources")]
        public List<FundingSource> FundingSources { get; set; }
        public List<Diagnostics> Diagnoses { get; set; }
        public List<Diagnostics> SecondaryDiagnoses { get; set; }
        [JsonProperty("gender")]
        public List<Gender> Gender { get; set; }
        [JsonProperty("language")]
        public List<Language> Language { get; set; }
        [JsonProperty("race")]
        public List<Race> Race { get; set; }
        [JsonProperty("ethnicity")]
        public List<Ethencity> Ethnicity { get; set; }
        [JsonProperty("userFields")]
        public List<ProductResearchCodeValues> ResearchCodes { get; set; }
        [JsonProperty("validationMessages")]
        public List<string> ValidationMessages { get; set; }
        [JsonProperty("userId")]
        public object ChildUserID { get; set; }
        [JsonProperty("location")]
        public List<LocationResponseModel> Location { get; set; }

        public int OfflineStudentId { get; set; }
        [JsonProperty("addedBy")]
        public int AddedBy { get; set; }
        public int isDeleteStatus { get; set; }
        public string updatedOn { get; set; }
        public string updatedOnUTC { get; set; }
    }
    //public class ResearchCodes : BindableObject
    //{
    //    public int researchCodeValueId { get; set; }
    //    public int researchCodeId { get; set; }
    //    public int organizationId { get; set; }
    //    private string _value;
    //    public string value
    //    {
    //        get
    //        {
    //            return _value;
    //        }
    //        set
    //        {
    //            _value = value;
    //            OnPropertyChanged(nameof(this.value));
    //        }
    //    }
    //    public int userId { get; set; }
    //    public ResearchCode ResearchCode { get; set; }
    //}
    //public class ResearchCode : BindableObject
    //{
    //    public int researchCodeId { get; set; }
    //    public int organizationId { get; set; }
    //    private string _valueName;
    //    public string valueName
    //    {
    //        get
    //        {
    //            return _valueName;
    //        }
    //        set
    //        {
    //            _valueName = value;
    //            OnPropertyChanged(nameof(this.valueName));
    //        }
    //    }
    //    public int sequence { get; set; }
    //    public bool isDefault { get; set; }
    //}

    public class Race : BindableObject
    {
        public int value { get; set; }
        public string text { get; set; }
        private bool isselected;
        public bool selected
        {
            get
            {
                return isselected;
            }
            set
            {
                isselected = value;
                OnPropertyChanged(nameof(selected));
            }
        }
    }

    public class FundingSource : BindableObject
    {
        public int value { get; set; }
        public string text { get; set; }
        private bool isselected;
        public bool selected
        {
            get
            {
                return isselected;
            }
            set
            {
                isselected = value;
                OnPropertyChanged(nameof(selected));
            }
        }
    }
    public class Ethencity : BindableObject
    {
        public int value { get; set; }
        public string text { get; set; }
        private bool isselected;
        public bool selected
        {
            get
            {
                return isselected;
            }
            set
            {
                isselected = value;
                OnPropertyChanged(nameof(selected));
            }
        }

    }
    public class Diagnostics : BindableObject
    {
        public int value { get; set; }
        public string text { get; set; }
        private bool isselected;
        public bool selected
        {
            get
            {
                return isselected;
            }
            set
            {
                isselected = value;
                OnPropertyChanged(nameof(selected));
            }
        }
    }
    public class SecondaryDiagnos
    {
        public int value { get; set; }
        public object textValue { get; set; }
        public object text { get; set; }
        public bool selected { get; set; }
    }

    public class Language : BindableObject
    {
        public int value { get; set; }
        public string text { get; set; }
        private bool isselected;
        public bool selected
        {
            get
            {
                return isselected;
            }
            set
            {
                isselected = value;
                OnPropertyChanged(nameof(selected));
            }
        }
    }

    public class ChildResponse
    {
        [JsonProperty("children")]
        public List<Child> Childrens { get; set; }
        [JsonProperty("researchCodes")]
        public List<ProductResearchCodes> ResearchCodes { get; set; }

        /// <summary>
        /// if it is -1 unknown is the reason
        /// </summary>
        public int StatusCode { get; set; }
    }
}
