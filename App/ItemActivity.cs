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
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace App
{
    [Activity(Theme = "@style/MyTheme", Label = "App", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ItemActivity : AppCompatActivity
    {
        private ListView mListView;
        private string user;
        private ItemsAdapter mitems;
        private Items items;
        private Button button;
        private ProgressBar bar;
        private Toolbar toolbar;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
       
            base.OnCreate(bundle);

            
            SetContentView(Resource.Layout.ItemsList);
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "My Items List";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            mListView = FindViewById<ListView>(Resource.Id.myListView);
            bar = FindViewById<ProgressBar>(Resource.Id.loadingPanel);
            button = FindViewById<Button>(Resource.Id.buttn);
            button.Visibility = ViewStates.Invisible;

            user = Intent.GetStringExtra("MyData") ?? "";

            GetItemsFromDB();
          
    }

        private async void GetItemsFromDB()
        {
            try
            {

                    ClientRequests inst = new ClientRequests();
                    var response = await inst.GetItems(user);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    List<Items> jsn = new List<Items>();
                    jsn = JsonConvert.DeserializeObject<List<Items>>(responseBody);

                        mitems = new ItemsAdapter(this, jsn);

                        mListView.Adapter = mitems;
                        mListView.ItemClick += MListView_ItemClick;

                   
                    bar.Visibility = ViewStates.Gone;
                

            }
            catch (Exception ex)
            {
                bar.Visibility = ViewStates.Gone;
                Console.WriteLine(ex.ToString());
                Toast.MakeText(this, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
            }


        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            OnBackPressed();
            return base.OnOptionsItemSelected(item);
        }


        private void StartEndWork(object sender, EventArgs e)
        {

            var db = new Database();
            ClientRequests inst = new ClientRequests();

            new Android.App.AlertDialog.Builder(this)
                          .SetPositiveButton("Yes", async (sender1, args) =>
                          {

                              try
                              {

                               
                                  var locations = db.GetLocalLocation(user);
                                  string json = JsonConvert.SerializeObject(locations);
                                  try
                                  {


                                      var response = await inst.PostLocalLocation(json);
                                    
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
       
                                  var response = await inst.DeleteUserFromTeam(user);
                                      if (response.IsSuccessStatusCode)
                                      {
                                          Toast.MakeText(this, "Ended Work!", ToastLength.Short).Show();
                                      
                                     string delete = db.DeleteLocalLocations(user);
                                          Finish();
                                      }
                                      else { Toast.MakeText(this, "You are not in a Team!", ToastLength.Short).Show(); }
                                  
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

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {

                var db = new Database();
                items = mitems[e.Position];
                var kind = items.item_kind.Trim();
                var id = items.item_id.Trim();

                new Android.App.AlertDialog.Builder(this)
                                .SetPositiveButton("Yes", async (sender1, args) =>
                                {


                                    ClientRequests inst = new ClientRequests();
                                    var response = await inst.ReturnItem(id);

                                    if (response.IsSuccessStatusCode)
                                        {

                                       
                                        var result2 = db.DeleteItems(id);

                                            if (result2 == "Items deleted!")

                                            {
                                                Toast.MakeText(this, "Returned!", ToastLength.Short).Show();

                                                mitems.RemoveItem(e.Position);
                                                mListView.Adapter = mitems;
                                                mitems.NotifyDataSetChanged();

                                                Console.WriteLine(e.Position);
                                                if (mitems.Count == 0)
                                                {
                                                    button.Visibility = ViewStates.Visible;
                                                    button.Click += StartEndWork;
                                                }
                                            }

                                        }
                                   

                                })
                                .SetNegativeButton("No", (sender2, args) =>
                                {
                                    new Intent(this, typeof(ItemActivity));

                                })
                .SetMessage("Return Item: " + kind + " with Id: " + id + "?")
                .SetTitle("Question?")
                .Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Toast.MakeText(this, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
            }

        }

      }

    }

