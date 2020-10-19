using BDI3Mobile.View;
using BDI3Mobile.Views.PopupViews;
using Rg.Plugins.Popup.Services;
using System;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDI3Mobile.SessionManager
{
    public sealed class SessionManager
    {

        Timer timer;
        bool doLogout = true;
        CustomPopupView popup;
        static string SessionExpiringPopUpTitle = "SessionExpiring";
        const double SessionExpiryInMilliSeconds = 900000;
        const double SessionWarningInMilliSeconds = 300000;

        DateTime StartTime;
        //const double SessionExpiryInMilliSeconds = 9000;
        // const double SessionWarningInMilliSeconds = 8000;

        public ICommand SearchErrorContinueCommand { get; set; }
        static readonly Lazy<SessionManager> lazy =
            new Lazy<SessionManager>(() => new SessionManager());

        public static SessionManager Instance { get { return lazy.Value; } }

        SessionManager()
        {
            SearchErrorContinueCommand = new Command(SessionWarningContinueCommand);
            popup = new Views.PopupViews.CustomPopupView(new Views.PopupViews.CustomPopUpDetails() { Header = "Session Expiring", Message = "Session will expire in 5 minutes, press Continue to extend time.", Height = 211, Width = 450 }, title: SessionExpiringPopUpTitle);
            popup.CloseWhenBackgroundIsClicked = true;
            popup.BindingContext = this;
        }
        public void StartSessionTimer()
        {
            timer = new Timer();
            timer.Interval = SessionExpiryInMilliSeconds;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            StartTime = DateTime.Now;
        }
        private void StartTimer()
        {
            timer.Start();
        }
        private void StopTimer()
        {
            timer.Stop();
        }
        private void ResetTimer()
        {
            timer.Stop();
            timer.Start();
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async() =>
            {
                if (PopupNavigation.Instance.PopupStack.Count > 0 && !(Application.Current.MainPage is LoginView || Application.Current.MainPage is NavigationPage navPage && navPage.CurrentPage is LoginView))
                {
                    await PopupNavigation.Instance.PopAllAsync(false);
                }
                if (doLogout)
                {
                    if(!(Application.Current.MainPage is LoginView || Application.Current.MainPage is NavigationPage navigationPage && navigationPage.CurrentPage is LoginView))
                    {
                        App.LogoutAction(true);
                    }
                    return;
                }
                else
                {
                    ExtendWarningSession();
                    if (!(Application.Current.MainPage is LoginView || Application.Current.MainPage is NavigationPage navigationPage && navigationPage.CurrentPage is LoginView))
                    {
                        var popUpNavigationInstance = PopupNavigation.Instance;
                        await popUpNavigationInstance.PushAsync(popup);
                    }
                }
                return;
            });
        }
        public void ExtendSession()
        {
            if(PopupNavigation.Instance.PopupStack.Count > 0)
            {
                foreach(var popup in PopupNavigation.Instance.PopupStack)
                {
                    if(popup is CustomPopupView && (popup as CustomPopupView).Title == SessionExpiringPopUpTitle)
                    {
                        return;
                    }
                }
            }
            doLogout = false;
            timer.Stop();
            timer.Interval = SessionExpiryInMilliSeconds;
            timer.Start();
            StartTime = DateTime.Now;
        }
        private void ExtendWarningSession()
        {
            doLogout = true;
            timer.Stop();
            timer.Interval = SessionWarningInMilliSeconds;
            timer.Start();
        }
        public async void SessionWarningContinueCommand(object obj)
        {
            doLogout = false;
            timer.Stop();
            timer.Interval = SessionExpiryInMilliSeconds;
            timer.Start();
            StartTime = DateTime.Now;
            await PopupNavigation.Instance.PopAsync();
        }

        //Bug Fix: 3610
        /// <summary>
        /// On app resume from suspended state - hanldes system sleep/wake up events
        /// </summary>
        public async void ResumeSession()
        {
            TimeSpan t = DateTime.Now - StartTime;

            if (t.TotalMinutes > 20)
            {
                if (PopupNavigation.Instance.PopupStack.Count > 0 && !(Application.Current.MainPage is LoginView || Application.Current.MainPage is NavigationPage navPage && navPage.CurrentPage is LoginView))
                {
                    await PopupNavigation.Instance.PopAllAsync(false);
                }
                if (!(Application.Current.MainPage is LoginView || Application.Current.MainPage is NavigationPage navigationPage && navigationPage.CurrentPage is LoginView))
                {
                    App.LogoutAction(true);
                }

            }
            else if (t.TotalMinutes > 14)
            {
                ExtendWarningSession();
                if (!(Application.Current.MainPage is LoginView || Application.Current.MainPage is NavigationPage navigationPage && navigationPage.CurrentPage is LoginView))
                {
                    var popUpNavigationInstance = PopupNavigation.Instance;
                    await popUpNavigationInstance.PushAsync(popup);
                }
            }
            return;
        }
    }
}