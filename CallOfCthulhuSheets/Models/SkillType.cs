namespace CallOfCthulhuSheets.Models
{
    public class SkillType : Tableable
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
