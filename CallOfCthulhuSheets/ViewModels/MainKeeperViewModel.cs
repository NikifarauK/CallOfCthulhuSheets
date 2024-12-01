using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;
using CallOfCthulhuSheets.Views;
using MvvmHelpers;

namespace CallOfCthulhuSheets.ViewModels
{
    public class MainKeeperViewModel : BaseViewModel
    {
        private ObservableRangeCollection<Campaign> campaigns;
        public ObservableRangeCollection<Campaign> Campaigns
        {
            get
            {
                if (campaigns == null)
                {
                    campaigns = new ObservableRangeCollection<Campaign>();
                    _ = RefreshAsync();
                }
                return campaigns;
            }
        }



        private AsyncCommand refreshAsyncCommand;
        public ICommand RefreshAsyncCommand
        {
            get
            {
                if (refreshAsyncCommand == null)
                {
                    refreshAsyncCommand = new AsyncCommand(RefreshAsync);
                }

                return refreshAsyncCommand;
            }
        }

        private async Task RefreshAsync()
        {
            IsBusy = true;

            var campaignes = ( SqliteRepo.GetItemsAsync<Campaign>()).ToList();
            var curentPlayerId = Preferences.Get("CurrentPlayerId", "");
            var playersCompaignes = campaignes.Where((o) => o.PlayerId == curentPlayerId).ToList();
            Campaigns.Clear();
            Campaigns.AddRange(playersCompaignes);

            IsBusy = false;
        }


        private Campaign selectedCampaign;
        public Campaign SelectedCampaign { get => selectedCampaign; set => SetProperty(ref selectedCampaign, value); }


        private AsyncCommand<Campaign> itemSelectedCommand;
        public AsyncCommand<Campaign> ItemSelectedCommand
        {
            get
            {
                if (itemSelectedCommand == null)
                {
                    itemSelectedCommand = new AsyncCommand<Campaign>(ItemSelected);
                }

                return itemSelectedCommand;
            }
        }

        private async Task ItemSelected(Campaign cmp)
        {
            if (cmp == null) return;
            SelectedCampaign = null;
            var route = $"{nameof(CampaignPage)}?CampaignId={cmp?.Id}";
            await Shell.Current.GoToAsync(route);
        }

        private AsyncCommand newCampaign;
        public ICommand NewCampaign
        {
            get
            {
                if (newCampaign == null)
                {
                    newCampaign = new AsyncCommand(PerformNewCampaign);
                }

                return newCampaign;
            }
        }

        private async Task PerformNewCampaign()
        {
            await ItemSelected(new Campaign() { Id = "new" }); ;
        }


        private AsyncCommand<Campaign> deleteCommand;
        public AsyncCommand<Campaign> DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                    deleteCommand = new AsyncCommand<Campaign>(Delete);
                return deleteCommand;
            }
        }

        private async Task Delete(Campaign arg)
        {
            IsBusy = true;
             SqliteRepo.DeleteItemAsync(arg);
            await RefreshAsync();
            IsBusy = false;
        }
    }
}
