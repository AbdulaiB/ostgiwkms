﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ostgiwkms.Services
{
    public class TextGenerationRequest
    {
        private readonly HttpClient _httpClient;

        public class NovelApiResponse
        {
            public string Event {  get; set; }
            public int Id { get; set; }
            public Data Data { get; set; }
        }

        public class EventStream
        {
            private readonly System.IO.Stream _stream;
            private readonly System.IO.StreamReader _reader;

            public EventStream(System.IO.Stream stream)
            {
                _stream = stream ?? throw new ArgumentNullException(nameof(stream));
                _reader = new System.IO.StreamReader(_stream);
            }

            public async IAsyncEnumerable<string> ReadAllAsync()
            {
                while (!_reader.EndOfStream)
                {
                    string line = await _reader.ReadLineAsync();
                    yield return line;
                }
            }
        }

        public TextGenerationRequest(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<string> GenerateNovelStream(string inputText, double _temperature, int _min_length, int _max_length)
        {
            string output = "";
            string apiURL = "https://api.novelai.net/ai/generate-stream";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6InE4TEFVd1YzQkZMMXdMZW5COGl2eCIsIm5jIjoiSVVhb19jVmdJMGtWMThjZ1N2bXZpIiwiaWF0IjoxNzAxNzkzNzgwLCJleHAiOjE3MDQzODU3ODB9.0P6pLkLqtdCNv7yuGhBlKcD8upS6GQVhbMBFhPKnb6E");

            //Preparing data to be sent to the api
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
                    top_a = 0.1,
                    top_k = 15,
                    top_p = 0.85,
                    phrase_rep_pen = "aggressive",
                    tail_free_sampling = 0.915,
                    repetition_penalty = 2.8,
                    repetition_penalty_range = 2048,
                    repetition_penalty_slope = 0.02,
                    repetition_penalty_frequency = 0.02,
                    repetition_penalty_presence = 1,
                    no_repeat_ngram_size = 30,
                    num_return_sequences = 10,
                    num_logprobs = 0,
                    generate_until_sentence = true,
                    return_full_text = true
                }
            };
            /*
            try
            {
                //Posting the request to the API
                var response = await _httpClient.PostAsJsonAsync(apiURL, requestData);

                //handling the response
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    return responseData;
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                //Handles exceptions
                return $"Exception: {ex.Message}";
            }
            */
            try
            {
                // Posting the request to the API
                var response = await _httpClient.PostAsJsonAsync(apiURL, requestData);

                // Handling the response
                if (response.IsSuccessStatusCode)
                {

                    var responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Data: {responseData}");

                    // Deserialize the JSON string into an instance of NovelApiResponse
                    //var novelApiResponse = JsonConvert.DeserializeObject<NovelApiResponse>(responseData);
                    //dynamic testTrial = JsonConvert.DeserializeObject<dynamic>(responseData);

                    var contentType = response.Content.Headers.ContentType?.MediaType;
                    if (contentType != null && contentType.Contains("text/event-stream"))
                    {
                        var eventStream = new EventStream(await response.Content.ReadAsStreamAsync());
                        await foreach (var eventItem in eventStream.ReadAllAsync())
                        {
                            // Handle event, i.e., print to console
                            Console.WriteLine(eventItem);

                            // Extract token from the event
                            string token = ExtractTokenFromJson(eventItem);

                            // Use the extracted token as needed
                            output += token;
                        }

                    }
                    else
                    {
                        // Handle JSON response
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(jsonResponse);
                    }



                    Console.WriteLine($"Content Type: {contentType}");


                    // Access the "token" property
                    //string tokenValue = testTrial.ToString();
                    //var testThingy = novelApiResponse.Data.Token;
                    
                    //string readableThingy = testThingy.ToString();

                    return output;
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                // Handles exceptions
                return $"Exception: {ex.Message}";
            }


            static string ExtractTokenFromJson(string jsonString)
            {
                try
                {
                    // Parse the event data field
                    int dataIndex = jsonString.IndexOf("data:");
                    if (dataIndex >= 0)
                    {
                        string eventData = jsonString.Substring(dataIndex + "data:".Length).Trim();

                        // Parse the inner JSON within the data field
                        var dataJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(eventData);

                        // Check if the "token" property exists
                        if (dataJson.token != null)
                        {
                            return dataJson.token;
                        }
                        else
                        {
                            Console.WriteLine($"Token not found in JSON: {eventData}");
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid event format: {jsonString}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error extracting token from JSON: {ex.Message}");
                    return null;
                }
            }

        }
    }
}
