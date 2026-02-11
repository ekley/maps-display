using NetTopologySuite.Features;
using NetTopologySuite.IO;
using NetTopologySuite.Geometries;
using System.Text.Json;

namespace MapsDisplay.Features.LocalAuthority.Services.Builders
{
    public static class LookupGenerator
    {
        public static void Generate()
        {
            const string GEOJSON_FILE_NAME = "local_authority_district.geojson";
            string datasetsDirectory = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Features", "LocalAuthority", "Data", "Datasets");
            string geoJsonPath = Path.Combine(datasetsDirectory, GEOJSON_FILE_NAME);
            string jsonPath = Path.Combine(datasetsDirectory, "lookup.json");

            if (File.Exists(jsonPath))
            {
                Console.WriteLine("lookup.json file already exists. Skipping file generating");
                return;
            }

            var geoJsonText = File.ReadAllText(geoJsonPath);
            var reader = new GeoJsonReader();
            var featureCollection = reader.Read<FeatureCollection>(geoJsonText);

            var lookup = new Dictionary<string, object>();

            foreach (var feature in featureCollection)
            {
                var name = feature.Attributes["name"]?.ToString();
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Feature with missing or null 'name' attribute encountered. Skipping.");
                    continue;
                }

                var geometry = feature.Geometry;
                var geometryData = new object();

                if (geometry is Point point)
                {
                    geometryData = new
                    {
                        type = "Point",
                        coordinates = new[] { point.X, point.Y }
                    };
                }
                else if (geometry is Polygon polygon)
                {
                    geometryData = new
                    {
                        type = "Polygon",
                        coordinates = new[]
                        {
                                polygon.Shell.Coordinates
                                    .Select(c => new[] { c.X, c.Y })
                                    .ToArray()
                            }
                        .Concat(
                            polygon.Holes.Select(
                                hole => hole.Coordinates
                                            .Select(c => new[] { c.X, c.Y })
                                            .ToArray()
                            )
                        ).ToArray()
                    };
                }
                else if (geometry is MultiPolygon multiPolygon)
                {
                    geometryData = new
                    {
                        type = "MultiPolygon",
                        coordinates = multiPolygon.Geometries
                            .Select(p => p.Coordinates
                                .Select(c => new[] { c.X, c.Y })
                                .ToArray())
                            .ToArray()
                    };
                }
                else
                {
                    geometryData = new
                    {
                        type = geometry.GeometryType,
                        coordinates = "Unsupported geometry type"
                    };
                }

                lookup[name] = geometryData;
            }

            var outputJson = JsonSerializer.Serialize(lookup);
            File.WriteAllText(jsonPath, outputJson);

            Console.WriteLine("lookup.json has been created!");
        }
    }
}
