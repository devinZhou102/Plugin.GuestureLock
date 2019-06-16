using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace GuetureLock.Sample
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                case Device.iOS:
                    MainPage = new NavigationPage(new MainPage());
                   // MainPage = new MainShell();
                    break;
                default:
                    MainPage = new NavigationPage(new MainPage());
                    break;
            }

		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
