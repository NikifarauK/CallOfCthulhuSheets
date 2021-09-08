using CallOfCthulhuSheets.Models;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace CallOfCthulhuSheets.Services
{
    public static class SqliteRepo
    {
        private static SQLiteAsyncConnection db;

        public static void Init(SQLiteAsyncConnection db)
        {

            SqliteRepo.db = db;
        }

        public static async Task AddItemAsync<T>(T item) where T : Tableable
        {
            try
            {
                await db.InsertOrReplaceWithChildrenAsync(item, recursive: true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public static async Task DeleteItemAsync<T>(T item) where T : Tableable
        {
            try
            {
                await WriteOperations.DeleteAsync(db, item, recursive: true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }

        }

        public static async Task<T> GetItemAsync<T>(string id) where T : Tableable, new()
        {
            try
            {
                return await db.GetWithChildrenAsync<T>(id, true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public static async Task<List<T>> GetItemsAsync<T>() where T : Tableable, new()
        {
            try
            {
                return await db.GetAllWithChildrenAsync<T>(recursive: true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public static async Task UpdateItemAsync<T>(T item) where T : Tableable
        {
            try
            {
                await db.UpdateWithChildrenAsync(item);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw e;
            }
        }
    }
}
