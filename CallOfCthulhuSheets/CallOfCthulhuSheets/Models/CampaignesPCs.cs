using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

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
