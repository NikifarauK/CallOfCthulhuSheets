using System.Diagnostics;
using System.Reflection;
using CallOfCthulhuSheets.Extensions;
using CallOfCthulhuSheets.Models;
using SQLite;

namespace CallOfCthulhuSheets.Services
{
    public class DataBaseHandler
    {
        static SQLiteOpenFlags flag = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

        static readonly Lazy<SQLiteConnection> lazyIniter = new Lazy<SQLiteConnection>(() =>
        {
            var dbName = "CoCDataGuid.db";
            var dbFolder = FileSystem.AppDataDirectory;
            var dbPath = Path.Combine(dbFolder, dbName);


            if (!File.Exists(dbPath))
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(DataBaseHandler)).Assembly;
                var stream = assembly.GetManifestResourceStream($"CallOfCthulhuSheets.{dbName}");
                using (var binaryReader = new BinaryReader(stream))
                {
                    using (var binaryWriter = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int length = 0, iter = 0;
                        while ((length = binaryReader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            binaryWriter.Write(buffer, 0, length);
                            ++iter;
                        }
                    }
                }
            }

            //var t = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Sample.db");
            return new SQLiteConnection(dbPath,
                flag);
        }
        );

        public SQLiteConnection DB => lazyIniter.Value;

        public DataBaseHandler() => InitAsync().SafeFireAndForget(false, (e) => Debug.WriteLine((String?)e.Message));


        async Task InitAsync()
        {
            try
            {
                _ = DB.CreateTables(CreateFlags.None,
                    typeof(Campaign),
                    typeof(CampaignesPCs),
                    typeof(CampaignesNPCs),
                    typeof(Characteristic),
                    typeof(Encounter),
                    typeof(EncountersNPCs),
                    typeof(Investigator),
                    typeof(InvestigatorsSkills),
                    typeof(InvestigatorsItems),
                    typeof(Item),
                    typeof(Occupation),
                    typeof(OccupSkillDependensy),
                    typeof(OccupSkillTypesDependensy),
                    typeof(Player),
                    typeof(Session),
                    typeof(Skill),
                    typeof(SkillType)
                );

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }
    }
}
