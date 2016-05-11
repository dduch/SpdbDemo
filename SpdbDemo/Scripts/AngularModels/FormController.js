///<reference path="FormModule.js" />

FormModule.controller('FormController', ['$scope', 'sharedMapService', '$http', '$mdDialog', '$mdMedia', function ($scope, sharedMapService, $http, $mdDialog, $mdMedia) {
    $scope.navigation = {
        start: '',
        destination: '',
        useMyLocation: false,
        latitude: undefined,
        longitude: undefined,
        autocompleteData: function (query) {
            return $scope.querySearch(query);
        },
        selectedStartItem: undefined,
        selectedDestinationItem: undefined,
        startSearchText: "",
        destinationSearchText: "",
        speed: 1,
    };

    $scope.showAdvanced = function (ev) {
        $mdDialog.show({
            controller: DialogController,
            templateUrl: 'dialog1.tmpl.html',
            parent: angular.element(document.body),
            targetEvent: ev
        })
        .then(function (answer) {
        }, function () {});
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
                $scope.reverseGeocoding();
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
        return $http.get(CONST.NominatimSearch + query + ",Warszawa" + CONST.FormatType + '&addressdetails=1')
                    .then(function (response) {
                        var result = new Array();
                        for(object in response.data)
                        {
                            var toTrim = response.data[object].display_name.indexOf(",województwo");
                            response.data[object].display_name = response.data[object].display_name.substring(0, toTrim);
                            result.push();
                        }
                        return result;
                    }, function (response) { });
    },

    $scope.reverseGeocoding = function () {
        $http.get(CONST.NominatimReverse + 'lat=' + $scope.navigation.latitude + '&lon=' + $scope.navigation.longitude + '&addressdetails=1')
                   .then(function (response) {
                       $scope.navigation.selectedStartItem = response.data;
                       $scope.navigation.selectedStartItem.display_name = response.data.address.road + ', ' +
                           response.data.address.house_number + ', ' + response.data.address.suburb + ', ' + response.data.address.city;
                   }, function (response) { });
    }

    $scope.onSearchClickAction = function () {
        var requestDTO = {
            StartPosition: {
                latitude: $scope.navigation.selectedStartItem.lat,
                longitude: $scope.navigation.selectedStartItem.lon,
            },
            DestinationPosition: {
                latitude: $scope.navigation.selectedDestinationItem.lat,
                longitude: $scope.navigation.selectedDestinationItem.lon,
            },
            Speed: $scope.navigation.speed,
        }

     $http({
            method: 'POST',
            url: window.location.origin + '/api/Route/FindRoute',
            data: JSON.stringify(requestDTO),
        }).then(function successCallback(response) {
            for (var i = 0; i < sharedMapService.map.layers.length; ++i) {
                if (sharedMapService.map.layers[i].name == "Route") {
                    sharedMapService.map.removeLayer(sharedMapService.map.layers[i]);
                    break;
                }
            }
            $scope.drawRoute(response.data);
            $mdDialog.hide();
        }, function errorCallback(response) {
            alert(response);
        });

     $scope.showAdvanced();
    },

    $scope.drawRoute = function (data) {
        var lineLayer = new OpenLayers.Layer.Vector("Route");
        sharedMapService.map.addLayer(lineLayer);
        sharedMapService.map.addControl(new OpenLayers.Control.DrawFeature(lineLayer, OpenLayers.Handler.Path));

        var points = new Array();
        var fromProjection = new OpenLayers.Projection("EPSG:4326"); 
        var toProjection = new OpenLayers.Projection("EPSG:900913"); 

        for (coordinate in data.Waypoints) {
            points.push(new OpenLayers.Geometry.Point(data.Waypoints[coordinate].Latitude, data.Waypoints[coordinate].Longitude)
                .transform(fromProjection, toProjection));
        }

        var line = new OpenLayers.Geometry.LineString(points);

        var style = {
            strokeColor: '#09295a',
            strokeOpacity: 0.5,
            strokeWidth: 5
        };

        var lineFeature = new OpenLayers.Feature.Vector(line, null, style);
        lineLayer.addFeatures([lineFeature]);
        drawMarkers(data);
    },

    drawMarkers = function (data) {
        for (var i = 0; i < sharedMapService.map.layers.length; ++i) {
            if (sharedMapService.map.layers[i].name == "Markers") {
                sharedMapService.map.layers[i].clearMarkers();
                break;
            }
        }

        if (data.Waypoints[0].Latitude != data.Waypoints[data.Stations[0].WaypointIndex].Latitude
            && data.Waypoints[0].Longitude != data.Waypoints[data.Stations[0].WaypointIndex].Longitude) {
            addMarkerToLayer(data.Waypoints[0].Latitude, data.Waypoints[0].Longitude, '/Resources/startMarker48.png')
        }

        if (data.Waypoints[data.Waypoints.length - 1].Latitude != data.Waypoints[data.Stations[data.Stations.length - 1].WaypointIndex].Latitude
            && data.Waypoints[data.Waypoints.length - 1].Longitude != data.Waypoints[data.Stations[data.Stations.length - 1].WaypointIndex].Longitude) {
            addMarkerToLayer(data.Waypoints[data.Waypoints.length - 1].Latitude, data.Waypoints[data.Waypoints.length - 1].Longitude, '/Resources/stopMarker48.png')
        }

        for(station in data.Stations){
            var lat = data.Waypoints[data.Stations[station].WaypointIndex].Latitude;
            var lon = data.Waypoints[data.Stations[station].WaypointIndex].Longitude;
            addMarkerToLayer(lat, lon, '/Resources/stationMarker48.png')
        }
    },

    addMarkerToLayer = function(lat, lon, iconPath){
        var fromProjection = new OpenLayers.Projection("EPSG:4326");   // Transform from WGS 1984
        var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Proj
        var position = new OpenLayers.LonLat(lat, lon).transform(fromProjection, toProjection);
        var size = new OpenLayers.Size(48, 48);
        var offset = new OpenLayers.Pixel(-(size.w / 2), -size.h);
        var icon = new OpenLayers.Icon(iconPath, size, offset);

        for (var i = 0; i < sharedMapService.map.layers.length; ++i) {
            if (sharedMapService.map.layers[i].name == "Markers") {
                sharedMapService.map.layers[i].addMarker(new OpenLayers.Marker(position, icon));
                break;
            }
        }
    }

}]);

function DialogController($scope, $mdDialog) {
    $scope.hide = function () {
        $mdDialog.hide();
    };

    $scope.cancel = function () {
        $mdDialog.cancel();
    };

    $scope.answer = function (answer) {
        $mdDialog.hide();
    };
}