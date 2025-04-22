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

            this.map.layers.add(authorityLayer);
            this.currentLayerId = authorityLayer.id; // Store reference to current layer
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

        const localAuthorities = new atlas.layer.PolygonLayer(source, null, {
            'source-layer': 'local_authorities_layer',
            sourceLayer: 'local_authorities_layer',
            fillColor: 'rgba(0, 100, 255, 0.4)',
            strokeColor: '#004aad',
            strokeWidth: 2
        });

        this.map.layers.add(localAuthorities);
        this.currentLayerId = localAuthorities.id;
    }
};
