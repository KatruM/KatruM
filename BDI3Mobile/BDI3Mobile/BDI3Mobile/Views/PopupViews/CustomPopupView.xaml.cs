using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomPopupView : PopupPage
    {
        public new string Title { get; set;} 
		public CustomPopupView (CustomPopUpDetails details, string title = "defalut")
		{
			InitializeComponent ();
            Title = title;
            ContentFrame.HeightRequest = details.Height;
            ContentFrame.WidthRequest = details.Width;
            Header.Text = details.Header;
            if(title == "SessionExpiring")
            {
                var formattedString = new FormattedString();
                formattedString.Spans.Add(new Span { Text = "Session will expire in 5 minutes, press ", FontSize=14 });
                formattedString.Spans.Add(new Span { Text = "Continue", FontAttributes= FontAttributes.Bold, FontSize=14 });
                formattedString.Spans.Add(new Span { Text = " to extend time.", FontSize=14 });
                Message.FormattedText = formattedString;
            }
            else
            {
                Message.Text = details.Message;
            }
		}
	}

    public class CustomPopUpDetails
    {
        public double Height;
        public double Width;
        public string Header;
        public string Message;
    }
}