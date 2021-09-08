using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallOfCthulhuSheets.Models
{
    public class OccupSkillTypesDependensy : ITableable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }


        [ForeignKey(typeof(SkillType))]
        public int SkillTypesId { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public SkillType Type {get; set;}

        public int SkillCount { get; set; }

        [ForeignKey(typeof(Occupation))]
        public int OccupId{ get; set; }


        public override string ToString()
        {
            return Type?.Name + " " + SkillCount;
        }
    }
}
