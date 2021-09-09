using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallOfCthulhuSheets.Models
{
    public class Investigator : Tableable
    {
        public bool IsPlayersCharacter { get; set; }

        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(32)]
        public string Birthplace { get; set; }

        public int Age { get; set; }

        public bool? IsMajorWounded{ get; set; }

        public bool? IsDying       { get; set; }
        
        public bool? IsUnconscius  { get; set; }

        [MaxLength(32)]
        public string Sex { get; set; }

        [ForeignKey(typeof(Player))]
        public string PlayerId { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Player Owner { get; set; }

        [ForeignKey(typeof(Characteristic))]
        public string CharacteristicId { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public Characteristic Characteristic { get; set; }

        [ForeignKey(typeof(Occupation))]
        public string OccupationId { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Occupation Occupation { get; set; }

        [Ignore]
        public int MaxHitPoints { get => Characteristic == null ? 0 : (Characteristic.Siz + Characteristic.Con) / 10; }
        public int CurrentHitPoints { get; set; }

        [Ignore]
        public int MaxSanity { get => Characteristic?.Pow ?? 0; }
        public int CurrentSanity { get; set; }

        [Ignore]
        public int MaxMagicPoints { get => (Characteristic?.Pow ?? 0) / 5; }
        public int CurrentMagicPoints { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<InvestigatorsSkills> InvestigatorsSkills { get; set; }

        [ManyToMany(typeof(InvestigatorsItems), CascadeOperations = CascadeOperation.CascadeRead)]
        public List<Item> Inventory { get; set; }

        [MaxLength(512)]
        public string Description { get; set; }


        public int GetMoveRate() //KRB page 33
        {
            int speed = 9;
            if (Characteristic.Str < Characteristic.Siz) speed--;
            if (Characteristic.Dex < Characteristic.Siz) speed--;
            if (Age > 40) speed -= (Age - 40) % 10;
            if (speed < 2) speed = 2;
            return speed;
        }

        public Dice GetDamageBonus() // KRB page 33 table 1
        {
            var amount = -2;
            var sides = 0;
            var strSizSum = Characteristic.Siz + Characteristic.Str;
            if (strSizSum > 64)
            {
                amount++;
                if (strSizSum > 84)
                {
                    amount++;
                    if (strSizSum > 124)
                    {
                        amount++;
                        sides = 4;
                        if (strSizSum > 164)
                        {
                            sides = 6;
                            if (strSizSum > 204) amount += (strSizSum - 204) % 80 + 1;
                        }
                    }
                }
            }
            return new Dice(amount, sides);
        }

        public int GetBuilt() // KRB page33 table 1
        {
            var amount = -2;
            var strSizSum = Characteristic.Siz + Characteristic.Str;
            if (strSizSum > 64)
            {
                amount++;
                if (strSizSum > 84)
                {
                    amount++;
                    if (strSizSum > 124)
                    {
                        amount++;
                        if (strSizSum > 164)
                        {
                            amount++;
                            if (strSizSum > 204) amount += (strSizSum - 204) % 80 + 1;
                        }
                    }
                }
            }
            return amount;
        }
    }
}
