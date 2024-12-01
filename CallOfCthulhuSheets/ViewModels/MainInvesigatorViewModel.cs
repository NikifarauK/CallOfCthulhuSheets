using AsyncAwaitBestPractices.MVVM;
using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;
using CallOfCthulhuSheets.Views;
using MvvmHelpers;

namespace CallOfCthulhuSheets.ViewModels
{
    public class MainInvesigatorViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Investigator> Investigators { get; set; }

        public MainInvesigatorViewModel()
        {
            Investigators = new ObservableRangeCollection<Investigator>();
            _ = RefreshAsyncCommand.ExecuteAsync();
        }


        private AsyncCommand<Investigator> itemSelectedCommand;
        public AsyncCommand<Investigator> ItemSelectedCommand
        {
            get
            {
                if (itemSelectedCommand == null)
                {
                    itemSelectedCommand = new AsyncCommand<Investigator>(ItemSelected);
                }

                return itemSelectedCommand;
            }
        }

        private async Task ItemSelected(Investigator inv)
        {
            if (inv == null)
            {
                return;
            }

            SelectedInvestigator = null;
            var route = $"{nameof(InvestigatorDetailsPage)}?InvestigatorId={inv.Id}";
            await Shell.Current.GoToAsync(route);
        }


        private AsyncCommand refreshAsyncCommand;
        public AsyncCommand RefreshAsyncCommand
        {
            get
            {
                if (refreshAsyncCommand == null)
                {
                    refreshAsyncCommand = new AsyncCommand(Refresh);
                }

                return refreshAsyncCommand;
            }
        }

        private async Task Refresh()
        {
            IsBusy = true;

            var inv1 = ( SqliteRepo.GetItemsAsync<Investigator>()).ToList();
            var curentPlayerId = Preferences.Get("CurrentPlayerId", "");
            var inv =inv1.Where((o) => o.PlayerId == curentPlayerId && o.IsPlayersCharacter).ToList();
            Investigators.Clear();
            Investigators.AddRange(inv);

            IsBusy = false;
        }

        private AsyncCommand<object> deleteCommand;
        public AsyncCommand<object> DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                    deleteCommand = new AsyncCommand<object>(Delete);
                return deleteCommand;
            }
        }

        private async Task Delete(object arg)
        {
            try
            {
                IsBusy = true;
                var t = arg as Investigator;
                if (t != null)
                {
                     SqliteRepo.DeleteItemAsync<Investigator>(t);
                    await Refresh();
                }
                IsBusy = false;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("!!!!!!!!!" + e.Message);
            }
        }



        private object selectedInvestigator;
        public object SelectedInvestigator
        {
            get => selectedInvestigator;
            set => SetProperty(ref selectedInvestigator, value);
        }

        private AsyncCommand newInvestigetor;
        public AsyncCommand NewInvestigetor
        {
            get
            {
                if (newInvestigetor == null)
                    newInvestigetor = new AsyncCommand(CreateInvestigator);
                return newInvestigetor;
            }
        }

        private async Task CreateInvestigator()
        {
            await Shell.Current.GoToAsync($"{nameof(NewInvestigatorPage)}?IsPC={true}");
        }
    }
}
