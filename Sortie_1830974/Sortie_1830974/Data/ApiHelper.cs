using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sortie_1830974.Models;

namespace Sortie_1830974.Data
{
    public static class ApiHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ApiBaseUrl = "https://localhost:7142/api";

        public static async Task<(Ticket? Ticket, string? ErrorMessage)> GetTicketById(int ticketId, string apiKey)
        {
            string url = $"{ApiBaseUrl}/Ticket/Id?ticketId={ticketId}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var ticket = JsonConvert.DeserializeObject<Ticket>(content);

                if (ticket.State != "Payé")
                    return (ticket, $"Ticket with ID \"{ticketId}\" is unpaid");

                return (ticket, null);
            }

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.NotFound:
                    return (null, $"no ticket with ID\"{ticketId}\" was found");
                default:
                    return (null, $"Unexpected error: {response.StatusCode}");
            }
        }
    }
}
