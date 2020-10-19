using BDI3Mobile.Models;
using System.Collections.Generic;

namespace BDI3Mobile.LookUps.ChildPageLookUps
{
    public class LanguageLookUp
    {
        public static List<Language> GetLanguages()
        {
            return new List<Language>()
            {
                new Language {value = 1,text =  "English" },
                new Language {value = 2,text =  "Spanish" },
                new Language{value = 3, text = "American Sign Language" },
                new Language{value = 4,text = "Arabic" },
                new Language {value = 5, text = "Armenian" },
                new Language{value =6, text= "Bengali" },
                new Language{value = 7, text = "Cantonese" },
                new Language{value = 8, text= "Dutch" },
                new Language{value =9,text = "French" },
                new Language{value =10, text =  "French Creole" },
                new Language{value = 11, text= "German" },
                new Language{value = 12,text= "Greek" },
                new Language{value =13,text = "Gujarati" },
                new Language {value = 14, text = "Hakka" },
                new Language {value= 15, text = "Hebrew" },
                new Language{value =16, text = "Hindi" },
                new Language {value = 17, text = "Hmong" },
                new Language {value = 18, text= "Hokkien" },
                new Language{ value = 19, text = "Italian" },
                new Language {value = 20, text =  "Japanese" },
                new Language{ value = 21, text =  "Khmer" },
                new Language {value = 22, text = "Korean" },
                new Language{ value = 23, text = "Laotian" },
                new Language{ value = 24, text = "Mandarin (Standard)" },
               new Language{value = 25, text = "Marathi" },
               new Language{value = 26, text = "Navajo" },
               new Language{ value = 27, text = "Persian" },
               new Language{ value = 28, text = "Polish" },
               new Language{value = 29, text = "Portuguese" },
               new Language { value = 1, text = "Punjabi" },
               new Language {value = 30, text = "Russian" },
               new Language {value = 31, text = "Serbo-Croatian" },
               new Language{ value = 32, text = "Tagalog" },
               new Language{ value = 33, text = "Taishanese" },
               new Language{value = 34, text = "Tamil" },
               new Language{value = 35, text = "Thai" },
               new Language{ value = 36, text = "Urdu" },
               new Language{ value = 37, text = "Vietnamese" },
               new Language{value = 38, text = "Yiddish" },
               new Language {value = 39, text = "Other" }
            };
        }
    }
}
