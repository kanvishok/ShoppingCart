(function (checkoutModule) {

    'use strict';

    checkoutModule.controller('checkoutController', checkoutController);

    checkoutController.$inject = ['$scope', '$sce', '$routeParams','$location', 'apiCallerService'];

    function checkoutController($scope, $sce, $routeParams,$location, apiCallerService) {
        var baseUri = "http://localhost/ShoppingCart.Api/";
        var authenticatedCustomerId = 1;

        var _getBasket = function () {

            var params = {
                //  customerId: authenticatedCustomerId
            };

            var uri = baseUri + "api/basket/get-basket?customerId=" + authenticatedCustomerId;

            apiCallerService.get(uri, params, _getBasketCompleted, _getBasketFaild);
        }

        var _getBasketCompleted = function (response) {

            $scope.basket = response.data;
        }

        var _getBasketFaild = function (response) {

            alert("Opps..!" + response.data.ExceptionMessage);
        }

        var _checkout = function () {

            var params = {
                  customerId: authenticatedCustomerId
            };

            var uri = baseUri + "api/checkout/checkout-basket";

            apiCallerService.post(uri, params, _checkoutCompleted, _checkoutFaild);
        }

        var _checkoutCompleted = function (response) {

            $scope.basket = response.data;

            $location.path("/confirmation");
        }

        var _checkoutFaild = function (response) {

            alert("Opps..!" + response.data.ExceptionMessage);
        }

        var _backToProducts = function() {

            $location.path('/products');
        }
        $scope.getBasket = _getBasket;
        $scope.backToProducts = _backToProducts;
        $scope.checkout = _checkout;

        $scope.getBasket();
    }

})(angular.module('checkoutModule'));