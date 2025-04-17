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
            string geoJsonDirectory = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Features", "LocalAuthority", "Data", "Datasets");

            string geoJsonPath = Path.Combine(geoJsonDirectory, "local_authority_district.geojson");
            if (File.Exists(geoJsonPath))
            {
                Console.WriteLine("File already exists. Skipping Docker command.");
                return; // Return early if the file already exists
            }

            var geoJsonText = File.ReadAllText(geoJsonPath);
            var reader = new GeoJsonReader();
            var featureCollection = reader.Read<FeatureCollection>(geoJsonText);

            var lookup = new Dictionary<string, object>();

            foreach (var feature in featureCollection)
            {
                var name = feature.Attributes["name"].ToString();
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

            string jsonPath = Path.Combine(geoJsonDirectory, "lookup.json");
            var outputJson = JsonSerializer.Serialize(lookup);
            File.WriteAllText(jsonPath, outputJson);

            Console.WriteLine("lookup.json has been created!");
        }
    }
}
