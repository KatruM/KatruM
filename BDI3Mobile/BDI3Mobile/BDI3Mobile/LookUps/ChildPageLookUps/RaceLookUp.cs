using BDI3Mobile.Models;
using System.Collections.ObjectModel;

namespace BDI3Mobile.LookUps.ChildPageLookUps
{
    public class RaceLookUp
    {
        public static ObservableCollection<Race> GetRaces()
        {
            return new ObservableCollection<Race>()
            {
                new Race() { value = 1, text = "American Indian or Alaska Native"},
                new Race() { value =2, text="Asian"},
                new Race() { value =3, text="Black or African American"},
                new Race() { value =4, text="Native Hawaiian or Other Pacific Islander"},
                new Race() { value = 5, text = "White"},
                new Race() { value = 6,text="Other"}
            };
        }
    }
}
