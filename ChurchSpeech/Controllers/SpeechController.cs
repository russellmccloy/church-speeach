using Microsoft.AspNetCore.Mvc;

namespace ChurchSpeech.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.CognitiveServices.Speech;
    using Microsoft.CognitiveServices.Speech.Audio;
    using System.IO;
    using System.Threading.Tasks;

    namespace YourNamespace.Controllers
    {
        public class SpeechController : Controller
        {
            private readonly string subscriptionKey = "dbdbf103df264c8fa2aeecabd9360553";
            private readonly string region = "australiaeast";

            public IActionResult Index()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> TranscribeAudio(IFormFile audioFile)
            {
                if (audioFile == null || audioFile.Length == 0)
                {
                    ViewBag.Error = "Please select a valid audio file.";
                    return View("Index");
                }

                var resultText = await TranscribeAudioAsync(audioFile);
                ViewBag.Transcription = resultText;
                return View("Index");
            }

            private async Task<string> TranscribeAudioAsync(IFormFile audioFile)
            {
                string transcription = string.Empty;

                var config = SpeechConfig.FromSubscription(subscriptionKey, region);
                var audioConfig = AudioConfig.FromWavFileInput(audioFile.FileName);

                using (var recognizer = new SpeechRecognizer(config, audioConfig))
                {
                    var result = await recognizer.RecognizeOnceAsync();
                    if (result.Reason == ResultReason.RecognizedSpeech)
                    {
                        transcription = result.Text;
                    }
                    else if (result.Reason == ResultReason.NoMatch)
                    {
                        transcription = "No speech could be recognized.";
                    }
                    else if (result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = CancellationDetails.FromResult(result);
                        transcription = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
                    }
                }

                return transcription;
            }
        }
    }

}
