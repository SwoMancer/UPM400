using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Threading;

using Logging.LogType;
using Logging.Response;

namespace Logging
{
    public static class LoggingAPI
    {

        //private const string APIS_SERVER = "https://192.168.1.149/API/";
        private const string APIS_SERVER = "https://wrede.blue/api/";
        private const string API = "Logging/api/csharp/set";

        public static Answer Add(CTypeErrorLog log)
        {
            Answer answer = new Answer();

            answer = RestCall(log);

            return answer;
        }
        public static void AddF(CTypeErrorLog log)
        {
            Action<object> logAction = (object obj) => RestCall(log);
            Task logTask = new Task(logAction, "logging in FoodAPI: " + DateTime.Now.ToString());
            logTask.Start();

        }
        private static Answer RestCall(CTypeErrorLog log) => RestCallAsync(log).GetAwaiter().GetResult();
        private static async Task<Answer> RestCallAsync(CTypeErrorLog log)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(APIS_SERVER + API);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string text = log.ToJSONString();

                HttpContent content = new StringContent(log.ToJSONString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("", content);

                var responseString = await response.Content.ReadAsStringAsync();

                return JsonToAnswer(responseString);
            }
            catch (Exception ex)
            {
                return Answer.Error("[Log: RestCallAsync()] " + ex.ToString());
            }
        }
        private static Answer JsonToAnswer(string json)
        {
            //"isSuccess":true
            //"json":null
            json = json.Trim(new Char[] { '{', '}' });
            string[] values = json.Split(',');

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Substring(values[i].IndexOf(":") + 1);
            }

            Answer answer = new Answer();

            answer.IsASuccess = bool.Parse(values[0]);
            answer.Json = values[1];

            if (values[1] == "null")
            {
                answer.Json = null;
            }

            return answer;
        }
    }

}
