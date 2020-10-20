using Android.Webkit;
using BDI3Mobile.CustomRenderer;

namespace BDI3Mobile.Droid.CustomRenderer
{
    internal class ExtendedWebViewClient : WebViewClient
    {
        private ExtendedWebView _xwebView;

        public ExtendedWebViewClient(ExtendedWebView xwebView)
        {
            _xwebView = xwebView;
        }
    }
}