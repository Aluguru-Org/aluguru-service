﻿using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Crosscutting.AzureStorage
{
    public interface IAzureStorageGateway
    {
        public Task<string> UploadFile(string container, string fileName, IFormFile file);
    }

    public class AzureStorageGateway : IAzureStorageGateway
    {
        private readonly AzureStorageSettings _settings;
        public AzureStorageGateway(IOptions<AzureStorageSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<string> UploadFile(string container, string fileName, IFormFile file)
        {
            try
            {
                var containerClient = new BlobContainerClient(_settings.ConnectionString, container);

                BlobClient blob = containerClient.GetBlobClient(fileName);                

                using (var stream = file.OpenReadStream())
                {
                    var response = await blob.UploadAsync(stream);                    
                }

                return blob.Uri.ToString();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
