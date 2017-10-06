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
using System.Net;

namespace App
{
    public class TeamDialog : DialogFragment
    {
        private EditText pin;
        private string title;
        private ProgressBar bar;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.team_sign_in, container, false);
            Button button = view.FindViewById<Button>(Resource.Id.button);
            pin = view.FindViewById<EditText>(Resource.Id.pinn);
            bar = view.FindViewById<ProgressBar>(Resource.Id.loadingPanel);

            button.Click += StartTeamSignIn;

            return view;
        }

        async private void StartTeamSignIn(object sender, EventArgs e)
        {
            bar.Visibility = ViewStates.Visible;

            try { 

            string user = Arguments.GetString("MyData2");
            user = user.Trim();
            if (Arguments.GetString("MyData1") != null && Arguments.GetString("MyData3") != null)

                {
                    string pinn = Arguments.GetString("MyData1"); 
                    pinn = pinn.Trim();
                    string title = Arguments.GetString("MyData3");
                    title = title.Trim();
                    Dialog.SetCanceledOnTouchOutside(false);
                    if (pinn.Equals(pin.Text))
                    {
                     try
                      {
                       
                               
                                var values = new Dictionary<string, string>
                             {


                                { "t_member", user},
                                { "t_title", title },
                                {"t_identity","member" },
                                {"t_pin",pinn }
                             };
                          
                               ClientRequests inst = new ClientRequests();
                                var response = await inst.PostUserInTeam(values);

                                var responseString = await response.Content.ReadAsStringAsync();
                                var jsn = JsonConvert.DeserializeObject<dynamic>(responseString);


                                if (response.IsSuccessStatusCode)
                                {
                                    Activity.Finish();
                                    Toast.MakeText(Activity, "Entered Team ", ToastLength.Short).Show();
                                }
                                else
                                {
                                    Toast.MakeText(Activity, jsn, ToastLength.Short).Show();
                                    bar.Visibility = ViewStates.Invisible;
                                }

                                                 
                     }

                     catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            Toast.MakeText(Activity, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
                            bar.Visibility = ViewStates.Invisible;
                        }
                    }
                    else
                    {
                        Toast.MakeText(Activity, "Incorrect PIN", ToastLength.Short).Show();
                        bar.Visibility = ViewStates.Invisible;
                    }
                    Dialog.SetCanceledOnTouchOutside(true);
               
        }

            else
            {
                try
                {
                        if (pin.Text.Trim() != "")
                        { 
                    
                        ClientRequests inst = new ClientRequests();
                        var response = await inst.GetTeam(pin.Text);
                            string responseBody = await response.Content.ReadAsStringAsync();

                        var jsn = JsonConvert.DeserializeObject<dynamic>(responseBody);
                       
                       if(response.IsSuccessStatusCode)
                        {
                                title = (string)jsn["title"];
                                title = title.Trim();

                                var values2 = new Dictionary<string, string>
                                         {

                                       { "t_title", title },
                                       { "t_member", user},
                                       {"t_identity","member" },
                                       {"t_pin",pin.Text}

                                         };


                            var response2 = await inst.PostUserInTeam(values2);

                            var responseString = await response2.Content.ReadAsStringAsync();

                                var jsn2 = JsonConvert.DeserializeObject<dynamic>(responseString);


                                 if (response2.IsSuccessStatusCode)
                                 {
                                    Toast.MakeText(Activity, "Entered Team ", ToastLength.Short).Show();
                                    Activity.Finish();
                                }

                                 else
                                 {
                                     Toast.MakeText(Activity, jsn2, ToastLength.Short).Show();
                                     bar.Visibility = ViewStates.Invisible;
                                }

                            }
                            else
                            {
                               
                                Toast.MakeText(Activity, jsn, ToastLength.Short).Show();
                                bar.Visibility = ViewStates.Invisible;
                            }
                    }
                        else {
                            Toast.MakeText(Activity, "Incorrect PIN", ToastLength.Short).Show();
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