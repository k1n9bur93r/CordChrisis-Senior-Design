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
        private string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=chordcrisis;AccountKey=ZNKvIn5Bx2PFXPrDd1KVL1tGth5qS9gikjZ8Gd5qR9iDfHzQI5Wc3v35pEL5f6PX2ZHVpEUPJY4oI8EAI8u0PQ==;EndpointSuffix=core.windows.net";

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
