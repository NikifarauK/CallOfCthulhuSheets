using System;
using System.Collections.Generic;
using System.Text;

namespace CallOfCthulhuSheets.Models
{
    public class Dice
    {
        private int Amount { get; set; }
        private int Sides { get; set; }
        private int Modifier { get; set; }

        public Dice(int amount, int sides, int modifier = 0)
        {
            Amount = amount;
            Sides = sides;
            Modifier = modifier;
        }

        public int Roll()
        {
            if (Amount < 1) return Amount + Modifier;
            int randomed = 0;
            var randomizer = new Random();
            for (int i = 1; i <= Amount; i++)
                randomed += randomizer.Next(1, Sides + 1);
            return randomed + Modifier;
        }

        public static int RollPercentage()
        {
            return new Random().Next(1, 101);
        }

        public static bool StatImpruveCheck(ref int stat)
        {
            if (RollPercentage() > stat)
            {
                stat += new Dice(1, 8).Roll();
                if (stat > 99) stat = 99;
                return true;
            }
            else
                return false;
        }

        public override string ToString()
        {
            if (Amount < 1)
                return (Amount + Modifier).ToString();
            var mod = (Modifier != 0 ? "+" + Modifier : "");
            return "" + Amount + "d" + Sides + mod;
        }
    }
}
