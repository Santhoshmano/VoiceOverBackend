using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace VoiceOver.Utils
{
    public class S3UploadUtil: IS3UploadUtil
    {
        private static string _bucketName = "demoappuploadbucket";

        public S3UploadUtil()
        {
        }

        public async Task<bool> uploadToS3(string filename)
        {
            var objectName = Path.GetFileName(filename);

            using (var s3Client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
            {
                var putObjectRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = objectName,
                    ContentType = "audio/mpeg",
                    FilePath = filename
                };

                var response = await s3Client.PutObjectAsync(putObjectRequest);

                return response.HttpStatusCode == System.Net.HttpStatusCode.Accepted;
            }
        }
    }
}
