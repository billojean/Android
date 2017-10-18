
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Java.Lang;

namespace App
{
    public class SignInResultCallback : Object, IResultCallback
    {
        public MainActivity Activity { get; set; }

        public async void OnResult(Object result)
        {
            var googleSignInResult = result as GoogleSignInResult;
            
            await Activity.HandleSignInResult(googleSignInResult);
        }
    }
}