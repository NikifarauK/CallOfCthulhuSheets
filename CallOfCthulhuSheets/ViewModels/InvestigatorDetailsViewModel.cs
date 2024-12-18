﻿using AsyncAwaitBestPractices.MVVM;
using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;
using MvvmHelpers;

namespace CallOfCthulhuSheets.ViewModels
{
    public class InvestigatorDetailsViewModel : BaseViewModel
    {
        private Investigator m_Investigator;

        private ObservableRangeCollection<Atrr> characteristicsOfInv;
        public ObservableRangeCollection<Atrr> CharacteristicsOfInv 
        {
            get
            {
                if (characteristicsOfInv == null)
                    _ = GetCharacteristics();
                return characteristicsOfInv;
            }
            set => SetProperty(ref characteristicsOfInv, value);
        }

        public ObservableRangeCollection<InvestigatorsSkills> InvSkills { get; set; }

        private int age;
        public int Age
        {
            get
            {
                age = m_Investigator.Age;
                return age;
            }
            set
            {
                if (SetProperty(ref age, value))
                    m_Investigator.Age = value;
            }
        }

        private AsyncCommand sortSkills;
        public AsyncCommand SortSkills
        {
            get
            {
                if (sortSkills == null)
                    sortSkills = new AsyncCommand(SkillsSorting);
                return sortSkills;
            }
        }

        public InvestigatorDetailsViewModel(Investigator investigator)
        {
            m_Investigator = investigator ?? new Investigator();
            _ = GetCharacteristics();

            InvSkills = new ObservableRangeCollection<InvestigatorsSkills>();
            InvSkills.AddRange(m_Investigator.InvestigatorsSkills);
        }

        async Task GetCharacteristics()
        {
            if (m_Investigator.Characteristic == null)
                m_Investigator.Characteristic =  SqliteRepo.GetItemAsync<Characteristic>(m_Investigator.CharacteristicId);
            CharacteristicsOfInv = new ObservableRangeCollection<Atrr>();
            foreach (var i in Characteristic.CharacteristicS)
            {
                CharacteristicsOfInv.Add(new Atrr { Name = i, Value = m_Investigator?.Characteristic?.GetValueByEnum(i) });
            }
        }

        async Task SkillsSorting()
        {
            List<InvestigatorsSkills> skills;
            if (m_Investigator.InvestigatorsSkills == null)
            {
                skills = SqliteRepo.GetItemsAsync<InvestigatorsSkills>()
                    .Where( (o) => o.InvestigatorId == m_Investigator.Id).ToList();
                m_Investigator.InvestigatorsSkills = skills;
            }
            else
            {
                skills = m_Investigator.InvestigatorsSkills;
            }
            skills.Sort(new Comparison<InvestigatorsSkills>
            (
                (l, r) => (r.CurrentSkillValue ?? 0) - (l.CurrentSkillValue ?? 0)
            ));
            InvSkills.Clear();
            InvSkills.AddRange(skills);

        }


        private string name;
        public string Name
        {
            get => m_Investigator.Name;
            set
            {
                if (SetProperty(ref name, value))
                    m_Investigator.Name = value;
            }
        }

        public string Birthplace { get => m_Investigator.Birthplace; }

        private string currentHP;
        public string CurrentHP
        {
            get => m_Investigator.CurrentHitPoints.ToString();
            set
            {
                if (SetProperty(ref currentHP, value))
                {
                    int.TryParse(value, out int res);
                    m_Investigator.CurrentHitPoints = res;
                }
            }
        }

        public string MaxHP { get => m_Investigator.MaxHitPoints.ToString(); }

        private string currentMP;
        public string CurrentMP
        {
            get => m_Investigator.CurrentMagicPoints.ToString();
            set
            {
                if (SetProperty(currentMP, value, StringComparer.InvariantCultureIgnoreCase,
                        value,
                        async (_, _) => await SkillsSorting()))
                {
                    int.TryParse(value, out int res);
                    m_Investigator.CurrentMagicPoints = res;
                }
            }
        }

        public string MaxMP { get => m_Investigator.MaxMagicPoints.ToString(); }


        private string currentSan;
        public string CurrentSan
        {
            get => m_Investigator.CurrentSanity.ToString();
            set
            {
                if (SetProperty(ref currentSan, value))
                {
                    int.TryParse(value, out int res);
                    m_Investigator.CurrentSanity = res;
                }
            }
        }

        public string MaxSan { get => m_Investigator.MaxSanity.ToString(); }
        
        public int MoveSpeed { get => m_Investigator.GetMoveRate(); }

        public string DamageBonus { get => m_Investigator.GetDamageBonus().ToString(); }

        public int Built { get => m_Investigator.GetBuilt(); }

        private AsyncCommand rollPercentage;
        public AsyncCommand RollPercentage
        {
            get
            {
                if (rollPercentage == null)
                    rollPercentage = new AsyncCommand(Percentage);
                return rollPercentage;
            }
        }

        private async Task Percentage()
        {
            var roll = Dice.RollPercentage();
            await Shell.Current.DisplayAlert("", "" + roll, "ok");
        }
    }
}
