using Shared.Dtos;
using Shared.Utilities;

namespace Shared.Services.Interfaces
{
    public interface ILocalAuthorityApiService
    {
        Task<Result<GeometryDto>> BboxByNameAsync(string name);

    }
}
