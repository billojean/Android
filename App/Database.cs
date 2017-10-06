using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace App
{
   public  class Database
    {
        string docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
  
        public string createDatabase()
        {
            try
            {
                var connection = new SQLiteConnection(System.IO.Path.Combine(docsFolder, "db_sqlnet.db"));
                //connection.DropTable<Items>();
                //connection.DropTable<LocationLocal>();
                connection.CreateTable<Items>();
                connection.CreateTable<LocationLocal>();
                return "Database created";
            }
            catch (SQLiteException ex)
            {
                return ex.Message; 
            }
        }

        public string insertUpdateLocation(List<LocationLocal> data)
        {
            try
            {
                var db = new SQLiteConnection(System.IO.Path.Combine(docsFolder, "db_sqlnet.db"));
                if (db.InsertAll(data) != 0)
                    db.UpdateAll(data);
                return "List of data inserted to localDB";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
        public string insertUpdateItems(IEnumerable<Items> data)
        {
            try
            {
                var db = new SQLiteConnection(System.IO.Path.Combine(docsFolder, "db_sqlnet.db"));
                if (db.InsertAll(data) != 0)
                    db.UpdateAll(data);
                return "List of data inserted to localDB";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public TableQuery<Items> GetItems(string user)
        {
                var db = new SQLiteConnection(System.IO.Path.Combine(docsFolder, "db_sqlnet.db"));
                var items = db.Table<Items>().Where(v => v.item_owner.Equals(user));
                return items;           
        }
        public TableQuery<LocationLocal>GetLocalLocation(string user)
        {
            var db = new SQLiteConnection(System.IO.Path.Combine(docsFolder, "db_sqlnet.db"));
            var locations = db.Table<LocationLocal>().Where(v => v.username.Equals(user));
            return locations;
        }

        public string  DeleteLocalLocations(string user)
        {
            try
            {

                var db = new SQLiteConnection(System.IO.Path.Combine(docsFolder, "db_sqlnet.db"));
                var delete = db.Query<LocationLocal>("DELETE FROM LocationLocal WHERE username=?", user);

                return "Local locations deleted!";

            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
        public int findNumberRecords()
        {
            try
            {
                var db = new SQLiteConnection(System.IO.Path.Combine(docsFolder, "db_sqlnet.db"));
                // this counts all records in the database, it can be slow depending on the size of the database
                var count = db.ExecuteScalar<int>("SELECT Count(*) FROM LocationLocal");


                return count;
            }
            catch (SQLiteException)
            {
                return -1;
            }
        }
        public string deleteItems(string item_id)
        {
            try
            {

                var db = new SQLiteConnection(System.IO.Path.Combine(docsFolder, "db_sqlnet.db"));
                var delete = db.Query<Items>("DELETE FROM Items WHERE item_id=?", item_id);

                return "Items deleted!";

            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
    }
}