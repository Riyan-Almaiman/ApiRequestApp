using System.Text.Json;

namespace ApiRequestApp.Services
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;

        public TokenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetToken(string username, string password)
        {
            string tokenRequestUrl = "https://account.maxar.com/auth/realms/mds/protocol/openid-connect/token";
            var tokenRequestPayload = new Dictionary<string, string>
        {
            { "client_id", "mgp" }, 
            { "username", username.Trim() },
            { "password", password.Trim() },
            { "grant_type", "password" }
        };

            var requestContent = new FormUrlEncodedContent(tokenRequestPayload);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync(tokenRequestUrl, requestContent);
            }
            catch (HttpRequestException ex)
            {
                // Handle network-related exceptions
                throw new Exception($"An error occurred while sending the request: {ex.Message}");
            }

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseContent);

                if (tokenResponse != null && tokenResponse.ContainsKey("access_token"))
                {
                    var accessToken = tokenResponse["access_token"].GetString();
                    if (!string.IsNullOrWhiteSpace(accessToken))
                    {
                        return accessToken;
                    }
                    else
                    {
                        throw new Exception("The access token was null or empty.");
                    }
                }
                else
                {
                    throw new Exception("The token response did not contain an access token.");
                }
            }
            else
            {
                // Handle non-success status codes
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Token request failed with status code {response.StatusCode} and content: {errorContent}");
            }
        }
    }

}
