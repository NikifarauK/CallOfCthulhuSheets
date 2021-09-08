using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite;
using Xamarin.Essentials;
using System.Diagnostics;
using System.Threading.Tasks;

using CallOfCthulhuSheets.Extensions;
using CallOfCthulhuSheets.Models;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using System.Reflection;

namespace CallOfCthulhuSheets.Services
{
    public class DataBaseHandler
    {
        static SQLiteOpenFlags flag = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

        static readonly Lazy<SQLiteAsyncConnection> lazyIniter = new Lazy<SQLiteAsyncConnection>(() =>
        {
            var dbName = "CoCData.db";
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
            return new SQLiteAsyncConnection(dbPath,
                flag);
        }
        );

        public SQLiteAsyncConnection DB => lazyIniter.Value;

        public DataBaseHandler() => InitAsync().SafeFireAndForget(false, (e) => Debug.WriteLine(e.Message));


        async Task InitAsync()
        {
            try
            {
                //TODO: create tables in connection: await DB.CreateTablesAsync(CreateFlags.None, typeof(Person)).ConfigureAwait(false);
                _ = await DB.CreateTablesAsync(CreateFlags.None,
                    typeof(Campaign),
                    typeof(Characteristic),
                    typeof(Investigator),
                    typeof(InvestigatorsSkills),
                    typeof(InvestigatorsItems),
                    typeof(Item),
                    typeof(Occupation),
                    typeof(OccupSkillDependensy),
                    typeof(OccupSkillTypesDependensy),
                    typeof(Player),
                    typeof(Skill),
                    typeof(SkillType)
                 ).ConfigureAwait(false);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }
    }
}
