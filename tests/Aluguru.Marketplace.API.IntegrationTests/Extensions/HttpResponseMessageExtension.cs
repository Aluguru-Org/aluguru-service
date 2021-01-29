using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.API.IntegrationTests.Extensions
{
    public static class HttpResponseMessageExtension
    {
        public static T Deserialize<T>(this HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
