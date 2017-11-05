using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace App
{
   public  class Database
    {
        static string docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        SQLiteConnection db = new SQLiteConnection(System.IO.Path.Combine(docsFolder, "db_sqlnet.db"));

        public string CreateDatabase()  
        {
            try
            {
                var connection = new SQLiteConnection(System.IO.Path.Combine(docsFolder, "db_sqlnet.db"));
                //connection.DropTable<Items>();
                //connection.DropTable<LocationLocal>();
                connection.CreateTable<User>();
                connection.CreateTable<Items>();
                connection.CreateTable<LocationLocal>();
                return "Database created";
            }
            catch (SQLiteException ex)
            {
                return ex.Message; 
            }
        }

        public string InsertUpdateLocation(List<LocationLocal> data)
        {
            try
            {
                
                if (db.InsertAll(data) != 0)
                    db.UpdateAll(data);
                return "List of data inserted to localDB";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
        public string InsertUpdateItems(IEnumerable<Items> data)
        {
            try
            {
               
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
               
                var items = db.Table<Items>().Where(v => v.item_owner.Equals(user));
                return items;           
        }

        public string InsertLoggedInUser(User user)
        {
            try
            {
                
                if (db.Insert(user) != 0)
                    db.Update(user);
                return "List of data inserted to localDB";
            }

            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
        public User GetLoggedInUser()      
        {
           
            return db.Table<User>().SingleOrDefault(); 
        }

        public TableQuery<LocationLocal>GetLocalLocation(string user)
        {
           
            var locations = db.Table<LocationLocal>().Where(v => v.username.Equals(user));
            return locations;
        }

        public string DeleteLocalLocations(string user)
        {
            try
            {

                
                var delete = db.Query<LocationLocal>("DELETE FROM LocationLocal WHERE username=?", user);

                return "Local locations deleted!";

            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
        public string DeleteUser(string user)
        {
            try
            {

              
                var delete = db.Query<User>("DELETE FROM User WHERE username=?", user);

                return "Local locations deleted!";

            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public int FindNumberRecords()
        {
            try
            {
               
                var count = db.ExecuteScalar<int>("SELECT Count(*) FROM LocationLocal");


                return count;
            }
            catch (SQLiteException)
            {
                return -1;
            }
        }
        public string DeleteItems(string item_id)
        {
            try
            {

               
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