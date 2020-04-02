//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//

using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace speech
{
    class Program
    {
        // It's always a good idea to access services in an async fashion
        static async Task Main()
        {
            // Configure the subscription information for the service to access.
            // Use either key1 or key2 from the Speech Service resource you have created
            // replace YOUR_REGION with the region in which you deployed the cognitive service
            // such as "westus"
            var config = SpeechConfig.FromSubscription("YOUR_COG_KEY", "YOUR_REGION");

            await RecognizeSpeechAsync(config);
            await SynthesisToSpeakerAsync(config);
        }

        static async Task RecognizeSpeechAsync(SpeechConfig config)
        {
            
            // Setup the audio configuration, in this case, using a file that is in local storage.
            using (var audioInput = AudioConfig.FromWavFileInput("../../data/speech/light-on.wav"))

            // Pass the required parameters to the Speech Service which includes the configuration information
            // and the audio file name that you will use as input
            using (var recognizer = new SpeechRecognizer(config, audioInput))
            {
                Console.WriteLine("Recognizing first result...");
                var result = await recognizer.RecognizeOnceAsync();

                switch (result.Reason)
                {
                    case ResultReason.RecognizedSpeech:
                        // The file contained speech that was recognized and the transcription will be output
                        // to the terminal window
                        Console.WriteLine($"We recognized: {result.Text}");
                        break;
                    case ResultReason.NoMatch:
                        // No recognizable speech found in the audio file that was supplied.
                        // Out an informative message
                        Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                        break;
                    case ResultReason.Canceled:
                        // Operation was cancelled
                        // Output the reason
                        var cancellation = CancellationDetails.FromResult(result);
                        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }
                        break;
                }
            }
        }
        public static async Task SynthesisToSpeakerAsync(SpeechConfig config)
        {

            // Creates a speech synthesizer using the default speaker as audio output.
            using (var synthesizer = new SpeechSynthesizer(config))
            {
                // Receive a text from console input and synthesize it to speaker.
                Console.WriteLine("Type some text that you want to speak...");
                Console.Write("> ");
                string text = Console.ReadLine();

                using (var result = await synthesizer.SpeakTextAsync(text))
                {
                    if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                    {
                        Console.WriteLine($"Speech synthesized to speaker for text [{text}]");
                    }
                    else if (result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }
                    }
                }

                // This is to give some time for the speaker to finish playing back the audio
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
