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
using Android.Content.PM;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.Widget;
using Android.Support.V4.View;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Graphics;

namespace App
{
    [Activity(Theme = "@style/MyTheme",Label = "App", ScreenOrientation = ScreenOrientation.Portrait)]
    public class TakeItemActivity : AppCompatActivity
    {
        private string user;
        private Toolbar toolbar;
        private VehicleFragment vehiclefragment;
        private LaptopFragment laptopfragment;
        private SparePartsFragment sparepartfragment;
        private Fragment currentfragm;
        private Button but1;
        private Button but2;
        private Button but3;

        protected override void OnCreate(Bundle savedInstanceState)
        {
   
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TakeItem);
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Take An Item";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

             but1 = FindViewById<Button>(Resource.Id.button1);
             but2 = FindViewById<Button>(Resource.Id.button2);
             but3 = FindViewById<Button>(Resource.Id.button3);

            user = Intent.GetStringExtra("MyData") ?? "";
            Bundle mybundle = new Bundle();
            mybundle.PutString("MyData", user);
            vehiclefragment = new VehicleFragment();
            vehiclefragment.Arguments = mybundle;
            
            laptopfragment = new LaptopFragment();
            laptopfragment.Arguments = mybundle;

            sparepartfragment = new SparePartsFragment();
            sparepartfragment.Arguments = mybundle;

            but1.SetTextColor(Color.ParseColor("#64DD17"));

            var trans = FragmentManager.BeginTransaction();
            trans.Add(Resource.Id.fragcontainer, vehiclefragment, "fragm");

            trans.Commit();
           
            currentfragm = vehiclefragment;

            but1.Click += (object sender, EventArgs e) =>
            {
                but1.SetTextColor(Color.ParseColor("#64DD17"));

                ShowFragment(vehiclefragment);
          
            };
            but2.Click += (object sender, EventArgs e) =>
            {
                but2.SetTextColor(Color.ParseColor("#64DD17"));
                               
                ShowFragment(laptopfragment);
            };
            but3.Click += (object sender, EventArgs e) =>
            {
                but3.SetTextColor(Color.ParseColor("#64DD17"));
                               
                ShowFragment(sparepartfragment);
            };
           
       

        }
        private void ShowFragment(Fragment fragment)
        {
            
            if (currentfragm== vehiclefragment && fragment!= vehiclefragment)
            { but1.SetTextColor(Color.ParseColor("#FFFFFF")); }
            if (currentfragm == laptopfragment && fragment != laptopfragment)
            { but2.SetTextColor(Color.ParseColor("#FFFFFF")); }
            if (currentfragm == sparepartfragment && fragment != sparepartfragment)
            { but3.SetTextColor(Color.ParseColor("#FFFFFF")); }
            var trans = FragmentManager.BeginTransaction();
      
            trans.Replace(Resource.Id.fragcontainer, fragment);
      
            trans.Commit();

            currentfragm = fragment;
        }


         public override bool OnOptionsItemSelected(IMenuItem item)
         {

             OnBackPressed();
             return base.OnOptionsItemSelected(item);
         }


    }
   
}