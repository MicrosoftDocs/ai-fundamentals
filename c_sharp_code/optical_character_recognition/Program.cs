using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace optical_character_recognition
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Add your Computer Vision subscription key and endpoint to your environment variables. 
            // Close/reopen your project for them to take effect.
            string subscriptionKey = "368a11e4e8b54172b6aa12989bfd1267";
            string endpoint = "https://gercompvis.cognitiveservices.azure.com/";

            // the OCR method endpoint
            string uriBase = endpoint + "vision/v2.1/ocr";

            string imageFilePath = "../../data/ocr/advert.jpg";

            if (File.Exists(imageFilePath))
            {
                // Call the REST API method.
                Console.WriteLine("\nWait a moment for the results to appear.\n");
                await MakeOCRRequest(imageFilePath, subscriptionKey, uriBase);
            }
            else
            {
                Console.WriteLine("\nInvalid file path");
            }
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();

        }

        static async Task MakeOCRRequest(string imageFilePath, string subscriptionKey, string uriBase)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters. 
                // The language parameter doesn't specify a language, so the 
                // method detects it automatically.
                // The detectOrientation parameter is set to true, so the method detects and
                // and corrects text orientation before detecting text.
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API method.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Read the contents of the specified local image
                // into a byte array.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                // Add the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Asynchronously call the REST API method.
                    response = await client.PostAsync(uri, content);
                }

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                RootObject results = JsonConvert.DeserializeObject<RootObject>(contentString);
                Console.WriteLine();
                Console.WriteLine("Words found in the image.");
                Console.WriteLine();

                foreach(Region reg in results.regions)                
                {
                    foreach(Line ln in reg.lines)
                    {
                        foreach(Word wd in ln.words)
                        Console.WriteLine(wd.text);
                    }
                }
            
                Console.WriteLine();

                // Display the JSON response.
                // Console.WriteLine("\nResponse:\n\n{0}\n",
                //     JToken.Parse(contentString).ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}
