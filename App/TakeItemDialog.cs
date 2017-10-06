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
using System.Net.Http;
using Newtonsoft.Json;
using SQLite;
using System.Net;
using System.Net.Http.Headers;

namespace App
{
    public class TakeItemDialog : DialogFragment
    {
        private EditText id;
        private ProgressBar bar;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.ItemDialog, container, false);
            Button button = view.FindViewById<Button>(Resource.Id.button);
            id = view.FindViewById<EditText>(Resource.Id.itemid);
            bar = view.FindViewById<ProgressBar>(Resource.Id.loadingPanel);
            button.Click += StartTakeItem;
            
            return view;
        }

        private async void StartTakeItem(object sender, EventArgs e)
        {
            bar.Visibility = ViewStates.Visible;
            try
            {
                var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                var pathToDatabase = System.IO.Path.Combine(docsFolder, "db_sqlnet.db"); ;
                var result = createDatabase(pathToDatabase);

                Dialog.SetCanceledOnTouchOutside(false);
                string user = Arguments.GetString("MyData");
                string kind = Arguments.GetString("MyData2");
                if (!(string.IsNullOrEmpty(user)) && !(string.IsNullOrEmpty(id.Text)))
                {

                    var peopleList = new List<Items>
                {

                    new Items {item_id = id.Text, item_owner = user,item_kind = kind  },

                };



                    try
                    {
             
                           
                            var values = new Dictionary<string, string>
                                 {

                                    { "item_id", id.Text },
                                    {"item_owner" ,user},
                                    {"item_kind" ,kind}
                                 };
          

                            ClientRequests inst = new ClientRequests();

                             var response = await inst.PostItem(values);

                            var responseString = await response.Content.ReadAsStringAsync();

                            var msg = JsonConvert.DeserializeObject<dynamic>(responseString);


                            if (response.IsSuccessStatusCode)
                            {
                                var result2 = insertUpdateAllData(peopleList, pathToDatabase);

                                var records = findNumberRecords(pathToDatabase);
                               
                                Toast.MakeText(Activity, "Successfully Taken.", ToastLength.Short).Show();
                                Activity.Finish();
                                
                            }

                            else
                            {
                                Toast.MakeText(Activity, msg, ToastLength.Short).Show();
                                bar.Visibility = ViewStates.Invisible;
                            }
                       
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Toast.MakeText(Activity, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
                        bar.Visibility = ViewStates.Invisible;
                    }
                    Dialog.SetCanceledOnTouchOutside(true);
                }
                else { Toast.MakeText(Activity, "Failed to Take Item", ToastLength.Short).Show();
                    bar.Visibility = ViewStates.Invisible;
                    Dialog.SetCanceledOnTouchOutside(true);
                }
            }
            catch (Java.Lang.NullPointerException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private string createDatabase(string path)
        {
            try
            {
                var connection = new SQLiteConnection(path);
                //connection.DropTable<Items>();
                connection.CreateTable<Items>();
                return "Database created";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
        private string insertUpdateAllData(IEnumerable<Items> data, string path)
        {
            try
            {
                var db = new SQLiteConnection(path);
                if (db.InsertAll(data) != 0)
                    db.UpdateAll(data);
                return "List of data inserted to localDB";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
        private int findNumberRecords(string path)
        {
            try
            {
                var db = new SQLiteConnection(path);
                // this counts all records in the database, it can be slow depending on the size of the database
                var count = db.ExecuteScalar<int>("SELECT Count(*) FROM Items");

                // for a non-parameterless query
                // var count = db.ExecuteScalar<int>("SELECT Count(*) FROM Person WHERE FirstName="Amy");

                return count;
            }
            catch (SQLiteException)
            {
                return -1;
            }
        }

     
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);

        }
    }
}