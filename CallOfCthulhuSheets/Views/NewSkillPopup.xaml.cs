﻿using CallOfCthulhuSheets.Models;
using CommunityToolkit.Maui.Views;

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
               
                Close(newSkill);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}