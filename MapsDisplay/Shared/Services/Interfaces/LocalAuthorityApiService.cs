using Shared.Models;

namespace Shared.Services.Interfaces
{
    public interface ILocalAuthorityApiService
    {
        Task<GeometryDto> BboxByNameAsync(string name);

    }
}
