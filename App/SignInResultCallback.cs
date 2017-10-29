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