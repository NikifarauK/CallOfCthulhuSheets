using CallOfCthulhuSheets.Services;
using CallOfCthulhuSheets.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace CallOfCthulhuSheets
{
    public partial class App : Application
    {
        //public bool IsLogedIn { get; set; }

        Models.Player CurrentPlayer { get; set; }

        readonly DataBaseHandler dbh;
        public App()
        {
            InitializeComponent();
            //DependencyService.Register<MockDataStore>();
            dbh = new DataBaseHandler();
            SqliteRepo.Init(dbh.DB);
            CurrentPlayer = new Models.Player();
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
