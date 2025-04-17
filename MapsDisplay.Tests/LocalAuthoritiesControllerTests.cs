using MapsDisplay.Features.LocalAuthority.Controllers;
using MapsDisplay.Features.LocalAuthority.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Shared.Models;

namespace MapsDisplay.Tests
{
    public class LocalAuthoritiesControllerTests
    {
        [Fact]
        public void GetGeometryByName_ReturnsOk_WhenFound()
        {
            string MockGeoPath = Path.Combine(
                Directory.GetCurrentDirectory(), "TestData", "MockGeometry.json");
            var json = File.ReadAllText(MockGeoPath);
            var geometryData = JsonConvert.DeserializeObject<Dictionary<string, GeometryDto>>(json);

            var mockService = new Mock<ILocalAuthorityService>();

            mockService.Setup(s => s.LoadLookup()).Returns(geometryData);

            var controller = new LocalAuthoritiesController(mockService.Object);

            var result = controller.GetGeometryByName("Oxford");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var geometry = Assert.IsType<GeometryDto>(okResult.Value);
            Assert.NotNull(geometry);
        }

        [Fact]
        public void GetSimilarNames_ReturnsOk_WhenFound()
        {
            var mockService = new Mock<ILocalAuthorityService>();
            var mockData = new Dictionary<string, GeometryDto>
                {
                    { "Broxbourne", new GeometryDto() },
                    { "Broxtowe", new GeometryDto() },
                    { "Oxford", new GeometryDto() },
                    { "South Oxfordshire", new GeometryDto() },
                    { "West Oxfordshire", new GeometryDto() }
                };

            mockService.Setup(s => s.LoadLookup()).Returns(mockData);

            var controller = new LocalAuthoritiesController(mockService.Object);

            var result = controller.GetSimilarNames("Ox");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var similarNames = Assert.IsType<List<string>>(okResult.Value);

            Assert.Contains("Oxford", similarNames);
            Assert.Contains("South Oxfordshire", similarNames);
            Assert.Contains("West Oxfordshire", similarNames);
            Assert.True(similarNames.Count <= 6);
        }
    }
}