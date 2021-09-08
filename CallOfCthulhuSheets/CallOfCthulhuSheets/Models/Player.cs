using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Net;

namespace CallOfCthulhuSheets.Models
{
    public class Player : Tableable
    {
        public string Name { get; set; }

        [Ignore]
        public IPAddress IPAddress { get; set; }

        public string IpAddressStr { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Investigator> Investigators { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Campaign> Campaigns { get; set; }

    }
}