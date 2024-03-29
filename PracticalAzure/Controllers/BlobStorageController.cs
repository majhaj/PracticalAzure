﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;

namespace PracticalAzure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlobStorageController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            var containerName = "documents";

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = containerClient.GetBlobClient(file.FileName);

            var blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = file.ContentType;

            await blobClient.UploadAsync(file.OpenReadStream(), blobHttpHeaders);

            return Ok();
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery]string blobName)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=projectname1sagwc;AccountKey=/7dgIMi6aXaatqfXVaHurj9gVYa6jCcBbj0pC7laF3+y0IcWWKZ8r+8hhJ7uTzuXMbdbMJDC5M0b+AStdz3a/A==;EndpointSuffix=core.windows.net";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            var containerName = "documents";

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = containerClient.GetBlobClient(blobName );

            var downloadResponse = await blobClient.DownloadContentAsync();
            var content = downloadResponse.Value.Content.ToStream();
            var contentType = blobClient.GetProperties().Value.ContentType;

            return File(content, contentType, fileDownloadName: blobName);
        }
    }
}
