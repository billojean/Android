using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;


namespace App
{
    class SignInDialog : DialogFragment
    {
        private EditText user;
        private EditText pass;
        private ProgressBar bar;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.dialog_sign_in, container, false);
            Button button = view.FindViewById<Button>(Resource.Id.button1);
            user = view.FindViewById<EditText>(Resource.Id.username);
            pass = view.FindViewById<EditText>(Resource.Id.password);
            button.Click += StartUserLogin;
            bar = view.FindViewById<ProgressBar>(Resource.Id.loadingPanel); 
            return view;

        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);

        }

        async void StartUserLogin(object sender, EventArgs e)
        {
            bar.Visibility = ViewStates.Visible;
            var db = new Database();

            try {

                try
                {
                    var values = new Dictionary<string, string>
                 {
                    { "UserName", user.Text },
                    { "PassWord", pass.Text }
                 };
                       
                        ClientRequests inst = new ClientRequests();
                        var response = await inst.UserLogIn(values);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();

                            var jsn = JsonConvert.DeserializeObject<User>(responseBody);

                            var result = db.InsertLoggedInUser(jsn);

                    

                            var MenuActivity = new Intent(Activity, typeof(MenuActivity));
                            StartActivity(MenuActivity);
                            Toast.MakeText(Activity, "Logged in", ToastLength.Short).Show();
                        }
                        else {
                            bar.Visibility = ViewStates.Invisible;
                            Toast.MakeText(Activity, "Wrong Username or Password", ToastLength.Short).Show();

                        }


                    
                }
                catch (Exception ex)
                {
                    bar.Visibility = ViewStates.Invisible;
                    Console.WriteLine(ex.ToString());
                    Toast.MakeText(Activity, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
                }
               
            }
            catch (Java.Lang.NullPointerException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


    }
}