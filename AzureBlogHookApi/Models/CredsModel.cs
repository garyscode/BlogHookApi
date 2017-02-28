using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBlogHookApi.Models
{
    public class CredsModel
    {
        public string app_consumer_key { get; set; }
        public string app_consumer_secret { get; set; }
        public string access_token { get; set; }
        public string access_token_secret { get; set; }
    }
}