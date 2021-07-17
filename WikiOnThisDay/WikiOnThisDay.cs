using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using LinqToTwitter.OAuth;
using LinqToTwitter;
using WikiOnThisDay.Classes;
using System.Linq;

namespace WikiOnThisDay
{
    public class WikiOnThisDay
    {

        private readonly HttpClient _httpClient;
        public WikiOnThisDay(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient();
        }

        [Function(nameof(WikiOnThisDay))]
        public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo myTimer, FunctionContext context)
        {

            var response = await _httpClient.GetAsync($"https://byabbe.se/on-this-day/{DateTime.Now.Month}/{DateTime.Now.Day}/events.json");

            var body = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<Root>(body);

            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = Environment.GetEnvironmentVariable("ConsumerKey"),
                    ConsumerSecret = Environment.GetEnvironmentVariable("ConsumerSecret"),
                    AccessToken = Environment.GetEnvironmentVariable("AccessToken"),
                    AccessTokenSecret = Environment.GetEnvironmentVariable("AccessTokenSecret")
                }
            };

            var tContext = new TwitterContext(auth);

            var hour = DateTime.Now.Hour;

            Event wEvent;
            if (data.events.Count() > 23)
            {
                var skips = data.events.Count() - 23;
                wEvent = data.events.Skip(skips).ElementAtOrDefault(hour);
            }
            else
                wEvent = data.events.ElementAtOrDefault(hour);

            if (wEvent != null)
            {
                await tContext.TweetAsync(wEvent.year + " - " + wEvent.description);

            }



        }



    }

}
