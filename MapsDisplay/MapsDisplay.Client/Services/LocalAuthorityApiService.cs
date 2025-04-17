using Shared.Models;
using Shared.Services.Interfaces;
using System.Net.Http.Json;

namespace MapsDisplay.Client.Services
{
    public class LocalAuthorityApiService : ILocalAuthorityApiService
    {
        private readonly HttpClient _http;

        public LocalAuthorityApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<GeometryDto> BboxByNameAsync(string name)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<GeometryDto>(
                    $"api/LocalAuthorities/geometry?name={Uri.EscapeDataString(name)}");

                return response ?? new GeometryDto();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching geometry data", ex);
            }
        }

        public async Task<List<string>> FilterByNameAsync(string name)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<List<string>>(
                    $"api/LocalAuthorities/similar-names?name={Uri.EscapeDataString(name)}");

                return response ?? new List<string>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching similar names", ex);
            }
        }
    }
}
