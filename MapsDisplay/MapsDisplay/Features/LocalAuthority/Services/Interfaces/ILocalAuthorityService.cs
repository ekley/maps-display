using Shared.Models;
using Shared.Utilities;

namespace MapsDisplay.Features.LocalAuthority.Services.Interfaces
{
    public interface ILocalAuthorityService
    {
        Result<Dictionary<string, GeometryDto>> LoadLookup();
    }
}
