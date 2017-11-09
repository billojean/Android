using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App
{
    class ClientRequests
    {
        public const string Baseurl = "http://192.168.1.2:80/";
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


            var response = await client.PostAsync("api/teams/Postteam", content);

            return response;

        }

        public async Task<List<Team>> GetTeams()
        {

            var response = await client.GetAsync("api/teams/Getteams");
            string responseBody = await response.Content.ReadAsStringAsync();
            var jsn = JsonConvert.DeserializeObject<List<Team>>(responseBody);
            return jsn;
        }
        public async Task<List<Vehicles>> GetVehicles()
        {

            var response = await client.GetAsync("api/Vehicles/Getvehicles");
            string responseBody = await response.Content.ReadAsStringAsync();

            var vehicles = JsonConvert.DeserializeObject<List<Vehicles>>(responseBody);
            return vehicles;
        }
        public async Task<List<Laptops>> GetLaptops()
        {

            var response = await client.GetAsync("api/Laptops/Getlaptops");
            string responseBody = await response.Content.ReadAsStringAsync();

            var laptops = JsonConvert.DeserializeObject<List<Laptops>>(responseBody);
            return laptops;
        }
        public async Task<List<SparePart>> GetSpareParts()
        {

            var response = await client.GetAsync("api/SpareParts/Getspareparts");
            string responseBody = await response.Content.ReadAsStringAsync();

            var spareparts = JsonConvert.DeserializeObject<List<SparePart>>(responseBody);
            return spareparts;
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
            var response = await client.PostAsync("api/items/Postitem", content);
            return response;
        }

        public async Task<List<Items>> GetItems(string user)
        {

            var response = await client.GetAsync("api/items/Getitems?user=" + (user));
            string responseBody = await response.Content.ReadAsStringAsync();
            var jsn = JsonConvert.DeserializeObject<List<Items>>(responseBody);
            return jsn;
        }

        public async Task<HttpResponseMessage> ReturnItem(string id)
        {

            var response = await client.DeleteAsync("api/items/Returnitem?id=" + id);
            return response;
        }

        public async Task<User> GetUserByEmail(string email)    
        {
 
            var response = await client.GetAsync("api/user/Getuser?Email=" + (email));
            string responseBody = await response.Content.ReadAsStringAsync();
            var jsn = JsonConvert.DeserializeObject<User>(responseBody);
            return jsn;
        }

        public async Task<HttpResponseMessage> UserLogIn(Dictionary<string, string> values)
        {

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("api/user/Login", content);
            return response;
        }

        public async Task<IEnumerable<Locations>> GetMembersLocation(string user)
        {

            var response = await client.GetAsync("api/locations/Getlocations?user=" + (user));
            string responseBody = await response.Content.ReadAsStringAsync();
            var jsn = JsonConvert.DeserializeObject<IEnumerable<Locations>>(responseBody);
            return jsn;
        }

        public async Task<HttpResponseMessage>PostLocation(string json)
        {

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/locations/Postlocation", content);
            return response;
        }
        public async Task<string> GetUserTeam(string user)
        {

            var response = await client.GetAsync("api/t_members/GetUserTeam?user=" + user);
            string responseString = await response.Content.ReadAsStringAsync();
            var jsn = JsonConvert.DeserializeObject<dynamic>(responseString);
            if (response.IsSuccessStatusCode)

            {
                return (string)jsn["T_title"];
            }
            else
            {

                return null;
            }
        }
        public async Task<List<TeamMembers>> GetTeamMembers(string user)
        {

            var response = await client.GetAsync("api/user/GetMembers?user=" + (user));

            string responseBody = await response.Content.ReadAsStringAsync();

            var tmembers = JsonConvert.DeserializeObject<List<TeamMembers>>(responseBody);  

            return tmembers;
        }
        public async Task<HttpResponseMessage> PostLocalLocation(string json)
        {

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/location_history/PostLocalLocation", content);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteUserFromTeam(string user)
        {

            var response = await client.DeleteAsync("api/teams/DeleteFromTeam?user=" + user);
            return response;
        }
        public async Task<HttpResponseMessage> HasItems(string user)
        {

            var response = await client.GetAsync("api/items/Hasitems?user=" + user);
            return response;
        }

    }
}
 