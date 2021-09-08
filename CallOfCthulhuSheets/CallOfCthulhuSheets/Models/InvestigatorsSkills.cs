using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallOfCthulhuSheets.Models
{
    public class InvestigatorsSkills : ITableable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        

        [ForeignKey(typeof(Skill))]
        public int SkillId { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Skill Skill { get; set; }

        [Ignore]
        public string Name { get => Skill?.Description; }

        [ForeignKey(typeof(Investigator))]
        public int InvestigatorId { get; set; }

        public int? CurrentSkillValue { get; set; }

        public bool? IsSuccessfulUsed { get; set; }
    }
}
