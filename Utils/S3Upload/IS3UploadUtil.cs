using System;
using System.Threading.Tasks;

namespace VoiceOver.Utils
{
    public interface IS3UploadUtil
    {
        Task<bool> uploadToS3(String filename);
    }
}
