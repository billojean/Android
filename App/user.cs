using System;
using SQLite;

namespace App
{
    public class User   
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string OfficePhone { get; set; }
        public string MobilePhone { get; set; }
        public byte[] Pic { get; set; }
        public bool IsLoggedIn
        {
            get { return HasLoggedIn(); }    

       
    }

        public bool HasLoggedIn()   
        {
            var db = new Database();
            var user = db.GetLoggedInUser();

            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}