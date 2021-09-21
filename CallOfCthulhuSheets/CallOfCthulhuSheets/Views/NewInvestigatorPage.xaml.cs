using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CallOfCthulhuSheets.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewInvestigatorPage : ContentPage
    {
        public NewInvestigatorPage()
        {
            InitializeComponent();
        }

        private void Label_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "Text")
                {
                    var t = sender as Label;
                    if (t == null)
                        return;
                    if (t.BindingContext is ViewModels.Atrr)
                        AtrrCollectionView.ScrollTo(t.BindingContext);
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }
        }
    }
}