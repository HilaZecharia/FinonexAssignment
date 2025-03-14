using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Json;

public class Client
{
    private const string EventsFile = "events.jsonl";

    public static async Task Main(string[] args)
    {
        try
        {
            if (!File.Exists(EventsFile))
            {
                Console.WriteLine($"File not exist: {EventsFile}");
                return;
            }
            string[] events = await File.ReadAllLinesAsync(EventsFile);
            foreach (var eventLine in events)
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        HttpRequestMessage requestMessage = CreateRequest(eventLine, client);

                        var response = await client.SendAsync(requestMessage);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Success to procssing event: {eventLine}");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to procssing event: {eventLine}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static HttpRequestMessage CreateRequest(string eventLine, HttpClient client)
    {
        var url = "http://localhost:8000/liveEvent";
        const string SecretHeader = "secret";
        //add 'secret' to Authorization in header
        client.DefaultRequestHeaders.Add("Authorization", SecretHeader);

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(eventLine, System.Text.Encoding.UTF8, "application/json")
        };
        return requestMessage;
    }
}


