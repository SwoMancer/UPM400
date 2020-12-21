using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FlickerAPI
{
    public static class API
    {
        private const string API_KEY = "08f3783f96193a84c4cb52e595c7fdcb";
        private const string API_ADDRESS = "https://www.flickr.com/services/rest/";

        //https://www.flickr.com/services/rest/?method=flickr.photos.search&api_key=08f3783f96193a84c4cb52e595c7fdcb&tags=food&safe_search=pizza&format=json&nojsoncallback=1
        //https://farm{farm-id}.staticflickr.com/{server-id}/{id}_{secret}.jpg

        public static async Task<Answer> GetAll(string foodItemName)
        {
            string url = "https://www.flickr.com/services/rest/?method=flickr.photos.search&api_key=" + API_KEY + "&tags=food&safe_search=" + foodItemName + "&format=json&nojsoncallback=1";
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("", content);

                var responseString = await response.Content.ReadAsStringAsync();

                return Answer.Complete(responseString);
            }
            catch (Exception ex)
            {
                return Answer.Error("[Log: RestCallAsync()] " + ex.ToString());
            }
        }
    }
}
