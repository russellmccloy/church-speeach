using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NAudio.Wave;
using System.IO;
using System.Threading.Tasks;

namespace ChurchSpeech.Controllers
{
    public class ConvertFileController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile mp3File)
        {
            if (mp3File != null && mp3File.Length > 0)
            {
                // Generate a unique filename for the WAV file
                var wavFileName = Path.GetFileNameWithoutExtension(mp3File.FileName) + ".wav";
                var relativePath = Path.Combine("uploads", wavFileName);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

                // Ensure the uploads directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                using (var mp3Stream = mp3File.OpenReadStream())
                using (var mp3Reader = new Mp3FileReader(mp3Stream))
                {
                    var outputFormat = new WaveFormat(22050, 16, 1); // Sample rate 22.05 kHz, 16-bit depth, mono

                    // Convert MP3 to WAV
                    using (var resampler = new MediaFoundationResampler(mp3Reader, outputFormat))
                    using (var wavFileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        WaveFileWriter.WriteWavFileToStream(wavFileStream, resampler);
                    }
                }

                // Pass the file path to the view
                ViewData["Message"] = $"File uploaded and converted to WAV: {wavFileName}";
                ViewData["FilePath"] = $"/{relativePath}";
            }
            else
            {
                ViewData["Message"] = "No file selected or file is empty.";
            }

            return View("Index");
        }
    }
}
