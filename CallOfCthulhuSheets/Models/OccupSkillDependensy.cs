using SQLiteNetExtensions.Attributes;

namespace CallOfCthulhuSheets.Models
{ 
    public class OccupSkillDependensy : Tableable
    {
        [ForeignKey(typeof(Occupation))]
        public string OccupationId { get; set; }

        [ForeignKey(typeof(Skill))]
        public string SkillId { get; set; }

    }
}