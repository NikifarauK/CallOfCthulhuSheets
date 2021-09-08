using CallOfCthulhuSheets.Models;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.ObjectModel;

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
            get => isUsed;
            set => SetProperty(ref isUsed, value);
        }

        public static List<Atrr> AttrListFromCharacteristic(Characteristic chrstic) 
        {
            var lst = new List<Atrr>();
            for (var i = ECharacteristic.Str; i <= ECharacteristic.Luck; i++)
            {
                lst.Add(new Atrr { name = i, val = chrstic.GetValueByEnum(i), IsUsed = false });
            }
            return lst;
        }
    }
}
