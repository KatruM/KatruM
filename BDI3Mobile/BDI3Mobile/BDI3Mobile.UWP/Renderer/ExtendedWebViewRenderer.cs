using BDI3Mobile.Common;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.UWP.Renderer;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;
using WebView = Windows.UI.Xaml.Controls.WebView;

[assembly: ExportRenderer(typeof(ExtendedWebView), typeof(ExtendedWebViewRenderer))]
namespace BDI3Mobile.UWP.Renderer
{
    //https://social.msdn.microsoft.com/Forums/en-US/4b09e2e1-90bc-4135-82fc-10d9067f0c6b/cuwpwebviewhow-to-resize-webview-height-based-on-html-content-in-windows-10-uwp?forum=wpdevelop
    public class ExtendedWebViewRenderer : ViewRenderer<ExtendedWebView, Windows.UI.Xaml.Controls.WebView>
    {

        protected override void OnDisconnectVisualChildren()
        {
            Console.WriteLine("Disconnecting children");
            base.OnDisconnectVisualChildren();
        }

        protected override void OnBringIntoViewRequested(BringIntoViewRequestedEventArgs e)
        {
            Console.WriteLine("I'm visible");
            base.OnBringIntoViewRequested(e);
        }

        private WebView webview = null;
        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedWebView> e)
        {
            try
            {
                base.OnElementChanged(e);

                if (e.NewElement == null)
                {
                    if (Control != null)
                    {
                        Control.NavigationCompleted -= OnWebViewNavigationCompleted;
                    }
                    webview = null;
                    return;
                }
                if (e.NewElement != null)
                {
                    if (Control == null)
                    {

                        Element.ResetRenderer = new Action(() =>
                        {
                            this.Dispose(true);
                        });
                        if (webview == null)
                        {
                            webview = new Windows.UI.Xaml.Controls.WebView();
                            SetNativeControl(webview);
                        }

                    }
                    if (!string.IsNullOrEmpty(Element.LocalFile))
                    {
                        if (Control != null)
                        {
                            Control.Source = new Uri(Element.LocalFile);
                        }
                    }
                    else
                    {
                        if (Control != null)
                        {
                            var filename = Element.IsMaterialContent ? "materialcontent.html" : Element.IsBehaviourContent ? "behaviourcontent.html" : Element.IsCaptureContent ? "capturecontent.html" :
                                                        Element.IsScoringDesc ? "scoringcontent.html" : null;
                            var foldername = "contenthtml";
                            if (!string.IsNullOrEmpty(filename))
                            {
                                Control.NavigationCompleted += OnWebViewNavigationCompleted;
                                Control.DefaultBackgroundColor = Windows.UI.Colors.Transparent;
                                var htmlSource = ((Xamarin.Forms.HtmlWebViewSource)(Element as ExtendedWebView).Source)
                                    .Html;
                                Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
                                if ((htmlSource == null))
                                {
                                    return;
                                }

                                if (!_htmlRegex.IsMatch(htmlSource))
                                {
                                    htmlSource = "<span>" + htmlSource + "</span>";
                                }
                                var bindingContext = Element.BindingContext.ToString();
                                if (bindingContext.Contains("AcademicSurveyLiteracyViewModel"))
                                    LoadData(GenerateString(htmlSource, true), filename, foldername);
                                else
                                    LoadData(GenerateString(htmlSource), filename, foldername);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at ExtendedWebViewRenderer OnElementChanged: " + ex.Message);

            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Element != null && Element is ExtendedWebView element && e.PropertyName.Equals("Source"))
            {
                var filename = Element.IsMaterialContent ? "materialcontent.html" : Element.IsBehaviourContent ? "behaviourcontent.html" : Element.IsCaptureContent ? "capturecontent.html" :
                            Element.IsScoringDesc ? "scoringcontent.html" : null;
                var foldername = "contenthtml";
                if (!string.IsNullOrEmpty(filename))
                {
                    Control.NavigationCompleted += OnWebViewNavigationCompleted;
                    var htmlSource = ((Xamarin.Forms.HtmlWebViewSource)(Element as ExtendedWebView).Source).Html;
                    if ((htmlSource == null))
                    {
                        return;
                    }
                    var bindingContext = Element.BindingContext.ToString();
                    if (bindingContext.Contains("AcademicSurveyLiteracyViewModel"))
                        LoadData(GenerateString(htmlSource, true), filename, foldername);
                    else
                        LoadData(GenerateString(htmlSource), filename, foldername);

                }
            }
            else if (e.PropertyName == "LocalFile")
            {
                if (!string.IsNullOrEmpty(Element.LocalFile))
                {
                    if (Control != null)
                    {
                        Control.Source = new Uri(Element.LocalFile);
                    }
                }
            }
        }

        private string GenerateString(string htmlstringtoappend, bool isAcademicContent = false)
        {
            var htmlstring = "";
            htmlstringtoappend = "<span>" + htmlstringtoappend + "</span>";
            if (htmlstringtoappend.Contains("math"))
            {
                if (isAcademicContent)
                {
                    htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta charset='utf-8'/></head>";
                    htmlstring += "<body><script type='text/javascript' src='es5/tex-mml-chtml.js' defer></script> <link rel='stylesheet' type='text/css' href='styles/record_form.css'>";
                }
                else
                {
                    htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta charset='utf-8'/></head>";
                    htmlstring += "<body><script type='text/javascript' src='es5/tex-mml-chtml.js' defer></script> <link rel='stylesheet' type='text/css' href='styles/assessment_form.css'>";
                }
            }
            else
            {
                if (isAcademicContent)
                {
                    htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta charset='utf-8'/></head>";
                    htmlstring += "<body><link rel='stylesheet' type='text/css' href='styles/record_forms.css'>";
                }
                else
                {
                    htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta charset='utf-8'/></head>";
                    htmlstring += "<body><link rel='stylesheet' type='text/css' href='styles/assessment_form.css'>";
                }
            }
            return htmlstring + htmlstringtoappend + "</body></html>";
        }

        private async void OnWebViewNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (Control == null)
                return;
            if (!args.IsSuccess)
                return;
            await Task.Delay(100);

            var documentElementoffsetHeightString = await Control.InvokeScriptAsync("eval", new[] { "document.documentElement.scrollHeight.toString()" });
            int documentElementoffsetHeight = Int32.Parse(documentElementoffsetHeightString);
            if (Element != null)
            {
                Element.HeightRequest = documentElementoffsetHeight;
            }

            var widthString = await Control.InvokeScriptAsync("eval", new[] { "document.body.scrollWidth.toString()" });
            if (int.TryParse(widthString, out int width))
            {
                if (Element != null)
                {
                    Element.WidthRequest = width;
                }
            }

            if (args.Uri.AbsolutePath.Contains("capturecontent.html"))
            {
                //Restrict the height inclusive of white spaces 
                if (Element.HeightRequest > NumericConstants.CaptureModeWebViewHeightRange)
                    Element.HeightRequest = NumericConstants.CaptureModeWebViewHeight;
            }

            if (args.Uri.AbsolutePath.Contains("scoringcontent.html"))
            {
                //Restrict the height inclusive of white spaces 
                if (Element.HeightRequest > NumericConstants.ScoringWebViewHeightRange)
                    Element.HeightRequest = NumericConstants.ScoringWebViewHeight;
            }

        }

        public async void LoadData(string contenttoWrite, string filename, string foldeername)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    if (Control != null)
                    {
                        await System.IO.File.WriteAllTextAsync(System.IO.Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, foldeername, filename), contenttoWrite);
                        try
                        {
                            Control.Source = new Uri("ms-appdata:///local/" + foldeername + "/" + filename);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO : Retry Machanism Need to find the bset way
                    LoadData(contenttoWrite, filename, foldeername);
                }
            });
        }
        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.NavigationCompleted -= OnWebViewNavigationCompleted;
            }
            if (Element != null)
            {
                Element.BindingContext = null;
            }
            webview = null;
            GC.Collect();
            GC.SuppressFinalize(this);
            base.Dispose(disposing);
        }
    }
}