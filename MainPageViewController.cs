using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using Myo;
using MonoTouch.CoreAnimation;
using OpenTK;

namespace MyoSampleApp
{
	partial class MainPageViewController : UIViewController
	{
		TLMPose currentPose;
		UINavigationController controller;


		public MainPageViewController (IntPtr handle) : base (handle)
		{

		}

		public override void ViewDidLoad()
		{
			Xamarin.Insights.Track("viewDidLoad");

			base.ViewDidLoad();

			accelerationProgressBar.Hidden = true;

			btnConnect.TouchUpInside += HandleTouchUpInside;

			TLMHub.Notifications.ObserveTLMHubDidConnectDevice(deviceConnected);
			TLMHub.Notifications.ObserveTLMHubDidDisconnectDevice(deviceDisconnected);

			TLMMyo.Notifications.ObserveTLMMyoDidReceiveArmRecognizedEvent(armRecognized);
			TLMMyo.Notifications.ObserveTLMMyoDidReceiveArmLostEvent(armLost);
			TLMMyo.Notifications.ObserveTLMMyoDidReceiveOrientationEvent(receiveOrientationEvent);
			TLMMyo.Notifications.ObserveTLMMyoDidReceiveAccelerometerEvent(receiveAccelerometerEvent);
			TLMMyo.Notifications.ObserveTLMMyoDidReceivePoseChanged(receivePoseChanged);
		}

		void HandleTouchUpInside (object sender, EventArgs e)
		{
			Xamarin.Insights.Track("touchupinside");

			controller = TLMSettingsViewController.SettingsInNavigationController();
			controller.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
			// Present the settings view controller modally.
			this.PresentViewController(controller, true, null);
		}

		void deviceConnected(object sender, NSNotificationEventArgs e)
		{
			Xamarin.Insights.Track("deviceConnected");

			var notification = e.Notification;

			// Set the text of the armLabel to "Perform the Sync Gesture"
			this.armLabel.Text = @"Perform the Sync Gesture";

			// Set the text of our helloLabel to be "Hello Myo".
			this.helloLabel.Text = @"Hello Myo";

			// Show the acceleration progress bar
			this.accelerationProgressBar.Hidden = false;
			this.accelerationLabel.Hidden = false;
		}

		void deviceDisconnected(object sender, NSNotificationEventArgs e)
		{
			Xamarin.Insights.Track("deviceDisconnected");

			var notification = e.Notification;
			// Remove the text of our label when the Myo has disconnected.
			this.helloLabel.Text = @"";
			this.armLabel.Text = @"";

			// Hide the acceleration progress bar
			this.accelerationProgressBar.Hidden = true;
			this.accelerationLabel.Hidden = true;
		}

		void armRecognized(object sender, NSNotificationEventArgs e)
		{
			Xamarin.Insights.Track("armRecognized");

			var notification = e.Notification;
			// Retrieve the arm event from the notification's userInfo with the kTLMKeyArmRecognizedEvent key.
			TLMArmRecognizedEvent armEvent = notification.UserInfo[TLMMyo.TLMKeyArmRecognizedEvent] as TLMArmRecognizedEvent;

			// Update the armLabel with arm information
			String armString = armEvent.Arm == TLMArm.TLMArmRight ? "Right" : "Left";
			String directionString = armEvent.xDirection == TLMArmXDirection.TLMArmXDirectionTowardWrist ? "Toward Wrist" : "Toward Elbow";
			armLabel.Text = string.Format("Arm: {0} X-Direction: {1}", armString, directionString);
		}

		void armLost (object sender, NSNotificationEventArgs e)
		{
			Xamarin.Insights.Track("armLost");

			var notification = e.Notification;
			// Reset the armLabel and helloLabel
			armLabel.Text = "Perform the Sync Gesture";
			helloLabel.Text = "Hello Myo";
			helloLabel.Font = UIFont.FromName("Helvetica Neue", 50);
			helloLabel.TextColor = UIColor.Black;
		}

		void receiveOrientationEvent(object sender, NSNotificationEventArgs e)
		{
			Xamarin.Insights.Track("receiveOrientationEvent");

			var notification = e.Notification;
			// Retrieve the orientation from the NSNotification's userInfo with the kTLMKeyOrientationEvent key.
			TLMOrientationEvent orientationEvent = notification.UserInfo[TLMMyo.TLMKeyOrientationEvent] as TLMOrientationEvent;

			// Create Euler angles from the quaternion of the orientation.
			TLMEulerAngles angles = TLMEulerAngles.GetAnglesWithQuaternion(orientationEvent.Quaternion);

			try {
				// Next, we want to apply a rotation and perspective transformation based on the pitch, yaw, and roll.
				var pitchF = Convert.ToSingle(angles.pitch.Radians);
				var yawF = Convert.ToSingle(angles.yaw.Radians);
				var rollF = Convert.ToSingle(angles.roll.Radians);

				var pitch = CATransform3D.MakeRotation(pitchF, -1.0f, 0.0f, 0.0f);
				var yaw = CATransform3D.MakeRotation(yawF, 0.0f, 1.0f, 0.0f);
				var roll = CATransform3D.MakeRotation(rollF, 0.0f, 0.0f, -1.0f);
		
				CATransform3D rotationAndPerspectiveTransform = pitch.Concat(yaw).Concat(roll);

				// Apply the rotation and perspective transform to helloLabel.
				helloLabel.Layer.Transform = rotationAndPerspectiveTransform;
			} catch(Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex.Message);
				Xamarin.Insights.Report(ex);
			}
		}

		void receiveAccelerometerEvent(object sender, NSNotificationEventArgs e)
		{
			Xamarin.Insights.Track("receiveAccelerometerEvent");

			var notification = e.Notification;
			// Retrieve the accelerometer event from the NSNotification's userInfo with the kTLMKeyAccelerometerEvent.
			TLMAccelerometerEvent accelerometerEvent = notification.UserInfo[TLMMyo.TLMKeyAccelerometerEvent] as TLMAccelerometerEvent;

			// Get the acceleration vector from the accelerometer event.
			Vector3 accelerationVector = accelerometerEvent.Vector;

			// Calculate the magnitude of the acceleration vector.
			float magnitude = accelerationVector.Length;

			// Update the progress bar based on the magnitude of the acceleration vector.
			accelerationProgressBar.Progress = magnitude / 8;

			/* Note you can also access the x, y, z values of the acceleration (in G's) like below
		     float x = accelerationVector.x;
		     float y = accelerationVector.y;
		     float z = accelerationVector.z;
		     */
//			accelerationLabel.Text = string.Format("x: {0}, y: {1}, z: {2}", 
//				accelerationVector.X, accelerationVector.Y, accelerationVector.Z);
		}

		void receivePoseChanged(object sender, NSNotificationEventArgs e)
		{
			Xamarin.Insights.Track("receivePoseChanged");

			var notification = e.Notification;
			// Retrieve the pose from the NSNotification's userInfo with the kTLMKeyPose key.
			TLMPose pose = notification.UserInfo[TLMMyo.TLMKeyPose] as TLMPose;
			currentPose = pose;

			// Handle the cases of the TLMPoseType enumeration, and change the color of helloLabel based on the pose we receive.
			switch(pose.Type) {
			case TLMPoseType.TLMPoseTypeUnknown:
			case TLMPoseType.TLMPoseTypeRest:
				// Changes helloLabel's font to Helvetica Neue when the user is in a rest or unknown pose.
				helloLabel.Text = @"Hello Myo";
				helloLabel.Font = UIFont.FromName("Helvetica Neue", 50);
				helloLabel.TextColor = UIColor.Black;
				break;
			case TLMPoseType.TLMPoseTypeFist:
				// Changes helloLabel's font to Noteworthy when the user is in a fist pose.
				helloLabel.Text = @"Fist";
				helloLabel.Font = UIFont.FromName("Noteworthy", 50);
				helloLabel.TextColor = UIColor.Green;
				break;
			case TLMPoseType.TLMPoseTypeWaveIn:
				// Changes helloLabel's font to Courier New when the user is in a wave in pose.
				helloLabel.Text = @"Wave In";
				helloLabel.Font = UIFont.FromName("Courier New", 50);
				helloLabel.TextColor = UIColor.Green;
				break;
			case TLMPoseType.TLMPoseTypeWaveOut:
				// Changes helloLabel's font to Snell Roundhand when the user is in a wave out pose.
				helloLabel.Text = @"Wave Out";
				helloLabel.Font = UIFont.FromName("Snell Roundhand", 50);
				helloLabel.TextColor = UIColor.Green;
				break;
			case TLMPoseType.TLMPoseTypeFingersSpread:
				// Changes helloLabel's font to Chalkduster when the user is in a fingers spread pose.
				helloLabel.Text = @"Fingers Spread";
				helloLabel.Font = UIFont.FromName("Chalkduster", 50);
				helloLabel.TextColor = UIColor.Green;
				break;
			case TLMPoseType.TLMPoseTypeThumbToPinky:
				// Changes helloLabel's font to Superclarendon when the user is in a twist in pose.
				helloLabel.Text = @"Thumb to Pinky";
				helloLabel.Font = UIFont.FromName("Georgia", 50);
				helloLabel.TextColor = UIColor.Green;
				break;
			}
		}
	}
}
