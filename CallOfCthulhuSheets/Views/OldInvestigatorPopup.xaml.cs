using System.ComponentModel;
using CommunityToolkit.Maui.Views;

namespace CallOfCthulhuSheets.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OldInvestigatorPopup : Popup, INotifyPropertyChanged
    {
        int needToDim;
        int LeftToDim
        {
            get
            {
                int.TryParse(NumberToDeminish.Text, out int res);
                return res;
            }
            set => NumberToDeminish.Text = value.ToString();
        }

        int StrVal
        {
            get => (int)StrSlider.Value;
            set
            {
                StrSlider.Value = value;
                StrLabel.Text = value.ToString();
            }
        }
        int StrDecrised;

        int ConVal
        {
            get => (int)ConSlider.Value;
            set
            {
                ConSlider.Value = value;
                ConLabel.Text = value.ToString();
            }
        }
        int ConDecrised;

        int DexVal
        {
            get => (int)DexSlider.Value;
            set
            {
                DexSlider.Value = value;
                DexLabel.Text = value.ToString();
            }
        }
        int DexDecrised;

        public OldInvestigatorPopup(int str, int con, int dex, int fisicalReductor)
        {
            InitializeComponent();
            needToDim = LeftToDim = fisicalReductor;
            StrSlider.Maximum = str;
            StrVal = str;
            ConSlider.Maximum = con;
            ConVal = con;
            DexSlider.Maximum = dex;
            DexVal = dex;

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (LeftToDim == 0)
                Close(new List<int>() { StrVal, ConVal, DexVal });
        }

        private void StrSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var slider = sender as Slider;
            StrVal = (int)slider.Value;
            StrDecrised = (int)slider.Maximum - StrVal;
            SolveLefted();
        }


        private void ConSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var slider = sender as Slider;
            ConVal = (int)slider.Value;
            ConDecrised = (int)slider.Maximum - ConVal;
            SolveLefted();
        }

        private void DexSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var slider = sender as Slider;
            DexVal = (int)slider.Value;
            DexDecrised = (int)slider.Maximum - DexVal;
            SolveLefted();
        }

        private void SolveLefted()
        {
            LeftToDim = needToDim - StrDecrised - ConDecrised - DexDecrised;
        }
    }
}