using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VoiceOver.Utils;
using VoiceOver.Utils.Transcribe;

namespace VoiceOver.Controllers
{
    public class VoiceController : Controller
    {
        private readonly S3UploadUtil s3UploadUtil;

        private readonly ITranscribe transcribe;
        public VoiceController()
        {
            this.s3UploadUtil = new S3UploadUtil();
            this.transcribe = new Transcribe();
        }
        

        [HttpPost("/voice"), DisableRequestSizeLimit]
        public async Task<IActionResult> Voice()
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
                    await s3UploadUtil.uploadToS3(fullPath);
                    var res = await transcribe.startTranscription(fileName);
                    return Ok(res);
                }
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
