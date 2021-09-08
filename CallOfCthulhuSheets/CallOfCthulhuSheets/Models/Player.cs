using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CallOfCthulhuSheets.Models
{
    public class Player : ITableable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
      
        public string Name { get; set; }

        public int AccessCode { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Investigator> Investigators { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Campaign> Campaigns { get; set; }

    }
}