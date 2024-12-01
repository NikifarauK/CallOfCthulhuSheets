using SQLite;
using SQLiteNetExtensions.Attributes;

namespace CallOfCthulhuSheets.Models
{
    public class CampaignesPCs :Tableable
    {
        [ForeignKey(typeof(Campaign))]
        public string CampaignId { get; set; }

        [ForeignKey(typeof(Investigator))]
        public string InvestigatorId { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public Investigator TheInvestigator { get; set; }


        [Ignore]
        public Player Owner { get => TheInvestigator.Owner; }
    }
}
