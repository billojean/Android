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
using System.Net;
using System.Net.Http;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Android.Content.PM;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using System.Net.Http.Headers;
using Android.Support.V7.Widget;

namespace App
{
    [Activity(Theme = "@style/MyTheme" ,Label = "App", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CreateTeamActivity : AppCompatActivity
    {
        private ProgressBar bar;
        private Dictionary<string, string> values;
        private EditText tpin;
        private EditText ttitle;
        private CheckBox checkbox;
        private string creator;
        private Toolbar toolbar;
        private Button butt9;

        protected override void OnCreate(Bundle savedInstanceState)
        {
      
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CreateTeam);
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Create Team";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            butt9 = FindViewById<Button>(Resource.Id.button9);
            checkbox = FindViewById<CheckBox>(Resource.Id.checkBox1);
            bar = FindViewById<ProgressBar>(Resource.Id.loadingPanel);
            ttitle = FindViewById<EditText>(Resource.Id.TeamTitle);
            tpin = FindViewById<EditText>(Resource.Id.pin);
            creator = Intent.GetStringExtra("MyData") ?? "";

            butt9.Click += StartCreateTeam;
             
           
        }

        private async void StartCreateTeam(object sender, EventArgs e)
        {
            bar.Visibility = ViewStates.Visible;
            try
            {
                int Counter = 0;

                Counter = Regex.Matches(tpin.Text, @"[a-zA-Z]+(\d{4})|(\d{4})[a-zA-Z]|(\d{3})[a-zA-Z]+(\d{1})|(\d{2})[a-zA-Z]+(\d{2})|(\d{1})[a-zA-Z]+(\d{3})").Count;
                    
                    if (Counter > 0)//check pin validation
                    {
                        if (ttitle.Text.Trim()!="")
                        {
                            if (checkbox.Checked)
                            {

                                values = new Dictionary<string, string>
                                    {

                                        { "title", ttitle.Text },
                                        { "pin", tpin.Text },
                                        { "creator",creator},
                                        { "visibility","true"}
                                    };

                            }
                            else
                            {
                                values = new Dictionary<string, string>
                                    {

                                        { "title", ttitle.Text },
                                        { "pin", tpin.Text },
                                        { "creator",creator},
                                        { "visibility","false"}
                                    };

                            }

                        ClientRequests inst = new ClientRequests();
                        HttpResponseMessage response = await inst.CreateTeam(values);
                        string responseString = await response.Content.ReadAsStringAsync();
                        var jsn = JsonConvert.DeserializeObject<dynamic>(responseString);

                        if (response.IsSuccessStatusCode)
                            {

                                Finish();
                                Toast.MakeText(this, "Created Successfully!", ToastLength.Short).Show();

                            }

                            else
                            {

                                Toast.MakeText(this, jsn, ToastLength.Short).Show();
                                bar.Visibility = ViewStates.Invisible;
                            }
                        }
                        else
                        {
                            Toast.MakeText(this, "Invalid Title", ToastLength.Short).Show();
                            bar.Visibility = ViewStates.Invisible;
                        }
                    }

                    else {
                        Toast.MakeText(this, "Invalid PIN", ToastLength.Short).Show();
                        bar.Visibility = ViewStates.Invisible;
                    }
               
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Toast.MakeText(this, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
                bar.Visibility = ViewStates.Invisible;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            
                    OnBackPressed();
                    return base.OnOptionsItemSelected(item);
        }

        


    }
}