///<reference path="Constants.js" />

var FormModule = angular.module('FormApp', ['ngMaterial']);

FormModule.factory('sharedMapService', function () {
    var sharedMap = {};
    sharedMap.map;
    sharedMap.initialize = function (){
        sharedMap.map = new OpenLayers.Map("map_canvas", {
            controls: [
                new OpenLayers.Control.Navigation(),
                new OpenLayers.Control.PanZoomBar(),
                new OpenLayers.Control.LayerSwitcher(),
                new OpenLayers.Control.Attribution()],
            maxExtent: new OpenLayers.Bounds(-20037508.34, -20037508.34, 20037508.34, 20037508.34),
            maxResolution: 156543.0399,
            numZoomLevels: 18,
            units: 'm',
            projection: new OpenLayers.Projection("EPSG:900913"),
            displayProjection: new OpenLayers.Projection("EPSG:4326")
        });

        // Define the map layer
        // Here we use a predefined layer that will be kept up to date with URL changes
        layerMapnik = new OpenLayers.Layer.OSM.Mapnik("Mapnik");
        sharedMap.map.addLayer(layerMapnik);
        layerCycleMap = new OpenLayers.Layer.OSM.CycleMap("CycleMap");
        sharedMap.map.addLayer(layerCycleMap);
        layerMarkers = new OpenLayers.Layer.Markers("Markers");
        sharedMap.map.addLayer(layerMarkers);

        // Add the Layer with the GPX Track
        var lgpx = new OpenLayers.Layer.Vector("Lakeside cycle ride", {
            strategies: [new OpenLayers.Strategy.Fixed()],
            protocol: new OpenLayers.Protocol.HTTP({
                url: "around_lake.gpx",
                format: new OpenLayers.Format.GPX()
            }),
            style: { strokeColor: "green", strokeWidth: 5, strokeOpacity: 0.5 },
            projection: new OpenLayers.Projection("EPSG:4326")
        });
        sharedMap.map.addLayer(lgpx);

        // Start position for the map (hardcoded here for simplicity,
        // but maybe you want to get this from the URL params)
        var lonLat = new OpenLayers.LonLat(CONST.WarsawLongitude, CONST.WarsawLatitude).transform(new OpenLayers.Projection("EPSG:4326"), sharedMap.map.getProjectionObject());
        sharedMap.map.setCenter(lonLat, CONST.DefaultZoom);
    }

    return sharedMap;
});
