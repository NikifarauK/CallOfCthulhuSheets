using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CallOfCthulhuSheets.Models
{
    public class Campaign : Tableable
    { 
        [MaxLength(32)]
        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey(typeof(Player))]
        public string PlayerId { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Player Owner { get; set; }

        [ManyToMany(typeof(CampaignesPCs), CascadeOperations =CascadeOperation.CascadeRead)]
        public List<Investigator> Investigators { get; set; }
        
        
        [ManyToMany(typeof(CampaignesNPCs), CascadeOperations =CascadeOperation.CascadeRead)]
        public List<Investigator> NPCs{ get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Session> Sessions { get; set; }
    }
}