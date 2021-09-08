using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.CommunityToolkit;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Threading.Tasks;
using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;
using CallOfCthulhuSheets.Extensions;
using CallOfCthulhuSheets.Views;
using Xamarin.Essentials;

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
            var registredPlayer = (await SqliteRepo.GetItemsAsync<Player>())?.Where((o) => o.Name == login).FirstOrDefault();

            var entersPlayer = new Player() { Name = Login };

            if (!entersPlayer.Name.Equals(registredPlayer?.Name))
            { 
                await SqliteRepo.AddItemAsync(entersPlayer);
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
