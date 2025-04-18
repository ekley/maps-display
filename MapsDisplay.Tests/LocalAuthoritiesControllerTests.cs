using MapsDisplay.Features.LocalAuthority.Controllers;
using MapsDisplay.Features.LocalAuthority.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Shared.Models;

namespace MapsDisplay.Tests
{
    public class LocalAuthoritiesControllerTests
    {
        private readonly Mock<ILocalAuthorityService> mockService;
        private readonly Mock<ILogger<LocalAuthoritiesController>> mockLogger;
        private readonly LocalAuthoritiesController controller;

        public LocalAuthoritiesControllerTests()
        {
            mockService = new Mock<ILocalAuthorityService>();
            mockLogger = new Mock<ILogger<LocalAuthoritiesController>>();
            controller = new LocalAuthoritiesController(mockService.Object, mockLogger.Object);
        }
        public void SetupMockLookup(Dictionary<string, GeometryDto>  mockData)
        {
            mockService.Setup(s => s.LoadLookup()).Returns(mockData);
        }

        private Dictionary<string, GeometryDto> LoadTestDataFromJson(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "TestData", fileName);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Test data file '{fileName}' was not found at path: {path}");
            }

            var json = File.ReadAllText(path);
            var data = JsonConvert.DeserializeObject<Dictionary<string, GeometryDto>>(json);

            if (data == null)
            {
                throw new InvalidOperationException($"Test data from '{fileName}' is null.");
            }

            return data;
        }

        [Fact]
        public void GetGeometryByName_ShouldReturnGeometry_WhenNameExists()
        {
            var mockData = LoadTestDataFromJson("MockGeometry.json");
            SetupMockLookup(mockData);

            var result = controller.GetGeometryByName("Oxford");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var geometry = Assert.IsType<GeometryDto>(okResult.Value);
            Assert.NotNull(geometry);
        }

        [Fact]
        public void GetSimilarNames_ShouldReturnMatchingNames_WhenPartialNameProvided()
        {
            
            var mockData = new Dictionary<string, GeometryDto>
                {
                    { "Broxbourne", new GeometryDto() },
                    { "Broxtowe", new GeometryDto() },
                    { "Oxford", new GeometryDto() },
                    { "South Oxfordshire", new GeometryDto() },
                    { "West Oxfordshire", new GeometryDto() }
                };

            SetupMockLookup(mockData);

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