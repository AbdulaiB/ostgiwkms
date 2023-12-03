using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ostgiwkms.Services
{
    public class TestStream
    {
        /*
        private readonly HttpClient _httpClient;

        public TestStream(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        static async Task StreamGeneration(string inputText, double _temperature, int _min_length, int _max_length)
        {
            string url = "https://api.novelai.net/ai/generate-stream"; // Replace with your API endpoint

            var requestData = new
            {
                //
                input = inputText,
                model = "kayra-v1",
                parameters = new
                {
                    use_string = true,
                    temperature = _temperature,
                    min_length = _min_length,
                    max_length = _max_length,
                    no_repeat_ngram_size = 30,
                }
            };


            try
            {
                var responseNovel = await _httpClient.PostAsJsonAsync(url, requestData);


                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    var content_type = response.Content.Headers.ContentType?.MediaType;

                    if (content_type != null && content_type.Contains("text/event-stream"))
                    {
                        using (var streamReader = new System.IO.StreamReader(await response.Content.ReadAsStreamAsync()))
                        {
                            // Handle event stream
                            while (true)
                            {
                                var line = await streamReader.ReadLineAsync();
                                if (line == null)
                                    break;

                                // Handle event, e.g., print to console
                                Console.WriteLine(line);
                            }
                        }
                    }
                    else
                    {
                        // Handle JSON response
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        // Process jsonResponse as needed
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle exceptions
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
            }
        }*/
    }
}
