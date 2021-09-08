using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CallOfCthulhuSheets.Models
{
    public class Campaign : Tableable
    { 
        [MaxLength(32)]
        public string Name { get; set; }


        [ForeignKey(typeof(Player))]
        public string PlayerId { get; set; }
        [ManyToOne]
        public Player Owner { get; set; }


    }
}