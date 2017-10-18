
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Common.Apis;
using Java.Lang;
namespace App
{

        public class SignOutResultCallback : Object, IResultCallback
        {
            public MenuActivity Activity { get; set; }

            public void OnResult(Object result)
            {

            }
        }
    }

