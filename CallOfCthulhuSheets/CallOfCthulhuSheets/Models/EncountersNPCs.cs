using SQLiteNetExtensions.Attributes;

namespace CallOfCthulhuSheets.Models
{
    public class EncountersNPCs
    {
        [ForeignKey(typeof(Encounter))]
        public string EncounterId { get; set; }

        [ForeignKey(typeof(Investigator))]
        public string NpcId { get; set; }
    }
}