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
        public string type { get; set; }
        public string name { get; set; }
        public List<Feature> features { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public object coordinates { get; set; }
    }

    // { "dataset": "local-authority-district", "end-date": "", "entity": "8600000", "entry-date": "2024-06-28", "name": "Hartlepool", "organisation-entity": "10", "prefix": "statistical-geography", "reference": "E06000001", "start-date": "", "typology": "geography" }
    public class Properties
    {
        public string name { get; set; }
        public string reference { get; set; }
    }
}
