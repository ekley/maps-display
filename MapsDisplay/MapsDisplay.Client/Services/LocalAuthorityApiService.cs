using Shared.Models;
using System.Net.Http.Json;

namespace MapsDisplay.Client.Services
{
    public class LocalAuthorityApiService
    {
        private readonly HttpClient _http;

        public LocalAuthorityApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<GeometryDto> BboxByNameAsync(string name)
        {
            var response = await _http.GetFromJsonAsync<GeometryDto>(
                $"api/LocalAuthorities/geometry?name={Uri.EscapeDataString(name)}");

            return response ?? new GeometryDto();
        }
    }
}
