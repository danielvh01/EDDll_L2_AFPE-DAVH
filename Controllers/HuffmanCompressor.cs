using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStructures;
using EDDll_L1_AFPE_DAVH.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Newtonsoft.Json;//Libreria usada para generar JSON en un formato especifico.

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EDDll_L2_AFPE_DAVH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HuffmanCompressor : ControllerBase
    {
        private readonly string filePath;
        public HuffmanCompressor(string filePath)
        {
            this.filePath = filePath;
        }
        public static IWebHostEnvironment _environment;
        public HuffmanCompressor(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public class FileUPloadAPI
        {
            public IFormFile FILE { get; set; }
        }

        [HttpPost("compress/{name}")]
        [ActionName("Post")]
        public  FileResult Post(string name, [FromForm] FileUPloadAPI? objFile)
        {            
            try
            {
                if (objFile.FILE != null)
                {
                    if (objFile.FILE.Length > 0)
                    {
                        if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                        {
                            Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                        }
                        using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + objFile.FILE.FileName))
                        {
                            objFile.FILE.CopyTo(fileStream);
                            fileStream.Flush();
                        }

                        StreamReader reader = new StreamReader(_environment.WebRootPath + "\\Upload\\" + objFile.FILE.FileName);
                        string content = reader.ReadToEnd();

                        Huffman compress = new Huffman(); 
                        
                        MemoryStream memoryStream = new MemoryStream(compress.Compress(content));
                        StreamReader streamReader = new StreamReader(memoryStream);                        
                        StreamWriter file = new StreamWriter(name+".huff", false);
                        file.Write(streamReader.ReadToEnd());
                        file.Close();

                        return File(System.IO.File.ReadAllBytes(name + ".huff"), "application/octet-stream", name + ".huff");
                    }
                    else
                    {
                        return File(System.IO.File.ReadAllBytes("Error.txt"), "application/octet-stream", "Error.txt");

                    }
                }
                else
                {
                    return File(System.IO.File.ReadAllBytes("Error.txt"), "application/octet-stream", "Error.txt");
                }
            }
            catch
            {
                return File(System.IO.File.ReadAllBytes("Error.txt"), "application/octet-stream", "Error.txt");
            }
            
        }

        // GET: api/<HuffmanCompressor>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<HuffmanCompressor>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<HuffmanCompressor>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<HuffmanCompressor>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HuffmanCompressor>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
