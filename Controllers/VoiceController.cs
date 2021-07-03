using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace VoiceOver.Controllers
{
    public class VoiceController : Controller
    {
        [HttpPost("/voice"), DisableRequestSizeLimit]
        public IActionResult Voice()
        {
            
            var file = Request.Form.Files[0];
            //var formCollection = await Request.ReadFormAsync();
            //var file = formCollection.Files.First();
            Console.WriteLine(Request.Form);
            var folderName = Path.Combine("Resources", "Voices");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            Console.WriteLine(file);
            Console.WriteLine(pathToSave);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
          
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return Ok();
            }
            else
            {
                return BadRequest();
            }
            
        }

        [HttpGet("/voice")]
        public IActionResult GetVoice()
        {
            return Ok("Hello");
        }
    }
}
