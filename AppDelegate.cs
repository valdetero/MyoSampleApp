﻿using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Myo;

namespace MyoSampleApp
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		static UIStoryboard Storyboard = UIStoryboard.FromName("MainPage", null);
		static UIViewController controller;

		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			TLMHub.SharedHub().ApplicationIdentifier = "com.myo.MySampleApp";
			TLMHub.SharedHub().AttachToAdjacent();

			// create a new window instance based on the screen size
			window = new UIWindow(UIScreen.MainScreen.Bounds);
			controller = Storyboard.InstantiateInitialViewController() as UIViewController;
			
			// If you have defined a root view controller, set it here:
			 window.RootViewController = controller;
			//window.RootViewController = new MainPageViewController();
			
			// make the window visible
			window.MakeKeyAndVisible();


			return true;
		}

		public override UIWindow Window { get; set; }
	}
}

