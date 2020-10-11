using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using fileshare_server.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fileshare_server.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class FileShareController : ControllerBase
    {
        [HttpGet("{userId}")]
        public IActionResult GetFileNames(string userId)
        {
            // Get a reference to a share and then create it
            ShareClient share = new ShareClient(Secret.AzureConnectionString, Secret.AzureFileShareName);
            if (!share.Exists())
                share.Create();

            // Get a reference to a directory and create it
            ShareDirectoryClient directory = share.GetDirectoryClient(Auth0Utils.GetUserFolder());
            if (!directory.Exists())
                directory.Create();

            var files = directory.GetFilesAndDirectories();

            return Ok(files.ToList());
        }

        [HttpGet("{userId}/{fileName}")]
        public IActionResult DownloadFile(string userId, string fileName)
        {
            // Get a reference to a share and then create it
            ShareClient share = new ShareClient(Secret.AzureConnectionString, Secret.AzureFileShareName);
            if (!share.Exists())
                share.Create();

            // Get a reference to a directory and create it
            ShareDirectoryClient directory = share.GetDirectoryClient(Auth0Utils.GetUserFolder());
            if (!directory.Exists())
                directory.Create();

            // Get a reference to a file and upload it
            ShareFileClient fileClient = directory.GetFileClient(fileName);

            if (fileClient.Exists())
            {
                //ShareFileDownloadInfo download = file.Download();
                return File(fileClient.OpenRead(), "application/octet-stream");
            } 
            else return NotFound();
        }

        // POST api/file
        [HttpPost("{userId}"), DisableRequestSizeLimit]
        public IActionResult Post(string userId)
        {
            try
            {
                IFormFileCollection files = Request.Form.Files;
                if (files.Count == 0)
                {
                    return BadRequest();
                }

                // Get a reference to a share and then create it
                ShareClient share = new ShareClient(Secret.AzureConnectionString, Secret.AzureFileShareName);
                if (!share.Exists())
                    share.Create();

                // Get a reference to a directory and create it
                ShareDirectoryClient directory = share.GetDirectoryClient(Auth0Utils.GetUserFolder());
                if (!directory.Exists())
                    directory.Create();

                // Get a reference to a file and upload it
                ShareFileClient fileClient;

                foreach (IFormFile file in files)
                {
                    fileClient = directory.GetFileClient(file.FileName);

                    using (Stream stream = file.OpenReadStream())
                    {
                        fileClient.Create(stream.Length);
                        fileClient.UploadRange(
                            new HttpRange(0, stream.Length),
                            stream);
                    }
                }

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // DELETE api/<FileShareController>/5
        [HttpDelete("{userId}/{fileName}")]
        public IActionResult DeleteFile(string userId, string fileName)
        {
            // Get a reference to a share and then create it
            ShareClient share = new ShareClient(Secret.AzureConnectionString, Secret.AzureFileShareName);
            if (!share.Exists())
                share.Create();

            // Get a reference to a directory and create it
            ShareDirectoryClient directory = share.GetDirectoryClient(Auth0Utils.GetUserFolder());
            if (!directory.Exists())
                directory.Create();

            // Get a reference to a file and upload it
            ShareFileClient fileClient = directory.GetFileClient(fileName);

            fileClient.DeleteIfExists();

            return Ok();
        }
    }
}
