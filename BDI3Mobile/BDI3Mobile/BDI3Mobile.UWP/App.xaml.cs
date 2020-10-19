using BDI3Mobile.IServices;
using BDI3Mobile.UWP.DependencyService;
using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Popup = Rg.Plugins.Popup.Popup;



namespace BDI3Mobile.UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static new App Current => (App)Application.Current;

        public event EventHandler IsIdleChanged;

        private DispatcherTimer idleTimer;
        public DispatcherTimer IdleTimer
        {
            get { return idleTimer; }
            set
            {
                idleTimer = value;
            }
        }
        private bool isIdle;
        public bool IsIdle
        {
            get
            {
                return isIdle;
            }

            private set
            {
                if (isIdle != value)
                {
                    isIdle = value;
                    IsIdleChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            Application.Current.Resuming += new EventHandler<Object>(App_Resuming);

        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {


            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                Popup.Init();

                // Need that .NET Native has worked
                Xamarin.Forms.Forms.Init(e, Popup.GetExtraAssemblies());
                var rendererAssemblies = new List<Assembly>() { };
                rendererAssemblies.AddRange(Popup.GetExtraAssemblies());
                Xamarin.Forms.Forms.Init(e, rendererAssemblies);
                Xamarin.Forms.DependencyService.Register<ICreateHtmlFiles, CreateHtmlFiles>();
                ConfigureSession();
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
                TappedEventHandler tapped = TappedCallback;
                DoubleTappedEventHandler doubleTapped = DoubleTappedCallback;
                RightTappedEventHandler rightTappedEventHandler = RightTappedCallback;

                Window.Current.CoreWindow.PointerMoved += onCoreWindowPointerMoved;
                Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
                Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
                Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void RightTappedCallback(object sender, RightTappedRoutedEventArgs e)
        {
            SessionManager.SessionManager.Instance.ExtendSession();
        }
        private void TappedCallback(object sender, TappedRoutedEventArgs e)
        {
            SessionManager.SessionManager.Instance.ExtendSession();
        }
        private void DoubleTappedCallback(object sender, DoubleTappedRoutedEventArgs e)
        {
            SessionManager.SessionManager.Instance.ExtendSession();
        }
        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            SessionManager.SessionManager.Instance.ExtendSession();
        }

        private void CoreWindow_KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            SessionManager.SessionManager.Instance.ExtendSession();
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            SessionManager.SessionManager.Instance.ExtendSession();
        }

        private void ConfigureSession()
        {
            SessionManager.SessionManager.Instance.StartSessionTimer();
        }
        private void onIdleTimerTick(object sender, object e)
        {
            idleTimer.Stop();
            IsIdle = true;
        }

        private void onCoreWindowPointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            SessionManager.SessionManager.Instance.ExtendSession();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
        /// <summary>
        /// On App resume from suspended state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_Resuming(object sender, object e)
        {
            //Bug Fix: 3610
            SessionManager.SessionManager.Instance.ResumeSession();
        }
    }
}
