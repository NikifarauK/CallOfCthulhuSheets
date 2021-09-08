using System;
using System.Collections.Generic;
using System.Text;
using CallOfCthulhuSheets.Services;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Extensions;
using Xamarin.Essentials;
using CallOfCthulhuSheets.Views;

namespace CallOfCthulhuSheets.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {

        private string login;
        public string Login { get => login; set => SetProperty(ref login, value); }

        private string password;
        public string Password { get => password; set => SetProperty(ref password, value); }

        private string confirmPassword;
        public string ConfirmPassword { get => confirmPassword; set => SetProperty(ref confirmPassword, value); }

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
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(Password))
            {
                await Shell.Current.DisplayAlert("Внимание", "Введите логин и пароль", "ok");
                return;
            }
            var logins = (await SqliteRepo.GetItemsAsync<Player>())
                .Aggregate(new List<string>(), (lst, next) =>
            {
                lst.Add(next.Name);
                return lst;
            });
            if (logins.Contains(Login))
            {
                Login = "";
                Password = "";
                ConfirmPassword = "";
                await Shell.Current.DisplayAlert("Внимание", "Такое имя пользавателя уже существует", "ok");
                return;
            }
            if (!string.Equals(password, confirmPassword))
            {
                Password = "";
                ConfirmPassword = "";
                await Shell.Current.DisplayAlert("Внимание", "Пароль и подтверждение должны совпадать", "ok");
                return;
            }
            var newPlayer = new Player() { Name = login };
            await SqliteRepo.AddItemAsync(newPlayer);
            Preferences.Set("IsLoged", true);
            Preferences.Set("CurrentPlayer", login);
            Shell.Current.FlyoutHeader = new FlyoutHeader();
            Preferences.Set("CurrentPlayerId", newPlayer.Id);
            await Shell.Current.GoToAsync($"//{nameof(StartPage)}");
        }
    }
}
