using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Aluguru.Marketplace.Crosscutting.Iugu
{    
    public interface IIuguService
    {

    }

    public class IuguService : IIuguService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IuguSettings _settings;

        public IuguService(IHttpClientFactory httpClientFactory, IOptions<IuguSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }


    }
}
