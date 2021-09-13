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
    [Route("api/[controller]")]
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
        public  FileResult Post(string name, [FromForm] FileUPloadAPI? objFile)
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

                        using FileStream fs = System.IO.File.OpenRead(_environment.WebRootPath + "\\Upload\\" + uniqueFileName);
                        byte[] buf = new byte[1024];
                        int c;
                        string content = "";
                        while ((c = fs.Read(buf, 0, buf.Length)) > 0)
                        {
                            content += Encoding.UTF8.GetString(buf, 0, c);
                        }

                        Huffman compressor = new Huffman();

                        MemoryStream memoryStream = new MemoryStream(compressor.Compress(content));
                        StreamReader streamReader = new StreamReader(memoryStream);
                        StreamWriter file = new StreamWriter(name + ".huff", false);
                        file.Write(streamReader.ReadToEnd());
                        file.Close();
                        //FileStream fs2 = new FileStream(name + ".huff", FileMode.Create);
                        //fs2.Write(memoryStream.ToArray());
                        //fs.Close();
                        CompressModel compressObj = new CompressModel
                        {
                            originalFileName = objFile.FILE.FileName,
                            CompressedFileName_Route = name + ".huff" + "-->" + "ruta de archivo comprimido",
                            rateOfCompression = 0.0,
                            compressionFactor = 0.0,
                            reductionPercentage = 0.0 + "%",
                        };
                        Singleton.Instance.compressions.InsertAtStart(compressObj);
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

        [HttpPost("decompress")]
        public FileResult Post([FromForm] FileUPloadAPI? objFile)
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
                        using FileStream fs = System.IO.File.OpenRead(_environment.WebRootPath + "\\Upload\\" + uniqueFileName);
                        byte[] buf = new byte[1024];
                        int c;
                        string content = "";
                        while ((c = fs.Read(buf, 0, buf.Length)) > 0)
                        {
                            content += Encoding.UTF8.GetString(buf, 0, c);
                        }


                        Huffman decompress = new Huffman();
                        //CALL METHOD IN ORDER TO DECOMPRESS
                        MemoryStream memoryStream = new MemoryStream(decompress.Compress(content));//CORREGIR A DECOMPRESS!!!!!!!!!!!!!!!!
                        StreamReader streamReader = new StreamReader(memoryStream);
                        StreamWriter file = new StreamWriter(objFile.FILE.FileName + ".txt", false);
                        file.Write(streamReader.ReadToEnd());
                        file.Close();
                        return File(System.IO.File.ReadAllBytes(objFile.FILE.FileName + ".txt"), "application/octet-stream", objFile.FILE.FileName + ".txt");
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


        // GET api/<HuffmanCompressor>
        [HttpGet]
        public FileResult Get()
        {
            StreamWriter file = new StreamWriter("COMPRESSIONS" + ".JSON", false);
            file.Write(Singleton.Instance.getCompressions());
            file.Close();
            return File(System.IO.File.ReadAllBytes("COMPRESSIONS" + ".JSON"), "application/octet-stream", "COMPRESSIONS.JSON");
        }       
    }
}
