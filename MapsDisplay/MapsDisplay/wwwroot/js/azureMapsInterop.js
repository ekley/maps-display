window.azureMaps = {
    map: null,
    currentLayer: null,

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
        if (!this.map || !this.currentLayer) return;

        const layerId = this.currentLayer.id;
        const doesLayerExist = this.map.layers._getMapboxLayerExists(layerId);

        if (doesLayerExist) {
            this.map.layers._removeLayer(layerId)
        }

        this.currentLayer = null;
    },
    setFilter: function (areaName, geometry) {
        const shape = {
            type: "Feature",
            geometry: {
                ...geometry
            },
            properties: { name: areaName }
        };
        const isValid = turf.booleanValid(shape);


        if (isValid) {
            const bbox = turf.bbox(shape);
            const dataSource = new atlas.source.DataSource();
            dataSource.add(shape);
            this.map.sources.add(dataSource);

            const authorityLayer = new atlas.layer.PolygonLayer(dataSource, null, {
                fillColor: 'rgba(0, 100, 255, 0.4)',
                strokeColor: '#004aad',
                strokeWidth: 2
            });

            // Apply filter to show only the matching feature by name
            authorityLayer.setOptions({
                filter: ['==', ['get', 'name'], areaName]
            });

            this.map.layers.add(authorityLayer);
            this.currentLayer = authorityLayer; // Store reference to current layer
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
        this.map.sources.add(source);

        this.currentLayer = new atlas.layer.PolygonLayer(source, null, {
            'source-layer': 'local_authorities_layer',
            sourceLayer: 'local_authorities_layer',
            fillColor: 'rgba(0, 100, 255, 0.4)',
            strokeColor: '#004aad',
            strokeWidth: 2
        });
        this.map.layers.add(this.currentLayer);
    }
};
