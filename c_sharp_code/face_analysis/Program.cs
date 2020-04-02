using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace face_analysis
{
    class Program
    {
        static void Main(string[] args)
        {
            // From your Face subscription in the Azure portal, get your subscription key and endpoint.
            // Set your environment variables using the names below. Close and reopen your project for changes to take effect.
            string SUBSCRIPTION_KEY = "YOUR_COG_KEY";
            string ENDPOINT = "YOUR_COG_ENDPOINT";

            // Used in the Detect Faces and Verify examples.
            // Recognition model 2 is used for feature extraction, use 1 to simply recognize/detect a face. 
            // However, the API calls to Detection that are used with Verify, Find Similar, or Identify must share the same 
            // recognition model.
            const string RECOGNITION_MODEL2 = RecognitionModel.Recognition02;
            const string RECOGNITION_MODEL1 = RecognitionModel.Recognition01;

            // Authenticate.
            IFaceClient client = Authenticate(ENDPOINT, SUBSCRIPTION_KEY);

            // Detect - get features from faces.
            DetectFaceExtract(client, RECOGNITION_MODEL2).Wait();
            FindSimilar(client, RECOGNITION_MODEL1).Wait();
        }

        // AUTHENTICATE
        // Uses subscription key and region to create a client.
        public static IFaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }


        // DETECT FACES
        // Detects features from faces and IDs them.
        public static async Task DetectFaceExtract(IFaceClient client, string recognitionModel)
        {
            Console.WriteLine("========DETECT FACES========");
            Console.WriteLine();

            // Create a list of images
            List<string> imageFileNames = new List<string>
                    {
                        "../../data/face/store_cam1.jpg",
                        "../../data/face/store_cam2.jpg",
                        "../../data/face/store_cam3.jpg"
                    };

            foreach (var imageFileName in imageFileNames)
            {
                IList<DetectedFace> detectedFaces;
                FileStream fileStream =
                new FileStream(imageFileName, FileMode.Open, FileAccess.Read);

                // Detect faces with all attributes from image url.
                detectedFaces = await client.Face.DetectWithStreamAsync(fileStream,
                        returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Accessories, FaceAttributeType.Age,
                FaceAttributeType.Blur, FaceAttributeType.Emotion, FaceAttributeType.Exposure, FaceAttributeType.FacialHair,
                FaceAttributeType.Gender, FaceAttributeType.Glasses, FaceAttributeType.Hair, FaceAttributeType.HeadPose,
                FaceAttributeType.Makeup, FaceAttributeType.Noise, FaceAttributeType.Occlusion, FaceAttributeType.Smile },
                        recognitionModel: recognitionModel);

                Console.WriteLine($"{detectedFaces.Count} face(s) detected from image `{imageFileName}`.");

                // Parse and print all attributes of each detected face.
                foreach (var face in detectedFaces)
                {
                    Console.WriteLine($"Face attributes for {imageFileName}:");

                    // Get bounding box of the faces
                    Console.WriteLine($"Rectangle(Left/Top/Width/Height) : {face.FaceRectangle.Left} {face.FaceRectangle.Top} {face.FaceRectangle.Width} {face.FaceRectangle.Height}");

                    // Get accessories of the faces
                    List<Accessory> accessoriesList = (List<Accessory>)face.FaceAttributes.Accessories;
                    int count = face.FaceAttributes.Accessories.Count;
                    string accessory; string[] accessoryArray = new string[count];
                    if (count == 0) { accessory = "NoAccessories"; }
                    else
                    {
                        for (int i = 0; i < count; ++i) { accessoryArray[i] = accessoriesList[i].Type.ToString(); }
                        accessory = string.Join(",", accessoryArray);
                    }
                    Console.WriteLine($"Accessories : {accessory}");

                    // Get face other attributes
                    Console.WriteLine($"Age : {face.FaceAttributes.Age}");
                    Console.WriteLine($"Blur : {face.FaceAttributes.Blur.BlurLevel}");

                    // Get emotion on the face
                    string emotionType = string.Empty;
                    double emotionValue = 0.0;
                    Emotion emotion = face.FaceAttributes.Emotion;
                    if (emotion.Anger > emotionValue) { emotionValue = emotion.Anger; emotionType = "Anger"; }
                    if (emotion.Contempt > emotionValue) { emotionValue = emotion.Contempt; emotionType = "Contempt"; }
                    if (emotion.Disgust > emotionValue) { emotionValue = emotion.Disgust; emotionType = "Disgust"; }
                    if (emotion.Fear > emotionValue) { emotionValue = emotion.Fear; emotionType = "Fear"; }
                    if (emotion.Happiness > emotionValue) { emotionValue = emotion.Happiness; emotionType = "Happiness"; }
                    if (emotion.Neutral > emotionValue) { emotionValue = emotion.Neutral; emotionType = "Neutral"; }
                    if (emotion.Sadness > emotionValue) { emotionValue = emotion.Sadness; emotionType = "Sadness"; }
                    if (emotion.Surprise > emotionValue) { emotionType = "Surprise"; }
                    Console.WriteLine($"Emotion : {emotionType}");

                    // Get more face attributes
                    Console.WriteLine($"Exposure : {face.FaceAttributes.Exposure.ExposureLevel}");
                    Console.WriteLine($"FacialHair : {string.Format("{0}", face.FaceAttributes.FacialHair.Moustache + face.FaceAttributes.FacialHair.Beard + face.FaceAttributes.FacialHair.Sideburns > 0 ? "Yes" : "No")}");
                    Console.WriteLine($"Gender : {face.FaceAttributes.Gender}");
                    Console.WriteLine($"Glasses : {face.FaceAttributes.Glasses}");

                    // Get hair color
                    Hair hair = face.FaceAttributes.Hair;
                    string color = null;
                    if (hair.HairColor.Count == 0) { if (hair.Invisible) { color = "Invisible"; } else { color = "Bald"; } }
                    HairColorType returnColor = HairColorType.Unknown;
                    double maxConfidence = 0.0f;
                    foreach (HairColor hairColor in hair.HairColor)
                    {
                        if (hairColor.Confidence <= maxConfidence) { continue; }
                        maxConfidence = hairColor.Confidence; returnColor = hairColor.Color; color = returnColor.ToString();
                    }
                    Console.WriteLine($"Hair : {color}");

                    // Get more attributes
                    Console.WriteLine($"HeadPose : {string.Format("Pitch: {0}, Roll: {1}, Yaw: {2}", Math.Round(face.FaceAttributes.HeadPose.Pitch, 2), Math.Round(face.FaceAttributes.HeadPose.Roll, 2), Math.Round(face.FaceAttributes.HeadPose.Yaw, 2))}");
                    Console.WriteLine($"Makeup : {string.Format("{0}", (face.FaceAttributes.Makeup.EyeMakeup || face.FaceAttributes.Makeup.LipMakeup) ? "Yes" : "No")}");
                    Console.WriteLine($"Noise : {face.FaceAttributes.Noise.NoiseLevel}");
                    Console.WriteLine($"Occlusion : {string.Format("EyeOccluded: {0}", face.FaceAttributes.Occlusion.EyeOccluded ? "Yes" : "No")} " +
                        $" {string.Format("ForeheadOccluded: {0}", face.FaceAttributes.Occlusion.ForeheadOccluded ? "Yes" : "No")}   {string.Format("MouthOccluded: {0}", face.FaceAttributes.Occlusion.MouthOccluded ? "Yes" : "No")}");
                    Console.WriteLine($"Smile : {face.FaceAttributes.Smile}");
                    Console.WriteLine();
                }
            }
        }


        // FIND SIMILAR
        // This example will take an image and find a similar one to it in another image.
        public static async Task FindSimilar(IFaceClient client, string RECOGNITION_MODEL1)
        {
            Console.WriteLine("========FIND SIMILAR========");
            Console.WriteLine();

            string firstImage = "../../data/face/store_cam3.jpg";

            string sourceImageFileName = "../../data/face/store_cam2.jpg";
            IList<Guid?> targetFaceIds = new List<Guid?>();

            // Detect faces from target image.
            var faces = await DetectFaceRecognize(client, firstImage, RECOGNITION_MODEL1);
            // Add detected faceId to list of GUIDs.
            targetFaceIds.Add(faces[0].FaceId.Value);


            // Detect faces from source image url.
            IList<DetectedFace> detectedFaces = await DetectFaceRecognize(client, sourceImageFileName, RECOGNITION_MODEL1);
            Console.WriteLine();

            // Find a similar face(s) in the list of IDs. Comapring only the first in list for testing purposes.
            IList<SimilarFace> similarResults = await client.Face.FindSimilarAsync(detectedFaces[0].FaceId.Value, null, null, targetFaceIds);
            foreach (var similarResult in similarResults)
            {
                Console.WriteLine($"Faces from {sourceImageFileName} & ID:{similarResult.FaceId} are similar with confidence: {similarResult.Confidence}.");
            }
            Console.WriteLine();
        }

        private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, string sourceImage, string RECOGNITION_MODEL1)
        {
            FileStream fileStream =
                new FileStream(sourceImage, FileMode.Open, FileAccess.Read);

            // Detect faces from image URL. Since only recognizing, use the recognition model 1.
            IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithStreamAsync(fileStream, recognitionModel: RECOGNITION_MODEL1);
            Console.WriteLine($"{detectedFaces.Count} face(s) detected from image `{Path.GetFileName(sourceImage)}`");
            return detectedFaces.ToList();
        }

    }
}