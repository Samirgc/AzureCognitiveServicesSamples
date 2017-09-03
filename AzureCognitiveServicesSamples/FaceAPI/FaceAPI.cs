using AzureCognitiveServicesSamples.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServicesSamples.FaceAPI
{

    /// <summary>
    /// I refactored the example mention in this link
    /// https://docs.microsoft.com/en-us/azure/cognitive-services/face/quickstarts/csharp 
    /// </summary>
    public static class FaceAPI
    {
        // change this to you subscriptionKey
        private const string subscriptionKey = "55f494c1b2e34d32aea53beaf3f3992d";
        private const string detectUriBase = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/detect";
        private const string verifyUriBase = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/verify";

        public static async Task<List<DetectResultRootObject>> DetectAsync(string imageFilePath)
        {
            HttpClient client = new HttpClient();

            // Add Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            // Assemble the URI for the REST API Call.
            string uri = detectUriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = FileHelper.GetFileAsByteArray(imageFilePath);
            List<DetectResultRootObject> result;
            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<List<DetectResultRootObject>>(contentString);

            }
            return result;
        }

        public static async Task<VerifyResult> VerifyAsync(string faceId1, string faceId2)
        {
            HttpClient client = new HttpClient();

            // Add Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Assemble the URI for the REST API Call.
            string uri = verifyUriBase;

            HttpResponseMessage response;


            string bodyContent = JsonConvert.SerializeObject(new { faceId1 = faceId1, faceId2 = faceId2 });
            VerifyResult result;
            using (StringContent content = new StringContent(bodyContent, Encoding.UTF8, "application/json"))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<VerifyResult>(contentString);

            }
            return result;
        }
    }
}
