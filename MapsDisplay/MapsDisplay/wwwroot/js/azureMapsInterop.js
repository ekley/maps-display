window.azureMaps = {
    map: null,
    polygonLayer: null,
    createMap: function (elementId, mapsKey) {
        this.map = new atlas.Map(elementId, {
            center: [-0.1276, 51.5074],  // London coordinates
            zoom: 14,
            view: 'Auto',
            language: 'en-US',
            authType: 'subscriptionKey',
            subscriptionKey: mapsKey
        });

        this.map.events.add('ready', () => {
            this.addGeoJsonData();
        });
    },
    clearMap: function () {
        if (!this.map || !this.polygonLayer) return;

        const layerId = this.polygonLayer.id;
        const doesLayerExist = this.map.layers._getMapboxLayerExists(layerId);

        if (doesLayerExist) {
            this.map.layers._removeLayer(layerId)
        } else {
            this.map.layers.add(this.polygonLayer);
        }
    },
    setFilter: function (areaName, geometry) {
        const shape = {
            type: "Feature",
            geometry: {
                ...geometry
            },
            properties: {}
        };
        const isValid = turf.booleanValid(shape);

        this.polygonLayer.setOptions({
            filter: ['==', ['get', 'name'], areaName]
        });
        if (isValid) {
            const bbox = turf.bbox(shape);

            // take the map camera to the searched place
            this.map.setCamera({
                bounds: bbox,
                padding: 40,
                maxZoom: 12
            });
        }
    },
    addGeoJsonData: function () {
        const source = new atlas.source.VectorTileSource('localAuthorities', {
            tiles: ['https://localhost:7177/api/tiles/{z}/{x}/{y}.pbf'],
            maxZoom: 22,
        });
        this.polygonLayer = new atlas.layer.PolygonLayer(source, null, {
            "source-layer": 'local_authorities_layer',
            sourceLayer: 'local_authorities_layer',
            fillColor: 'rgba(0, 100, 255, 0.4)',
            strokeColor: '#004aad',
            strokeWidth: 2
        });

        this.map.sources.add(source);
        this.map.layers.add(this.polygonLayer);
    }
};
