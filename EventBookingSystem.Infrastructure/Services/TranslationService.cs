using System.Text.Json;
using EventBookingSystem.Application.Contract;
using EventBookingSystem.Application.Common.Responses;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
namespace EventBookingSystem.Infrastructure.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly HttpClient _httpClient;

        public TranslationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> TranslateAsync(string text, string sourceLang="ar", string targetLang="en")
        {
            try
            {
                string apiUrl = "https://lt.vern.cc/translate";

                // 1. Properly structured JSON body
                var requestData = new
                {
                    q = text,
                    source = sourceLang,
                    target = targetLang,
                    format = "text", // Required by some LibreTranslate instances
                    api_key = "" // Leave empty if no key required
                };

                // 2. Serialize to JSON with correct formatting
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

              

                // 4. Send request
                var response = await _httpClient.PostAsync(apiUrl, content);

                // 5. Verify success
                if (!response.IsSuccessStatusCode)
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    return "";
                }

                // 6. Parse response
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseBody);
                return result.translatedText;
            }
            catch (Exception ex)
            {

                return "";
            }
        }

    }
}
