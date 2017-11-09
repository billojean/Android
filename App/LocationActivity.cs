using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Util;
using Newtonsoft.Json;
using Android.Content.PM;

namespace App
{
    [Activity(Label = "LocationActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LocationActivity : Activity, ILocationListener, IOnMapReadyCallback
    {
        private GoogleMap gmap;
        LocationManager locMgr;
        string tag = "LocationActivity";
        private Marker marker;
        private string user;
        private Marker marker2;

        public void OnMapReady(GoogleMap googleMap)
        {
            user = Intent.GetStringExtra("MyData") ?? "";
            LatLng location = new LatLng(37.85, 23.756);
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(9);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            gmap = googleMap;
            gmap.UiSettings.ZoomControlsEnabled = true;
            gmap.UiSettings.CompassEnabled = true;
            gmap.MoveCamera(CameraUpdateFactory.ZoomIn());
            gmap.MoveCamera(cameraUpdate);


        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
    
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Location);

            SetMap();


        }

        private void SetMap()
        {
            if (gmap == null)
            {

                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }

        }


        protected override void OnStart()
        {
            base.OnStart();
            Log.Debug(tag, "OnStart called");
        }

        // OnResume gets called every time the activity starts, so we'll put our RequestLocationUpdates
        // code here, so that 
        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug(tag, "OnResume called");

            // initialize location manager
            locMgr = GetSystemService(Context.LocationService) as LocationManager;


        
            var locationCriteria = new Criteria();
            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.Medium;
            string locationProvider = locMgr.GetBestProvider(locationCriteria, true);
            if (locationProvider != null)
            {
                Log.Debug(tag, "Starting location updates with " + locationProvider.ToString());
                locMgr.RequestLocationUpdates(locationProvider, 10000, 0, this);
            }
            else { Log.Info(tag, "No location providers available"); }
        }

        protected override void OnPause()
        {
            base.OnPause();

            // stop sending location updates when the application goes into the background
            // to learn about updating location in the background, refer to the Backgrounding guide
            // http://docs.xamarin.com/guides/cross-platform/application_fundamentals/backgrounding/


            // RemoveUpdates takes a pending intent - here, we pass the current Activity
            locMgr.RemoveUpdates(this);
            Log.Debug(tag, "Location updates paused because application is entering the background");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug(tag, "OnDestroy called");
        }

        protected override void OnStop()
        {
            base.OnStop();
            Log.Debug(tag, "OnStop called");
        }

        public async void OnLocationChanged(Location location)
        {
            gmap.Clear();
            Log.Debug(tag, "Location changed");
            MarkerOptions markerOpt1 = new MarkerOptions();
            markerOpt1.SetPosition(new LatLng(location.Latitude, location.Longitude));
            markerOpt1.SetTitle("Me");
            marker = gmap.AddMarker(markerOpt1);
            try
            {

                    ClientRequests inst = new ClientRequests();
                    var locations = await inst.GetMembersLocation(user);    

                    if (locations!=null)

                    {

                        foreach (var key in locations)
                        {

                            markerOpt1.SetPosition(new LatLng(key.latitude, key.longitude));
                            markerOpt1.SetTitle(key.username.Trim());
                            markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueBlue));
                            marker2 = gmap.AddMarker(markerOpt1);
                        }


                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Toast.MakeText(this, "Couldn't Establish Connection to Server", ToastLength.Short).Show();
            }
        }

        public void OnProviderDisabled(string provider)
        {
            Log.Debug(tag, provider + " disabled by user");
        }
        public void OnProviderEnabled(string provider)
        {
            Log.Debug(tag, provider + " enabled by user");
        }
        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            Log.Debug(tag, provider + " availability has changed to " + status.ToString());
        }

    }
}
