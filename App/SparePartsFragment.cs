using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Newtonsoft.Json;
using SQLite;
using System.Threading.Tasks;

namespace App
{
    public class SparePartsFragment : Fragment
    {
        private SparePartsAdapter _adapter;
        private ListView _listView;
        private Android.Support.V7.Widget.SearchView _searchView;
        private SparePart spareparts;
        private ProgressBar bar;

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            await GetSwappers();

            SetHasOptionsMenu(true);

            // Create your fragment here

        }

        private async Task GetSwappers()
        {
            try {
                ClientRequests inst = new ClientRequests();
                var response = await inst.GetSpareParts();
                string responseBody = await response.Content.ReadAsStringAsync();

                var spareparts = new List<SparePart>();

                spareparts = JsonConvert.DeserializeObject<List<SparePart>>(responseBody);
                _adapter = new SparePartsAdapter(Activity, spareparts);
                bar.Visibility = ViewStates.Gone;
                _listView.Adapter = _adapter;
               
                _listView.ItemClick += MListView_ItemClick;
            }
            catch (Exception ex)
            {
                bar.Visibility = ViewStates.Gone;
                Console.WriteLine(ex.ToString());
            }
        }

       

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            
            View view = inflater.Inflate(Resource.Layout.vehiclefragment, container, false);
            _listView = view.FindViewById<ListView>(Resource.Id.listView1);
            bar = view.FindViewById<ProgressBar>(Resource.Id.loadingPanel);

            return view;
        }
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            
            if (_adapter != null)
            {
                inflater.Inflate(Resource.Drawable.createmenu2, menu);

                var item = menu.FindItem(Resource.Id.menu_edit);

                var searchView = MenuItemCompat.GetActionView(item);
                _searchView = searchView.JavaCast<Android.Support.V7.Widget.SearchView>();

                _searchView.QueryTextChange += (s, e) => _adapter.Filter.InvokeFilter(e.NewText);

                _searchView.QueryTextSubmit += (s, e) =>
                {

                    Toast.MakeText(Activity, "Searched for: " + e.Query, ToastLength.Short).Show();

                };

                MenuItemCompat.SetOnActionExpandListener(item, new SearchViewExpandListener(_adapter));

                base.OnCreateOptionsMenu(menu, inflater);
            }
        }
        private class SearchViewExpandListener
          : Java.Lang.Object, MenuItemCompat.IOnActionExpandListener
        {
            private readonly IFilterable _adapter;


            public SearchViewExpandListener(IFilterable adapter)
            {
                _adapter = adapter;

            }

            public bool OnMenuItemActionCollapse(IMenuItem item)
            {
                _adapter.Filter.InvokeFilter("");

                return true;
            }

            public bool OnMenuItemActionExpand(IMenuItem item)
            {
                return true;
            }
        }
        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                Database db = new Database();
                spareparts = _adapter[e.Position];
                var kind = "Spare Part";
                var id = spareparts.Id.Trim();
                string user = Arguments.GetString("MyData");

                new Android.App.AlertDialog.Builder(Activity)
                                .SetPositiveButton("Yes", async (sender1, args) =>
                                {

                                    try
                                    {
                                    
                                        if (!(string.IsNullOrEmpty(user)) && !(string.IsNullOrEmpty(id)))
                                        {

                                            var itemsList = new List<Items>

                                            {

                                                new Items {item_id = id, item_owner = user,item_kind = kind  },

                                            };



                                            try
                                            {
                                             

                                                var values = new Dictionary<string, string>

                                                 {
                                                    { "item_id", id },
                                                    {"item_owner" ,user},
                                                    {"item_kind" ,kind}
                                                 };
                                         

                                                ClientRequests inst = new ClientRequests();

                                                var response = await inst.PostItem(values);

                                                var responseString = await response.Content.ReadAsStringAsync();

                                                var msg = JsonConvert.DeserializeObject<dynamic>(responseString);


                                                if (response.IsSuccessStatusCode)
                                                {
                                                    var result2 = db.insertUpdateItems(itemsList);

                                                    Toast.MakeText(Activity, "Successfully Taken.", ToastLength.Short).Show();
                                                    Activity.Finish();
                                                }

                                                else
                                                {
                                                    Toast.MakeText(Activity, msg, ToastLength.Short).Show();
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.ToString());
                                                Toast.MakeText(Activity, "Couldn't Establish Connection to Server", ToastLength.Short).Show();

                                            }

                                        }
                                        else {
                                            Toast.MakeText(Activity, "Failed to Take Item", ToastLength.Short).Show();


                                        }
                                    }
                                    catch (Java.Lang.NullPointerException ex)
                                    {
                                        Console.WriteLine(ex.ToString());
                                    }


                                })
                                .SetNegativeButton("No", (sender2, args) =>
                                {
                                    new Intent(Activity, typeof(TakeItemActivity));
                                    
                                })
                .SetMessage("Take Item: " + kind + " with Id: " + id + "?")
                .SetTitle("Question?")
                .Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Toast.MakeText(Activity, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
            }
        }

    }
}