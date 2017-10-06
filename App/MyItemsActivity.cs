
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
using System.Net.Http.Headers;
using SQLite;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.Widget;

namespace App
{
    [Activity(Theme = "@style/MyTheme", Label = "App", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyItemsActivity : AppCompatActivity
    {


        private ListView mListView;
        private string user;
        private ItemsAdapter mitems;
        private Items items;
        private Button button;
        private ProgressBar bar;
        private string docsFolder;
        private string pathToDatabase;
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
            docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            pathToDatabase = System.IO.Path.Combine(docsFolder, "db_sqlnet.db"); ;
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

                GetItemsFromLocalDB();

                Console.WriteLine(ex.ToString());

            }
        }

        private void GetItemsFromLocalDB()
        {
            try
            {
                Database db = new Database();
                var items = db.GetItems(user);
                string json = JsonConvert.SerializeObject(items);
                var data = JsonConvert.DeserializeObject<List<Items>>(json);
                mitems = new ItemsAdapter(this, data);
                mListView.Adapter = mitems;
                Toast.MakeText(this, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            OnBackPressed();
            return base.OnOptionsItemSelected(item);
        }


        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                Database db =new Database();

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
                                        
                                        var result2 = db.deleteItems(id);

                                            if (result2 == "Items deleted!")

                                            {
                                                Toast.MakeText(this, "Returned!", ToastLength.Short).Show();

                                                mitems.RemoveItem(e.Position);
                                                mListView.Adapter = mitems;
                                                mitems.NotifyDataSetChanged();

                                                Console.WriteLine(e.Position);
                                          
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
            catch (WebException ex)
            {
                Console.WriteLine(ex.ToString());
                Toast.MakeText(this, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
            }

        }


    }
}

