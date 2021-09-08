using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using CallOfCthulhuSheets.Views;
using System;

namespace CallOfCthulhuSheets.ViewModels
{
    [QueryProperty(nameof(SkillPoints), nameof(SkillPoints))]
    [QueryProperty(nameof(PersonalPoints), nameof(PersonalPoints))]
    [QueryProperty(nameof(OccupationId), nameof(OccupationId))]
    [QueryProperty(nameof(EduCount), nameof(EduCount))]
    [QueryProperty(nameof(HalfOfDex), nameof(HalfOfDex))]
    public class SkillPointsViewModel : BaseViewModel
    {
        private int skillPoints;
        public int SkillPoints
        {
            get => skillPoints;
            set
            {
                if (SetProperty(ref skillPoints, value))
                    OnPropertyChanged(nameof(ProfPointsLeft));
            }
        }

        private int personalPoints;
        public int PersonalPoints
        {
            get => personalPoints;
            set
            {
                if (SetProperty(ref personalPoints, value))
                    OnPropertyChanged(nameof(PersonalPointsLeft));
            }
        }

        private string occupationId;
        public string OccupationId
        {
            get => occupationId;
            set
            {
                occupationId = value;
                if (int.TryParse(occupationId, out int res))
                {
                    _ = RefreshCommand.ExecuteAsync(res);
                }
            }
        }

        public int EduCount { get; set; }
        public int HalfOfDex { get; set; }


        private bool isProfskillsComplete;
        public bool IsProfskillsComplete { get => isProfskillsComplete; set => SetProperty(ref isProfskillsComplete, value); }

        AsyncCommand<int> refreshCommand;
        public AsyncCommand<int> RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    refreshCommand = new AsyncCommand<int>(RefreshOccupation);
                }

                return refreshCommand;
            }
        }

        private async Task RefreshOccupation(int id)
        {
            IsBusy = true;
            ChosenOccupation = await SqliteRepo.GetItemAsync<Occupation>(id);
            ProfSkills.AddRange(ChosenOccupation?.ProfessionalSkills);
            SkillTypes.AddRange(ChosenOccupation?.OccupSkillTypesDependensies);
            int freeSkills = 8 - ProfSkills.Count - SkillTypes.Aggregate(0, (sum, next) => sum + next.SkillCount);
            if (freeSkills > 0)
            {
                SkillTypes.Add(new OccupSkillTypesDependensy()
                {
                    SkillTypesId = int.MaxValue,
                    Type = new SkillType() { Name = "остальное" },
                    SkillCount = freeSkills
                });
            }

            MaxCreditRating = ChosenOccupation.MaxCreditRating;
            MinCreditRating = ChosenOccupation.MinCreditRating;
            CreditRating = ChosenOccupation.MinCreditRating;
            ProfPointsChosen = (int)MinCreditRating;
            var typeIds = SkillTypes.Aggregate(new List<int>(), (lst, next) => { lst.Add(next.SkillTypesId); return lst; });
            var allSkills = await SqliteRepo.GetItemsAsync<Skill>();
            List<Skill> skillsToAdd = new List<Skill>();
            foreach (var skill in allSkills)
            {
                if (skill.Id == 18) // credit pating
                {
                    InvCredRating.Skill = skill;
                    continue;
                }

                if (!ProfSkills.Contains(skill, new SkillEqualiti()))
                {
                    skillsToAdd.Add(skill);
                }
            }
            SkillsToAdd.AddRange(skillsToAdd);
            foreach (var skill in SkillsToAdd)
            {
                if (skill.Id == 24)                 //dodge
                    skill.BasePoints = HalfOfDex;
                if (skill.Id == 43)                 //own language
                    skill.BasePoints = EduCount;
            }
            foreach (var skill in ProfSkills)
            {
                if (skill.Id == 24)
                    skill.BasePoints = HalfOfDex;
                if (skill.Id == 43)
                    skill.BasePoints = EduCount;
            }
            IsBusy = false;
        }

        private ObservableRangeCollection<Skill> skillsToAdd;
        public ObservableRangeCollection<Skill> SkillsToAdd
        {
            get
            {
                if (skillsToAdd == null)
                {
                    skillsToAdd = new ObservableRangeCollection<Skill>();
                }

                return skillsToAdd;
            }
        }

        private Occupation chosenOccupation;
        public Occupation ChosenOccupation { get => chosenOccupation; set => SetProperty(ref chosenOccupation, value); }

        private ObservableRangeCollection<Skill> profSkills;
        public ObservableRangeCollection<Skill> ProfSkills
        {
            get
            {
                if (profSkills == null)
                {
                    profSkills = new ObservableRangeCollection<Skill>();
                }
                return profSkills;
            }
        }

        private ObservableRangeCollection<OccupSkillTypesDependensy> skillTypes;
        public ObservableRangeCollection<OccupSkillTypesDependensy> SkillTypes
        {
            get
            {
                if (skillTypes == null)
                {
                    skillTypes = new ObservableRangeCollection<OccupSkillTypesDependensy>();
                }
                return skillTypes;
            }
        }

        private OccupSkillTypesDependensy occupSkillType;
        public OccupSkillTypesDependensy OccupSkillType { get => occupSkillType; set => SetProperty(ref occupSkillType, value); }


        private AsyncCommand skillTypeSelectedCommand;
        public AsyncCommand SkillTypeSelectedCommand
        {
            get
            {
                if (skillTypeSelectedCommand == null)
                {
                    skillTypeSelectedCommand = new AsyncCommand(SkillTypeSelected);
                }

                return skillTypeSelectedCommand;
            }
        }


        private async Task SkillTypeSelected()
        {
            if (OccupSkillType == null)
            {
                return;
            }

            var skills = SkillsToAdd.Where((o) => (o.SkillTypeId ?? int.MaxValue) == OccupSkillType.SkillTypesId).ToList();

            if (OccupSkillType.SkillTypesId == int.MaxValue)
                skills.AddRange(SkillsToAdd.Where((o) => (o.SkillTypeId == 7 || o.SkillTypeId == 8) && o.Id != 18)); //18 - credit rating's id

            var skillNames = skills.Aggregate(new List<string>(), (lst, next) => { lst.Add(next.ToString()); return lst; });

            skillNames.Add("новый");

            var chosenSkill = new Skill();

            var name = await Shell.Current.DisplayActionSheet("Выберите навык", "cancel", null, skillNames.ToArray());

            switch (name)
            {
                case "cancel":
                    OccupSkillType = null;
                    return;
                case "новый":
                    SkillType type = OccupSkillType.Type.Name != "остальное" ? OccupSkillType.Type : null;
                    chosenSkill = await Shell.Current.Navigation.ShowPopupAsync(
                        new NewSkillPopup(type)) as Skill;
                    if (chosenSkill == null)
                    {
                        OccupSkillType = null;
                        return;
                    }
                    await SqliteRepo.AddItemAsync(chosenSkill);
                    break;
                default:
                    foreach (var skill in skills)
                    {
                        if (string.Equals(skill.ToString(), name))
                        {
                            chosenSkill = skill;
                            break;
                        }
                    }
                    break;
            }
            if (--OccupSkillType.SkillCount == 0)
            {
                _ = SkillTypes.Remove(OccupSkillType);
                if (SkillTypes.Count == 0)
                {
                    IsProfskillsComplete = true;
                }
            }

            _ = SkillsToAdd.Remove(chosenSkill);
            ProfSkills.Add(chosenSkill);
            OccupSkillType = null;

            if (IsProfskillsComplete)
            {
                RefreshSkillsConverters();
            }
        }

        private void RefreshSkillsConverters()
        {
            IsBusy = true;
            var prof = ProfSkills.Aggregate(new List<SkillToInvistgatorsSkillConverter>(), (lst, next) =>
            {
                lst.Add(new SkillToInvistgatorsSkillConverter(next));
                return lst;
            });
            Profesional.AddRange(prof);
            foreach (var t in Profesional)
            {
                OnPropertyChanged(nameof(t.SkillPoint));
            }

            var pers = SkillsToAdd.Aggregate(new List<SkillToInvistgatorsSkillConverter>(), (lst, next) =>
            {
                lst.Add(new SkillToInvistgatorsSkillConverter(next));
                return lst;
            });
            Personal.AddRange(pers);
            foreach (var t in Personal)
            {
                OnPropertyChanged(nameof(t.SkillPoint));
            }
            IsBusy = false;
        }

        // Code below processing view when IsProfsSkillsCompleate == true
        private InvestigatorsSkills invCredRating;
        private InvestigatorsSkills InvCredRating 
        {
            get
            {
                if (invCredRating == null)
                    invCredRating = new InvestigatorsSkills();
                return invCredRating;
            }
            set => invCredRating = value;
        }

        private double previousCreditRating;
        private double creditRating = 0.0;
        public double CreditRating
        {
            get => creditRating;
            set
            {
                if (IsProfskillsApplied) return;
                if (value > maxCreditRating) value = maxCreditRating;
                if (value < minCreditRating) value = minCreditRating;
                if (SetProperty(ref creditRating, (int)value))
                {
                    int change = (int)creditRating - (int)previousCreditRating;
                    if (ProfPointsChosen + change > SkillPoints ||
                        ProfPointsChosen + change < 0)
                    {
                        _ = SetProperty(ref creditRating, previousCreditRating);
                        return;
                    }
                    InvCredRating.CurrentSkillValue = (int)creditRating;
                    ProfPointsChosen += change;
                    previousCreditRating = creditRating;
                }

            }
        }

        private double minCreditRating = 0.0;
        public double MinCreditRating
        {
            get => minCreditRating;
            set => SetProperty(ref minCreditRating, value);
        }

        private double maxCreditRating = 2.0;
        public double MaxCreditRating
        {
            get => maxCreditRating;
            set => SetProperty(ref maxCreditRating, value);
        }


        private int profPointsChosen;
        public int ProfPointsChosen
        {
            get => profPointsChosen;
            set
            {
                if (SetProperty(ref profPointsChosen, value))
                    OnPropertyChanged(nameof(ProfPointsLeft));
            }
        }
        public int ProfPointsLeft { get => SkillPoints - ProfPointsChosen; }

        private int personalPointsChosen;
        public int PersonalPointsChosen
        {
            get => personalPointsChosen;
            set
            {
                if (SetProperty(ref personalPointsChosen, value))
                {
                    OnPropertyChanged(nameof(PersonalPointsLeft));
                }
            }
        }
        public int PersonalPointsLeft { get => PersonalPoints - personalPointsChosen; }


        private ObservableRangeCollection<SkillToInvistgatorsSkillConverter> profesional;
        public ObservableRangeCollection<SkillToInvistgatorsSkillConverter> Profesional
        {
            get
            {
                if (profesional == null)
                    profesional = new ObservableRangeCollection<SkillToInvistgatorsSkillConverter>();
                return profesional;
            }
        }

        private ObservableRangeCollection<SkillToInvistgatorsSkillConverter> personal;
        public ObservableRangeCollection<SkillToInvistgatorsSkillConverter> Personal
        {
            get
            {
                if (personal == null)
                    personal = new ObservableRangeCollection<SkillToInvistgatorsSkillConverter>();
                return personal;
            }
        }


        private Command<object> profPointChangedCommand;
        public Command<object> ProfPointChangedCommand
        {
            get
            {
                if (profPointChangedCommand == null)
                    profPointChangedCommand = new Command<object>(ProfPointChanged);
                return profPointChangedCommand;
            }
        }

        private void ProfPointChanged(object obj)
        {
            var t = obj as SkillToInvistgatorsSkillConverter;
            if (t == null)
                return;
            if (IsProfskillsApplied)
            {
                PersnalPointChanged(obj);
                return;
            }
            int change = (int)t.SkillPoint - (int)t.PreviusSkillPoint;
            if (change + ProfPointsChosen > SkillPoints ||
                change + ProfPointsChosen < 0)
            {
                t.SkillPoint = t.PreviusSkillPoint;
                return;
            }
            ProfPointsChosen += change;
            t.PreviusSkillPoint = t.SkillPoint;
        }

        private void PersnalPointChanged(object obj)
        {
            var t = obj as SkillToInvistgatorsSkillConverter;
            if (t == null)
            {
                return;
            }

            int change = (int)t.SkillPoint - (int)t.PreviusSkillPoint;
            if (change + PersonalPointsChosen > PersonalPoints ||
                change + PersonalPointsChosen < 0)
            {
                t.SkillPoint = t.PreviusSkillPoint;
                return;
            }
            PersonalPointsChosen += change;
            t.PreviusSkillPoint = t.SkillPoint;
        }

        private bool isProfskillsApplied;
        public bool IsProfskillsApplied { get => isProfskillsApplied; set => SetProperty(ref isProfskillsApplied, value); }

        private bool isPersonalskillsApplied;
        public bool IsPersonalskillsApplied { get => isPersonalskillsApplied; set => SetProperty(ref isPersonalskillsApplied, value); }

        private Command profSkillsChosenCommand;
        public Command ProfSkillsChosenCommand
        {
            get
            {
                if (profSkillsChosenCommand == null)
                    profSkillsChosenCommand = new Command(
                        () =>
                        {
                            if (IsProfskillsApplied && PersonalPointsLeft == 0)
                            {
                                IsPersonalskillsApplied = true;

                                _ = BackButtonCommand.ExecuteAsync();
                            }

                            if (ProfPointsLeft == 0)
                            {
                                IsProfskillsApplied = true;
                                foreach (var t in Profesional)
                                {
                                    t.MinSkillPointValue = t.ToInvSkill.CurrentSkillValue ?? 0;
                                }
                            }
                        });
                return profSkillsChosenCommand;
            }
        }

        private AsyncCommand backButtonCommand;
        public AsyncCommand BackButtonCommand
        {
            get
            {
                if (backButtonCommand == null)
                    backButtonCommand = new AsyncCommand(BackButton);
                return backButtonCommand;
            }
        }

        private async Task BackButton()
        {
            if (!IsPersonalskillsApplied)
            {
                var str = await Shell.Current.DisplayActionSheet("Очки не распределены,\nвсе изменения будут потеряны", "cancel", "ok");
                switch (str)
                {
                    default:
                        return;
                    case "ok":
                        await Shell.Current.GoToAsync("..");
                        return;
                }
            }
            else
            {
                var InvSkills = new List<InvestigatorsSkills>();
                var InvSkillsCount = Profesional.Aggregate(0, (count, next) =>
                {
                    InvSkills.Add(next.ToInvSkill);
                    return ++count;
                });
                InvSkillsCount = Personal.Aggregate(InvSkillsCount, (count, next) =>
                {
                    InvSkills.Add(next.ToInvSkill);
                    return ++count;
                });
                InvSkills.Add(InvCredRating);
                try
                {
                    string serlizedSkillList = Newtonsoft.Json.JsonConvert.SerializeObject(InvSkills);
                    await Shell.Current.GoToAsync($"..?AllowedSkillsString={serlizedSkillList}");
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }
    }
}
