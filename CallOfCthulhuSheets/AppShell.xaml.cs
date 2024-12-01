using CallOfCthulhuSheets.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace CallOfCthulhuSheets
{
    public partial class AppShell : Microsoft.Maui.Controls.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            //Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            //Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));

            Routing.RegisterRoute(nameof(CampaignPage), typeof(CampaignPage));
            Routing.RegisterRoute(nameof(InvestigatorDetailsPage), typeof(InvestigatorDetailsPage));
            Routing.RegisterRoute(nameof(NewInvestigatorPage), typeof(NewInvestigatorPage));
            Routing.RegisterRoute(nameof(OccupationPage), typeof(OccupationPage));
            Routing.RegisterRoute(nameof(SkillPointsPage), typeof(SkillPointsPage));
            Routing.RegisterRoute(nameof(SessionPage), typeof(SessionPage));
            Routing.RegisterRoute(nameof(BattlePage), typeof(BattlePage));

            //Routing.RegisterRoute($"{nameof(MainInvestigatorPage)}/{nameof(InvestigatorDetailsPage)}", typeof(InvestigatorDetailsPage));
            //Routing.RegisterRoute($"{nameof(MainInvestigatorPage)}/{nameof(NewInvestigatorPage)}", typeof(NewInvestigatorPage));
            //Routing.RegisterRoute($"{nameof(MainInvestigatorPage)}/{nameof(NewInvestigatorPage)}/{nameof(OccupationPage)}", typeof(OccupationPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            Preferences.Set("IsLoged", false);
            Preferences.Set("CurrentPlayer", "");
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
