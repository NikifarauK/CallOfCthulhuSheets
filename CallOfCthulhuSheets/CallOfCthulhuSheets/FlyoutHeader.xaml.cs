using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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