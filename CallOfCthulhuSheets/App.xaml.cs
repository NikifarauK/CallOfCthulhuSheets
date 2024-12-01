using CallOfCthulhuSheets;
using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;

namespace CallOfCthulhuSheets
{
    public partial class App : Application
    {
        //public bool IsLogedIn { get; set; }

        Player CurrentPlayer { get; set; } = new Player();
        readonly DataBaseHandler dbh = new DataBaseHandler();

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            //DependencyService.Register<MockDataStore>();
            SqliteRepo.Init(dbh.DB);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
