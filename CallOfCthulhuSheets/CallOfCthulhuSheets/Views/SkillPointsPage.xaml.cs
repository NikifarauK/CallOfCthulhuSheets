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
    public partial class SkillPointsPage : ContentPage
    {
        public SkillPointsPage()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}