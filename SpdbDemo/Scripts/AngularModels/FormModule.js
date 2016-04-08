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
            numZoomLevels: 19,
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
        var lat = 52.229833;
        var lon = 21.011734;
        var zoom = 13;
        var lonLat = new OpenLayers.LonLat(lon, lat).transform(new OpenLayers.Projection("EPSG:4326"), sharedMap.map.getProjectionObject());
        sharedMap.map.setCenter(lonLat, zoom);
    }

    return sharedMap;
});


FormModule.directive('googleplace', function () {
    return {
        require: 'ngModel',
        scope: {
            ngModel: '=',
            details: '=?'
        },
        link: function (scope, element, attrs, model) {
            var options = {
                types: [],
                language: 'pl-PL',
                componentRestrictions: { country: "pl" },
            };
            scope.gPlace = new google.maps.places.Autocomplete(element[0], options);

            google.maps.event.addListener(scope.gPlace, 'place_changed', function () {
                scope.$apply(function () {
                    scope.details = scope.gPlace.getPlace();
                    model.$setViewValue(element.val());
                });
            });
        },
    };
});