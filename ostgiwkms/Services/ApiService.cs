using ostgiwkms.Services;
using System.Text.Json;

namespace ostgiwkms.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse> GetSwaggerDataAsync()
        {
            var response = await _httpClient.GetAsync("https/novelai.net/");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    // Deserialize the JSON response into ApiResponse using System.Text.Json
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonSerializer.Deserialize<ApiResponse>(jsonContent);

                    return responseData;
                }
                catch (JsonException ex)
                {
                    // Handle JSON deserialization errors
                    Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                    return new ApiResponse { Message = "Error deserializing JSON" };
                }
            }
            else
            {
                // Handle error scenarios, throw exceptions, or return a default value
                // (this depends on your application's error handling strategy).
                return new ApiResponse { Message = "Error retrieving data from Swagger API" };
            }
        }
    }
}
