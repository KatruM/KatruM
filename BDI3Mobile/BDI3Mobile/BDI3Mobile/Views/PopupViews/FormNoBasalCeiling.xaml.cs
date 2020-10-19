using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormNoBasalCeiling
    {
        public FormNoBasalCeiling(string message)
        {
            InitializeComponent();
            //Text="Basal and/or ceiling has not been obtained for one or more subdomains."
            this.message.Text = message;
        }
    }
}