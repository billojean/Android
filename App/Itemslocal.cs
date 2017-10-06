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
    class Itemslocal
    {
        [PrimaryKey]
        public string item_id { get; set; }
        public string item_owner { get; set; }
        public string item_kind { get; set; }

      

        public override string ToString()
        {
            return string.Format("[Item: item_id={0}, item_kind={1}, item_owner={2}]", item_id, item_kind, item_owner);
        }
    }
}