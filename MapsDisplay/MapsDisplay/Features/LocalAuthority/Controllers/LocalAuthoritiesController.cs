using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Text.Json;

namespace MapsDisplay.Features.LocalAuthority.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalAuthoritiesController : ControllerBase
    {

        [HttpGet("geometry")]
        public ActionResult<GeometryDto> GetGeometryByName([FromQuery] string name)
        {
            string lookupFile = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Features", "LocalAuthority", "Data", "Datasets", "lookup.json"
            );

            if (!System.IO.File.Exists(lookupFile))
                return NotFound("Lookup file not found.");

            var json = System.IO.File.ReadAllText(lookupFile);
            var lookup = JsonSerializer.Deserialize<Dictionary<string, GeometryDto>>(json);

            if (lookup == null || !lookup.TryGetValue(name, out var geometry))
                return NotFound($"Geometry for '{name}' not found.");

            return Ok(geometry);
        }
    }
}
