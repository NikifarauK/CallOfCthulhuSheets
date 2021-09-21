using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace CallOfCthulhuSheets.Models
{
    public enum EncounterTypes
    {
        Undefined, Social, Figth, Chase,
    }

    public class Encounter : Tableable
    {
        public readonly static EncounterTypes[] EncounterTypes = ((EncounterTypes[])Enum.GetValues(typeof(EncounterTypes)));

        [ForeignKey(typeof(Session))]
        public string SessionId { get; set; }
        [ManyToOne(CascadeOperations =CascadeOperation.All)]
        public Session Session { get; set; }

        public string Description { get; set; }

        public EncounterTypes EncounterType { get; set; }

        public bool IsCompleted { get; set; }

        [ManyToMany(typeof(EncountersNPCs), CascadeOperations = CascadeOperation.CascadeRead)]
        public List<Investigator> NPCs { get; set; }
    }
}