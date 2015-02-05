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
using Foundation;
using UIKit;
using Vernacular;
using WF.Player.Core;
using WF.Player.Core.Formats;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using WF.Player.iOS.Services.Core;
using WF.Player.Services.Settings;

namespace WF.Player.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : FormsApplicationDelegate
	{
		// class-level declarations
//		UIWindow window;
//		UINavigationController navCartSelect;
		NSObject observerSettings;

		// Google Maps API Key for iOS
//		const string MapsApiKey = "AIzaSyCgldJMI1uFzqYWU7kEjRz_-kVkDRZxBN0";

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

				// Rethrow any unhandled .NET exceptions as native iOS 
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

			// Activate Vernacular Catalog
			Catalog.Implementation = new ResourceCatalog 
				{
					GetResourceById = id => {
						var resource = 	NSBundle.MainBundle.LocalizedString(id, null);
						return resource == id ? null : resource;
					},
				};

			// Set Google Maps API Key
//			MapServices.ProvideAPIKey (MapsApiKey);

			// create a new window instance based on the screen size
//			window = new UIWindow (UIScreen.MainScreen.Bounds);

			// Start Xamarin.Forms
			Xamarin.Forms.Forms.Init ();
			Xamarin.FormsMaps.Init();

			// Set default color for NavigationButtons
			UIBarButtonItem.Appearance.TintColor = App.Colors.Bar.ToUIColor();

			// Create NavigationControlls
//			navCartSelect = new UINavigationController();

			// Init observer for changes of the settings
			observerSettings = NSNotificationCenter.DefaultCenter.AddObserver((NSString)"NSUserDefaultsDidChangeNotification", DefaultsChanged);

			// Create Xamarin.Forms App and load the first page
			LoadApplication(new App(new iOSPlatformHelper()));

			return base.FinishedLaunching(app, options);
		}

		public override void WillTerminate(UIApplication uiApplication)
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver(observerSettings);

			base.WillTerminate(uiApplication);
		}
		/// <summary>
		/// Receives the memory warning.
		/// </summary>
		/// <param name="application">Application.</param>
		public override void ReceiveMemoryWarning (UIApplication application)
		{
			// TODO: Delete
			Console.WriteLine ("ReceiveMemoryWarning");

			base.ReceiveMemoryWarning(application);

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

			// Update CartridgeStore
			if (App.Navigation.CurrentPage is CartridgeListPage)
			{
				((CartridgeListPage)App.Navigation.CurrentPage).RefreshCommand.Execute(null);
			}

			return true;
		}
			
		#region Private Functions

		void DefaultsChanged(NSNotification obj)
		{
			Settings.Current.Changed();
		}

		#endregion
	}
}