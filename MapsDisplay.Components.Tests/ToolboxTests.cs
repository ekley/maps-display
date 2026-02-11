using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MapsDisplay.Client.Components;
using Shared.Models;
using Shared.Services.Interfaces;
using Shared.Utilities;

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
            var component = RenderComponent<Toolbox>();
            var toggleButton = component.Find("#toggleLayer");
            var searchInput = component.Find(".search-box input");

            Assert.NotNull(toggleButton);
            Assert.NotNull(searchInput);
        }

        [Fact]
        public async Task SearchClick_Should_Call_BboxByNameAsync()
        {
            string testQuery = "Oxford";
            var mockGeometryDto = Result<GeometryDto>.Success(new GeometryDto
            {
                Type = "Polygon",
                Coordinates = new object()
            });

            _mockAuthorityService.Setup(service => service.BboxByNameAsync(testQuery)) // Mock the service call
                .ReturnsAsync(mockGeometryDto);
            JSInterop.SetupVoid("azureMaps.setFilter", _ => true); // Mock JS interop call

            var component = RenderComponent<Toolbox>();
            var searchBtn = component.Find("#search");
            var searchInput = component.Find("input[type='text']");

            searchInput.Change(testQuery);
            searchBtn.Click();

            await Task.Delay(800);
            _mockAuthorityService.Verify(service => service.BboxByNameAsync(testQuery), Times.Once); // Verify the service call was made
        }

        [Fact]
        public void HideDistrictsButton_Should_Call_JS_ClearMap()
        {
            JSInterop.SetupVoid("azureMaps.clearMap", _ => true); // Mock JS interop call

            var component = RenderComponent<Toolbox>();
            var hideButton = component.Find("button#toggleLayer");

            hideButton.Click();

            JSInterop.VerifyInvoke("azureMaps.clearMap"); // Verify the JS call was made
        }

        [Fact]
        public void SearchInput_Should_UpdateInputValue_WhenUserTypes()
        {
            var component = RenderComponent<Toolbox>();
            var searchInput = component.Find("input[type='text']");
            string testQuery = "Oxford";

            searchInput.Change(testQuery);

            Assert.Equal(testQuery, searchInput.GetAttribute("value"));
        }

    }
}