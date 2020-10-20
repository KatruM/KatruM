using Android.Content;
using Android.Views;
using Android.Webkit;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.Droid.CustomRenderer;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android;
//using WebView = Android.Webkit.WebView;
[assembly: Xamarin.Forms.ExportRenderer(typeof(ExtendedWebView), typeof(ExtendedWebViewRenderer))]
namespace BDI3Mobile.Droid.CustomRenderer
{
    public  class ExtendedWebViewRenderer : ViewRenderer<ExtendedWebView, WebView>
    {
        static ExtendedWebView _xwebView = null;
        WebView webview;
        Context _context;

        public ExtendedWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        //https://lukealderton.com/blog/posts/2016/may/autocustom-height-on-xamarin-forms-webview-for-android-and-ios/
        //class ExtendedWebViewClient : Android.Webkit.WebViewClient
        //{
        //    public override async void OnPageFinished(WebView view, string url)
        //    {
        //        if (_xwebView != null)
        //        {
        //            int i = 10;
        //            while (view.ContentHeight == 0 && i-- > 0) // wait here till content is rendered
        //                await System.Threading.Tasks.Task.Delay(100);
        //            _xwebView.HeightRequest = view.ContentHeight;
        //        }
        //        base.OnPageFinished(view, url);
        //    }
        //}

        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedWebView> e)
        {
            base.OnElementChanged(e);
            
            try
            {
                base.OnElementChanged(e);
                
                if (e.NewElement != null)
                {
                    if (Control == null)
                    {
                        Element.ResetRenderer = new Action(() =>
                        {
                            this.Dispose(false);
                        });
                        webview = new WebView(_context);
                        webview.Settings.JavaScriptEnabled = true;
                        SetNativeControl(webview);
                    }
                    if (!string.IsNullOrEmpty(Element.LocalFile))
                    {
                        webview.LoadUrl(Element.LocalFile);
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

                _xwebView = e.NewElement as ExtendedWebView;
                webview = Control;

                if (e.OldElement == null)
                {
                    Control.SetWebViewClient(new ExtendedWebViewClient(_xwebView));
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
                    webview.LoadUrl(Element.LocalFile);
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
        public async void LoadData(string contenttoWrite, string filename, string foldeername)
        {
            try
            {  
                await System.IO.File.WriteAllTextAsync(System.IO.Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, foldeername, filename), contenttoWrite);
                Control.LoadUrl("file://" + System.IO.Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, foldeername, filename));
                
            }
            catch (Exception ex)
            {
                //LoadData(contenttoWrite, filename, foldeername);
            }
        }
        //public override bool DispatchTouchEvent(MotionEvent e)
        //{
        //    Parent.RequestDisallowInterceptTouchEvent(true);
        //    return base.DispatchTouchEvent(e);
        //}
        void Control_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {
            // Executing this will prevent the Scrolling to be intercepted by parent views
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    Control.Parent.RequestDisallowInterceptTouchEvent(true);
                    break;
                case MotionEventActions.Up:
                    Control.Parent.RequestDisallowInterceptTouchEvent(false);
                    break;
            }
            // Calling this will allow the scrolling event to be executed in the WebView
            Control.OnTouchEvent(e.Event);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && Element != null)
            {
                //webview.Dispose();
                //webview = null;
            }
            base.Dispose(disposing);
        }
    }
}