using MapsDisplay.Features.LocalAuthority.Services.Interfaces;
using Shared.Models;
using Shared.Utilities;
using System.Text.Json;

namespace MapsDisplay.Features.LocalAuthority.Services.Implementations
{
    public class LocalAuthorityService : ILocalAuthorityService
    {
        public Result<Dictionary<string, GeometryDto>> LoadLookup()
        {
            try
            {
                const string JSON_FILE_NAME = "lookup.json";
                string lookupFile = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Features", "LocalAuthority", "Data", "Datasets", JSON_FILE_NAME
                );

                if (!File.Exists(lookupFile))
                    return Result<Dictionary<string, GeometryDto>>.Failure("Lookup file does not exist.");

                var json = File.ReadAllText(lookupFile);
                var data = JsonSerializer.Deserialize<Dictionary<string, GeometryDto>>(json);

                return data is not null
                    ? Result<Dictionary<string, GeometryDto>>.Success(data)
                    : Result<Dictionary<string, GeometryDto>>.Failure("Deserialized lookup data is null.");
            }
            catch (JsonException ex)
            {
                return Result<Dictionary<string, GeometryDto>>.Failure($"Failed to parse lookup.json: {ex.Message}");
            }
            catch (IOException ex)
            {
                return Result<Dictionary<string, GeometryDto>>.Failure($"I/O error when reading lookup.json: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<Dictionary<string, GeometryDto>>.Failure($"Unexpected error: {ex.Message}");
            }
        }
    }
}
