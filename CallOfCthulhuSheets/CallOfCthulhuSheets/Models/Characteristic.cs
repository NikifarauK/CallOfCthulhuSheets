using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallOfCthulhuSheets.Models
{
    public enum ECharacteristic
    {
        Str, Con, Siz, Dex, App, Int, Pow, Edu, Luck
    }

    public class Characteristic : Tableable
    {

        private int[] characteristics = new int[9];

        public readonly static ECharacteristic[] CharacteristicS = ((ECharacteristic[])Enum.GetValues(typeof(ECharacteristic)));

        #region Properties
        public int Str
        {
            get => characteristics[(int)ECharacteristic.Str];
            set => characteristics[(int)ECharacteristic.Str] = value;
        }

        public int Con
        {
            get => characteristics[(int)ECharacteristic.Con];
            set => characteristics[(int)ECharacteristic.Con] = value;
        }

        public int Siz
        {
            get => characteristics[(int)ECharacteristic.Siz];
            set => characteristics[(int)ECharacteristic.Siz] = value;
        }

        public int Dex
        {
            get => characteristics[(int)ECharacteristic.Dex];
            set => characteristics[(int)ECharacteristic.Dex] = value;
        }

        public int App
        {
            get => characteristics[(int)ECharacteristic.App];
            set => characteristics[(int)ECharacteristic.App] = value;
        }

        public int Int
        {
            get => characteristics[(int)ECharacteristic.Int];
            set => characteristics[(int)ECharacteristic.Int] = value;
        }

        public int Pow
        {
            get => characteristics[(int)ECharacteristic.Pow];
            set => characteristics[(int)ECharacteristic.Pow] = value;
        }

        public int Edu
        {
            get => characteristics[(int)ECharacteristic.Edu];
            set => characteristics[(int)ECharacteristic.Edu] = value;
        }

        public int Luck
        {
            get => characteristics[(int)ECharacteristic.Luck];
            set => characteristics[(int)ECharacteristic.Luck] = value;
        }
        #endregion

        public int GetSkillPointsByBasicChracteristic(ECharacteristic basicCharcterictic)
        {
            return Edu * 2 + characteristics[(int)basicCharcterictic] * 2;
        }


        public int GetValueByEnum(ECharacteristic enumer)
        {
            if (enumer > ECharacteristic.Luck)
                throw new ArgumentOutOfRangeException(nameof(ECharacteristic));
            return characteristics[(int)enumer];
        }

        public void SetValueByEnum(int val, ECharacteristic enumer)
        {
            if (val > 99) val = 99;
            characteristics[(int)enumer] = val;
        }

    }
}
