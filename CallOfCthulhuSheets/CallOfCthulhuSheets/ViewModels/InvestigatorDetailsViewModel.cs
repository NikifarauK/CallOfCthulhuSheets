using CallOfCthulhuSheets.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Runtime.CompilerServices;
using CallOfCthulhuSheets.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CallOfCthulhuSheets.ViewModels
{
    public class InvestigatorDetailsViewModel : BaseViewModel
    {
        private Investigator m_Investigator;

        public ObservableRangeCollection<Atrr> CharacteristicsOfInv { get; set; }

        public ObservableRangeCollection<InvestigatorsSkills> InvSkills { get; set; }

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
            CharacteristicsOfInv = new ObservableRangeCollection<Atrr>();
            foreach (var i in Characteristic.CharacteristicS)
            {
                CharacteristicsOfInv.Add(new Atrr { Name = i, Value = investigator?.Characteristic?.GetValueByEnum(i) });
            }

            InvSkills = new ObservableRangeCollection<InvestigatorsSkills>();
            InvSkills.AddRange(m_Investigator.InvestigatorsSkills);
        }

        async Task GetCharacteristics()
        {
            m_Investigator.Characteristic = await SqliteRepo.GetItemAsync<Characteristic>(m_Investigator.CharacteristicId);
        }

        async Task SkillsSorting()
        {
            var skills = (await SqliteRepo.GetItemsAsync<InvestigatorsSkills>()).Where( (o) => o.InvestigatorId == m_Investigator.Id).ToList();
            m_Investigator.InvestigatorsSkills = skills;
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
                if (SetProperty(ref currentMP, value, onChanged: async () => await SkillsSorting()))
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
