using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallOfCthulhuSheets.Models
{
    public class InvestigatorsItems : ITableable
    {
        public int Id { get; set; }


        [ForeignKey(typeof(Investigator))]
        public int InvestigatorId { get; set; }

        [ForeignKey(typeof(Item))]
        public int ItemId { get; set; }
    }
}
