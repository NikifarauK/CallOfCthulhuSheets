using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallOfCthulhuSheets.Models
{
    public class CampaignesNPCs :Tableable
    {
        [ForeignKey(typeof(Campaign))]
        public string CampaignId { get; set; }

        [ForeignKey(typeof(Investigator))]
        public string InvestigatorId { get; set; }
    }
}
