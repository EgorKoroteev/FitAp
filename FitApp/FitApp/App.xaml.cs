using FitApp.Services;
using FitApp.Database;
using FitApp.Views;
using System;
using System.IO;
using Xamarin.Forms;

namespace FitApp
{
    public partial class App : Application
    {
        static DataBaseHelper databaseHelper;

        public static DataBaseHelper DatabaseHelper
        {
            get
            {
                if (databaseHelper == null)
                {
                    databaseHelper = new DataBaseHelper(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FitAppDataBase.db3"));
                }
                return databaseHelper;
            }
        }
        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();

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
