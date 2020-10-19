using BDI3Mobile.Models;
using System.Collections.Generic;

namespace BDI3Mobile.LookUps.ChildPageLookUps
{
    public class EthencityLookUp
    {
        public static List<Ethencity> GetEthencities()
        {
            return new List<Ethencity>()
            {
                new Ethencity() {value = 1, text = "Not Spanish/Hispanic/Latino"},
                new Ethencity() {value= 2, text= "Spanish/Hispanic/Latino"}
            };
        }
    }
}
