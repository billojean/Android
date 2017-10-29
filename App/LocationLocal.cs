using System;
using SQLite;

namespace App
{
    public class LocationLocal
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string username { get; set; }

        public string t_title { get; set; }
        public double latitude { get; set; }
       
        public double longitude { get; set; }
       
        public DateTime datetime { get; set; }

        public override string ToString()
        {
            return string.Format("[Location: Id={0},username={1}, t_title={2},latitude={3}, longtitude={4}, datetime={5}]",Id, username, t_title, latitude, longitude, datetime);
        }
    }
}