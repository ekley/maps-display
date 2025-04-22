namespace Shared.Models
{
    public class LocalAuthorityBoundary
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string GeoJson { get; set; }

        public LocalAuthorityBoundary(string name, string code, string geoJson)
        {
            Name = name;
            Code = code;
            GeoJson = geoJson;
        }
    }

    public class FeatureCollection
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<Feature> Features { get; set; } = new List<Feature>();
    }

    public class Feature
    {
        public string Type { get; set; } = string.Empty;
        public Geometry Geometry { get; set; } = new Geometry();
        public Properties Properties { get; set; } = new Properties();
    }

    public class Geometry
    {
        public string Type { get; set; } = string.Empty;
        public object Coordinates { get; set; } = new object();
    }

    public class Properties
    {
        public string Name { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
    }
}
