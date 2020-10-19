using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using LabelHtml.Forms.Plugin.iOS;
using UIKit;
using Xamarin.Forms;

namespace BDI3Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]

    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            HtmlLabelRenderer.Initialize();
            Rg.Plugins.Popup.Popup.Init();
            Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            LoadApplication(new App());
            ConfigureSession();
            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            if (Window != null)
            {
                var allTapGesture = new WindowGesture();
                Window.AddGestureRecognizer(allTapGesture);
            }
            return base.FinishedLaunching(app, options);

        }
        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
           

        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
            SessionManager.SessionManager.Instance.ResumeSession();
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        private void ConfigureSession()
        {
            SessionManager.SessionManager.Instance.StartSessionTimer();
        }
        class WindowGesture : UIGestureRecognizer
        {
            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);
                //SessionManager.SessionManager.Instance.ExtendSession();
            }
            public override void TouchesEnded(NSSet touches, UIEvent evt)
            {
                SessionManager.SessionManager.Instance.ExtendSession();
                this.State = UIGestureRecognizerState.Failed;
                base.TouchesEnded(touches, evt);
            }
            public override void TouchesMoved(NSSet touches, UIEvent evt)
            {
                base.TouchesMoved(touches, evt);
            }
        }
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            return UIInterfaceOrientationMask.Landscape;
        }
       
    }
}
