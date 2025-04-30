using MapsDisplay.Features.LocalAuthority.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace MapsDisplay.Features.LocalAuthority.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalAuthoritiesController : ControllerBase
    {
        private readonly ILocalAuthorityService _service;
        private readonly ILogger<LocalAuthoritiesController> _logger;

        public LocalAuthoritiesController(ILocalAuthorityService service, ILogger<LocalAuthoritiesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("geometry")]
        public ActionResult<GeometryDto> GetGeometryByName([FromQuery, Required, MinLength(3)] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name parameter is required.");
            }

            name = name.Trim();
            _logger.LogInformation("Request for geometry of '{name}'", name);

            var result = _service.LoadLookup();

            if (!result.IsSuccess)
            {
                _logger.LogError("Lookup loading failed: {Error}", result.Error);
                return StatusCode(500, result.Error);
            }

            var lookup = result.Value!;
            if (!lookup.TryGetValue(name, out var geometry))
            {
                _logger.LogWarning("Geometry for '{name}' not found.", name);
                return NotFound($"Geometry for '{name}' not found.");
            }

            return Ok(geometry);
        }
    }
}
