using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CallOfCthulhuSheets.Models
{
    public class Session : Tableable
    {
        [MaxLength(64)]
        public string Name { get; set; }

        public int Number { get; set; }

        [MaxLength(512)]
        public string Descrption { get; set; }

        public bool IsCompleted { get; set; }


        [ForeignKey(typeof(Campaign))]
        public string CampaignId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Encounter> Encounters { get; set; }
    }
}
