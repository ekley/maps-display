using Shared.Dtos;
using Shared.Utilities;

namespace MapsDisplay.Features.LocalAuthority.Services.Interfaces
{
    public interface ILocalAuthorityService
    {
        Result<Dictionary<string, GeometryDto>> LoadLookup();
    }
}
