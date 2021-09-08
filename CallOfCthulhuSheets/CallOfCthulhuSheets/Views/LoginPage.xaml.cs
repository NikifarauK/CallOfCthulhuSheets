using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CallOfCthulhuSheets.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (Device.RuntimePlatform == Device.UWP)
                await Task.Delay(100);
            if (Preferences.Get("IsLoged", false))
            {
                try
                {
                    await Shell.Current.GoToAsync($"//{nameof(StartPage)}");
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}