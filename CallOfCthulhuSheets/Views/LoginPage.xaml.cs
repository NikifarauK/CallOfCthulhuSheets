using System.Diagnostics;

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
            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
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