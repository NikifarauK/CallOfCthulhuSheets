using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using CallOfCthulhuSheets.Views;
using System.Diagnostics;
using Newtonsoft.Json;

namespace CallOfCthulhuSheets.ViewModels
{
    [QueryProperty(nameof(OccupationId), "OccupationId")]
    [QueryProperty(nameof(AllowedSkillsString), "AllowedSkillsString")]
    public class NewInvestigatorViewModel : BaseViewModel
    {

        public NewInvestigatorViewModel()
        {
            newInvestigator = new Investigator
            {
                Characteristic = new Characteristic(),
                Occupation = new Occupation(),
                InvestigatorsSkills = new List<InvestigatorsSkills>(),
                Inventory = new List<Item>()
            };
            Atrribs = new ObservableRangeCollection<Atrr>();
            var lst = Atrr.AttrListFromCharacteristic(newInvestigator.Characteristic);
            lst.Sort(new Comparison<Atrr>((l, r) => (int)l?.Name - (int)r?.Name));
            Atrribs.AddRange(lst);
        }


        public bool IsComplete
        {
            get => IsAtrrComplete && isAtrrModified && isOccupChosen && isSkillsAllocated;
        }

        private bool isAtrrComplete;
        public bool IsAtrrComplete
        {
            get
            {
                foreach (var atr in Atrribs)
                {
                    if (!(atr.IsUsed ?? false))
                    {
                        SetProperty(ref isAtrrComplete, false);
                        return isAtrrComplete;
                    }
                }
                SetProperty(ref isAtrrComplete, true);
                return isAtrrComplete;
            }
            set => SetProperty(ref isAtrrComplete, value);
        }

        private bool isAtrrModified;
        public bool IsAtrrModified { get => isAtrrModified; set => SetProperty(ref isAtrrModified, value); }

        private bool isOccupChosen;
        public bool IsOccupChosen { get => isOccupChosen; set => SetProperty(ref isOccupChosen, value); }

        private bool isSkillsAllocated;
        public bool IsSkillsAllocated { get => isSkillsAllocated; set => SetProperty(ref isSkillsAllocated, value); }


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
            if (!IsComplete ||
                string.IsNullOrEmpty(Name) ||
                string.IsNullOrEmpty(Birthplace) ||
                string.IsNullOrEmpty(InvsDescription) ||
                string.IsNullOrEmpty(sex))
            {
                var str = await Shell.Current.DisplayActionSheet("Персонаж не завершен,\nвсе изменения будут потеряны", "cancel", "ok");
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
                Player current = await SqliteRepo.GetItemAsync<Player>(Xamarin.Essentials.Preferences.Get("CurrentPlayerId", ""));
                newInvestigator.Owner = current;
                newInvestigator.Occupation = ChosenOccupation;
                newInvestigator.IsPlayersCharacter = true;
                newInvestigator.CurrentHitPoints = NewInvestigator.MaxHitPoints;
                newInvestigator.CurrentMagicPoints = NewInvestigator.MaxMagicPoints;
                newInvestigator.CurrentSanity = NewInvestigator.MaxSanity;
                await SqliteRepo.AddItemAsync(newInvestigator.Characteristic);
                await SqliteRepo.AddItemAsync(newInvestigator);

                await Shell.Current.GoToAsync("..");

            }
        }

        private Investigator newInvestigator;
        public Investigator NewInvestigator
        {
            get => newInvestigator;
            set => SetProperty(ref newInvestigator, value);
        }


        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (SetProperty(ref name, value))
                {
                    newInvestigator.Name = name;
                }
            }
        }

        private string birthplace;
        public string Birthplace
        {
            get => birthplace;
            set
            {
                if (SetProperty(ref birthplace, value))
                {
                    newInvestigator.Birthplace = birthplace;
                }
            }
        }

        private int age;
        public int Age
        {
            get => age;
            set
            {
                if (SetProperty(ref age, value))
                {
                    newInvestigator.Age = age;
                }
            }
        }

        private string sex;
        public string Sex
        {
            get => sex;
            set
            {
                if (SetProperty(ref sex, value))
                {
                    newInvestigator.Sex = sex;
                }
            }
        }

        public ObservableRangeCollection<Atrr> Atrribs { get; set; }


        private AsyncCommand<Atrr> randomAtrr;
        public AsyncCommand<Atrr> RandomAtrr
        {
            get
            {
                if (randomAtrr == null)
                    randomAtrr = new AsyncCommand<Atrr>(CreateAtrr);
                return randomAtrr;
            }
        }

        private async Task CreateAtrr(Atrr arg)
        {
            try
            {
                Dice dice = GetDiceOnCreation(arg.Name);
                int diceRolledValue = dice.Roll();
                var result = diceRolledValue * 5;
                await Shell.Current.DisplayAlert(arg.Name.ToString() + ": " + dice, "" + diceRolledValue + " x 5 = " + result, "OK");
                arg.Value = result;
                newInvestigator.Characteristic.SetValueByEnum(result, arg.Name);
                arg.IsUsed = true;
                _ = IsAtrrComplete;
            }
            catch (Exception e) { Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!" + e.Message); }
        }

        private Dice GetDiceOnCreation(ECharacteristic name)
        {
            Dice dice = null;
            if (name == ECharacteristic.Siz || name == ECharacteristic.Int || name == ECharacteristic.Edu)
                dice = new Dice(2, 6, 6);
            else
                dice = new Dice(3, 6);
            return dice;
        }

        private AsyncCommand modifieByAgeCommand;
        public AsyncCommand ModifieByAgeCommand
        {
            get
            {
                if (modifieByAgeCommand == null)
                    modifieByAgeCommand = new AsyncCommand(ModifieByAge);
                return modifieByAgeCommand;
            }
        }

        //-----------------------------------------------------------
        private async Task ModifieByAge()
        {
            if (Age < 15 || Age > 89)
            {
                await Shell.Current.DisplayAlert("Внимание", "Сперва укажите возраст, от 15 до 89 лет", "OK");
                return;
            }

            if (!IsAtrrComplete)
            {
                await Shell.Current.DisplayAlert("Внимание", "Сперва определите Характеристики", "OK");
                return;
            }

            var report = "При возрасте " + Age + ":\n";
            if (Age < 20)
            {
                report += $"{ECharacteristic.Str} либо {ECharacteristic.Siz} уменьшаются на 5\n";
                report += $"{ECharacteristic.Edu} также уменьшается на 5\n";

                newInvestigator.Characteristic.Edu -= 5;
                report += $"{ECharacteristic.Edu} = {newInvestigator.Characteristic.Edu}";

                var reroll = GetDiceOnCreation(ECharacteristic.Luck).Roll() * 5;
                var result = reroll > newInvestigator.Characteristic.Luck ? reroll : newInvestigator.Characteristic.Luck;
                foreach (var atr in Atrribs)
                {
                    if (atr.Name == ECharacteristic.Luck)
                        atr.Value = result;
                }
                report += ECharacteristic.Luck.ToString() + " с преимуществом: " + newInvestigator.Characteristic.Luck + " , " + reroll + " " + result.ToString();
                newInvestigator.Characteristic.Luck = result;
                await Shell.Current.DisplayAlert("Изменения", report, "ok");

                var echar = await Shell.Current.DisplayActionSheet("Что уменьшить?", null, null, "Str", "Siz");

                ECharacteristic diminish;
                switch (echar)
                {
                    case "Str":
                        newInvestigator.Characteristic.Str -= 5;
                        diminish = ECharacteristic.Str;
                        break;
                    case "Siz":
                        newInvestigator.Characteristic.Siz -= 5;
                        diminish = ECharacteristic.Siz;
                        break;
                    default:
                        diminish = ECharacteristic.Str;
                        break;
                }
                foreach (var atr in Atrribs)
                {
                    if (atr.Name == diminish)
                        atr.Value = newInvestigator.Characteristic.GetValueByEnum(diminish);
                }
            }
            else if (Age < 40)
            {
                report += "делается проверка улучшения " + ECharacteristic.Edu + "\n";

                var stat = newInvestigator.Characteristic.Edu;
                if (Dice.StatImpruveCheck(ref stat))
                    report += "Удачно: Было " + newInvestigator.Characteristic.Edu + "\nСтало " + stat;
                else
                    report += "Неудачно!";

                newInvestigator.Characteristic.Edu = stat;
                foreach (var atr in Atrribs)
                {
                    if (atr.Name == ECharacteristic.Edu)
                        atr.Value = stat;
                }
                await Shell.Current.DisplayAlert("Изменения", report, "ok");
            }
            else
            {
                var elderModificator = (Age - 40) / 10;
                int fisicalReductor = 5 * (int)Math.Pow(2, elderModificator);
                int apperanceReduction = 5 * (elderModificator + 1);
                int eduImpruvChecks = (elderModificator + 2) < 4 ? elderModificator + 2 : 4;

                report += $"{ECharacteristic.Str}, {ECharacteristic.Con} и {ECharacteristic.Dex} должны быть\nсуммарно уменьшены  на {fisicalReductor}\n";
                report += $"{ECharacteristic.App} уменьшается на {apperanceReduction}\n";
                report += $"{eduImpruvChecks} проверок улучшения {ECharacteristic.Edu}\n";

                await Shell.Current.DisplayAlert("Изменения", report, "ok");
                var strConDexnewValuesList = await Shell.Current.Navigation.ShowPopupAsync(new OldInvestigatorPopup(newInvestigator.Characteristic.Str,
                                                 newInvestigator.Characteristic.Con, newInvestigator.Characteristic.Dex, fisicalReductor));

                if (strConDexnewValuesList == null)
                    return;
                var lst = strConDexnewValuesList as List<int>;
                var successfulEduImpruv = 0;
                report = "";
                foreach (var atrr in Atrribs)
                {
                    if (atrr.Name == ECharacteristic.Str)
                    {
                        atrr.Value = lst[0];
                        newInvestigator.Characteristic.SetValueByEnum(lst[0], ECharacteristic.Str);

                        report += $"{ECharacteristic.Str} = {atrr.Value}\n";
                    }
                    else if (atrr.Name == ECharacteristic.Con)
                    {
                        atrr.Value = lst[1];
                        newInvestigator.Characteristic.SetValueByEnum(lst[1], ECharacteristic.Con);

                        report += $"{ECharacteristic.Con} = {atrr.Value}\n";
                    }
                    else if (atrr.Name == ECharacteristic.Dex)
                    {
                        atrr.Value = lst[2];
                        newInvestigator.Characteristic.SetValueByEnum(lst[2], ECharacteristic.Dex);

                        report += $"{ECharacteristic.Dex} = {atrr.Value}\n";
                    }
                    else if (atrr.Name == ECharacteristic.App)
                    {
                        newInvestigator.Characteristic.App -= 25;
                        if (newInvestigator.Characteristic.App < 1) newInvestigator.Characteristic.App = 1;

                        atrr.Value = newInvestigator.Characteristic.App;

                        report += $"{ECharacteristic.App} = {atrr.Value}\n";
                    }
                    else if (atrr.Name == ECharacteristic.Edu)
                    {
                        for (int i = 0; i < eduImpruvChecks; i++)
                        {
                            var stat = newInvestigator.Characteristic.Edu;
                            if (Dice.StatImpruveCheck(ref stat))
                            {
                                successfulEduImpruv++;
                                atrr.Value = newInvestigator.Characteristic.Edu = stat;
                            }
                        }
                        report += $"Сделано {eduImpruvChecks} проверок {ECharacteristic.Edu},\n";
                        report += $"из них {successfulEduImpruv} успешно, теперь {ECharacteristic.Edu} = {atrr.Value}";
                    }
                }

                await Shell.Current.DisplayAlert("Изменения", report, "ok");
            }
            OnPropertyChanged(nameof(NewInvestigator));
            IsAtrrModified = true;
            MoveSpeed = newInvestigator.GetMoveRate().ToString();
            Built = newInvestigator.GetBuilt().ToString();
            DamageBonus = newInvestigator.GetDamageBonus().ToString();
        }


        private AsyncCommand randomAllAtrr;
        public AsyncCommand RandomAllAtrr
        {
            get
            {
                if (randomAllAtrr == null)
                {
                    randomAllAtrr = new AsyncCommand(PerformRandomAllAtrr);
                }
                return randomAllAtrr;
            }
        }
        private async Task PerformRandomAllAtrr()
        {
            foreach (var atrr in Atrribs)
            {
                try
                {
                    await CreateAtrr(atrr);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            IsAtrrComplete = true;
        }


        private string occupationId;
        public string OccupationId
        {
            get => occupationId;
            set
            {
                occupationId = value;
                _ = RefreshOccupation(occupationId);

            }
        }

        private AsyncCommand choseOccupCommand;
        public AsyncCommand ChoseOccupCommand
        {
            get
            {
                if (choseOccupCommand == null)
                {
                    choseOccupCommand = new AsyncCommand(ChoseOccup);
                }

                return choseOccupCommand;
            }
        }

        private async Task ChoseOccup()
        {
            if (!IsAtrrModified)
            {
                await Shell.Current.DisplayAlert("Внимание", "Сперва определите и модифицируйте Характеристики", "ok");
                return;
            }
            try
            {
                await Shell.Current.GoToAsync($"{nameof(OccupationPage)}");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }

        }

        private string moveSpeed;
        public string MoveSpeed { get => moveSpeed; set => SetProperty(ref moveSpeed, value); }

        private string built;
        public string Built { get => built; set => SetProperty(ref built, value); }


        private string damageBonus;
        public string DamageBonus { get => damageBonus; set => SetProperty(ref damageBonus, value); }

        Occupation chosenOccupation;

        public Occupation ChosenOccupation { get => chosenOccupation; set => SetProperty(ref chosenOccupation, value); }

        private async Task RefreshOccupation(string id)
        {
            IsBusy = true;
            ChosenOccupation = await SqliteRepo.GetItemAsync<Occupation>(id);
            if (ChosenOccupation == null)
            {
                IsBusy = false;
                return;
            }
            IsOccupChosen = true;
            BaseAtribsVariants = new ObservableRangeCollection<ECharacteristic>();
            var atribs = new List<ECharacteristic>() { ChosenOccupation.SkillPointsBaseCharacteristic ?? ECharacteristic.Edu };
            if (ChosenOccupation.SkillPointsBaseCharacteristicSec != null)
            {
                atribs.Add(ChosenOccupation.SkillPointsBaseCharacteristicSec ?? ECharacteristic.Edu);
            }

            if (ChosenOccupation.SkillPointsBaseCharacteristicThird != null)
            {
                atribs.Add(ChosenOccupation.SkillPointsBaseCharacteristicThird ?? ECharacteristic.Edu);
            }
            BaseAtribsVariants.AddRange(atribs);
            if (atribs.Count == 1)
            {
                BaseAtrib = atribs[0];
            }

            IsBusy = false;
        }

        private ObservableRangeCollection<ECharacteristic> baseAtribsVariants;
        public ObservableRangeCollection<ECharacteristic> BaseAtribsVariants { get => baseAtribsVariants; set => SetProperty(ref baseAtribsVariants, value); }

        private ECharacteristic? baseAtrib = null;
        public ECharacteristic? BaseAtrib
        {
            get => baseAtrib;
            set
            {
                if (SetProperty(ref baseAtrib, value))
                {
                    OnPropertyChanged(nameof(SkillPoints));
                }
            }
        }


        private int skillPoints;
        public int SkillPoints
        {
            get
            {
                skillPoints = NewInvestigator.Characteristic.GetSkillPointsByBasicChracteristic(BaseAtrib ?? ECharacteristic.Edu);
                return skillPoints;
            }
        }


        private AsyncCommand allocateSkillPointsCommand;
        public AsyncCommand AllocateSkillPointsCommand
        {
            get
            {
                if (allocateSkillPointsCommand == null)
                {
                    allocateSkillPointsCommand = new AsyncCommand(AllocateSkillPoints);
                }

                return allocateSkillPointsCommand;
            }
        }

        private async Task AllocateSkillPoints()
        {
            if (!IsOccupChosen || BaseAtrib == null)
            {
                return;
            }
            try
            {
                await Shell.Current.GoToAsync($"{nameof(SkillPointsPage)}?SkillPoints={SkillPoints}" +
                    $"&OccupationId={OccupationId}&PersonalPoints={NewInvestigator.Characteristic.Int * 2}" +
                    $"&EduCount={newInvestigator.Characteristic.Edu}&HalfOfDex={newInvestigator.Characteristic.Dex / 2}");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }


        private string allowedSkillsString;
        public string AllowedSkillsString
        {
            get => allowedSkillsString;
            set
            {
                IsBusy = true;
                allowedSkillsString = value;
                List<InvestigatorsSkills> t = JsonConvert.DeserializeObject<List<InvestigatorsSkills>>(value);
                if (t == null)
                {
                    IsBusy = false;
                    return;
                }
                newInvestigator.InvestigatorsSkills = t;
                IsSkillsAllocated = t != null;
                IsBusy = false;
            }
        }


        public string InvsDescription
        {
            get => newInvestigator.Description;
            set => NewInvestigator.Description = value;
        }
    }
}
