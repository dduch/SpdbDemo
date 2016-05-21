///<reference path="FormModule.js" />

FormModule.controller('FormController', ['$scope', 'sharedMapService', '$http', '$mdDialog', '$mdMedia', function ($scope, sharedMapService, $http, $mdDialog, $mdMedia) {
    $scope.navigation = {
        start: '',
        destination: '',
        useMyLocation: false,
        latitude: undefined,
        longitude: undefined,
        autocompleteData: function (query){
            return $scope.querySearch(query);
        },
        selectedStartItem: undefined,
        selectedDestinationItem: undefined,
        startSearchText: "",
        destinationSearchText: "",
        speed: 15,
        pathDetails: new Array()
    };

    $scope.isResult = false;
    $scope.routeDistance = 0.0;
    $scope.routeCost = 0.0;
    var stations = new Array();

    $scope.mapHeight = Math.round($(window).height() * 0.65);

    $scope.showAdvanced = function (ev) {
        $mdDialog.show({
            controller: DialogController,
            templateUrl: 'dialog1.tmpl.html',
            parent: angular.element(document.body),
            targetEvent: ev
        })
        .then(function (answer) {
        }, function () { });
    };

    $scope.showRouteAlert = function (ev) {
        $mdDialog.show(
          $mdDialog.alert()
            .parent(angular.element(document.body))
            .clickOutsideToClose(true)
            .title('Finding route alert')
            .textContent("I'm sorry I can not find any route, change your request or try again.")
            .ariaLabel('Finding route alert')
            .ok('Got it!')
            .targetEvent(ev)
        );
    };

    $scope.showSearchAlert = function (ev) {
        $mdDialog.show(
          $mdDialog.alert()
            .parent(angular.element(document.body))
            .clickOutsideToClose(true)
            .title('Search alert')
            .textContent("Please fill start and destination input. \n Both of them should be completed with one of the autocomplete options.")
            .ariaLabel('Search alert')
            .ok('Got it!')
            .targetEvent(ev)
        );
    };

    angular.element(document).ready(function () {
        sharedMapService.initialize();

        for (station in CONST.stations.Stations) {
            var s = {
                display_name: CONST.stations.Stations[station].Localization + ", " + CONST.stations.Stations[station].District,
                lat: CONST.stations.Stations[station].Latitude,
                lon: CONST.stations.Stations[station].Longitude
            }
            stations.push(s);
        }
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
        else if($scope.isResult == false){
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
                        for (object in response.data) {
                            var toTrim = response.data[object].display_name.indexOf(", województwo");
                            if (toTrim > -1) {
                                response.data[object].display_name = response.data[object].display_name.substring(0, toTrim);
                                result.push(response.data[object]);
                            }
                        }

                        for (station in stations) {
                            if (stations[station].display_name.indexOf(query) > 0) {
                                result.push(stations[station]);
                            }
                        }

                        return result;
                    }, function (response) { });
    },

    $scope.reverseGeocoding = function () {
        $http.get(CONST.NominatimReverse + 'lat=' + $scope.navigation.latitude + '&lon=' + $scope.navigation.longitude + '&addressdetails=1')
                   .then(function (response) {
                       $scope.navigation.selectedStartItem = response.data;
                       var road = response.data.address.road;
                       var houseNumber = response.data.address.house_number;
                       var suburb = response.data.address.suburb;
                       var city = response.data.address.city;
                       $scope.navigation.selectedStartItem.display_name = "";

                       if (road != undefined){
                           $scope.navigation.selectedStartItem.display_name += road;
                       }
                       if(houseNumber != undefined){
                           $scope.navigation.selectedStartItem.display_name += ", " + houseNumber;
                       }
                       if (suburb != undefined) {
                           $scope.navigation.selectedStartItem.display_name += ", " + suburb;
                       }
                       if (city != undefined) {
                           $scope.navigation.selectedStartItem.display_name += ", " + city;
                       }

                   }, function (response) { });
    }

    $scope.onSearchClickAction = function () {

        if ($scope.navigation.selectedDestinationItem == undefined || $scope.navigation.selectedDestinationItem == null
            || $scope.navigation.selectedStartItem == undefined || $scope.navigation.selectedStartItem == null) {
            $scope.showSearchAlert();
            return;
        }

        $scope.routeDistance = 0.0;
        $scope.routeCost = 0.0;

        var requestDTO = {
            StartPosition: {
                latitude: $scope.navigation.selectedStartItem.lat,
                longitude: $scope.navigation.selectedStartItem.lon,
            },
            DestinationPosition: {
                latitude: $scope.navigation.selectedDestinationItem.lat,
                longitude: $scope.navigation.selectedDestinationItem.lon,
            },
            Speed: $scope.navigation.speed / 3.6, // Convert km/h to m/s
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
            $mdDialog.hide();
            $scope.showRouteAlert();
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
            points.push(new OpenLayers.Geometry.Point(data.Waypoints[coordinate].Longitude, data.Waypoints[coordinate].Latitude)
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
        $scope.navigation.pathDetails = new Array();
        for (var i = 0; i < sharedMapService.map.layers.length; ++i) {
            if (sharedMapService.map.layers[i].name == "Markers") {
                sharedMapService.map.layers[i].clearMarkers();
                break;
            }
        }

        for (keypoint in data.Keypoints) {
            if (data.Keypoints[keypoint].DistanceFromPrevious > 0) {
                data.Keypoints[keypoint].DistanceFromPrevious = (data.Keypoints[keypoint].DistanceFromPrevious / 1000).toFixed(2);
            }
            else {
                data.Keypoints[keypoint].DistanceFromPrevious = data.Keypoints[keypoint].DistanceFromPrevious.toFixed(2);
            }
            if (keypoint == 0) {
                addMarkerToLayer(data.Waypoints[0].Longitude, data.Waypoints[0].Latitude, '/Resources/startMarker48.png')
                var info = {
                    From: data.Keypoints[keypoint].Name,
                    Type: "On foot",
                    Distance: data.Keypoints[keypoint].DistanceFromPrevious,
                }
                $scope.navigation.pathDetails.push(info);
            }
            else if (keypoint != data.Keypoints.length - 1) {
                var lat = data.Waypoints[data.Keypoints[keypoint].WaypointIndex].Latitude;
                var lon = data.Waypoints[data.Keypoints[keypoint].WaypointIndex].Longitude;
                addMarkerToLayer(lon, lat, '/Resources/stationMarker48.png')
                var info = {
                    From: data.Keypoints[keypoint].Name,
                    Type: "Cycling",
                    Distance: data.Keypoints[keypoint].DistanceFromPrevious,
                }
                $scope.navigation.pathDetails.push(info);
            }
           else if(keypoint == data.Keypoints.length - 1){
                var lat = data.Waypoints[data.Keypoints[keypoint].WaypointIndex].Latitude;
                var lon = data.Waypoints[data.Keypoints[keypoint].WaypointIndex].Longitude;
                addMarkerToLayer(lon, lat, '/Resources/stopMarker48.png')
                var info = {
                    From: data.Keypoints[keypoint].Name,
                    Type: "On foot",
                    Distance: data.Keypoints[keypoint].DistanceFromPrevious,
                }
                $scope.navigation.pathDetails.push(info);
            }
        }
        $scope.isResult = true;
        $scope.routeDistance = (data.RouteLength/1000).toFixed(3);
        $scope.routeCost = data.EstimatedCost.toFixed(2);
    },

    addMarkerToLayer = function (lat, lon, iconPath) {
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