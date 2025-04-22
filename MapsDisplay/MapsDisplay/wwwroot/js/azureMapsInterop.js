window.azureMaps = {
    map: null,
    currentLayerId: null,

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
        if (!this.map || !this.currentLayerId) return;
        const doesLayerExist = this.map.layers._getMapboxLayerExists(this.currentLayerId);

        if (doesLayerExist) {
            this.map.layers._removeLayer(this.currentLayerId)
        }

        this.currentLayerId = null;
    },
    addLayer: function (source, srcLayerName = undefined) {
        const sourceLayerProp = srcLayerName ? {
            'source-layer': srcLayerName,
            sourceLayer: srcLayerName,
        } : {};
        const layer = new atlas.layer.PolygonLayer(source, null, {
            ...sourceLayerProp, // optional property
            fillColor: 'rgba(0, 100, 255, 0.4)',
            strokeColor: '#004aad',
            strokeWidth: 2
        });

        this.map.sources.add(source);
        this.map.layers.add(layer);
        this.currentLayerId = layer.id;
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
            const source = new atlas.source.DataSource();

            source.add(shape);
            this.addLayer(source);

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

        this.addLayer(source, 'local_authorities_layer');
    }
};
