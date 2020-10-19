using System;
using Xamarin.Forms;

namespace BDI3Mobile.CustomRenderer
{
    public class MyEntry : Entry
    {

    }

    public class BorderlessEntry : Entry
    {

    }

    public class ExtendedWebView : WebView
    {
        public Action ResetRenderer { get; set; }
        // TODO : Need to change to theEnum for the better Code Refactor and Approach
        private bool isMaterialContent;
        public bool IsMaterialContent
        {
            get
            {
                return isMaterialContent;
            }
            set
            {
                isMaterialContent = value;
            }
        }

        private bool isScoringDesc;
        public bool IsScoringDesc
        {
            get
            {
                return isScoringDesc;
            }
            set
            {
                isScoringDesc = value;
            }
        }

        private bool isBehaviourContent;
        public bool IsBehaviourContent
        {
            get
            {
                return isBehaviourContent;
            }
            set
            {
                isBehaviourContent = value;
            }
        }

        private bool isCaptureContent;
        public bool IsCaptureContent
        {
            get
            {
                return isCaptureContent;
            }
            set
            {
                isCaptureContent = value;
            }
        }
        private bool enableHandler;
        public bool EnableHandler
        {
            get
            {
                return enableHandler;
            }
            set
            {
                enableHandler = value;
            }
        }
        public static readonly BindableProperty LocalFileProperty =
            BindableProperty.Create(nameof(LocalFile), typeof(string), typeof(ExtendedWebView), 
                "", BindingMode.OneWay);

        public string LocalFile
        {
            get { return (string)GetValue(LocalFileProperty); }
            set
            {
                SetValue(LocalFileProperty, value);
            }
        }

        public static readonly BindableProperty DestroyWebViewProperty =
            BindableProperty.Create(nameof(DestroyWebView), typeof(bool), typeof(ExtendedWebView),
                false, BindingMode.OneWay);

        public bool DestroyWebView
        {
            get { return (bool)GetValue(DestroyWebViewProperty); }
            set
            {
                SetValue(DestroyWebViewProperty, value);
            }
        }

    }
}
