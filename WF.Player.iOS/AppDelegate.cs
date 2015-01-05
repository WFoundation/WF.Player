///
/// WF.Player.iPhone - A Wherigo Player for iPhone which use the Wherigo Foundation Core.
/// Copyright (C) 2012-2014  Dirk Weltz <web@weltz-online.de>
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
/// 
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
/// 
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
/// 

using System;
using System.IO;
using System.Threading.Tasks;
using Google.Maps;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Vernacular;
using WF.Player.Core;
using WF.Player.Core.Formats;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace WF.Player.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		UINavigationController navCartSelect;
//		CartridgeList viewCartSelect;
//		ScreenController screenCtrl;
		NSObject observerSettings;

		// Google Maps API Key for iOS
		const string MapsApiKey = "AIzaSyCgldJMI1uFzqYWU7kEjRz_-kVkDRZxBN0";

		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			#if __HOCKEYAPP__

			// Init HockeyApp
			// We MUST wrap our setup in this block to wire up
			// Mono's SIGSEGV and SIGBUS signals
			HockeyApp.Setup.EnableCustomCrashReporting (() => {

				//Get the shared instance
				var manager = HockeyApp.BITHockeyManager.SharedHockeyManager;

				//Configure it to use our APP_ID
				manager.Configure ("2311139d00212362b8ae16fcb2c427ee");

				//Start the manager
				manager.StartManager ();

				//Authenticate (there are other authentication options)
				manager.Authenticator.AuthenticateInstallation ();

				//Rethrow any unhandled .NET exceptions as native iOS 
				// exceptions so the stack traces appear nicely in HockeyApp
				AppDomain.CurrentDomain.UnhandledException += (sender, e) => 
					HockeyApp.Setup.ThrowExceptionAsNative(e.ExceptionObject);

				TaskScheduler.UnobservedTaskException += (sender, e) => 
					HockeyApp.Setup.ThrowExceptionAsNative(e.Exception);
			});

			#endif

			#if __INSIGHTS__

			Xamarin.Insights.Initialize("92177c266f2ef360d4bd2c0376fc84fc4be2c406");

			#endif

			// Activate TestFlight
//			MonoTouch.TestFlight.TestFlight.TakeOffThreadSafe(@"d8fc2051-04bd-4612-b83f-19786b749aab");

			double version;
			double.TryParse(UIDevice.CurrentDevice.SystemVersion, out version);

			if (version >= 8)
			{
				App.PathCartridges = NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User) [0].RelativePath;
				App.PathDatabase = NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.LibraryDirectory, NSSearchPathDomain.User) [0].RelativePath;
			}
			else
			{
				App.PathCartridges = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				App.PathDatabase = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + ".." + System.IO.Path.DirectorySeparatorChar + "Library";
			}

			// Activate Vernacular Catalog
			Catalog.Implementation = new ResourceCatalog 
				{
					GetResourceById = id => {
						var resource = 	NSBundle.MainBundle.LocalizedString(id, null);
						return resource == id ? null : resource;
					},
				};

			// Screen always on when app is running
			UIApplication.SharedApplication.IdleTimerDisabled = true;

			// Set Google Maps API Key
			MapServices.ProvideAPIKey (MapsApiKey);

			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			// Start Xamarin.Forms
			Xamarin.Forms.Forms.Init ();
			Xamarin.FormsMaps.Init();

			// Set default color for NavigationButtons
			UIBarButtonItem.Appearance.TintColor = App.Colors.Bar.ToUIColor();

			// Create NavigationControlls
			navCartSelect = new UINavigationController();

			// Init observer for changes of the settings
			observerSettings = NSNotificationCenter.DefaultCenter.AddObserver((NSString)"NSUserDefaultsDidChangeNotification", DefaultsChanged);

			// Set the root view controller on the window. The nav
			// controller will handle the rest
			// If you have defined a view, add it here:
			// window.AddSubview (navigationController.View);
//			window.RootViewController = navCartSelect;
			window.RootViewController = App.GetMainPage ().CreateViewController ();

			// make the window visible
			window.MakeKeyAndVisible ();

			return true;
		}

		/// <Docs>Reference to the UIApplication that invoked this delegate method.</Docs>
		/// <remarks>Application are allocated approximately 5 seconds to complete this method. Application developers should use this
		/// time to save user data and tasks, and remove sensitive information from the screen.</remarks>
		/// <altmember cref="M:MonoTouch.UIKit.UIApplicationDelegate.WillEnterForeground"></altmember>
		/// <summary>
		/// Dids the enter background.
		/// </summary>
		/// <param name="application">Application.</param>
		public override void DidEnterBackground (UIApplication application)
		{
			// TODO: Delete
			Console.WriteLine ("DidEnterBackground");

			App.EnterBackground();

//			// Save game before going into background
//			if (screenCtrl != null && screenCtrl.Engine != null && screenCtrl.Engine.GameState == WF.Player.Core.Engines.EngineGameState.Playing) {
//				// Save game automatically
//				Console.WriteLine ("Start Save");
//				screenCtrl.Engine.Save (new FileStream (screenCtrl.Engine.Cartridge.SaveFilename, FileMode.Create));
//				Console.WriteLine ("Ende Save");
//				// Pause engine until we have focus again
//				screenCtrl.Engine.Pause ();
//			}
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// TODO: Delete
			Console.WriteLine ("WillEnterForeground");

			App.EnterForeground();

//			if (screenCtrl != null && screenCtrl.Engine != null && screenCtrl.Engine.GameState == WF.Player.Core.Engines.EngineGameState.Paused) {
//				// Resume engine, so we continue
//				screenCtrl.Engine.Resume ();
//			}
		}

		public override void OnResignActivation(UIApplication application)
		{
			// TODO: Delete
			Console.WriteLine ("OnResignActivation");

//			// Save game before going into background
//			if (screenCtrl != null && screenCtrl.Engine != null && screenCtrl.Engine.GameState == WF.Player.Core.Engines.EngineGameState.Playing) {
//				// Save game automatically
//				screenCtrl.Engine.Save (new FileStream (screenCtrl.Engine.Cartridge.SaveFilename, FileMode.Create));
//				// Pause engine until we have focus again
//				screenCtrl.Engine.Pause ();
//			}
		}

		public override void OnActivated(UIApplication application)
		{
			// TODO: Delete
			Console.WriteLine ("OnActivated");

//			if (screenCtrl != null && screenCtrl.Engine != null && screenCtrl.Engine.GameState == WF.Player.Core.Engines.EngineGameState.Paused) {
//				// Resume engine, so we continue
//				screenCtrl.Engine.Resume ();
//			}
		}

		public override void ReceiveMemoryWarning (UIApplication application)
		{
			// TODO: Delete
			Console.WriteLine ("ReceiveMemoryWarning");

			GC.Collect();

//			// Save game before we could get killed
//			if (screenCtrl != null && screenCtrl.Engine != null) {
//				// Free memory
//				screenCtrl.Engine.FreeMemory ();
//				// Save game automatically
//				screenCtrl.Engine.Save (new FileStream (screenCtrl.Engine.Cartridge.SaveFilename, FileMode.Create));
//			}
		}

		// Is called by other apps from "open in" dialogs
		public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			var sourceFile = url.Path;
			var destFile = System.IO.Path.Combine(Environment.GetFolderPath (Environment.SpecialFolder.Personal), System.IO.Path.GetFileName(sourceFile));

			if (!File.Exists (sourceFile))
				return false;

			var fileExists = false;

			if (File.Exists (destFile)) {
				fileExists = true;
				File.Delete (destFile);
			}

			File.Copy (sourceFile, destFile);

			Cartridge cart = new Cartridge(destFile);
			CartridgeLoaders.LoadMetadata(new FileStream(destFile, FileMode.Open), cart);

			// TODO
			// If there was a cartridge with the same filename, than replace
			if (fileExists) {
			}

			if (window.RootViewController.PresentedViewController is UINavigationController && window.RootViewController.PresentedViewController == navCartSelect) {
				// Now create a new list for cartridges
//				viewCartSelect = new CartridgeList(this);
				// Add the cartridge view to the navigation controller
				// (it'll be the top most screen)
				navCartSelect.PopToRootViewController (true);
//				navCartSelect.SetViewControllers(new UIViewController[] {(UIViewController)viewCartSelect}, false);
				//				CartridgeDetail cartDetail = new CartridgeDetail(this);
				//				((UINavigationController)window.RootViewController.PresentedViewController).PushViewController (cartDetail,true);
				//				cartDetail.Cartridge = cart;
			}

			return true;
		}
			
		#region Private Functions

		void DefaultsChanged(NSNotification obj)
		{
		}

		#endregion
	}
}