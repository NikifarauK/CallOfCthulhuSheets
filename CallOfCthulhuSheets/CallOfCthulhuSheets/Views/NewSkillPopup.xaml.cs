using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CallOfCthulhuSheets.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewSkillPopup : Popup
    {
        private SkillType skillType;
        SkillType SkillType 
        {
            get => skillType;
            set
            {
                typeLabel.Text = value?.ToString();
                skillType = value;
            }
        }

        private string name 
        {
            get => NameEntry.Text;
            set => NameEntry.Text = value; 
        }

        private string descr
        {
            get => DescrEntry.Text;
            set => DescrEntry.Text = value;
        }

        private int points 
        { 
            get => int.Parse(PointsEntry.Text); 
            set => PointsEntry.Text = value.ToString(); 
        }

        public NewSkillPopup(SkillType type)
        {
            InitializeComponent();

            SkillType = type;
             //= SkillType.ToString();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(descr) ||
                string.IsNullOrEmpty(PointsEntry.Text))
            {
                Shell.Current.DisplayAlert("Внимание", "Заполните поля", "OK");
                return;
            }
            Skill newSkill = new Skill()
            {
                BasePoints = points,
                Name = name,
                Description = descr,
                Type = SkillType
            };
            try
            {
               
                Dismiss(newSkill);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}