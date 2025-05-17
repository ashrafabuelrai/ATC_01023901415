

using EventBookingSystem.Web.Models;
using EventBookingSystem.Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static EventBookingSystem.Application.Common.Utility.SD;

namespace EventBookingSystem.Web.Services
{
    public class BaseService : IBaseService
    {
        public ApiResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            responseModel = new ApiResponse();
            this.httpClient = httpClient;
        }
        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("EventBookingSystem");
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Headers.Add("Accept", "application/json");
                requestMessage.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    if (apiRequest.Data is MultipartFormDataContent multiContent)
                    {
                        requestMessage.Content = multiContent;
                    }
                    else
                    {
                        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                            Encoding.UTF8, "application/json");
                    }
                }
                Console.WriteLine(JsonConvert.SerializeObject(apiRequest.Data));

                switch (apiRequest.ApiType)
                {
                    case ApiType.POST:
                        requestMessage.Method = HttpMethod.Post;
                        break;
                    case ApiType.GET:
                        requestMessage.Method = HttpMethod.Get;
                        break;
                    case ApiType.PUT:
                        requestMessage.Method = HttpMethod.Put;
                        break;
                    default:
                        requestMessage.Method = HttpMethod.Delete;
                        break;

                }
                HttpResponseMessage responseMessage = null;
                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }
                responseMessage = await client.SendAsync(requestMessage);

                
                if (!responseMessage.IsSuccessStatusCode)
                {
                    string errorContent = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine("BadRequest: " + errorContent);
                    // You can also deserialize the error if it's JSON
                    

                    responseModel.StatusCode = responseMessage.StatusCode;
                    responseModel.IsSuccess = false;
                    responseModel.ErrorMessage = new List<string> { errorContent.ToString() }; // ⬅️ سجل تفاصيل الخطأ

                    var res = JsonConvert.SerializeObject(responseModel);
                    var returnObj = JsonConvert.DeserializeObject<T>(res);
                    return returnObj;

                }
                var repsoneContent = await responseMessage.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(repsoneContent);

            }
            catch (Exception ex)
            {

                responseModel.ErrorMessage = new List<string>() { ex.Message };
                responseModel.IsSuccess = false;

                var res = JsonConvert.SerializeObject(responseModel);
                return JsonConvert.DeserializeObject<T>(res);
            }
        }
    }
}
