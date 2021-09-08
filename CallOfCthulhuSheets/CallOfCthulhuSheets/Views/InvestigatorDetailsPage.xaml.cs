using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;
using CallOfCthulhuSheets.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

            int.TryParse(InvestigatorId, out int result);
            var investigator = await SqliteRepo.GetItemAsync<Investigator>(result);
            BindingContext = new InvestigatorDetailsViewModel(investigator);
        }
    }
}