using BDI3Mobile.ViewModels;
using System.Collections.Generic;

namespace BDI3Mobile.LookUps.ChildPageLookUps
{
    public class GenderLookUp
    {
        public static List<Gender> GetGenders()
        {
            return new List<Gender>()
            {
                new Gender() { value =1, text = "Male" },
                new Gender() { value =2, text = "Female" },
                new Gender() { value =3, text = "Unspecified" }
            };
        }
    }
}
