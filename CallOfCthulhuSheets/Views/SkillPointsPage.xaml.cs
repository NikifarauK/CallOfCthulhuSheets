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