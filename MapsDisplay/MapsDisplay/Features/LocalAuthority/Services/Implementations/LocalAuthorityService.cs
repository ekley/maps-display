using MapsDisplay.Features.LocalAuthority.Services.Interfaces;
using Shared.Models;
using System.Text.Json;

namespace MapsDisplay.Features.LocalAuthority.Services.Implementations
{
    public class LocalAuthorityService : ILocalAuthorityService
    {
        public Dictionary<string, GeometryDto>? LoadLookup()
        {
            string lookupFile = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Features", "LocalAuthority", "Data", "Datasets", "lookup.json"
            );

            if (!File.Exists(lookupFile))
                return null;

            var json = File.ReadAllText(lookupFile);
            return JsonSerializer.Deserialize<Dictionary<string, GeometryDto>>(json);
        }
    }
}
