using System;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using BDI3Mobile.CustomRenderer;

using BDI3Mobile.iOS.CustomRenderer;
using Foundation;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(ExtendedWebView), typeof(ExtendedWebViewRenderer))]
namespace BDI3Mobile.iOS.CustomRenderer
{
    public class ExtendedWebViewRenderer : ViewRenderer<ExtendedWebView, WKWebView>
    {
        WKWebView webview;


        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedWebView> e)
        {
            base.OnElementChanged(e);
            
            if (Control == null)
            {
                var config = new WKWebViewConfiguration();
                config.DataDetectorTypes = WebKit.WKDataDetectorTypes.Address;
                webview = new WKWebView(Frame, config);
                
                SetNativeControl(webview);
            }
            if (Element != null)
            {
                if (!string.IsNullOrEmpty(Element.LocalFile))
                {
                    
                    string url = Element.LocalFile;
                    if (url.Contains("ms-appdata:"))
                    {
                        var urlToAppend = url.Substring(20, url.Length - 20);
                        var iOSPath = Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, urlToAppend);
                        webview.LoadRequest(new NSUrlRequest(new NSUrl("file://" + iOSPath)));
                    }
                    else
                    {
                        var iOSPath1 = Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, Element.LocalFile);
                        NSString urlString = (NSString)("file://" + iOSPath1);
                        webview.LoadFileUrl(new NSUrl(urlString), new NSUrl(urlString));
                        //webview.LoadRequest(new NSUrlRequest(new NSUrl("file://" + iOSPath1)));
                    }

                }
                
                else
                {
                    var filename = Element.IsMaterialContent ? "materialcontent.html" : Element.IsBehaviourContent ? "behaviourcontent.html" : Element.IsCaptureContent ? "capturecontent.html" :
                        Element.IsScoringDesc ? "scoringcontent.html" : null;
                    var foldername = "contenthtml";
                    if (!string.IsNullOrEmpty(filename))
                    {
                        var htmlSource = ((Xamarin.Forms.HtmlWebViewSource)(Element as ExtendedWebView).Source)
                            .Html;
                        Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
                        if (htmlSource != null)
                        {
                            if (!_htmlRegex.IsMatch(htmlSource))
                            {
                                htmlSource =  "<span>" + htmlSource + "</span>";

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
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Element is ExtendedWebView element && e.PropertyName.Equals("Source"))
            {
                var filename = Element.IsMaterialContent ? "materialcontent.html" : Element.IsBehaviourContent ? "behaviourcontent.html" : Element.IsCaptureContent ? "capturecontent.html" :
                            Element.IsScoringDesc ? "scoringcontent.html" : null;
                var foldername = "contenthtml";
                if (!string.IsNullOrEmpty(filename))
                {
                    var htmlSource = ((Xamarin.Forms.HtmlWebViewSource)(Element as ExtendedWebView).Source).Html;
                    Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

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
                    string url = Element.LocalFile;
                    if (!string.IsNullOrEmpty(Element.LocalFile))
                    {
                        if (url.Contains("ms-appdata:"))
                        {
                            var urlToAppend = url.Substring(20, url.Length - 20);
                            var iOSPath = Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, urlToAppend);
                            webview.LoadRequest(new NSUrlRequest(new NSUrl("file://" + iOSPath)));
                        }
                        else
                        {
                            var iOSPath1 = Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, Element.LocalFile);
                            NSString urlString = (NSString)("file://" + iOSPath1);
                            webview.LoadFileUrl(new NSUrl(urlString), new NSUrl(urlString));
                            //webview.LoadRequest(new NSUrlRequest(new NSUrl("file://" + iOSPath1)));
                        }

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
                    htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta charset='utf-8' name='viewport' content='initial-scale = 1.0' /></head>";
                    htmlstring += "<body><script type='text/javascript' src='es5/tex-mml-chtml.js' defer></script> <link rel='stylesheet' type='text/css' href='styles/record_form.css'>";
                }
                else
                {
                    htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta charset='utf-8' name='viewport' content='initial-scale = 1.0' /></head>";
                    htmlstring += "<body><script type='text/javascript' src='es5/tex-mml-chtml.js' defer></script> <link rel='stylesheet' type='text/css' href='styles/assessment_form.css'>";
                }
            }
            else
            {
                if (isAcademicContent)
                {
                    htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta charset='utf-8' name='viewport' content='initial-scale = 1.0' /></head>";
                    htmlstring += "<body><link rel='stylesheet' type='text/css' href='styles/record_forms.css'>";
                }
                else
                {
                    htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta charset='utf-8' name='viewport' content='initial-scale = 1.0' /></head>";
                    htmlstring += "<body><link rel='stylesheet' type='text/css' href='styles/assessment_form.css'>";
                }
            }
            return htmlstring + htmlstringtoappend + "</body></html>";
        }
        public async void LoadData(string contenttoWrite, string filename, string foldeername)
        {
            try
            {
                await System.IO.File.WriteAllTextAsync(System.IO.Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, foldeername, filename), contenttoWrite);
                NSUrl nSUrl = new NSUrl("file://" + Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, foldeername, filename));
                Control.LoadFileUrl(nSUrl, nSUrl);

            }
            catch (Exception ex)
            {
                //LoadData(contenttoWrite, filename, foldeername);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && Element != null)
            {
                webview = null;
            }
            base.Dispose(disposing);
        }



    }

    public class NavigationDelegat : WKNavigationDelegate
    {
        public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            string fontSize = "300%"; // 300% is the size of font , set it as you want like 200% or 150%
            string stringsss = String.Format(@"document.getElementsByTagName('body')[0].style.webkitTextSizeAdjust= '{0}'", fontSize);
            WKJavascriptEvaluationResult handler = (NSObject result, NSError err) => {
                if (err != null)
                {
                    System.Console.WriteLine(err);
                }
                if (result != null)
                {
                    System.Console.WriteLine(result);
                }
            };
            webView.EvaluateJavaScript(stringsss, handler);

        }
    }

}
