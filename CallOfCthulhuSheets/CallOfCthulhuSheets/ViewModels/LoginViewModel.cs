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

        private string password;

        public string Password { get => password; set => SetProperty(ref password, value); }

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
            var registredPlayer = (await SqliteRepo.GetItemsAsync<Player>()).Where((o) => o.Name == login).FirstOrDefault();

            var entersPlayer = new Player() { Name = Login };

            entersPlayer.AccessCode = entersPlayer.GetLoginAccessHashingByPassword(Password);

            if (entersPlayer.AccessCode != registredPlayer?.AccessCode)
            {
                Login = "";
                Password = "";
                await Shell.Current.DisplayAlert("Error", "Wrong login or password", "OK").ConfigureAwait(false);
            }
            else
            {
                Preferences.Set("IsLoged", true);
                Preferences.Set("CurrentPlayer", login);
                Preferences.Set("CurrentPlayerId", registredPlayer.Id);
                Shell.Current.FlyoutHeader = new FlyoutHeader();
                Login = "";
                Password = "";
                await Shell.Current.GoToAsync($"//{nameof(StartPage)}");
                
            }
        }

        private AsyncCommand registerCommand;

        public AsyncCommand RegisterCommand
        {
            get
            {
                if (registerCommand == null)
                {
                    registerCommand = new AsyncCommand(Register);
                }

                return registerCommand;
            }
        }

        private async Task Register()
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }
    }
}
