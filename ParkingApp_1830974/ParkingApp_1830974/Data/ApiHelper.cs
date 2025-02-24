using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using ParkingApp_1830974.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Entree_1830974.Data
{
    public static class ApiHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ApiBaseUrl = "https://localhost:7142/api";

        public static async Task<IEnumerable<LicensePlateDTO>> GetAllActiveLicensePlates(string apiKey)
        {
            string url = $"{ApiBaseUrl}/Parking/Plates";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<LicensePlateDTO>>(content);
        }
    }
}
