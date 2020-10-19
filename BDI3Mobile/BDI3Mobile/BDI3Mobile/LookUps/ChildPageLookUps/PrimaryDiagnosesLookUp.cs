using BDI3Mobile.Models;
using System.Collections.Generic;

namespace BDI3Mobile.LookUps.ChildPageLookUps
{
    public class PrimaryDiagnosticsLookUp
    {
        public static List<Diagnostics> GetPrimaryDiagnostics()
        {
            return new List<Diagnostics>
            {
                new Diagnostics(){ value = 1, text = "Autism",selected = false },
                new Diagnostics(){ value = 2, text = "Deaf-blind",selected = false },
                new Diagnostics(){ value = 3, text = "Emotional disturbance",selected = false },
                new Diagnostics(){ value = 4, text = "Health Impairment",selected = false },
                new Diagnostics(){ value = 5, text = "Hearing impairment, including deafness",selected = false },
                new Diagnostics(){ value = 6, text = "Intellectual disabilities",selected = false },
                new Diagnostics(){ value = 7, text = "Multiple disabilities (excluding deaf-blind)",selected = false },
                new Diagnostics(){ value = 8, text = "Non-categorical/developmental delay",selected = false },
                new Diagnostics(){ value = 9, text = "Orthopedic impairment",selected = false },
                new Diagnostics(){ value = 10, text = "Specific learning disability",selected = false },
                new Diagnostics(){ value = 11, text = "Speech or language impairments",selected = false },
                new Diagnostics(){ value = 12, text = "Traumatic brain injury",selected = false },
                new Diagnostics(){ value = 13, text = "Visual impairment, including blindness",selected = false },
            };
        }
    }
}
