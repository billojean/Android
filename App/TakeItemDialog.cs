using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

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
                var db = new Database();
                Dialog.SetCanceledOnTouchOutside(false);
                string user = Arguments.GetString("MyData");
                string kind = Arguments.GetString("MyData2");
                if (!(string.IsNullOrEmpty(user)) && !(string.IsNullOrEmpty(id.Text)))
                {

                    var item = new List<Items>  
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
                                var result2 = db.InsertUpdateItems(item);
                               
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
        
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);

        }
    }
}