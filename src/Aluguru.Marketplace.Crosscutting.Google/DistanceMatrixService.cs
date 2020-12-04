using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Aluguru.Marketplace.Crosscutting.Google.Dtos;
using System.Linq;

namespace Aluguru.Marketplace.Crosscutting.Google
{
    public interface IDistanceMatrixService
    {
        public Task<double> Distance(string fromAddress, string toAddress);
    }

    public class DistanceMatrixService : IDistanceMatrixService
    {
        private readonly GoogleSettings _settings;

        public DistanceMatrixService(IOptions<GoogleSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<double> Distance(string fromAddress, string toAddress)
        {
            using var client = new HttpClient();

            var builder = new UriBuilder(_settings.DistanceMatrixUri);
            
            var query = HttpUtility.ParseQueryString(builder.Query);

            query["key"] = _settings.ApiKey;
            query["origins"] = fromAddress;
            query["destinations"] = toAddress;

            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to calculate distance on Google Distance Matrix API");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var distanceMatrix = JsonSerializer.Deserialize<DistanceMatrixDTO>(responseContent);

            return distanceMatrix.Rows.First().Elements.First().Distance.Value;
        }
    }
}
