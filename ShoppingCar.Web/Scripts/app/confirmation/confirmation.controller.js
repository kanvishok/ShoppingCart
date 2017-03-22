(function (confirmationModule) {

    'use strict';

    confirmationModule.controller('confirmationController', confirmationController);

    confirmationController.$inject = ['$scope', '$sce', '$routeParams', 'apiCallerService'];

    function confirmationController($scope, $sce, $routeParams, apiCallerService) {
        
        //var _getBasket = function () {

        //    var params = {
        //        //  customerId: authenticatedCustomerId
        //    };

        //    var uri = baseUri + "api/basket/get-checkout?customerId=" + authenticatedCustomerId;

        //    apiCallerService.get(uri, params, _getBasketCompleted, _getBasketFaild);
        //}

        //var _getBasketCompleted = function (response) {

        //    $scope.basket = response.data;
        //}

        //var _getBasketFaild = function (response) {

        //    alert("Opps..!" + response.data.ExceptionMessage);
        //}
    }

})(angular.module('confirmationModule'));