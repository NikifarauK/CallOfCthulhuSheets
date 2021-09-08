using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CallOfCthulhuSheets.Models
{
    public class Player : Tableable
    {
        public string Name { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Investigator> Investigators { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Campaign> Campaigns { get; set; }

    }
}