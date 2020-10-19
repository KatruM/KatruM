using System;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.iOS.CustomRenderer;
using UIKit;

namespace BDI3Mobile.Droid.CustomRenderer
{
    public class ExtendedUIWebViewDelegate : UIWebViewDelegate
    {
       
            ExtendedWebViewRenderer webViewRenderer;

            public ExtendedUIWebViewDelegate(ExtendedWebViewRenderer _webViewRenderer = null)
            {
                webViewRenderer = _webViewRenderer ?? new ExtendedWebViewRenderer();
            }

            public override async void LoadingFinished(UIWebView webView)
            {
                var wv = webViewRenderer.Element as ExtendedWebView;
                if (wv != null)
                {
                    await System.Threading.Tasks.Task.Delay(100); // wait here till content is rendered
                    wv.HeightRequest = (double)webView.ScrollView.ContentSize.Height;
                }
            }
    }
}
