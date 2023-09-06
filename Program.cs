using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;



namespace labtwocomputervision
{
    class Program
    {


     // Add your Computerstatic 
     static string key = "107cad7e28014b2bbc9f9dd6d2770118";
     static string endpoint = "https://cogserviceforcomputervision.cognitiveservices.azure.com/";
     
      



        static async Task Main(string[] args)
        {



            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, key);



            // Prompt the user to enter an image URL
            Console.Write("Enter the URL of the image: ");
            string imageUrl = Console.ReadLine();



            // Analyze the user-provided image URL
            await AnalyzeImageUrl(client, imageUrl);
        }



        /*
         * AUTHENTICATE
         * Creates a Computer Vision client used by each example.
         */
        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }



        public static async Task AnalyzeImageUrl(ComputerVisionClient client, string imageUrl)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - URL");
            Console.WriteLine();



            // Creating a list that defines the features to be extracted from the image. 
            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Tags,
                VisualFeatureTypes.Description
            };



            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
            // Analyze the URL image 
            ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);



            // Image tags and their confidence score
            Console.WriteLine("Tags:");
            foreach (var tag in results.Tags)
            {
                Console.WriteLine($"{tag.Name} {tag.Confidence * 100:F1}%");
            }



            // Image captions
            Console.WriteLine("\nCaptions:");
            foreach (var caption in results.Description.Captions)
            {
                Console.WriteLine($"{caption.Text} (Confidence: {caption.Confidence})");
            }



            // Output current directory for debugging
            Console.WriteLine($"Current Directory: {Environment.CurrentDirectory}");



            // Generate and display thumbnail
            Console.WriteLine("\nGenerating Thumbnail...");
            var thumbnailStream = await client.GenerateThumbnailAsync(50, 50, imageUrl, true);
            using (var thumbnailImage = System.Drawing.Image.FromStream(thumbnailStream))
            {
                thumbnailImage.Save("thumbnail.png");
            }



            Console.WriteLine("Thumbnail image saved as 'thumbnail.png'");



            Console.WriteLine();
        }
    }
}
