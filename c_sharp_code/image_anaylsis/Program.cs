using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace image_anaylsis
{
    static class Program
    {
        // Add your Computer Vision subscription key and endpoint.
        static string subscriptionKey = "YOUR_COG_KEY";

        static string endpoint = "YOUR_COG_ENDPOINT";

        // the Analyze method endpoint
        static string uriBase = endpoint + "vision/v2.1/analyze";

        static async Task Main()
        {
            // Create an Http Client to access the resources
            HttpClient client = new HttpClient();

            try
            {
                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);

            }

            await DescribeImage(client);
        }

        static async Task DescribeImage(HttpClient client)
        {
            // Get the path and filename to process from the user.
            Console.WriteLine("Analyze an image:");

            string[] imageFilePaths = { "../../data/vision/store_cam1.jpg", "../../data/vision/store_cam2.jpg" };

            // Call the REST API method.
            Console.WriteLine("\nWait for the results to appear.\n");
            await MakeAnalysisRequest(imageFilePaths, client);
        }

        /// <summary>
        /// Gets the analysis of the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file to analyze.</param>
        static async Task MakeAnalysisRequest(string[] imageFilePaths, HttpClient client)
        {
            try
            {
                // Request parameters. A third optional parameter is "details".
                // The Analyze Image method returns information about the following
                // visual features:
                // Categories:  categorizes image content according to a
                //              taxonomy defined in documentation.
                // Description: describes the image content with a complete
                //              sentence in supported languages.
                // Color:       determines the accent color, dominant color, 
                //              and whether an image is black & white.
                string requestParameters =
                    "visualFeatures=Description, Tags, Adult, Objects, Faces";

                // Assemble the URI for the REST API method.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                foreach (string str in imageFilePaths)
                {
                    // Read the contents of the specified local image
                    // into a byte array.
                    byte[] byteData = GetImageAsByteArray(str);

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
                    // Console.WriteLine(contentString);

                    RootObject results = JsonConvert.DeserializeObject<RootObject>(contentString);

                    Console.WriteLine($"Image description: {results.description.captions[0].text}");
                    Console.WriteLine($"Confidence score: {results.description.captions[0].confidence}");
                    Console.WriteLine();

                    Console.WriteLine("Additional Image Features");
                    Console.WriteLine();
                    Console.WriteLine("Ratings:");

                    Console.WriteLine($"- Adult: {results.adult.isAdultContent.ToString()}");
                    Console.WriteLine($"- Racy: {results.adult.isRacyContent.ToString()}");
                    Console.WriteLine($"- Gore: {results.adult.isGoryContent.ToString()}");

                    Console.WriteLine();

                    Console.WriteLine("Tags:");
                    foreach (Tag tag in results.tags)
                    {
                        Console.WriteLine($"- {tag.name}");
                    }

                    Console.WriteLine();

                    Console.WriteLine("Detected Objects:");
                    foreach(Object obj in results.objects)
                    {
                        Console.WriteLine($"- {obj.@object}");
                    }

                    Console.WriteLine();

                    Console.WriteLine($"Number of faces found: {results.faces.Count}");


                    // Display the JSON response.
                    //Console.WriteLine("\nResponse:\n\n{0}\n",
                    //JToken.Parse(contentString).ToString());
                }
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
