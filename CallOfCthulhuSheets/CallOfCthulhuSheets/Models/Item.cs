using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CallOfCthulhuSheets.Models
{
    public enum EItemType {Weapon, Book,  }

    public class Item : ITableable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
       

        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string Description { get; set; }

        public EItemType ItemType { get; set; }


    }
}