using System;
using System.Threading.Tasks;

namespace VoiceOver.Utils.Transcribe
{
    public interface ITranscribe
    {
        Task<string> startTranscription(string fileName);
        
    }
}
