using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePopupView : PopupPage
    {
        public new string Title { get; set;} 
        public string FirstImage { get; set; }
        public string SecondImage { get; set; }
        public string ThirdImage { get; set; }
		public ImagePopupView(string mainImage, string[] allImagesLoc)
		{
            InitializeComponent();

            FirstImage = allImagesLoc[0];
            SecondImage = allImagesLoc[1];
            ThirdImage = allImagesLoc[2];
            mainimage.Source = mainImage;

            if(allImagesLoc != null & allImagesLoc[0] != null & allImagesLoc[1] != null)
                image1.IsVisible = image2.IsVisible = true;
            else
                image1.IsVisible = image2.IsVisible = false;
            //Title = title;
            //ContentFrame.HeightRequest = details.Height;
            //ContentFrame.WidthRequest = details.Width;
            //Header.Text = details.Header;
            //if(title == "SessionExpiring")
            //{
            //    var formattedString = new FormattedString();
            //    formattedString.Spans.Add(new Span { Text = "Session will expire in 5 minutes, press ", FontSize=14 });
            //    formattedString.Spans.Add(new Span { Text = "Continue", FontAttributes= FontAttributes.Bold, FontSize=14 });
            //    formattedString.Spans.Add(new Span { Text = " to extend time.", FontSize=14 });
            //    Message.FormattedText = formattedString;
            //}
            //else
            //{
            //    Message.Text = details.Message;
            //}
		}
        private void CloseImagePopup(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void ImageCarosalView(object sender, EventArgs e)
        {
            if(SecondImage != null)
            {
                string[] seperator = { "File:", " " };
                int count = 2;
                if(mainimage.Source.ToString().Split(seperator, count, StringSplitOptions.RemoveEmptyEntries)[0].Equals(FirstImage) && SecondImage != null)
                {
                    mainimage.Source = SecondImage;
                }
                else if (mainimage.Source.ToString().Split(seperator, count, StringSplitOptions.RemoveEmptyEntries)[0].Equals(SecondImage) && ThirdImage != null)
                {
                    mainimage.Source = ThirdImage;
                }
                else
                {
                    mainimage.Source = FirstImage;
                }
                //mainimage.Source = (mainimage.Source.ToString().Split(seperator, count, StringSplitOptions.RemoveEmptyEntries)[0].Equals(FirstImage) && SecondImage != null) ? SecondImage : FirstImage;
            }
        }
    }

    public class CustomPopUpDetail
    {
        public double Height;
        public double Width;
        public string Header;
        public string Message;
    }
}