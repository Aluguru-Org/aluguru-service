using Aluguru.Marketplace.Crosscutting.Google.Dtos.Geocode;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Aluguru.Marketplace.Crosscutting.Google
{
    public interface IGeocodeService
    {
        Task<GeocodeResponse> Geocode(string address, string filter = "BR");
    }

    public class GeocodeService : IGeocodeService
    {
        private readonly GoogleSettings _settings;

        public GeocodeService(IOptions<GoogleSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<GeocodeResponse> Geocode(string address, string filter = "BR")
        {
            using var client = new HttpClient();

            var builder = new UriBuilder(_settings.GeocodeUri);

            var query = HttpUtility.ParseQueryString(builder.Query);

            query["key"] = _settings.ApiKey;
            query["address"] = address;
            query["components"] = $"country:{filter}";

            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to geocode location on Google Geocode API");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var geocode = JsonSerializer.Deserialize<GeocodeDTO>(responseContent);

            if (geocode.Status == "ZERO_RESULTS")
            {
                return new GeocodeResponse(false);
            }

            var result = geocode.Results.First();

            var streetNumber = result.AddressComponents.Where(x => x.Types.Contains("street_number")).FirstOrDefault()?.LongName ?? "";
            var street = result.AddressComponents.Where(x => x.Types.Contains("route")).FirstOrDefault()?.LongName ?? "";
            var neighborhood = result.AddressComponents.Where(x => x.Types.Contains("sublocality") && x.Types.Contains("sublocality_level_1")).FirstOrDefault()?.LongName ?? "";
            var city = result.AddressComponents.Where(x => x.Types.Contains("administrative_area_level_2") && x.Types.Contains("political")).FirstOrDefault()?.LongName ?? "";
            var state = result.AddressComponents.Where(x => x.Types.Contains("administrative_area_level_1") && x.Types.Contains("political")).FirstOrDefault()?.LongName ?? "";
            var country = result.AddressComponents.Where(x => x.Types.Contains("country") && x.Types.Contains("political")).FirstOrDefault()?.LongName ?? "";
            var zipCode = result.AddressComponents.Where(x => x.Types.Contains("postal_code")).FirstOrDefault()?.LongName ?? "";

            return new GeocodeResponse(true, result.FormattedAddress, street, streetNumber, neighborhood, city, state, country, zipCode);
        }
    }
}
