using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Util;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Android.Support.V7.App;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;

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

        protected override void OnCreate(Bundle bundle)
        {
    
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            mbtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);
            bar = FindViewById<ProgressBar>(Resource.Id.loadingPanel);

            var signInButton = FindViewById<SignInButton>(Resource.Id.btnSignIn2);
            signInButton.SetSize(SignInButton.SizeStandard);
            var db = new Database();
            var rslt = db.CreateDatabase();
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

        private bool IsLoggedIn()   
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

        protected override void OnStart()
        {
            base.OnStart();

            if(IsLoggedIn())
            {
                bar.Visibility = ViewStates.Visible;
                var MenuActivity = new Intent(this, typeof(MenuActivity));
                StartActivity(MenuActivity);
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
            var db = new Database();
            Log.Debug(TAG, "handleSignInResult:" + result.IsSuccess);

            if (result.IsSuccess)
            {
                bar.Visibility = ViewStates.Visible;

                var acct = result.SignInAccount;
                gmail = acct.Email;

                try
                {
                    ClientRequests inst = new ClientRequests();
                    var user = await inst.GetUserByEmail(gmail);    
                    if (user!=null)

                    {
                        var reslt = db.InsertLoggedInUser(user); 
                        var MenuActivity = new Intent(this, typeof(MenuActivity));
                        StartActivity(MenuActivity);
                        Toast.MakeText(this, "Logged in", ToastLength.Short).Show();
                        Console.WriteLine($"{user.FirstName} {user.LastName}");
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





