using AsyncAwaitBestPractices.MVVM;
using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;
using CallOfCthulhuSheets.Views;

namespace CallOfCthulhuSheets.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        private string login;

        public string Login { get => login; set => SetProperty(ref login, value); }

        private AsyncCommand enterCommand;

        public AsyncCommand EnterCommand
        {
            get
            {
                if (enterCommand == null)
                {
                    enterCommand = new AsyncCommand(Enter);
                }

                return enterCommand;
            }
        }

        private async Task Enter()
        {
            var registredPlayer = ( SqliteRepo.GetItemsAsync<Player>())?.Where((o) => o.Name == login).FirstOrDefault();

            var entersPlayer = new Player() { Name = Login };

            if (!entersPlayer.Name.Equals(registredPlayer?.Name))
            { 
                 SqliteRepo.AddItemAsync(entersPlayer);
                registredPlayer = entersPlayer;
            }

            Preferences.Set("IsLoged", true);
            Preferences.Set("CurrentPlayer", login);
            Preferences.Set("CurrentPlayerId", registredPlayer.Id);
            Shell.Current.FlyoutHeader = new FlyoutHeader();
            await Shell.Current.GoToAsync($"//{nameof(StartPage)}");
        }

    }
}
