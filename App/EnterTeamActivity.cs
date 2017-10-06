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
    [Activity(Theme = "@style/MyTheme", Label = "App", ScreenOrientation = ScreenOrientation.Portrait)]
    public class EnterTeamActivity : AppCompatActivity 
    {
      
        
        private ListView mListView;
        private BaseAdapter<team> mteam;
        private string user;
        private EditText pin;
        private ProgressBar bar;
        private Toolbar toolbar;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
      
            base.OnCreate(bundle);

           
            SetContentView(Resource.Layout.EnterTeam);
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Enter Team";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            mListView = FindViewById<ListView>(Resource.Id.myListView);
            bar = FindViewById<ProgressBar>(Resource.Id.loadingPanel);
            pin = FindViewById<EditText>(Resource.Id.pin);
            user = Intent.GetStringExtra("MyData") ?? "";
            GetTeamsFromDB();           

                mListView.ItemClick += MListView_ItemClick;

        }

        private async void GetTeamsFromDB()
        {
            try
            {

                ClientRequests inst = new ClientRequests();

                HttpResponseMessage response = await inst.GetTeams();

                    string responseBody = await response.Content.ReadAsStringAsync();

          
                    List<team> jsn = new List<team>();

          
                        jsn = JsonConvert.DeserializeObject<List<team>>(responseBody);

                        mteam = new TeamAdapter(this, jsn);
                        mListView.Adapter = mteam;

                mListView.Visibility = ViewStates.Visible;
                bar.Visibility = ViewStates.Invisible;
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Toast.MakeText(this, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
                bar.Visibility = ViewStates.Gone;
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Drawable.createmenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (""+item.TitleFormatted)
            {
                case "Edit":

                    Bundle mybundle = new Bundle();
                    mybundle.PutString("MyData2", user);

                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    TeamDialog teamDialog = new TeamDialog();
                    teamDialog.Show(transaction, "dialog fragment");
                    teamDialog.Arguments = mybundle;
                    return true;

                case "Refresh":
                    mListView.Visibility = ViewStates.Invisible;
                    bar.Visibility = ViewStates.Visible;
                    GetTeamsFromDB();
                    return true;

                default:

                    OnBackPressed();
                    return base.OnOptionsItemSelected(item);
            }
            
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var items = mteam[e.Position];
  
            Bundle mybundle = new Bundle();
            mybundle.PutString("MyData1", items.pin);
            mybundle.PutString("MyData2", user);
            mybundle.PutString("MyData3", items.title);
         
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                TeamDialog teamDialog = new TeamDialog();
                teamDialog.Show(transaction, "dialog fragment");
                teamDialog.Arguments = mybundle;
 
        }


    }
}