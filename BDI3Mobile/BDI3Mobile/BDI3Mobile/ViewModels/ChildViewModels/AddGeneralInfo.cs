using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BDI3Mobile.ViewModels
{
    public partial class AddChildViewModel
    {
        private bool showGender = false, showLocations = false, showRaces = false;
        private string lastName;
        public string LastName
        {

            get { return lastName; }
            set
            {
                if (!string.IsNullOrEmpty(value)) IsLastNameEmpty = false;
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (!string.IsNullOrEmpty(value)) IsFirstNameEmpty = false;
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        private string middleName;
        public string MiddleName
        {
            get { return middleName; }
            set
            {
                middleName = value;
                OnPropertyChanged(nameof(MiddleName));
            }
        }
        private string childID;
        public string ChildID
        {
            get { return childID; }
            set
            {
                childID = value;
                OnPropertyChanged(nameof(ChildID));
            }
        }

        private string parent1Name;
        public string Parent1Name
        {
            get { return parent1Name; }
            set
            {
                parent1Name = value;
                OnPropertyChanged(nameof(Parent1Name));
            }
        }
        private string parent2Name;
        public string Parent2Name
        {
            get { return parent2Name; }
            set
            {
                parent2Name = value;
                OnPropertyChanged(nameof(Parent2Name));
            }
        }
        private string parent1Email;
        public string Parent1Email
        {
            get { return parent1Email; }
            set
            {
                parent1Email = value;
                if (!string.IsNullOrEmpty(value) && value.Equals(Parent2Email))
                {
                    Parent1EmailBorderColor = Common.Colors.RedColor;
                    Parent1EmailTextColor = Common.Colors.RedColor;
                    ParentEmailerror = true;
                }
                else
                {
                    Parent1EmailBorderColor = Common.Colors.BorderColor;
                    Parent1EmailTextColor = Common.Colors.LightGrayColor;
                    ParentEmailerror = false;
                }
                OnPropertyChanged(nameof(Parent1Email));
            }
        }
        private bool parentEmailerror = false;
        public bool ParentEmailerror
        {
            get { return parentEmailerror; }
            set
            {
                parentEmailerror = value;
                OnPropertyChanged(nameof(ParentEmailerror));
            }
        }


        private string parent2Email;
        public string Parent2Email
        {
            get { return parent2Email; }
            set
            {

                if (!string.IsNullOrEmpty(value) && value.Equals(Parent1Email))
                {
                    Parent1EmailBorderColor = Common.Colors.RedColor;
                    Parent1EmailTextColor = Common.Colors.RedColor;
                    ParentEmailerror = true;
                }
                else
                {
                    Parent1EmailBorderColor = Common.Colors.BorderColor;
                    Parent1EmailTextColor = Common.Colors.LightGrayColor;
                    ParentEmailerror = false;
                }
                parent2Email = value;
                OnPropertyChanged(nameof(Parent2Email));
            }


        }
        private DateTime dateofBirth = DateTime.Now;
        public DateTime DateofBirth
        {
            get { return dateofBirth; }
            set
            {
                dateofBirth = value;
                OnPropertyChanged(nameof(DateofBirth));
            }
        }
        private DateTime? enrollDate = DateTime.Now;
        public DateTime? EnrollDate
        {
            get { return enrollDate; }
            set
            {
                enrollDate = value;
                OnPropertyChanged(nameof(EnrollDate));
            }
        }
        private string guardian1Name;
        public string Guardian1Name
        {
            get { return guardian1Name; }
            set
            {
                guardian1Name = value;
                OnPropertyChanged(nameof(Guardian1Name));
            }
        }
        private string guardian2Name;
        public string Guardian2Name
        {
            get { return guardian2Name; }
            set
            {
                guardian2Name = value;
                OnPropertyChanged(nameof(Guardian2Name));
            }
        }
        private string guardian1Email;
        public string Guardian1Email
        {
            get { return guardian1Email; }
            set
            {
                guardian1Email = value;
                OnPropertyChanged(nameof(Guardian1Email));
            }
        }
        private string guardian2Email;
        public string Guardian2Email
        {
            get { return guardian2Email; }
            set
            {
                guardian2Email = value;
                OnPropertyChanged(nameof(Guardian2Email));
            }
        }
        public List<Gender> Genders { get; set; }
        private string gender;
        public string Gender
        {
            get { return gender; }
            set
            {
                if (value.Length > 0) IsGenderEmpty = false;
                gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }
        private Color parent2EmailBorderColor = Common.Colors.BorderColor;
        public Color Parent2EmailBorderColor
        {
            get { return parent2EmailBorderColor; }
            set
            {
                parent2EmailBorderColor = value;
                OnPropertyChanged(nameof(Parent2EmailBorderColor));
            }
        }
        private Color parent2EmailPlaceHolderColor = Common.Colors.LightGrayColor;
        public Color Parent2EmailPlaceHolderColor
        {
            get { return parent2EmailPlaceHolderColor; }
            set
            {
                parent2EmailPlaceHolderColor = value;
                OnPropertyChanged(nameof(Parent2EmailPlaceHolderColor));
            }
        }
        private Color parent1EmailBorderColor = Common.Colors.BorderColor;
        public Color Parent1EmailBorderColor
        {
            get { return parent1EmailBorderColor; }
            set
            {
                parent1EmailBorderColor = value;
                OnPropertyChanged(nameof(Parent1EmailBorderColor));
            }
        }
        private Color parent1EmailTextColor = Common.Colors.LightGrayColor;
        public Color Parent1EmailTextColor
        {
            get { return parent1EmailTextColor; }
            set
            {
                parent1EmailTextColor = value;
                OnPropertyChanged(nameof(Parent1EmailTextColor));
            }
        }
        public bool ShowGender
        {
            get { return showGender; }
            set
            {
                showGender = value;
                IsTabStop = !value;
                OnPropertyChanged(nameof(ShowGender));
            }
        }
        public bool ShowLocations
        {
            get { return showLocations; }
            set
            {
                showLocations = value;
                OnPropertyChanged(nameof(ShowLocations));
            }
        }
        void SelectGender(object obj)
        {
            Cover = true;
            ShowGender = true;
        }
        void GenderSelected(object obj)
        {
            var gender = obj as Gender;
            foreach (var item in Genders)
            {
                if(item.value == gender.value)
                {
                    // Bug Fix: CLINICAL 4055
                    if (item.selected)
                    {
                        item.selected = !item.selected;
                        Gender = "";
                    }
                    else
                    {
                        item.selected = !item.selected;
                        Gender = gender.text;
                    }
                }
                else
                {
                    item.selected = false;
                }
            }
        }
    }

    public class Gender : BindableObject
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
}