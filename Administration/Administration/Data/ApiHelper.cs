using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Administration.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;

namespace Administration.Data
{
    internal class ApiHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ApiBaseUrl = "https://localhost:7142/api";

        public static async Task<Parking> GetParkingState(string apiKey)
        {
            string url = $"{ApiBaseUrl}/Parking/State";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Parking>(content);
        }
    }
}
