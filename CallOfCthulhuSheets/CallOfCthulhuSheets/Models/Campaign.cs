using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CallOfCthulhuSheets.Models
{
    public class Campaign : ITableable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get ; set; }
       

        [MaxLength(32)]
        public string Name { get; set; }


        [ForeignKey(typeof(Player))]
        public int PlayerId { get; set; }
        [ManyToOne]
        public Player Owner { get; set; }


    }
}