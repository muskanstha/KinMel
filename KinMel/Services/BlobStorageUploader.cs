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
    public class BlobStorageUploader
    {
        private CloudStorageAccount storageAccount;

        // Create a blob client.
        private static CloudBlobClient blobClient;

        // Get a reference to a container named "mycontainer."
        private static CloudBlobContainer container;

        public BlobStorageUploader()
        {
            storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                    "kinmelstorage",
                    "oP0l/YwDAH3A3vK7A91MunMomfX574OuqvHWc5KMevPmIOgHSVMxQqlp5RmJaTXTnvBhgfD6NPnFO9fzOxzSXw=="), true);
            blobClient  = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference("kinmel");
        }
        public  async Task<string> UploadBlobs(string slug, List<IFormFile> imageFiles)
        {
            

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
            return await ListBlobsFolder(slug);
        }

        public  async Task<string> UploadMainBlob(string slug, IFormFile imageFile)
        {
            
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


            return blockBlob.Uri.ToString();

        }
        public  async Task<string> UploadProfilePictureBlob(string id, IFormFile imageFile)
        {
           

            string[] imageTypes = imageFile.ContentType.Split('/');

            // Get a reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("images/profiles/" + id + "/profile-picture." + imageTypes[1]);
            //CloudBlockBlob blockBlob = container.GetBlockBlobReference("images/classifiedads/" + slug + "/" + imageFile.FileName);

            // Create or overwrite the "myblob" blob with the contents of a local file
            // named "myfile".


            //await blockBlob.UploadFromStreamAsync(imageFile.OpenReadStream());

            using (var fileStream = imageFile.OpenReadStream())
            {
                blockBlob.Properties.ContentType = imageFile.ContentType;

                await blockBlob.UploadFromStreamAsync(fileStream);
            }


            return blockBlob.Uri.ToString();

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

        public  async Task<string> ListBlobsFolder(string slug)
        {
           
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
