﻿
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
using Android.Support.V4.Widget;
using System.Threading.Tasks;
using Android.Support.V7.App;
//using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Android.Gms;


namespace App
{
    [Activity(Theme = "@style/MyTheme", Icon = "@drawable/people_icon", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    [Register("com.coteams.MainActivity")]
    public class MainActivity : AppCompatActivity, GoogleApiClient.IOnConnectionFailedListener
    {


        const string TAG = "MainActivity";

        const int RC_SIGN_IN = 9001;

        private ProgressBar bar;
        private Button mbtnSignIn;
   
        public static GoogleApiClient mGoogleApiClient;
        private string gmail;
        private Toolbar toolbar;

        protected override void OnCreate(Bundle bundle)
        {
    
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            /*toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetLogo(Resource.Drawable.people_icon);
            SupportActionBar.Title = "Collaboration Teams";*/
            mbtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);
            bar = FindViewById<ProgressBar>(Resource.Id.loadingPanel);

            var signInButton = FindViewById<SignInButton>(Resource.Id.btnSignIn2);
            signInButton.SetSize(SignInButton.SizeStandard);
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                    .RequestEmail()
                    .Build();
            // [END configure_signin]

            // [START build_client]
            // Build a GoogleApiClient with access to the Google Sign-In API and the
            // options specified by gso.
            mGoogleApiClient = new GoogleApiClient.Builder(this)
                             .EnableAutoManage(this /* FragmentActivity */, this /* OnConnectionFailedListener */)
                             .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                             .Build();
            mbtnSignIn.Click += (object sender, EventArgs args) =>
            {

                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                SignInDialog signInDialog = new SignInDialog();
                signInDialog.Show(transaction, "dialog fragment");

            };


            signInButton.Click += LoginByGoogle;

        }

        protected async override void OnStart()
        {
            base.OnStart();

            var opr = Auth.GoogleSignInApi.SilentSignIn(mGoogleApiClient);
            if (opr.IsDone)
            {
                // If the user's cached credentials are valid, the OptionalPendingResult will be "done"
                // and the GoogleSignInResult will be available instantly.
                Log.Debug(TAG, "Got cached sign-in");
                var result = opr.Get() as GoogleSignInResult;
                await HandleSignInResult(result);
            }
            else
            {
                // If the user has not previously signed in on this device or the sign-in has expired,
                // this asynchronous branch will attempt to sign in the user silently.  Cross-device
                // single sign-on will occur in this branch.
                
                opr.SetResultCallback(new SignInResultCallback { Activity = this });
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
         
        }

        protected async override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Log.Debug(TAG, "onActivityResult:" + requestCode + ":" + resultCode + ":" + data);

            // Result returned from launching the Intent from GoogleSignInApi.getSignInIntent(...);
            if (requestCode == RC_SIGN_IN)
            {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                await HandleSignInResult(result);
            }
        }

        public async Task HandleSignInResult(GoogleSignInResult result)
        {
            Log.Debug(TAG, "handleSignInResult:" + result.IsSuccess);
            if (result.IsSuccess)
            {
                bar.Visibility = ViewStates.Visible;
                // Signed in successfully, show authenticated UI.
                var acct = result.SignInAccount;
                gmail = acct.Email;

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
            else
            {
                bar.Visibility = ViewStates.Invisible;
                //Toast.MakeText(this, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
                // Signed out, show unauthenticated UI.

            }
        }
        private void LoginByGoogle(object sender, EventArgs e)
        {
           
                var signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);
                StartActivityForResult(signInIntent, RC_SIGN_IN);
            

                }

       


        public void OnConnectionFailed(ConnectionResult result)
        {
            // An unresolvable error has occurred and Google APIs (including Sign-In) will not
            // be available.
            Log.Debug(TAG, "onConnectionFailed:" + result);
        }




        public override void OnBackPressed()
        {
            Intent intent = new Intent(Intent.ActionMain);
            intent.AddCategory(Intent.CategoryHome);
            StartActivity(intent);
        }
    }
   


    


    }





