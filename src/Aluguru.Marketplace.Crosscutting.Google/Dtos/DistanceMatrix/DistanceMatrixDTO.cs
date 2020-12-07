using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Google.Dtos.DistanceMatrix
{
    public partial class DistanceMatrixDTO
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("destination_addresses")]
        public string[] DestinationAddresses { get; set; }

        [JsonPropertyName("origin_addresses")]
        public string[] OriginAddresses { get; set; }
        [JsonPropertyName("rows")]
        public RowDTO[] Rows { get; set; }
    }
}
