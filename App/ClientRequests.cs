using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace App
{
    class ClientRequests:IDisposable
    {
        public const string Baseurl = "http://192.168.1.3:80/";
        static HttpClient client = new HttpClient();

        public ClientRequests()
        {
            client.BaseAddress = new Uri(Baseurl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(20);
        }


        public async Task<HttpResponseMessage> CreateTeam(Dictionary<string, string> values)
        {


            var content = new FormUrlEncodedContent(values);


            HttpResponseMessage response = await client.PostAsync("api/teams/Postteam", content);

            return response;

        }

        public async Task<HttpResponseMessage> GetTeams()
        {

            HttpResponseMessage response = await client.GetAsync("api/teams/Getteams");
            return response;
        }
        public async Task<HttpResponseMessage> GetVehicles()
        {

            HttpResponseMessage response = await client.GetAsync("api/Vehicles/Getvehicles");
            return response;
        }
        public async Task<HttpResponseMessage> GetLaptops()
        {

            HttpResponseMessage response = await client.GetAsync("api/Laptops/Getlaptops");
            return response;
        }
        public async Task<HttpResponseMessage> GetSpareParts()
        {

            HttpResponseMessage response = await client.GetAsync("api/SpareParts/Getspareparts");
            return response;
        }

        public async Task<HttpResponseMessage> PostUserInTeam(Dictionary<string, string> values)
        {

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("api/t_members/Post_member", content);
            return response;
        }

        public async Task<HttpResponseMessage> GetTeam(string pin)
        {

            var response = await client.GetAsync("api/teams/Getteam/" + pin);
            return response;
        }

        public async Task<HttpResponseMessage> PostItem(Dictionary<string, string> values)
        {

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync("api/items/Postitem", content);
            return response;
        }

        public async Task<HttpResponseMessage> GetItems(string user)
        {

            HttpResponseMessage response = await client.GetAsync("api/items/Getitems?user=" + (user));
            return response;
        }

        public async Task<HttpResponseMessage> ReturnItem(string id)
        {

            HttpResponseMessage response = await client.DeleteAsync("api/items/Returnitem?id=" + id);
            return response;
        }

        public async Task<HttpResponseMessage> GetUser(string email)
        {
 
            HttpResponseMessage response = await client.GetAsync("api/user/Getuser?Email=" + (email));
            return response;
        }

        public async Task<HttpResponseMessage> UserLogIn(Dictionary<string, string> values)
        {

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync("api/user/Login", content);
            return response;
        }

        public async Task<HttpResponseMessage> GetMembersLocation(string user)
        {

            HttpResponseMessage response = await client.GetAsync("api/locations/Getlocations?user=" + (user));
            return response;
        }

        public async Task<HttpResponseMessage>PostLocation(string json)
        {

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/locations/Postlocation", content);
            return response;
        }
        public async Task<HttpResponseMessage> GetUserTeam(string user)
        {
 
            HttpResponseMessage response = await client.GetAsync("api/t_members/GetUserTeam?user=" + user);
            return response;
        }
        public async Task<HttpResponseMessage> GetTeamMembers(string user)
        {

            HttpResponseMessage response = await client.GetAsync("api/user/GetMembers?user=" + (user));
            return response;
        }
        public async Task<HttpResponseMessage> PostLocalLocation(string json)
        {

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/location_history/PostLocalLocation", content);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteUserFromTeam(string user)
        {

            HttpResponseMessage response = await client.DeleteAsync("api/teams/DeleteFromTeam?user=" + user);
            return response;
        }
        public async Task<HttpResponseMessage> HasItems(string user)
        {

            HttpResponseMessage response = await client.GetAsync("api/items/Hasitems?user=" + user);
            return response;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (client != null) client.Dispose();
            }
        }
    }
}
 