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
            // TO-DO
        });
    }
};
