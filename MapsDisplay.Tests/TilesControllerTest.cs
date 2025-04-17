using MapsDisplay.Features.LocalAuthority.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace MapsDisplay.Tests
{
    public class TileDatabaseFixture : IDisposable
    {
        public string TempDbPath { get; private set; }

        public TileDatabaseFixture()
        {
            TempDbPath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "test.mbtiles");

            // Setup the database before any tests run
            using (var connection = new SqliteConnection($"Data Source={TempDbPath};"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS tiles (
                    zoom_level INTEGER, 
                    tile_column INTEGER, 
                    tile_row INTEGER, 
                    tile_data BLOB
                );
            ";
                command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            using (var connection = new SqliteConnection($"Data Source={TempDbPath};"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DROP TABLE IF EXISTS tiles;";
                command.ExecuteNonQuery();
            }
        }

        public void InsertTile(int zoom, int x, int y, byte[] tileData)
        {
            using (var connection = new SqliteConnection($"Data Source={TempDbPath};"))
            {
                connection.Open();
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"
                INSERT INTO tiles (zoom_level, tile_column, tile_row, tile_data) 
                VALUES (@z, @x, @y, @data);
            ";
                insertCommand.Parameters.AddWithValue("@z", zoom);
                insertCommand.Parameters.AddWithValue("@x", x);
                insertCommand.Parameters.AddWithValue("@y", y);
                insertCommand.Parameters.AddWithValue("@data", tileData);
                insertCommand.ExecuteNonQuery();
            }
        }
    }
    public class TilesControllerTest:IClassFixture<TileDatabaseFixture>
    {
        private TileDatabaseFixture _fixture;

        public TilesControllerTest(TileDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetTile_ReturnsTile_WhenTileFound()
        {
            // Setup: Insert mock tile data
            _fixture.InsertTile(5, 10, 11, new byte[] { 1, 2, 3, 4 });

            var controller = new TilesController(_fixture.TempDbPath);

            var result = controller.GetTile(5, 10, 20);

            var fileContentResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/x-protobuf", fileContentResult.ContentType);
            Assert.NotNull(fileContentResult.FileContents);
            var expectedData = new byte[] { 1, 2, 3, 4 };
            Assert.Equal(expectedData, fileContentResult.FileContents);
        }

        [Fact]
        public void GetTile_Returns204NoContent_WhenTileNotFound()
        {
            var controller = new TilesController(_fixture.TempDbPath);

            var result = controller.GetTile(108, 10, 20);

            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }
    }
}
