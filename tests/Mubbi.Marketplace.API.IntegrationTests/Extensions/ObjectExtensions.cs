using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Mubbi.Marketplace.API.IntegrationTests.Extensions
{
    public static class ObjectExtensions
    {
        public static StringContent ToStringContent(this object data, string mediaType = "application/json")
        {
            return new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, mediaType);
        }
    }
}
