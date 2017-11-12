using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Android.Content.PM;
using SQLite;
using Android.Gms.Common.Apis;
using Android.Util;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V4.Widget;
using System.Threading.Tasks;
using Android.Gms.Auth.Api;

namespace App
{
    [Activity(Theme = "@style/MyTheme", Label = "App", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MenuActivity : AppCompatActivity
    {

        private ToggleButton butt9;
        const string TAG = "MenuActivity";
       
        private DrawerLayout mDrawerLayout;
        private ListView mLeftDrawer;
        private MyActionBarDrawerToggle mDrawerToggle;
        private User user;
        private string fullname;
        private InfoAdapter listAdapter;
        private string[] mLeftDataSet;
        private TextView itemsText;
        private Toolbar toolbar;
        private GoogleApiClient mGoogleApiClient;

        protected override void OnCreate(Bundle savedInstanceState)
        {
           
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.menu2);
            var db = new Database();
            user = db.GetLoggedInUser();
            fullname = user.FirstName + " " + user.LastName;

            Button butt1 = FindViewById<Button>(Resource.Id.button1);
            Button butt2 = FindViewById<Button>(Resource.Id.button2);
            Button butt3 = FindViewById<Button>(Resource.Id.button3);
            Button butt4 = FindViewById<Button>(Resource.Id.button4);
            Button butt5 = FindViewById<Button>(Resource.Id.button5);
            Button butt6 = FindViewById<Button>(Resource.Id.button6);
            Button butt7 = FindViewById<Button>(Resource.Id.button7);
            Button butt8 = FindViewById<Button>(Resource.Id.button8);
            butt9 = FindViewById<ToggleButton>(Resource.Id.button9);
            itemsText = FindViewById<TextView>(Resource.Id.itemscount);
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawerlayout);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            mGoogleApiClient = new GoogleApiClient.Builder(this)
                    .AddApi(Auth.GOOGLE_SIGN_IN_API)
                    .Build();

            butt9.Checked = true;

            mDrawerToggle = new MyActionBarDrawerToggle(
                 this,
                 mDrawerLayout,
                 Resource.String.openDrawer,
                 Resource.String.closeDrawer
             );
            mDrawerLayout.AddDrawerListener(mDrawerToggle);

            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Cooperation Teams App";

            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            mDrawerToggle.SyncState();
            butt1.Click += StartCreateTeam;
            butt2.Click += StartTakeItem;
            butt3.Click += StartMyTeam;
            butt4.Click += StartMyItems;
            butt5.Click += StartEnterTeam;
            butt6.Click += StartLocation;
            butt7.Click += StartEndWork;
            butt8.Click += StartLogOut;

            

            mLeftDrawer.ItemClick += MListView_ItemClick;

                App1.StartLocationService(user.UserName, true);


            butt9.Click += (o, eg) =>
            {
                if (butt9.Checked)
                {
              
                        App1.StopLocationService();
                        App1.StartLocationService(user.UserName, true);
                        Toast.MakeText(this, "You are now sending location!", ToastLength.Short).Show();                
                }
                else
                {
                
                        App1.StopLocationService();
                        Toast.MakeText(this, "Location service is stopped!", ToastLength.Short).Show();
                        App1.StartLocationService(user.UserName, false);                   
                }
            };

        }
        
        protected override void OnStart()
        {
            base.OnStart();
            mGoogleApiClient.Connect();
        }

        protected override void OnResume()
        {
            base.OnResume();

            CountMyItems();
            GetLeftDrawerInfo();
        }

        private  void CountMyItems()
        {
            try {
                var db = new Database();
                var table = db.GetItems(user.UserName);
                string json = JsonConvert.SerializeObject(table);
                var data = JsonConvert.DeserializeObject<List<Items>>(json);

                if (data.Count > 0)
                {
                    itemsText.Visibility = ViewStates.Visible;                   
                }
                else
                {
                    itemsText.Visibility = ViewStates.Invisible;
                }
                itemsText.Text = "" + data.Count;
            }
            catch(SQLiteException ex)
            { Console.WriteLine(ex.ToString()); }
        }

        private async void GetLeftDrawerInfo()
        {
            Title = await GetUserTeamTitle();
            mLeftDataSet = new string[] {fullname, "Current Team: " + Title, "My Items("+itemsText.Text+")" };
            listAdapter = new InfoAdapter(this, mLeftDataSet);
            mLeftDrawer.Adapter = listAdapter;
        }

        private async Task<string> GetUserTeamTitle()
        {
            try
            {
                 ClientRequests inst = new ClientRequests();
                 var title = await inst.GetUserTeam(user.UserName);
                 return title;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if(e.Position == 1)
            { StartMyTeam(null, null); }
            if(e.Position == 2)
            { StartMyItems(null, null); }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            GetLeftDrawerInfo();
            mDrawerToggle.OnOptionsItemSelected(item);
            return base.OnOptionsItemSelected(item);
        }

        private void StartMyItems(object sender, EventArgs e)
        {
            var MyItemsActivity = new Intent(this, typeof(MyItemActivity));
            MyItemsActivity.PutExtra("MyData", user.UserName);
            StartActivity(MyItemsActivity);
        }

        private void StartLocation(object sender, EventArgs e)
        {
            
     
                var LocationActivity = new Intent(this, typeof(LocationActivity));
                LocationActivity.PutExtra("MyData", user.UserName);
                StartActivity(LocationActivity);
            
        }

        private void StartMyTeam(object sender, EventArgs e)
        {
            var MyTeamActivity = new Intent(this, typeof(MyTeamActivity));
            MyTeamActivity.PutExtra("MyData", user.UserName);
            StartActivity(MyTeamActivity);
        }

        async void StartEndWork(object sender, EventArgs e)
        {

            ClientRequests inst = new ClientRequests();

            var db = new Database();
            try
            {
              
                var response = await inst.HasItems(user.UserName);
                    if (response.IsSuccessStatusCode)
                    {
                        new Android.App.AlertDialog.Builder(this)
                            .SetPositiveButton("Yes", (sender1, args) =>
                            {

                                var ItemActivity = new Intent(this, typeof(MyItemActivity));
                                ItemActivity.PutExtra("MyData", user.UserName);
                                StartActivity(ItemActivity);

                            })
                            .SetMessage("You have items procceed to return?")
                            .SetTitle("Question?")
                            .Show();
                    }


                    else {

                        new Android.App.AlertDialog.Builder(this)
                           .SetPositiveButton("Yes", async (sender1, args) =>
                           {

                               try
                               {
                                   var locations = db.GetLocalLocation(user.UserName);

                                   string json = JsonConvert.SerializeObject(locations);
                                  
                                   try
                                   {
                                       
                                       var response2 = await inst.PostLocalLocation(json);
                                      
                                   }
                                   catch (Exception ex)
                                   {

                                       Console.WriteLine(ex.ToString());
                                       Toast.MakeText(this, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
                                   }
                               }
                               catch (SQLiteException)
                               {
                                  
                               }
                           try
                           {
                                  
                                   var response3 = await inst.DeleteUserFromTeam(user.UserName);
                                   if (response3.IsSuccessStatusCode)
                                   {
                                           
                                           Toast.MakeText(this, "Ended Work!", ToastLength.Short).Show();
                                       
                                   }
                                   else { Toast.MakeText(this, "You are not in a Team!", ToastLength.Short).Show(); }
                                  
                                  string delete= db.DeleteLocalLocations(user.UserName); 
                                   
                               }
                               catch (Exception ex)
                               {

                                   Console.WriteLine(ex.ToString());
                                   Toast.MakeText(this, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
                               }
                           })

                         .SetNegativeButton("No", (sender2, args) =>
                         {
                             new Intent(this, typeof(MenuActivity));

                         })
            .SetMessage("Are you sure you want to End Work?")
            .SetTitle("Question?")
            .Show();

                        
                    }
               
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                Toast.MakeText(this, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
            }


        }

        void StartCreateTeam(object sender, EventArgs e)
        {
            var CreateTeamActivity = new Intent(this, typeof(CreateTeamActivity));
            CreateTeamActivity.PutExtra("MyData", user.UserName);
            StartActivity(CreateTeamActivity);
        }

        void StartEnterTeam(object sender, EventArgs e)
        {
            var enterTeamActivity = new Intent(this, typeof(EnterTeamActivity));
            enterTeamActivity.PutExtra("MyData", user.UserName);
            StartActivity(enterTeamActivity);
        }

        void StartTakeItem(object sender, EventArgs e)
        {
            var TakeItemActivity = new Intent(this, typeof(TakeItemActivity));
            TakeItemActivity.PutExtra("MyData", user.UserName);
            StartActivity(TakeItemActivity);
        }

        void StartLogOut(object sender, EventArgs e)
        {
            new Android.App.AlertDialog.Builder(this)
            .SetPositiveButton("Yes",  (sender1, args) =>
            {
                Auth.GoogleSignInApi.SignOut(mGoogleApiClient);
                var db = new Database();
                string result = db.DeleteUser(user.UserName);
                var LogOut = new Intent(this, typeof(MainActivity));
                StartActivity(LogOut);
                Finish();
                Toast.MakeText(this, "Logged Out!", ToastLength.Short).Show();

            })
           .SetNegativeButton("No", (sender2, args) =>
           {
               new Intent(this, typeof(MenuActivity));

           })
           .SetMessage("Are you sure you want to Logout?")
           .SetTitle("Question?")
           .Show();

        }
   
        protected override void OnDestroy()
        {
            try
            {
                Log.Debug(TAG, "On destroy called");

                if (App1.started == true)
                { App1.StopLocationService(); }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());

            }
            base.OnDestroy();

        }

        public override void OnBackPressed()

        {
            if (user==null)
            { base.OnBackPressed(); }

            else {
                Intent intent = new Intent(Intent.ActionMain);
                intent.AddCategory(Intent.CategoryHome);
                StartActivity(intent);
            }
        }


    }

}