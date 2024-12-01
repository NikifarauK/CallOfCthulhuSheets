using CallOfCthulhuSheets.Models;
using MvvmHelpers;

namespace CallOfCthulhuSheets.ViewModels
{
    public class BattleViewModel : BaseViewModel
    {
        public BattleViewModel()
        {
            Investigators = new ObservableRangeCollection<Investigator>();
            var lst = new List<Investigator>()
            {
                new Investigator(){Name="Джон", CurrentHitPoints=10, Characteristic = Characteristic.GetRandomCharacteristic()  , IsPlayersCharacter = true },
                new Investigator(){Name="Смит", CurrentHitPoints=8, Characteristic = Characteristic.GetRandomCharacteristic()   , IsPlayersCharacter = true },
                new Investigator(){Name="Древний 1", CurrentHitPoints=40, Characteristic = Characteristic.GetRandomCharacteristic(), IsPlayersCharacter = false },
                new Investigator(){Name="Древний 2", CurrentHitPoints=7, Characteristic = Characteristic.GetRandomCharacteristic(), IsPlayersCharacter = false }
            };
            lst.Sort(new Comparison<Investigator>((l, r) => r.Speed - l.Speed));
            Investigators.AddRange(lst);
        }

        public ObservableRangeCollection<Investigator> Investigators { get; }
    }
}
