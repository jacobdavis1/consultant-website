using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fileshare_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileShareController : ControllerBase
    {
        // GET: api/<FileShareController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FileShareController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FileShareController>
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Post()
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
                ShareDirectoryClient directory = share.GetDirectoryClient("testdir");
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

        // PUT api/<FileShareController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FileShareController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
