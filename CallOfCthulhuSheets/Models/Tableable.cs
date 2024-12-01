using SQLite;

namespace CallOfCthulhuSheets.Models
{
    public abstract class Tableable
    {
        private string id;
        [PrimaryKey, MaxLength(36)]
        public virtual string Id 
        {
            get
            {
                if(id == null)
                {
                    id = Guid.NewGuid().ToString();
                }
                return id;
            } 
            set => id = value; 
        }
    }
}
