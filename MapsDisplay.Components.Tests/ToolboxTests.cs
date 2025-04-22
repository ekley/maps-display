using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MapsDisplay.Client.Components;
using Shared.Models;
using Shared.Services.Interfaces;

namespace MapsDisplay.Components.Tests
{
    public class ToolboxTests : TestContext
    {
        private readonly Mock<ILocalAuthorityApiService> _mockAuthorityService;

        public ToolboxTests()
        {
            _mockAuthorityService = new Mock<ILocalAuthorityApiService>();
            Services.AddSingleton(_mockAuthorityService.Object);
        }

        [Fact]
        public void Toolbox_Rendering()
        {
            var cut = RenderComponent<Toolbox>();
            var toggleButton = cut.Find("#toggleLayer");
            var searchInput = cut.Find(".search-box input");

            Assert.NotNull(toggleButton);
            Assert.NotNull(searchInput);
        }

        [Fact]
        public async Task SearchClick_Should_Call_BboxByNameAsync()
        {
            string testQuery = "Oxford";
            var expectedGeometry = new GeometryDto
            {
                Coordinates = new List<double>(),
                Type = "Polygon"
            };
            _mockAuthorityService.Setup(service => service.BboxByNameAsync(testQuery))
                        .ReturnsAsync(expectedGeometry);
            
            var ToolBoxComponent = RenderComponent<Toolbox>();
            var searchBtn = ToolBoxComponent.Find("#search");

            ToolBoxComponent.Find("input[type='text']").Change(testQuery);
            searchBtn.Click();

            await Task.Delay(800);
            _mockAuthorityService.Verify(service => service.BboxByNameAsync(testQuery), Times.Once);
        }

    }
}