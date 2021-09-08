using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallOfCthulhuSheets.Models
{
    public class Occupation : ITableable
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
       

        public string Name { get; set; }

        public int MaxCreditRating { get; set; }
        public int MinCreditRating { get; set; }

        private ECharacteristic? skillPointsBaseCharacteristic;
        private ECharacteristic? skillPointsBaseCharacteristicSec;
        private ECharacteristic? skillPointsBaseCharacteristicThird;

        public ECharacteristic? SkillPointsBaseCharacteristic { get => skillPointsBaseCharacteristic ?? ECharacteristic.Edu; set => skillPointsBaseCharacteristic = value; } // if null => Edu
        public ECharacteristic? SkillPointsBaseCharacteristicSec { get => skillPointsBaseCharacteristicSec; set => skillPointsBaseCharacteristicSec = value; }
        public ECharacteristic? SkillPointsBaseCharacteristicThird { get => skillPointsBaseCharacteristicThird; set => skillPointsBaseCharacteristicThird = value; }


        [ManyToMany(typeof(OccupSkillDependensy), CascadeOperations = CascadeOperation.All)]
        public List<Skill> ProfessionalSkills { get; set; }

        
        [ManyToMany(typeof(OccupSkillTypesDependensy), CascadeOperations = CascadeOperation.CascadeRead)]
        public List<SkillType> AddedProfSkillTypes { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<OccupSkillTypesDependensy> OccupSkillTypesDependensies { get; set; }

        public override string ToString()
        {
            var str = "";
            if (SkillPointsBaseCharacteristicSec != null || SkillPointsBaseCharacteristicThird != null)
                str += $" ({SkillPointsBaseCharacteristicSec} {SkillPointsBaseCharacteristicThird})";
            return $"{Name} {SkillPointsBaseCharacteristic}" + str;
        }
    }
}
