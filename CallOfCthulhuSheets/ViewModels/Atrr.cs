using CallOfCthulhuSheets.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CallOfCthulhuSheets.ViewModels
{
    public class Atrr : ObservableObject
    {
        ECharacteristic name;
        public ECharacteristic Name { get => name; set => name = value; }

        int? val;
        public int? Value
        {
            get => val;
            set => SetProperty(ref val, value);
        }
        bool? isUsed;
        public bool? IsUsed
        {
            get => isUsed ?? false;
            set => SetProperty(ref isUsed, value);
        }

        public static List<Atrr> AttrListFromCharacteristic(Characteristic chrstic) 
        {
            var lst = new List<Atrr>();
            foreach (var i in Characteristic.CharacteristicS)
            {
                lst.Add(new Atrr { name = i, val = chrstic.GetValueByEnum(i), IsUsed = false });
            }
            return lst;
        }

        public static void CharacteristicFromAtrrList(List<Atrr> attrs, ref Characteristic characteristic, bool isPlayers)
        {
            if (attrs.Count < Characteristic.CharacteristicS.Length)
                throw new ArgumentException();
            foreach(var atr in attrs)
            {
                characteristic.SetValueByEnum(atr.Value ?? characteristic.GetValueByEnum(atr.Name), atr.Name, isPlayers);
            }
        }
    }
}
