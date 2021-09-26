using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStructures;
using EDDll_L2_AFPE_DAVH.Models;
using EDDll_L2_AFPE_DAVH.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using Newtonsoft.Json;//Libreria usada para generar JSON en un formato especifico.

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EDDll_L2_AFPE_DAVH.Controllers
{
    [Route("api/")]
    [ApiController]
    public class compress : ControllerBase
    {      
        public static IWebHostEnvironment _environment;
        public compress(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public class FileUPloadAPI
        {
            public IFormFile FILE { get; set; }
        }        
        [HttpPost("compress/{name}")]        
        public  IActionResult Post(string name, [FromForm] FileUPloadAPI? objFile)
        {            
            try
            {
                if (objFile.FILE != null)
                {
                    if (objFile.FILE.Length > 0)
                    {
                        string uniqueFileName = objFile.FILE.FileName + "-" + Guid.NewGuid().ToString();
                        if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                        {
                            Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                        }

                        using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + uniqueFileName))
                        {
                            objFile.FILE.CopyTo(fileStream);
                            fileStream.Flush();
                        }

                        byte [] content = System.IO.File.ReadAllBytes(_environment.WebRootPath + "\\Upload\\" + uniqueFileName);

                        IHuffmanCompressor compressor = new Huffman();

                        byte[] textCompressed = compressor.Compress(Encoding.UTF8.GetString(content));


                        CompressModel compressObj = new CompressModel
                        {
                            originalFileName = objFile.FILE.FileName,
                            CompressedFileName_Route = name + ".huff" + "-->" + _environment.WebRootPath + "\\Upload\\",
                            rateOfCompression = Math.Round((Convert.ToDouble(textCompressed.Length) / Convert.ToDouble(content.Length)),2).ToString(),
                            compressionFactor = Math.Round((Convert.ToDouble(content.Length) / Convert.ToDouble(textCompressed.Length)),2).ToString(),
                            reductionPercentage = Math.Round((Convert.ToDouble(textCompressed.Length) / Convert.ToDouble(content.Length)) * 100, 2).ToString() + "%"
                        };

                        Singleton.Instance.compressions.InsertAtStart(compressObj);

                        return File(textCompressed, "application/text", name + ".huff");
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch
            {
                return StatusCode(500);
            }
            
        }

        [HttpPost("decompress")]
        public IActionResult Post([FromForm] FileUPloadAPI? objFile)
        {
            try
            {
                if (objFile.FILE != null)
                {
                    if (objFile.FILE.Length > 0)
                    {
                        string uniqueFileName = objFile.FILE.FileName + "-" + Guid.NewGuid().ToString();
                        if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                        {
                            Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                        }
                        using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + uniqueFileName))
                        {
                            objFile.FILE.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                        byte[] content = System.IO.File.ReadAllBytes(_environment.WebRootPath + "\\Upload\\" + uniqueFileName);

                        Huffman decompress = new Huffman();

                        string textDecompressed = decompress.Decompress(content);

                        string OriginalFileName = "";
                        foreach(var compression in Singleton.Instance.compressions)
                        {
                            string fileOriginalName = compression.CompressedFileName_Route.Substring(0, compression.CompressedFileName_Route.IndexOf("--"));
                            if (fileOriginalName == objFile.FILE.FileName)
                            {
                                OriginalFileName = compression.originalFileName;
                                break;
                            }
                        }
                        System.IO.File.WriteAllText(OriginalFileName, textDecompressed);
                        return File(System.IO.File.ReadAllBytes(OriginalFileName), "application/octet-stream", OriginalFileName);
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }


        // GET api/<HuffmanCompressor>

        [HttpGet("compressions")]
        public IActionResult Get()
        {
            try
            {
                StreamWriter file = new StreamWriter("COMPRESSIONS" + ".JSON", false);
                file.Write(Singleton.Instance.getCompressions());
                file.Close();
                return File(System.IO.File.ReadAllBytes("COMPRESSIONS" + ".JSON"), "application/octet-stream", "COMPRESSIONS.JSON");
            }
            catch
            {
                return StatusCode(500);
            }
        }       
    }
}
