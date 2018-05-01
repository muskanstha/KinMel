using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace KinMel.Services
{
    public class BlobStorageHelper
    {
        //private static CloudStorageAccount storageAccount = new CloudStorageAccount(
        //    new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
        //        "kinmelstorage",
        //        "oP0l/YwDAH3A3vK7A91MunMomfX574OuqvHWc5KMevPmIOgHSVMxQqlp5RmJaTXTnvBhgfD6NPnFO9fzOxzSXw=="), true);

        //// Create a blob client.
        //private static CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //// Get a reference to a container named "mycontainer."
        //private static CloudBlobContainer container = blobClient.GetContainerReference("kinmel");


        public static async Task<string> UploadBlobs(string slug, List<IFormFile> imageFiles)
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(
              new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                  "kinmelstorage",
                  "oP0l/YwDAH3A3vK7A91MunMomfX574OuqvHWc5KMevPmIOgHSVMxQqlp5RmJaTXTnvBhgfD6NPnFO9fzOxzSXw=="), true);

            // Create a blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get a reference to a container named "mycontainer."
            CloudBlobContainer container = blobClient.GetContainerReference("kinmel");


            //var filePath = Path.GetTempFileName();

            //var filePath = Path.GetTempPath();
            //var filename = Path.GetRandomFileName();


            foreach (IFormFile imageFile in imageFiles)

            {
                string[] imageTypes = imageFile.ContentType.Split('/');

                // Get a reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference("images/classifiedads/" + slug + "/" + slug + "-" + imageFiles.IndexOf(imageFile) + "." + imageTypes[1]);
                //CloudBlockBlob blockBlob = container.GetBlockBlobReference("images/classifiedads/" + slug + "/" + imageFile.FileName);

                // Create or overwrite the "myblob" blob with the contents of a local file
                // named "myfile".


                //await blockBlob.UploadFromStreamAsync(imageFile.OpenReadStream());

                using (var fileStream = imageFile.OpenReadStream())
                {
                    blockBlob.Properties.ContentType = imageFile.ContentType;

                    await blockBlob.UploadFromStreamAsync(fileStream);
                }


            }

            return "Ok";

        }

        public static async Task<string> UploadBlob(string slug, IFormFile imageFile)
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                    "kinmelstorage",
                    "oP0l/YwDAH3A3vK7A91MunMomfX574OuqvHWc5KMevPmIOgHSVMxQqlp5RmJaTXTnvBhgfD6NPnFO9fzOxzSXw=="), true);

            // Create a blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get a reference to a container named "mycontainer."
            CloudBlobContainer container = blobClient.GetContainerReference("kinmel");


                string[] imageTypes = imageFile.ContentType.Split('/');

                // Get a reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference("images/classifiedads/" + slug + "/" + slug + "-main." + imageTypes[1]);
                //CloudBlockBlob blockBlob = container.GetBlockBlobReference("images/classifiedads/" + slug + "/" + imageFile.FileName);

                // Create or overwrite the "myblob" blob with the contents of a local file
                // named "myfile".


                //await blockBlob.UploadFromStreamAsync(imageFile.OpenReadStream());

                using (var fileStream = imageFile.OpenReadStream())
                {
                    blockBlob.Properties.ContentType = imageFile.ContentType;

                    await blockBlob.UploadFromStreamAsync(fileStream);
                }


            return "Ok";

        }

        //public static async void DownloadBlob()
        //{
        //    // Get a reference to a blob named "photo1.jpg".
        //    CloudBlockBlob blockBlob = container.GetBlockBlobReference("photo1.jpg");

        //    // Save the blob contents to a file named "myfile".
        //    using (var fileStream = System.IO.File.OpenWrite(@"path\myfile"))
        //    {
        //        await blockBlob.DownloadToStreamAsync(fileStream);
        //    }
        //}

        public static async Task<string> ListBlobsFolder(string slug)
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                    "kinmelstorage",
                    "oP0l/YwDAH3A3vK7A91MunMomfX574OuqvHWc5KMevPmIOgHSVMxQqlp5RmJaTXTnvBhgfD6NPnFO9fzOxzSXw=="), true);

            // Create a blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get a reference to a container named "mycontainer."
            CloudBlobContainer container = blobClient.GetContainerReference("kinmel");


            List<string> uris = new List<string>();

            BlobContinuationToken blobContinuationToken = null;
            do
            {
                //var results = await container.ListBlobsSegmentedAsync("1-test", blobContinuationToken);
                var directory = container.GetDirectoryReference("images/classifiedads/" + slug);
                var results = await directory.ListBlobsSegmentedAsync(blobContinuationToken);

                // Get the value of the continuation token returned by the listing call.
                blobContinuationToken = results.ContinuationToken;

                foreach (IListBlobItem item in results.Results)
                {
                    uris.Add(item.Uri.ToString());

                    //Console.WriteLine(item.Uri);
                }

                blobContinuationToken = results.ContinuationToken;
            } while (blobContinuationToken != null); // Loop while the continuation token is not null. 

            string uriJson = JsonConvert.SerializeObject(uris);
            //var fromJson = JsonConvert.DeserializeObject(uriJson);

            return uriJson;
        }

    }
}
