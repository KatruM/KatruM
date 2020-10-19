using BDI3Mobile.Models;
using System.Collections.Generic;

namespace BDI3Mobile.LookUps.ChildPageLookUps
{
    public class FundingSourceLookUp
    {
        public static List<FundingSource> GetFundingSources()
        {
            return new List<FundingSource>()
            {
                new FundingSource { value =1, text="Early Head Start"},
                new FundingSource { value =2,text="Head Start"},
                new FundingSource { value =3,text="Pre-K"},
                new FundingSource { value =4,text="Voluntary Pre-K"},
                new FundingSource { value =5,text="Preschool State Grant"},
                new FundingSource { value =6,text="Title I"},
                new FundingSource { value =7, text="IDEA"},
                new FundingSource { value =8, text="Part B Section 611"},
                new FundingSource { value =9,text="Part B Section 619"},
                new FundingSource { value =10,text="Part C Early Intervention"},
                new FundingSource { value =11,text="Medicaid"},
                new FundingSource { value =12,text="Military"},
                new FundingSource { value =13,text="Child Care"},
                new FundingSource { value =14,text="Federal Child Care and Development Fund"},
                new FundingSource { value =15,text="Temporary Assistance for Needy Families (TANF)"},
                new FundingSource { value =16,text="Private"},
                new FundingSource { value =17,text="State"},
                new FundingSource { value =18,text="Local"},
                new FundingSource { value =19,text="Block Grants"},
                new FundingSource { value =20,text="Even Start"},
                new FundingSource { value =21,text="Early Learning Coalition (ELC)"},
                new FundingSource { value =22,text="Community Based Organization (CBO)"},
                new FundingSource { value =23,text="Other"}
            };
        }
    }
}
