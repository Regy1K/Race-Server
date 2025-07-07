using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        string baseUrl = "http://localhost:8080/api/races"; // Your REST API endpoint base URL
        int raceId = 1; // Example race id

        using HttpClient client = new HttpClient();

        try
        {
            Console.WriteLine("Getting all races...");
            HttpResponseMessage response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("All Races JSON: " + json);
            }
            else
            {
                Console.WriteLine("Error getting all races: " + response.StatusCode);
            }

            Console.WriteLine($"Getting race with ID {raceId}...");
            response = await client.GetAsync($"{baseUrl}/{raceId}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Race {raceId} JSON: " + json);
            }
            else
            {
                Console.WriteLine($"Error getting race {raceId}: " + response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception occurred: " + ex.Message);
        }
    }
}