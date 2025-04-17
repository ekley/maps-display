using MapsDisplay.Features.LocalAuthority.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
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

            var lookup = _service.LoadLookup();

            if (lookup == null)
            {
                _logger.LogWarning("Lookup data is null.");
                return StatusCode(500, "Internal server error. Lookup data is unavailable.");
            }

            if (!lookup.TryGetValue(name, out var geometry))
            {
                _logger.LogWarning("Geometry for '{name}' not found.", name);
                return NotFound($"Geometry for '{name}' not found.");
            }

            return Ok(geometry);
        }

        [HttpGet("similar-names")]
        public ActionResult<List<string>> GetSimilarNames([FromQuery, Required, MinLength(3)] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name parameter is required.");
            }

            name = name.Trim();

            _logger.LogInformation("Request for similar names to '{name}'", name);

            var lookup = _service.LoadLookup();

            if (lookup == null)
            {
                _logger.LogWarning("Lookup data is null.");
                return StatusCode(500, "Internal server error. Lookup data is unavailable.");
            }

            var similarNames = lookup.Keys
                .Where(key => key.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Take(6)
                .ToList();

            if (!similarNames.Any())
            {
                _logger.LogWarning("No similar names found for '{name}'.", name);
            }

            return Ok(similarNames);
        }
    }
}
