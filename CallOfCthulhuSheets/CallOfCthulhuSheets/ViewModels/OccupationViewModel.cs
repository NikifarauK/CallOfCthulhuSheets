using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;

namespace CallOfCthulhuSheets.ViewModels
{
    public class OccupationViewModel : BaseViewModel
    {
        private const int PROF_SKILLS = 8;

        public OccupationViewModel()
        {
            Occupations = new ObservableRangeCollection<Occupation>();
            AllSkills = new ObservableRangeCollection<Skill>();
            RefreshAsyncCommand.ExecuteAsync();
            ChosenSkills = new List<object>();
            BaseAtrr = ECharacteristic.Edu;
            SkillsCanChoose = PROF_SKILLS;
        }

        bool isOccupationChosed;
        public bool IsOccupationChosed { get => isOccupationChosed; set => SetProperty(ref isOccupationChosed, value); }

        private Occupation chosenOccup;
        public Occupation ChosenOccup { get => chosenOccup; set => SetProperty(ref chosenOccup, value); }

        private Occupation createdOccupation;
        public Occupation CreatedOccupation
        {
            get
            {
                if (createdOccupation == null)
                    createdOccupation = new Occupation();
                return createdOccupation;
            }
            set => SetProperty(ref createdOccupation, value);
        }

        public ObservableRangeCollection<Occupation> Occupations { get; set; }
        public ObservableRangeCollection<Skill> AllSkills { get; set; }


        private ECharacteristic? baseAtrr;
        public ECharacteristic? BaseAtrr
        {
            get => baseAtrr;
            set
            {
                SetProperty(ref baseAtrr, value);
                CreatedOccupation.SkillPointsBaseCharacteristic = baseAtrr;
            }
        }

        private ECharacteristic? secAtrr;
        public ECharacteristic? SecAtrr
        {
            get => secAtrr;
            set
            {
                SetProperty(ref secAtrr, value);
                CreatedOccupation.SkillPointsBaseCharacteristicSec = secAtrr;
            }
        }

        private ECharacteristic? thirdAtrr;
        public ECharacteristic? ThirdAtrr
        {
            get => thirdAtrr;
            set
            {
                SetProperty(ref thirdAtrr, value);
                CreatedOccupation.SkillPointsBaseCharacteristicThird = thirdAtrr;
            }
        }


        private ObservableRangeCollection<ECharacteristic?> atrribs;
        public ObservableRangeCollection<ECharacteristic?> Atrribs
        {
            get
            {
                if (atrribs == null)
                {
                    var t = new List<ECharacteristic?>() { null };
                    for (var i = ECharacteristic.Str; i <= ECharacteristic.Luck; i++)
                    {
                        t.Add(i);
                    }
                    atrribs = new ObservableRangeCollection<ECharacteristic?>();
                    atrribs.AddRange(t);
                }
                return atrribs;
            }
        }

        private AsyncCommand refreshAsyncCommand;
        public AsyncCommand RefreshAsyncCommand
        {
            get
            {
                if (refreshAsyncCommand == null)
                {
                    refreshAsyncCommand = new AsyncCommand(RefreshAllSkills);
                }

                return refreshAsyncCommand;
            }
        }

        private async Task RefreshAllSkills()
        {
            IsBusy = true;

            var ocups = await SqliteRepo.GetItemsAsync<Occupation>();

            var sklls = (await SqliteRepo.GetItemsAsync<Skill>())
                .OrderBy((o) => o.SkillTypeId);

            Occupations.Clear();
            AllSkills.Clear();

            Occupations.AddRange(ocups);
            AllSkills.AddRange(sklls);

            IsBusy = false;
        }


        private string occupName;
        public string OccupName { get => occupName; set => SetProperty(ref occupName, value); }

        private string minCR;
        public string MinCR { get => minCR; set => SetProperty(ref minCR, value); }

        private string maxCR;
        public string MaxCR { get => maxCR; set => SetProperty(ref maxCR, value); }

        private ObservableRangeCollection<SkillType> skillTypes;
        public ObservableRangeCollection<SkillType> SkillTypes
        {
            get
            {
                if (skillTypes == null)
                {
                    skillTypes = new ObservableRangeCollection<SkillType>();
                    _ = RefreshSkillTypes();
                }
                return skillTypes;
            }
        }

        private async Task RefreshSkillTypes()
        {
            IsBusy = true;
            SkillTypes.Clear();
            SkillTypes.Add(null);
            var t = await SqliteRepo.GetItemsAsync<SkillType>();
            SkillTypes.AddRange(t.Where((o) => o.Name != "uncommon" || o.Name != "special"));
            IsBusy = false;
        }

        private ObservableRangeCollection<OccupSkillTypesDependensy> ostDep;
        public ObservableRangeCollection<OccupSkillTypesDependensy> OstDep
        {
            get
            {
                if (ostDep == null)
                {
                    ostDep = new ObservableRangeCollection<OccupSkillTypesDependensy>();
                }
                return ostDep;
            }
        }
        public SkillType ChosenSkillType { get; set; }

        private int skillTypeCount;
        public int SkillTypeCount { get => skillTypeCount; set => SetProperty(ref skillTypeCount, value); }


        private int previusSkillsCanChose;
        private int skillsCanChoose;
        public int SkillsCanChoose
        {
            get => skillsCanChoose;
            set
            {
                previusSkillsCanChose = skillsCanChoose;
                var t1 = OstDep.Aggregate(0, (total, next) => total + next.SkillCount);
                SetProperty(ref skillsCanChoose,
                    PROF_SKILLS - t1 - ChosenSkillsCount);
            }
        }


        private List<object> chosenSkills;
        public List<object> ChosenSkills
        {
            get => chosenSkills;
            set
            {
                SetProperty(ref chosenSkills, value);
                ChosenSkillsCount = chosenSkills.Count;
            }
        }


        private int chosenSkillsCount;
        public int ChosenSkillsCount
        {
            get
            {
                chosenSkillsCount = ChosenSkills.Count;
                return chosenSkillsCount;
            }
            set => SetProperty(ref chosenSkillsCount, ChosenSkills.Count);
        }

        private AsyncCommand helpCommandAsync;
        public AsyncCommand HelpCommandAsync
        {
            get
            {
                if (helpCommandAsync == null)
                {
                    helpCommandAsync = new AsyncCommand(async () =>
                    {
                        await Shell.Current.DisplayAlert
                   ("Справка", "Согласно 7th Ed Invetigators Handbook\n стр 70...93:\n" +
                   "введите название профессии,\n" +
                   "затем введите значения кредитного рейтинга,\n" +
                   "затем выберите дополниельные Характеристики" +
                   "для определения Очков Проффессиональных навыков\n" +
                   $"({ECharacteristic.Edu} уже учтена по умолчанию).\n" +
                   "Далее выберите группы Умений с указанием количества навыков " +
                   "в группе для данной профессии.\n" +
                   "В конце выберите умения из списка", "ok"); ;
                    });
                }

                return helpCommandAsync;
            }
        }


        private AsyncCommand skillTypeAddingCommand;
        public AsyncCommand SkillTypeAddingCommand
        {
            get
            {
                if (skillTypeAddingCommand == null)
                {
                    skillTypeAddingCommand = new AsyncCommand(SkillTypeAdding);
                }

                return skillTypeAddingCommand;
            }
        }

        private async Task SkillTypeAdding()
        {
            if (SkillTypeCount < 1 || ChosenSkillType == null)
            {
                await Shell.Current.DisplayAlert("Внимание", "Сперва выберите тип характеристики и счетчик", "ok");
                return;
            }

            if (SkillTypeCount > SkillsCanChoose)
            {
                await Shell.Current.DisplayAlert("Внимание", "Не должно быть болше 8", "ok");
                return;
            }

            var sop = new OccupSkillTypesDependensy()
            {
                SkillCount = SkillTypeCount,
                SkillTypesId = ChosenSkillType.Id,
                Type = ChosenSkillType
            };
            OstDep.Add(sop);
            SkillTypes.Remove(ChosenSkillType);
            SkillTypeCount = 0;
            SkillsCanChoose = SkillsCanChoose;
        }


        private Command<OccupSkillTypesDependensy> skillTypeDeleteCommand;
        public Command<OccupSkillTypesDependensy> SkillTypeDeleteCommand
        {
            get
            {
                if (skillTypeDeleteCommand == null)
                {
                    skillTypeDeleteCommand = new Command<OccupSkillTypesDependensy>((osp) =>
                   {
                       if (OstDep.Contains(osp))
                       {
                           OstDep.Remove(osp);
                           SkillTypes.Add(osp.Type);
                           SkillsCanChoose = SkillsCanChoose;
                       }
                   });
                }

                return skillTypeDeleteCommand;
            }
        }

        private AsyncCommand selectedSkillsChangedCommand;

        public AsyncCommand SelectedSkillsChangedCommand
        {
            get
            {
                if (selectedSkillsChangedCommand == null)
                {
                    selectedSkillsChangedCommand = new AsyncCommand(SelectedSkillsChanged);
                }

                return selectedSkillsChangedCommand;
            }
        }

        
        private async Task SelectedSkillsChanged()
        {
            if (SkillsCanChoose < 1 && previusSkillsCanChose < SkillsCanChoose)
            {
                ChosenSkills.RemoveAt(ChosenSkills.Count - 1);
                await Shell.Current.DisplayAlert("Внимание", "Выбрать больше 8 нельзя", "ok");
            }

            ChosenSkillsCount = ChosenSkills.Count;
            SkillsCanChoose = SkillsCanChoose;
            previusSkillsCanChose = SkillsCanChoose;
        }


        private AsyncCommand saveAndChooseCommand;
        public AsyncCommand SaveAndChooseCommand
        {
            get
            {
                if (saveAndChooseCommand == null)
                    saveAndChooseCommand = new AsyncCommand(SaveAndChoose);
                return saveAndChooseCommand;
            }
        }

        private async Task SaveAndChoose()
        {
            IsBusy = true;
            if(string.IsNullOrEmpty(OccupName) ||
                string.IsNullOrEmpty(MaxCR) ||
                string.IsNullOrEmpty(MinCR) || SkillsCanChoose > 5)
            {
                await Shell.Current.DisplayAlert("Внимание", "Заполните все требуемые поля", "ok");
                return;
            }
            ChosenOccup = new Occupation()
            {
                Name = OccupName,
                MaxCreditRating = int.Parse(MaxCR),
                MinCreditRating = int.Parse(MinCR),
                SkillPointsBaseCharacteristic = BaseAtrr,
                SkillPointsBaseCharacteristicSec = SecAtrr,
                SkillPointsBaseCharacteristicThird = ThirdAtrr,
                ProfessionalSkills = new List<Skill>(),
                AddedProfSkillTypes = new List<SkillType>(),
                OccupSkillTypesDependensies = OstDep.ToList()
            };
            var skills = ChosenSkills.Aggregate(new List<Skill>() , (arr, next) => { arr.Add(next as Skill); return arr; });
            ChosenOccup.ProfessionalSkills = skills;

            var skillTypes = OstDep.Aggregate(new List<SkillType>(), (lst, next) => { lst.Add(next.Type); return lst; });

            await SqliteRepo.AddItemAsync(ChosenOccup);
            await RefreshAsyncCommand.ExecuteAsync();
            IsOccupationChosed = true;
            IsBusy = false;
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
            if (!IsOccupationChosed)
            {
                var str = await Shell.Current.DisplayActionSheet("Профессия не выбрана,\nвсе изменения будут потеряны", "cancel", "ok");
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
                await Shell.Current.GoToAsync($"..?OccupationId={ChosenOccup.Id}");
            }
        }

        private Command acceptOccupationCommand;

        public Command AcceptOccupationCommand
        {
            get
            {
                if (acceptOccupationCommand == null)
                {
                    acceptOccupationCommand = new Command(AcceptOccupation);
                }

                return acceptOccupationCommand;
            }
        }

        private void AcceptOccupation()
        {
            if (ChosenOccup == null)
                return;
            
            IsOccupationChosed = true;
            
            OccupName = ChosenOccup.Name;
            
            BaseAtrr = ChosenOccup.SkillPointsBaseCharacteristic;
            
            SecAtrr = ChosenOccup.SkillPointsBaseCharacteristicSec;
            
            ThirdAtrr = ChosenOccup.SkillPointsBaseCharacteristicThird;
            
            MinCR = ChosenOccup.MinCreditRating.ToString();
            
            MaxCR = ChosenOccup.MaxCreditRating.ToString();

            OstDep.Clear();
            //var osts = ChosenOccup.OccupSkillTypesDependensies.
            OstDep.AddRange(ChosenOccup.OccupSkillTypesDependensies);

            AllSkills.Clear();
            AllSkills.AddRange(ChosenOccup.ProfessionalSkills);
            ChosenSkills = ChosenOccup.ProfessionalSkills.Aggregate(new List<object>(),
                (list, next) => { list.Add(next as object); return list; });

            OnPropertyChanged(nameof(ChosenSkillsCount));
        }
    }
}
