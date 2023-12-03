namespace ostgiwkms.Services
{ 
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AuthenticateAsync(string apiKey)
        {
            // Set the login endpoint URL
            string loginEndpoint = "https://api.novelai.net/user/login";

            try
            {
                // Create the content for the login request
                var content = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("key", apiKey),
            // Add any other required parameters for authentication
        });

                // Send the login request
                var response = await _httpClient.PostAsync(loginEndpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    // Authentication successful
                    // You may not need to do anything here, or you can handle it based on your requirements.
                }
                else
                {
                    // Handle authentication error
                    Console.WriteLine($"Authentication error: {response.StatusCode}");

                    // Optionally, print the response content for further analysis
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Content: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Exception during authentication: {ex.Message}");
            }
        }
    }
}
