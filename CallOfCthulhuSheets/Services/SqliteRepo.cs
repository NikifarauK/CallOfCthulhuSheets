using System.Diagnostics;
using CallOfCthulhuSheets.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace CallOfCthulhuSheets.Services
{
    public static class SqliteRepo
    {
        private static SQLiteConnection db;

        public static void Init(SQLiteConnection db)
        {

            SqliteRepo.db = db;
        }

        public static void AddItemAsync<T>(T item) where T : Tableable, new()
        {
            try
            {
                db.InsertOrReplaceWithChildren(item, recursive: true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public static void DeleteItemAsync<T>(T item) where T : Tableable
        {
            try
            {
                db.Delete(item, recursive: true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }

        }

        public static T GetItemAsync<T>(string id) where T : Tableable, new()
        {
            try
            {
                return db.GetWithChildren<T>(id, true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public static List<T> GetItemsAsync<T>() where T : Tableable, new()
        {
            try
            {
                return db.GetAllWithChildren<T>(recursive: true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public static void UpdateItemAsync<T>(T item) where T : Tableable
        {
            try
            {
                db.UpdateWithChildren(item);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw e;
            }
        }
    }
}
