using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallOfCthulhuSheets.Models
{
    public class InvestigatorsSkills : Tableable
    {
        [ForeignKey(typeof(Skill))]
        public string SkillId { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Skill Skill { get; set; }

        [Ignore]
        public string Name => Skill?.Description;

        [ForeignKey(typeof(Investigator))]
        public string InvestigatorId { get; set; }

        public int? CurrentSkillValue { get; set; }

        public bool? IsSuccessfulUsed { get; set; }
    }
}
