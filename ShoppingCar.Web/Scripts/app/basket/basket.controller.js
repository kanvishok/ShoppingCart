(function (basketModule) {

    'use strict';

    basketModule.controller('basketController', basketController);

    basketController.$inject = ['$scope', '$sce', '$routeParams', '$location', 'apiCallerService'];

    function basketController($scope, $sce, $routeParams, $location, apiCallerService) {

        var baseUri = "http://localhost/ShoppingCart.Api/";
        var authenticatedCustomerId = 1;

        var _getProducts = function () {

            var params = {};
            var uri = baseUri + "api/basket/list-items";

            apiCallerService.get(uri, params, _productLoadCompleted, _productLoadFaild);
        }

        var _productLoadCompleted = function (response) {

            $scope.products = response.data.AllItems;
        }

        var _productLoadFaild = function (response) {

            alert("Opps..!" + response.data.ExceptionMessage);
        }
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

        var _addToBasket = function (productId, quantity) {

            var params = {
                customerId: authenticatedCustomerId,
                productId: productId,
                quantity: Number(quantity)
            };
            var uri = baseUri + "api/basket/add-to-basket";

            apiCallerService.post(uri, params, _addToBasketCompleted, _addToBasketFaild);
        }


        var _addToBasketCompleted = function (response) {

            _getProducts();
            _getBasket();
            alert("Great..!" + "Product added successfully..!");
        }

        var _addToBasketFaild = function (response) {

            alert("Opps..!" + response.data.ExceptionMessage);
        }

        var _editBasketQuantity = function (productId, quantity) {

            var params = {
                customerId: authenticatedCustomerId,
                productId: productId,
                quantity: Number(quantity)
            };

            var uri = baseUri + "api/basket/edit-basket-item-quantity";

            apiCallerService.post(uri, params, _editBasketQuantityCompleted, _editBasketQuantityFaild);
        }

        var _editBasketQuantityCompleted = function (response) {

            _getProducts();
            _getBasket();
            alert("Great..!" + "Product quantity edited successfully..!");
        }

        var _editBasketQuantityFaild = function (response) {

            alert("Opps..!" + response.data.ExceptionMessage);
        }

        var _proceedToCheckOut = function () {

            if ($scope.basket.Items.length > 0) {
                
                $location.path('/checkoutsummary');
            }
        }

        $scope.getProducts = _getProducts;
        $scope.addToBasket = _addToBasket;
        $scope.editBasketQuantity = _editBasketQuantity;
        $scope.getBasket = _getBasket;
        $scope.proceedToCheckOut = _proceedToCheckOut;
        $scope.getProducts();
        $scope.getBasket();
    }

})(angular.module('basketModule'));