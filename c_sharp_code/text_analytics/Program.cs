using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;


namespace text_analytics
{
    class Program
    {
        private static readonly string key = "50de79fcdc0d44bcaaee82326c50a9bd";
        private static readonly string endpoint = "https://westus2.api.cognitive.microsoft.com/";

        private static string[] reviews ={"../../data/text/reviews/review1.txt",
        "../../data/text/reviews/review2.txt",
        "../../data/text/reviews/review3.txt",
        "../../data/text/reviews/review4.txt"};

        static void Main(string[] args)
        {
            var client = authenticateClient();

            foreach (string str in reviews)
            {
                string review = File.ReadAllText(str);
                sentimentAnalysisExample(client, review);
                keyPhraseExtractionExample(client, review);
            }

            string review1 = File.ReadAllText("../../data/text/reviews/review1.txt");
            EntityRecognitionExample(client, review1);


        }

        static TextAnalyticsClient authenticateClient()
        {
            ApiKeyServiceClientCredentials credentials = new ApiKeyServiceClientCredentials(key);
            TextAnalyticsClient client = new TextAnalyticsClient(credentials)
            {
                Endpoint = endpoint
            };
            return client;
        }

        static void sentimentAnalysisExample(ITextAnalyticsClient client, string review)
        {
            string sentimentValue;

            Console.WriteLine();
            Console.WriteLine("The review");
            Console.WriteLine();
            Console.WriteLine(review);
            Console.WriteLine();

            var result1 = client.Sentiment(review, "en");
            if (result1.Score < 0.5)
            {
                sentimentValue = "negative";
            }
            else
            {
                sentimentValue = "positive";
            }

            Console.WriteLine($"Sentiment Score: {result1.Score:0.00} is {sentimentValue}");
            Console.WriteLine();
        }

        static void keyPhraseExtractionExample(TextAnalyticsClient client, string review)
        {
            var result = client.KeyPhrases(review);

            // Printing key phrases
            Console.WriteLine("Key phrases:");

            foreach (string keyphrase in result.KeyPhrases)
            {
                Console.WriteLine($"\t{keyphrase}");
            }
        }

        static void EntityRecognitionExample(ITextAnalyticsClient client, string review)
        {

            var result = client.Entities(review);
            Console.WriteLine("Entities:");
            foreach (var entity in result.Entities)
            {
                Console.WriteLine($"- {entity.Type ?? "N/A"}: {entity.Name} {entity.WikipediaUrl}");
                //foreach (var match in entity.Matches)
                //{
                    //Console.WriteLine($"\t\tOffset: {match.Offset},\tLength: {match.Length},\tScore: {match.EntityTypeScore:F3}");
                //}
            }
        }

    }

    class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        private readonly string apiKey;

        public ApiKeyServiceClientCredentials(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            request.Headers.Add("Ocp-Apim-Subscription-Key", this.apiKey);
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}
