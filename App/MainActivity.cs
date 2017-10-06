
using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using Android.Content.PM;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Util;
using Android.Gms.Plus;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Xamarin.Auth;
using System.Threading.Tasks;

namespace App
{
    [Activity(Icon = "@drawable/people_icon", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity

    {

        
        private ProgressBar bar;
        private Button mbtnSignIn;
        SignInButton button1;
        GoogleInfo googleInfo;
        const string googleUserInfoAccessUrl = "https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}";
        private string gmail;

        protected override void OnCreate(Bundle bundle)
        {
    
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            mbtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);
            bar = FindViewById<ProgressBar>(Resource.Id.loadingPanel);
            button1 = FindViewById<SignInButton>(Resource.Id.btnSignIn2);
            FindViewById<SignInButton>(Resource.Id.btnSignIn2).SetSize(SignInButton.SizeWide);


            mbtnSignIn.Click += (object sender, EventArgs args) =>
            {

                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                SignInDialog signInDialog = new SignInDialog();
                signInDialog.Show(transaction, "dialog fragment");

            };


            button1.Click += LoginByGoogle;

        }
        private void LoginByGoogle(object sender, EventArgs e)
        {
            bool allowCancel = true;
            var auth = new OAuth2Authenticator(clientId: "767906086938-pp0omkhn89q85dsm34lgdvr8mutk65e3.apps.googleusercontent.com",
             scope: "https://www.googleapis.com/auth/userinfo.email",
             authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
             redirectUrl: new Uri("https://www.googleapis.com/plus/v1/people/me"),
             getUsernameAsync: null);
             auth.AllowCancel = allowCancel;
           

            auth.Completed += async (sender1, e1) =>
            {
                if (!e1.IsAuthenticated)
                {
                    return;
                }
                string access_token;
                e1.Account.Properties.TryGetValue("access_token", out access_token);
         
                if (await GetProfileInfoFromGoogle(access_token))
                {
                    try
                    {
                        ClientRequests inst = new ClientRequests();
                        var response = await inst.GetUser(gmail);
                        if (response.IsSuccessStatusCode)

                        {

                            string responseBody = await response.Content.ReadAsStringAsync();

                            var jsn = JsonConvert.DeserializeObject<users>(responseBody);


                            var MenuActivity = new Intent(this, typeof(MenuActivity));
                            MenuActivity.PutExtra("MyData", jsn.UserName);
                            MenuActivity.PutExtra("MyData2", true);
                            MenuActivity.PutExtra("MyData3", jsn.FirstName + " " + jsn.LastName);
                            StartActivity(MenuActivity);
                            Toast.MakeText(this, "Logged in", ToastLength.Short).Show();
                        }
                        else
                        {
                            
                            Toast.MakeText(this, "Wrong email", ToastLength.Short).Show();
                            bar.Visibility = ViewStates.Invisible;

                        }

                    }
                    catch (Exception ex)
                    {
                        bar.Visibility = ViewStates.Invisible;
                        Console.WriteLine(ex.ToString());
                        Toast.MakeText(this, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
                    }
                }
            };
            var intent = auth.GetUI(this);
            StartActivity(intent);
        }
        async Task<bool> GetProfileInfoFromGoogle(string access_token)
        {
          
            bar.Visibility = ViewStates.Visible;
            bool isValid = false;
         
            string userInfo = await getInfo(string.Format(googleUserInfoAccessUrl, access_token));
            if (userInfo != "Exception")
            {
         
                googleInfo = JsonConvert.DeserializeObject<GoogleInfo>(userInfo);
                isValid = true;
                gmail = googleInfo.email;

            }
            else
            {
                bar.Visibility = ViewStates.Invisible;
   
                isValid = false;
                Toast.MakeText(this, "Connection failed! Please try again", ToastLength.Short).Show();
            }

            return isValid;
        }

        async Task<string> getInfo(string strUri)
        {
            var client = new HttpClient();
            string strResultData;
            try
            {
                 strResultData = await client.GetStringAsync(new Uri(strUri));
            }
            catch (Exception)
            {
                strResultData = "Exception";

            }

            return strResultData;
        }

        public override void OnBackPressed()
        {
            Intent intent = new Intent(Intent.ActionMain);
            intent.AddCategory(Intent.CategoryHome);
            StartActivity(intent);
        }


    }
}





