
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CeilingObtainedPopupView 
    {
        string[] currentAdminsterLevelCollection;
        string currentAdminsterLevel = " ";
        public CeilingObtainedPopupView(string currentAdminsterLevel)
        {
            InitializeComponent();

            currentAdminsterLevelCollection = currentAdminsterLevel.Split('(');
            currentAdminsterLevel = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(currentAdminsterLevelCollection[0].ToLower()).Trim();
            string strCorrection = currentAdminsterLevel.Substring(currentAdminsterLevel.Length - 1);
            if (strCorrection == ".")
            {
                currentAdminsterLevel = currentAdminsterLevel.Remove(currentAdminsterLevel.Length - 1);
            }

            var formattedString1 = new FormattedString();
            formattedString1.Spans.Add(new Span { Text = "You are finished administering ", FontSize = 14 });
            formattedString1.Spans.Add(new Span { Text = currentAdminsterLevel + ".", FontAttributes = FontAttributes.Bold, FontSize = 14 });
            message_1.FormattedText = formattedString1;

            var formattedString2 = new FormattedString();
            formattedString2.Spans.Add(new Span { Text = "Click ", FontSize = 14 });
            formattedString2.Spans.Add(new Span { Text = "Save and Continue ", FontAttributes = FontAttributes.Bold, FontSize = 14 });
            formattedString2.Spans.Add(new Span { Text = "to return to the test session overview \nor " ,FontSize=14});
            formattedString2.Spans.Add(new Span { Text = "Cancel ", FontAttributes = FontAttributes.Bold, FontSize = 14 });
            formattedString2.Spans.Add(new Span { Text = "to continue making changes to  " + currentAdminsterLevel + ".", FontSize = 14 });
            message_2.FormattedText = formattedString2;
            SaveandContinue.Text = "Save & Continue";
        }
    }
}