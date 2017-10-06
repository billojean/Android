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

namespace App
{
    class teamMembers
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string t_title { get; set; }

        public string t_member { get; set; }

        public string t_identity { get; set; }

        public string Email { get; set; }

        public string OfficePhone { get; set; }

        public string MobilePhone { get; set; }

        public string Department { get; set; }
        public byte[] Pic { get; set; }
    }
}