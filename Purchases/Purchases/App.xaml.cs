using Acr.UserDialogs;
using Purchases.Services;
using Purchases.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Purchases {
    public partial class App:Application {

        public App () {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        public static void Toast(string message) {
            UserDialogs.Instance.Toast(message);
        }

        protected override void OnStart () {
        }

        protected override void OnSleep () {
        }

        protected override void OnResume () {
        }
    }
}
