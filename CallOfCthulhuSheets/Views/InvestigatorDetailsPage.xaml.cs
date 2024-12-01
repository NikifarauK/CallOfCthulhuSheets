using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;
using CallOfCthulhuSheets.ViewModels;

namespace CallOfCthulhuSheets.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(InvestigatorId), nameof(InvestigatorId))]
    public partial class InvestigatorDetailsPage : ContentPage
    {
        public string InvestigatorId { get; set; }
        public InvestigatorDetailsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var investigator = SqliteRepo.GetItemAsync<Investigator>(InvestigatorId);
            BindingContext = new InvestigatorDetailsViewModel(investigator);
        }
    }
}