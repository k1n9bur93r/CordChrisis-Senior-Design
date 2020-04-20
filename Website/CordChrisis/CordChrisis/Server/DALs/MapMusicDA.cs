using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.Shared.Models;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace CordChrisis.DAOs
{
    public class MapMusicDA
    {
        private string ConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION");
        public Stream GetMapMusic(string ID)
        {

            BlobServiceClient blobServiceClient = new BlobServiceClient(ConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("mapmusic");
            BlobClient blob= containerClient.GetBlobClient(ID + ".mp3");
            BlobDownloadInfo download = blob.Download();

            return download.Content;
        }

        public void UploadMapMusic(FileStream music, string ID)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(ConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("mapmusic");
            BlobClient blob = containerClient.GetBlobClient(ID+".mp3");
            blob.Upload(music, true);

        }

    }
}
