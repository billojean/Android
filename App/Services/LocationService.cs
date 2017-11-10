using System;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using Android.Locations;
using Android.Widget;
using System.Collections.Generic;
using SQLite;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace App.Services
{

    [Service]
    public class LocationService : Service, ILocationListener
    {
        public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
        public event EventHandler<ProviderDisabledEventArgs> ProviderDisabled = delegate { };
        public event EventHandler<ProviderEnabledEventArgs> ProviderEnabled = delegate { };
        public event EventHandler<StatusChangedEventArgs> StatusChanged = delegate { };
        
        public LocationService()
        {
        }

        // Set our location manager as the system location service
        protected LocationManager LocMgr = Android.App.Application.Context.GetSystemService("location") as LocationManager;

        readonly string logTag = "LocationService";
        IBinder binder;
        private string user;
        private bool ToggleIsChecked;
   
        public override void OnCreate()
        {
            base.OnCreate();
            Log.Debug(logTag, "OnCreate called in the Location Service");
        }

        // This gets called when StartService is called in our App1 class
        [Obsolete("deprecated in base class")]
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Debug(logTag, "LocationService started");

            user = intent.GetStringExtra("MyData") ?? "";
            ToggleIsChecked = intent.GetBooleanExtra("MyData2", true);
             return StartCommandResult.Sticky;
        }

        // This gets called once, the first time any client bind to the Service
        // and returns an instance of the LocationServiceBinder. All future clients will
        // reuse the same instance of the binder
        public override IBinder OnBind(Intent intent)
        {
            Log.Debug(logTag, "Client now bound to service");

            binder = new LocationServiceBinder(this);
            return binder;
        }

        // Handle location updates from the location manager
        public void StartLocationUpdates()
        {
            //we can set different location criteria based on requirements for our app -
            //for example, we might want to preserve power, or get extreme accuracy
            var locationCriteria = new Criteria
            {
                Accuracy = Accuracy.Coarse,
                PowerRequirement = Power.Medium
            };

            // get provider: GPS, Network, etc.
            var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);
            Log.Debug(logTag, string.Format("You are about to get location updates via {0}", locationProvider));

            // Get an initial fix on location
         
           if(locationProvider!=null)
            {
                LocMgr.RequestLocationUpdates(locationProvider, 10000, 1, this);
                Log.Debug(logTag, "Now sending location updates");
            }
            else
            {
                Toast.MakeText(this, "No provider Found! ", ToastLength.Short).Show();
            }
            }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug(logTag, "Service has been terminated");

            // Stop getting updates from the location manager:
            LocMgr.RemoveUpdates(this);
        }

        #region ILocationListener implementation
        // ILocationListener is a way for the Service to subscribe for updates
        // from the System location Service

        public async void OnLocationChanged(Android.Locations.Location location)
        {
            ClientRequests inst = new ClientRequests();
            Log.Debug(logTag, "OnLocationChanged called");
            this.LocationChanged(this, new LocationChangedEventArgs(location));
            if (ToggleIsChecked)
            {
               
                    try
                    {
                        JObject data = new JObject
                        {
                            ["username"] = user,
                            ["latitude"] = location.Latitude,
                            ["longitude"] = location.Longitude
                        };

                    string json = JsonConvert.SerializeObject(data);
                           
                            var response = await inst.PostLocation(json);

                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                
            }
            else
            {
                try
                {
                        var title = await inst.GetUserTeam(user);   

                         var db = new Database();

                         DateTime thisDay = DateTime.Now;
                        try
                        {
                            var locationList = new List<LocationLocal>
                            {

                                new LocationLocal {username = user, t_title = title, latitude = location.Latitude, longitude= location.Longitude ,datetime = thisDay},

                            };
                        
                        var result2 = db.InsertUpdateLocation(locationList);
                      
                        }
                        catch (SQLiteException)
                        {
                            
                        }
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                

            }
        }


        public void OnProviderDisabled(string provider)
        {
            this.ProviderDisabled(this, new ProviderDisabledEventArgs(provider));
            Log.Debug(logTag, "Location provider disabled event raised");

        }

        public void OnProviderEnabled(string provider)
        {
            this.ProviderEnabled(this, new ProviderEnabledEventArgs(provider));
            Log.Debug(logTag, "Location provider enabled event raised");
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            this.StatusChanged(this, new StatusChangedEventArgs(provider, status, extras));
            Log.Debug(logTag, "Location status changed, event raised");
        }

        #endregion

    }
}