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
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Quantization;
using SixLabors.ImageSharp.Processing.Text;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.Shapes;
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
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference("kinmel");
        }
        public async Task<string> UploadBlobs(string slug, List<IFormFile> imageFiles)
        {

            foreach (IFormFile imageFile in imageFiles)

            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference("images/classifiedads/" + slug + "/" + slug + "-" + imageFiles.IndexOf(imageFile) + ".jpg");

                using (var fileStream = imageFile.OpenReadStream())
                {
                    var image = Image.Load(fileStream);

                    var memoryStream = new MemoryStream();
                    image.Mutate(i =>
                    {
                        if (image.Height > image.Width && image.Height > 1080)
                        {
                            i.Resize(0, 1080);
                        }
                        else if (image.Width > image.Height && image.Width > 1920)
                        {
                            i.Resize(1920, 0);
                        }
                        image.SaveAsJpeg(memoryStream, new JpegEncoder()
                        {
                            Quality = 90
                        });
                    });

                    blockBlob.Properties.ContentType = "image/jpeg";
                    memoryStream.Position = 0;
                    await blockBlob.UploadFromStreamAsync(memoryStream);
                }
            }
            return await ListBlobsFolder(slug);
        }

        public async Task<string> UploadMainBlob(string slug, IFormFile imageFile)
        {


            CloudBlockBlob blockBlob = container.GetBlockBlobReference("images/classifiedads/" + slug + "/" + slug + "-main.jpg");

            using (var fileStream = imageFile.OpenReadStream())
            {
                var image = Image.Load(fileStream);

                var memoryStream = new MemoryStream();
                image.Mutate(i =>
                {
                    if (image.Height > image.Width && image.Height > 1080)
                    {
                        i.Resize(0, 1080);
                    }
                    else if (image.Width > image.Height && image.Width > 1920)
                    {
                        i.Resize(1920, 0);
                    }
                    image.SaveAsJpeg(memoryStream, new JpegEncoder()
                    {
                        Quality = 90
                    });
                });

                blockBlob.Properties.ContentType = "image/jpeg";
                memoryStream.Position = 0;
                await blockBlob.UploadFromStreamAsync(memoryStream);
            }


            return blockBlob.Uri.ToString();

        }
        public async Task<string> UploadProfilePictureBlob(string username, IFormFile imageFile)
        {

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("images/profiles/" + username + "/profile-picture.jpg");

            using (var fileStream = imageFile.OpenReadStream())
            {
                var image = Image.Load(fileStream);

                var memoryStream = new MemoryStream();
                image.Mutate(i =>
                {
                    if (image.Height > image.Width && image.Height > 720)
                    {
                        i.Resize(0, 720);
                    }
                    else if (image.Width > image.Height && image.Width > 1366)
                    {
                        i.Resize(1366, 0);
                    }
                    image.SaveAsJpeg(memoryStream, new JpegEncoder()
                    {
                        Quality = 90
                    });
                });

                blockBlob.Properties.ContentType = "image/jpeg";
                memoryStream.Position = 0;
                await blockBlob.UploadFromStreamAsync(memoryStream);
            }


            return blockBlob.Uri.ToString();

        }


        public async Task<string> ListBlobsFolder(string slug)
        {

            List<string> uris = new List<string>();

            BlobContinuationToken blobContinuationToken = null;
            do
            {
                var directory = container.GetDirectoryReference("images/classifiedads/" + slug);
                var results = await directory.ListBlobsSegmentedAsync(blobContinuationToken);

                blobContinuationToken = results.ContinuationToken;

                foreach (IListBlobItem item in results.Results)
                {
                    uris.Add(item.Uri.ToString());
                }

                blobContinuationToken = results.ContinuationToken;
            } while (blobContinuationToken != null);

            string uriJson = JsonConvert.SerializeObject(uris);

            return uriJson;
        }

    }
}
