using Shared.Models;
using Shared.Services.Interfaces;
using Shared.Utilities;
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

        public async Task<Result<GeometryDto>> BboxByNameAsync(string name)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<GeometryDto>(
                    $"api/LocalAuthorities/geometry?name={Uri.EscapeDataString(name)}");

                if (response is null)
                {
                    return Result<GeometryDto>.Failure("No geometry found for the specified name.");
                }

                return Result<GeometryDto>.Success(response);
            }
            catch (HttpRequestException httpEx)
            {
                return Result<GeometryDto>.Failure($"Network error while fetching geometry data: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                return Result<GeometryDto>.Failure($"Unexpected error: {ex.Message}");
            }
        }
    }
}
