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
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Collections.Specialized;
using Android.Content.PM;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using System.Net.Http.Headers;
using Android.Support.V7.Widget;

namespace App
{
   [Activity(Theme = "@style/MyTheme",Label = "App", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyTeamActivity : AppCompatActivity
    {


        private ListView mListView;
        private string user;
        private TeamMembersAdapter mteammembers;
        private ProgressBar bar;
        private Toolbar toolbar;

        protected override void OnCreate(Bundle bundle)
        {
          

            base.OnCreate(bundle);
          
            SetContentView(Resource.Layout.MyTeam);
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            bar = FindViewById<ProgressBar>(Resource.Id.loadingPanel);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Team Members";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            mListView = FindViewById<ListView>(Resource.Id.myListView);
            user = Intent.GetStringExtra("MyData") ?? "";
            GetTeamMembersFromDB();
           
        }

        private async void GetTeamMembersFromDB()
        {
            try
            {
                ClientRequests inst = new ClientRequests();
                var response = await inst.GetTeamMembers(user);

                    string responseBody = await response.Content.ReadAsStringAsync();
                  
                    List<teamMembers> jsn = new List<teamMembers>();

                        jsn = JsonConvert.DeserializeObject<List<teamMembers>>(responseBody);

                        mteammembers = new TeamMembersAdapter(this, jsn);
                        mListView.Adapter = mteammembers;

                mListView.Visibility = ViewStates.Visible;
                bar.Visibility = ViewStates.Gone;
               
            }
            catch (Exception ex)
            {
                bar.Visibility = ViewStates.Gone;
                Console.WriteLine(ex.ToString());
                Toast.MakeText(this, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Drawable.createmenu3, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch ("" + item.TitleFormatted)
            {
        
             case "Refresh":
                    mListView.Visibility = ViewStates.Invisible;
                    bar.Visibility = ViewStates.Visible;
                    GetTeamMembersFromDB();
            return true;

            default:

                    OnBackPressed();
            return base.OnOptionsItemSelected(item);
        }
    }
    }
}
