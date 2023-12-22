using ApiRequestApp.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace ApiRequestApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _bearerToken;

        public ApiService(HttpClient httpClient, string bearerToken)
        {
            _httpClient = httpClient;
            _bearerToken = bearerToken;
        }

        public async Task<string> SendOrderRequest(OrderRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Replace the placeholders with actual values
            string requestUrl = $"https://api.maxar.com/ordering/v1/pipelines/imagery/view-ready-ortho/order";

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            httpRequest.Content = content;

            // Send the request
            var response = await _httpClient.SendAsync(httpRequest);

            // Ensure the request was successful
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Request succeeded. Response content:");
                return responseContent;
            }
            else
            {
                Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                string errorContent = await response.Content.ReadAsStringAsync();
                return errorContent;

            }
        }
    }
}
