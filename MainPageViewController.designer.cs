// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace MyoSampleApp
{
	[Register ("MainPageViewController")]
	partial class MainPageViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel accelerationLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIProgressView accelerationProgressBar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel armLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnConnect { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel helloLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (accelerationLabel != null) {
				accelerationLabel.Dispose ();
				accelerationLabel = null;
			}
			if (accelerationProgressBar != null) {
				accelerationProgressBar.Dispose ();
				accelerationProgressBar = null;
			}
			if (armLabel != null) {
				armLabel.Dispose ();
				armLabel = null;
			}
			if (btnConnect != null) {
				btnConnect.Dispose ();
				btnConnect = null;
			}
			if (helloLabel != null) {
				helloLabel.Dispose ();
				helloLabel = null;
			}
		}
	}
}
