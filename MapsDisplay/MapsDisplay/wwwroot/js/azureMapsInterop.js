window.azureMaps = {
    map: null,
    polygonLayer: null,
    createMap: (elementId, mapsKey) => {
        this.map = new atlas.Map(elementId, {
            center: [-0.1276, 51.5074],  // London coordinates
            zoom: 14,
            view: 'Auto',
            language: 'en-US',
            authType: 'subscriptionKey',
            subscriptionKey: mapsKey
        });

        this.map.events.add('ready', () => {
            // TO-DO
        });
    },
    addGeoJsonData: () => {
        // Create a VectorTileSource to load tiles from your API endpoint
        const source = new atlas.source.VectorTileSource('localAuthorities', {
            tiles: ['https://localhost:7177/api/tiles/{z}/{x}/{y}.pbf'],
            maxZoom: 22,
        });
        const polygonLayer = new atlas.layer.PolygonLayer(source, null, {
            "source-layer": 'local_authorities_layer',
            sourceLayer: 'local_authorities_layer',
            fillColor: 'rgba(0, 100, 255, 0.4)',
            strokeColor: '#004aad',
            strokeWidth: 2
        });

        this.map.sources.add(source);
        this.map.layers.add(polygonLayer);
    },
    setFilter: function (areaName) {
        this.polygonLayer.setOptions({
            filter: ['==', ['get', 'name'], areaName]
        });
    }
};
