using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CallOfCthulhuSheets.Models
{
    public enum EItemType { Weapon, Book, Other, }

    public class Item : Tableable
    {
        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(32)]
        public string Description { get; set; }

        public EItemType ItemType { get; set; }
    }
}