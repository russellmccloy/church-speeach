﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using NAudio.Wave;

class Program 
{
    // This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION"
    static string speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
    static string speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");

    static void OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
    {
        switch (speechRecognitionResult.Reason)
        {
            case ResultReason.RecognizedSpeech:
                Console.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text}");
                break;
            case ResultReason.NoMatch:
                Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                break;
            case ResultReason.Canceled:
                var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                }
                break;
        }
    }

    async static Task Main(string[] args)
    {
        string wavFilePath = "output.wav";
        ConvertMp3ToWav("C:\\Users\\russe\\code\\church-speech\\console\\input.mp3", wavFilePath);

        var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);        
        speechConfig.SpeechRecognitionLanguage = "en-US";

        // using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        using var audioConfig = AudioConfig.FromWavFileInput(wavFilePath);
        //using var audioConfig = AudioConfig.FromWavFileInput("output.wav");
        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        Console.WriteLine("Speak into your microphone.");
        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        OutputSpeechRecognitionResult(speechRecognitionResult);
    }

      static void ConvertMp3ToWav(string mp3FilePath, string wavFilePath)
    {
        using (var reader = new Mp3FileReader(mp3FilePath))
        using (var writer = new WaveFileWriter(wavFilePath, reader.WaveFormat))
        {
            reader.CopyTo(writer);
        }
    }
}