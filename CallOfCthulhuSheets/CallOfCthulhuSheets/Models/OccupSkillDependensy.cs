using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CallOfCthulhuSheets.Models
{
    public class OccupSkillDependensy : ITableable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
       

        [ForeignKey(typeof(Occupation))]
        public int OccupationId { get; set; }

        [ForeignKey(typeof(Skill))]
        public int SkillId { get; set; }

    }
}