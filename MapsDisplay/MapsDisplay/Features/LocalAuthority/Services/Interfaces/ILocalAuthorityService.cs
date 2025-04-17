using Shared.Models;

namespace MapsDisplay.Features.LocalAuthority.Services.Interfaces
{
    public interface ILocalAuthorityService
    {
        Dictionary<string, GeometryDto>? LoadLookup();
    }
}
