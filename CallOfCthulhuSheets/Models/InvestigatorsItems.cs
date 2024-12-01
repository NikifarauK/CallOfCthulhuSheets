
using SQLiteNetExtensions.Attributes;

namespace CallOfCthulhuSheets.Models
{
    public class InvestigatorsItems : Tableable
    {
        [ForeignKey(typeof(Investigator))]
        public string InvestigatorId { get; set; }

        [ForeignKey(typeof(Item))]
        public string ItemId { get; set; }

        public int Malfunction { get; set; }

        public int Charges { get; set; }
    }
}
