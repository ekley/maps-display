using MapsDisplay.Features.LocalAuthority.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Text.Json;

namespace MapsDisplay.Features.LocalAuthority.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalAuthoritiesController : ControllerBase
    {
        private readonly ILocalAuthorityService _service;

        public LocalAuthoritiesController(ILocalAuthorityService service)
        {
            _service = service;
        }

        [HttpGet("geometry")]
        public ActionResult<GeometryDto> GetGeometryByName([FromQuery] string name)
        {
            var lookup = _service.LoadLookup();

            if (lookup == null || !lookup.TryGetValue(name, out var geometry))
                return NotFound($"Geometry for '{name}' not found.");

            return Ok(geometry);
        }

        [HttpGet("similar-names")]
        public ActionResult<List<string>> GetSimilarNames([FromQuery] string name)
        {
            var lookup = _service.LoadLookup();

            if (lookup == null)
                return NotFound("Lookup file is empty or corrupted.");

            var similarNames = lookup.Keys
                .Where(key => key.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Take(6)
                .ToList();

            return Ok(similarNames);
        }
    }

}
