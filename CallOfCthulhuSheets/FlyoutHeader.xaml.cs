using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Storage;

namespace CallOfCthulhuSheets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutHeader : ContentView
    {
        private string currentPlayer;
        public string CurrentPlayer
        {
            get
            {
                currentPlayer = Preferences.Get("CurrentPlayer", "");
                return currentPlayer;
            }
            set => OnPropertyChanged();
        }

        public FlyoutHeader()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}