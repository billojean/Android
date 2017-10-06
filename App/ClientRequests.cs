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
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace App
{
    class ClientRequests
    {
        static HttpClient client = new HttpClient();

        static void clientAttr ()
        {
            client.BaseAddress = new Uri(constants.Baseurl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(20);
        }


        public async Task<HttpResponseMessage> CreateTeam(Dictionary<string, string> values)
        {

            clientAttr();

            var content = new FormUrlEncodedContent(values);


            HttpResponseMessage response = await client.PostAsync("api/teams/Postteam", content);

            return response;

        }

        public async Task<HttpResponseMessage> GetTeams()
        {

            clientAttr();

            HttpResponseMessage response = await client.GetAsync("api/teams/Getteams");
            return response;
        }
        public async Task<HttpResponseMessage> GetVehicles()
        {

            clientAttr();

            HttpResponseMessage response = await client.GetAsync("api/Vehicles/Getvehicles");
            return response;
        }
        public async Task<HttpResponseMessage> GetLaptops()
        {

            clientAttr();

            HttpResponseMessage response = await client.GetAsync("api/Laptops/Getlaptops");
            return response;
        }
        public async Task<HttpResponseMessage> GetSpareParts()
        {

            clientAttr();

            HttpResponseMessage response = await client.GetAsync("api/SpareParts/Getspareparts");
            return response;
        }

        public async Task<HttpResponseMessage> PostUserInTeam(Dictionary<string, string> values)
        {
            clientAttr();
            var content = new FormUrlEncodedContent(values);


            var response = await client.PostAsync("api/t_members/Post_member", content);
            return response;
        }

        public async Task<HttpResponseMessage> GetTeam(string pin)
        {
            clientAttr();
            var response = await client.GetAsync("api/teams/Getteam/" + pin);
            return response;
        }

        public async Task<HttpResponseMessage> PostItem(Dictionary<string, string> values)
        {
            clientAttr();
            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync("api/items/Postitem", content);
            return response;
        }

        public async Task<HttpResponseMessage> GetItems(string user)
        {
            clientAttr();
            HttpResponseMessage response = await client.GetAsync("api/items/Getitems?user=" + (user));
            return response;
        }

        public async Task<HttpResponseMessage> ReturnItem(string id)
        {
            clientAttr();
            HttpResponseMessage response = await client.DeleteAsync("api/items/Returnitem?id=" + id);
            return response;
        }

        public async Task<HttpResponseMessage> GetUser(string email)
        {
            clientAttr();
            HttpResponseMessage response = await client.GetAsync("api/user/Getuser?Email=" + (email));
            return response;
        }

        public async Task<HttpResponseMessage> UserLogIn(Dictionary<string, string> values)
        {
            clientAttr();
            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync("api/user/Login", content);
            return response;
        }

        public async Task<HttpResponseMessage> GetMembersLocation(string user)
        {
            clientAttr();
            HttpResponseMessage response = await client.GetAsync("api/locations/Getlocations?user=" + (user));
            return response;
        }

        public async Task<HttpResponseMessage>PostLocation(string json)
        {
            clientAttr();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/locations/Postlocation", content);
            return response;
        }
        public async Task<HttpResponseMessage> GetUserTeam(string user)
        {
            clientAttr();
            HttpResponseMessage response = await client.GetAsync("api/t_members/GetUserTeam?user=" + user);
            return response;
        }
        public async Task<HttpResponseMessage> GetTeamMembers(string user)
        {
            clientAttr();
            HttpResponseMessage response = await client.GetAsync("api/user/GetMembers?user=" + (user));
            return response;
        }
        public async Task<HttpResponseMessage> PostLocalLocation(string json)
        {
            clientAttr();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/location_history/PostLocalLocation", content);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteUserFromTeam(string user)
        {
            clientAttr();
            HttpResponseMessage response = await client.DeleteAsync("api/teams/DeleteFromTeam?user=" + user);
            return response;
        }
        public async Task<HttpResponseMessage> HasItems(string user)
        {
            clientAttr();
            HttpResponseMessage response = await client.GetAsync("api/items/Hasitems?user=" + user);
            return response;
        }
    }
}
 