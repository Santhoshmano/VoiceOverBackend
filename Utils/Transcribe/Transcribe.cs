using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using Newtonsoft.Json;

namespace VoiceOver.Utils.Transcribe
{
    public class Transcript
    {
        public string transcript;
    }

    public class TranscriptionResponseResults {
        public List<Transcript> transcripts;
    }


    public class TranscriptionResponse
    {
        public TranscriptionResponseResults results;
    }


    public class Transcribe : ITranscribe
    {
        private string _bucketName = "demoappuploadbucket";

        public Transcribe() 
        {
        }

        public async Task<string> startTranscription(string fileName)
        {

            using (var transcribeClient = new AmazonTranscribeServiceClient(Amazon.RegionEndpoint.USEast1))
            {
                var media = new Media()
                {
                    MediaFileUri = string.Format("s3://{0}/{1}", _bucketName, fileName)
                };

                var transcriptionJobRequest = new StartTranscriptionJobRequest()
                {
                    LanguageCode = "en-us",
                    Media = media,
                    MediaFormat = MediaFormat.Mp3,
                    TranscriptionJobName = "demo-" + Guid.NewGuid().ToString(),
                    OutputBucketName = _bucketName,
                    OutputKey = "output/" + fileName.Split(".")[0] + ".json"
                    
                };

                var transcriptionJobResponse = await transcribeClient.StartTranscriptionJobAsync(transcriptionJobRequest);

                if (transcriptionJobResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    using (var s3Client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
                    {
                        var response = await s3Client.GetObjectAsync(new GetObjectRequest
                        {
                            BucketName = _bucketName,
                            Key = "output/" + fileName.Split(".")[0] + ".json"
                        });
                        using (var reader = new StreamReader(response.ResponseStream))
                        {
                            string resp =  await reader.ReadToEndAsync();
                            Console.WriteLine(resp);
                            var respJSON = JsonConvert.DeserializeObject<TranscriptionResponse>(resp);
                            return respJSON.results.transcripts[0].transcript;               
                        }
                    }
                }
                return "error";
            }
        }
    }
}
