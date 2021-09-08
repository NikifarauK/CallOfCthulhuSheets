using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CallOfCthulhuSheets.Models
{
    public class Skill : Tableable
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int BasePoints { get; set; }

        [ForeignKey(typeof(SkillType))]
        public string SkillTypeId { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead & CascadeOperation.CascadeInsert)]
        public SkillType Type { get; set; }


        public override string ToString()
        {
            return $"{Description}(" + (Type?.ToString() ?? " ") + ")";
        }
    }


    public class SkillEqualiti : IEqualityComparer<Skill>
    {
        public bool Equals(Skill x, Skill y)
        {
            return string.Equals(x.Name + x.BasePoints.ToString(), y.Name + y.BasePoints.ToString());
        }

        public int GetHashCode(Skill obj)
        {
            return obj.GetHashCode();
        }
    }
}