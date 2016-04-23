﻿///<reference path="FormModule.js" />

FormModule.controller('FormController', ['$scope', 'sharedMapService','$http', function ($scope, sharedMapService, $http) {
    $scope.navigation = {
        start: '',
        destination: '',
        useMyLocation: false,
        latitude: undefined,
        longitude: undefined,
        autocompleteData: function(query){
            return $scope.querySearch(query);
        },
        selectedStartItem: undefined,
        selectedDestinationItem: undefined,
        startSearchText: "",
        destinationSearchText: ""
    };

    angular.element(document).ready(function () {
        sharedMapService.initialize();
    });


    $scope.onUseMyLocationChange = function () {
        if ($scope.navigation.useMyLocation == true && navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                $scope.navigation.latitude = position.coords.latitude;
                $scope.navigation.longitude = position.coords.longitude;
                var lat = $scope.navigation.latitude;
                var lon = $scope.navigation.longitude;
                var fromProjection = new OpenLayers.Projection("EPSG:4326");   // Transform from WGS 1984
                var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection
                var position = new OpenLayers.LonLat(lon, lat).transform(fromProjection, toProjection);

                for (var i = 0; i < sharedMapService.map.layers.length; ++i) {
                    if (sharedMapService.map.layers[i].name == "Markers") {
                        sharedMapService.map.layers[i].addMarker(new OpenLayers.Marker(position));
                        break;
                    }
                }
            });
        }
        else {
            for (var i = 0; i < sharedMapService.map.layers.length; ++i) {
                if (sharedMapService.map.layers[i].name == "Markers") {
                    sharedMapService.map.layers[i].clearMarkers();
                    break;
                }
            }
        }
    },

    $scope.querySearch = function (query) {
        return $http.get(CONST.NominatimSearch + query + CONST.FormatType)
                    .then(function(response) {
                        return response.data;
                    }, function(response) {});
    },

    $scope.onSearchClickAction = function(){
     $http({
            method: 'POST',
            url: window.location.origin + '/api/Route/FindRoute',
            data: {
                Lat: $scope.navigation.latitude,
                Long: $scope.navigation.longitude
            }
        }).then(function successCallback(response) {
            $scope.drawRoute(response.data);
        }, function errorCallback(response) {
            alert(response);
        });
    },

    $scope.drawRoute = function(data){
        var lineLayer = new OpenLayers.Layer.Vector("Route");
        sharedMapService.map.addLayer(lineLayer);
        sharedMapService.map.addControl(new OpenLayers.Control.DrawFeature(lineLayer, OpenLayers.Handler.Path));

        var points = new Array();
        var fromProjection = new OpenLayers.Projection("EPSG:4326");   // Transform from WGS 1984
        var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection

        for (coordinate in data)
        {
            points.push(new OpenLayers.Geometry.Point(data[coordinate].Latitude, data[coordinate].Longitude)
                .transform(fromProjection, toProjection));
        }

        var line = new OpenLayers.Geometry.LineString(points);

        var style = {
            strokeColor: '#ff0000',
            strokeOpacity: 0.5,
            strokeWidth: 5
        };

        var lineFeature = new OpenLayers.Feature.Vector(line, null, style);
        lineLayer.addFeatures([lineFeature]);
    }

}]);