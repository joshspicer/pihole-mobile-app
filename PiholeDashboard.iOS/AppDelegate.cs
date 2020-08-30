using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Forms;

namespace PiholeDashboard.iOS
{

    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        App myApp;

        AppDelegate ()
        {
            Xamarin.Forms.Forms.SetFlags(new string[]
            {
              "AppTheme_Experimental",
              "CollectionView_Experimental",
              "RadioButton_Experimental"
            });

            global::Xamarin.Forms.Forms.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            myApp = new App();
        }

        public bool HandleShortcutItem(UIApplicationShortcutItem shortcutItem)
        {
            var handled = false;

            // Anything to process?
            if (shortcutItem == null) return false;

            // Take action based on the shortcut type
            switch (shortcutItem.Type)
            {
                case ShortcutIdentifier.StopCommand:
                    MessagingCenter.Send(myApp, "StopCommand");
                    handled = true;
                    break;
                case ShortcutIdentifier.StartCommand:
                    MessagingCenter.Send(myApp, "StartCommand");
                    handled = true;
                    break;
            }

            // Return results
            return handled;
        }

        public override void PerformActionForShortcutItem(UIApplication application,
                                                          UIApplicationShortcutItem shortcutItem,
                                                          UIOperationHandler completionHandler)
            => completionHandler(HandleShortcutItem(shortcutItem));
        

        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            // ** Constuctor now handles creation of App().

            LoadApplication(this.myApp);

            return base.FinishedLaunching(app, options);
        }
    }
}
