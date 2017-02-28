using AzureBlogHookApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Tweetinvi;

namespace BlogHookApi.Controllers
{
    public class BlogApiController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "blogValue1", "blogValue2" };
        }

        public string Post(HttpRequestMessage request)
        {
            string credsFile;
            string credsText;
            CredsModel creds = new CredsModel();

            try
            {
                credsFile = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + @"\Creds.json";
                credsText = File.ReadAllText(credsFile);

                creds = JsonConvert.DeserializeObject<CredsModel>(credsText);
            }
            catch {
                creds.app_consumer_key = Environment.GetEnvironmentVariable("APPSETTING_Twitter_app_consumer_key");
                creds.app_consumer_secret = Environment.GetEnvironmentVariable("APPSETTING_Twitter_app_consumer_secret");
                creds.access_token = Environment.GetEnvironmentVariable("APPSETTING_Twitter_access_token");
                creds.access_token_secret  = Environment.GetEnvironmentVariable("APPSETTING_Twitter_access_token_secret");
            }
            Auth.SetUserCredentials(creds.app_consumer_key, creds.app_consumer_secret, creds.access_token, creds.access_token_secret);
            var user = Tweetinvi.User.GetAuthenticatedUser();

            var randomTweet = new Random().Next();
            //var response = user.PublishTweet("This is random tweet #" + randomTweet.ToString());
            var content = request.Content.ReadAsStringAsync().Result;
            dynamic jObj = (JObject)JsonConvert.DeserializeObject(content);
            var gpm = JsonConvert.DeserializeObject<GitPushModel>(content);

            var postTitleLong = gpm.commits.FirstOrDefault().added.FirstOrDefault().ToString();
            var postTitle = postTitleLong.IndexOf("-");

            return postTitle.ToString();
        }
    }
}
