using Entree_1830974.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Entree_1830974.Data
{
    public static class ApiHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ApiBaseUrl = "https://localhost:7142/api";

        public static async Task<Ticket> GenerateTicket(string licensePlate, string apiKey)
        {
            string url = $"{ApiBaseUrl}/Ticket/GenerateTicket";
            var requestBody = new { LicensePlate = licensePlate };

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);

            var response = await _httpClient.PostAsJsonAsync(url, requestBody);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Ticket>(content);
        }

        public static async Task<(bool Success, Parking? Parking, string? ErrorMessage)> AddOccupiedSpace(string apiKey)
        {
            string url = $"{ApiBaseUrl}/Parking/Occupy";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);

            try
            {
                var response = await _httpClient.PutAsJsonAsync(url, new object { });

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var parking = JsonConvert.DeserializeObject<Parking>(content);
                    return (true, parking, null);
                }

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return (false, null, "Current parking state is not set");
                    case System.Net.HttpStatusCode.BadRequest:
                        return (false, null, "The parking is already full");
                    default:
                        return (false, null, $"Unexpected error: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                return (false, null, $"Network error: {e.Message}");
            }
            catch (JsonException e)
            {
                return (false, null, $"Error parsing response: {e.Message}");
            }
        }
    }
}
