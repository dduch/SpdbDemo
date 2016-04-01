var FormModule = angular.module('FormApp', ['ngMaterial']);

FormModule.controller('FormController', ['$scope', function ($scope) {
    $scope.navigation = {
        start: '',
        destination: '',
        useMyLocation: false
    };
}]);


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