using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallOfCthulhuSheets.Models
{
    public class SkillType : Tableable
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
