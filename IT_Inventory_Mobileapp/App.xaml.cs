using IT_Inventory_Mobileapp;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IT_Inventory_Mobileapp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
           // DependencyService.Register<ITeLeltar, TeLeltar>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
