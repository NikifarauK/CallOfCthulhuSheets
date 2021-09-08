using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallOfCthulhuSheets.Models
{
    public class OccupSkillTypesDependensy : Tableable
    {
        [ForeignKey(typeof(SkillType))]
        public string SkillTypesId { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public SkillType Type { get; set; }

        public int SkillCount { get; set; }

        [ForeignKey(typeof(Occupation))]
        public string OccupId { get; set; }


        public override string ToString()
        {
            return Type?.Name + " " + SkillCount;
        }
    }
}
