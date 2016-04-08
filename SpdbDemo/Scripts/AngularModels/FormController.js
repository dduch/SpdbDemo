///<reference path="FormModule.js" />

FormModule.controller('FormController', ['$scope', 'sharedMapService', function ($scope, sharedMapService) {
    $scope.navigation = {
        start: '',
        destination: '',
        useMyLocation: false,
        latitude: undefined,
        longitude: undefined
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
    }
}]);