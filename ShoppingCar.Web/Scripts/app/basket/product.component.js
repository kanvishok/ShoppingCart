
(function (basketModule) {

    basketModule.directive('productSelector', productSelector);

    function productSelector() {

        return {
            controller: basketModule.productController,
            templateUrl: './Scripts/app/basket/product.template.html',
            link: function (scope, element, attrs, ngModel) {

                scope.addToBasket = function () {
                    scope.$parent.addToBasket(scope.product.Id, scope.quantity);
                };

                scope.editBasketQuantity = function() {
                    scope.$parent.editBasketQuantity(scope.product.Id, scope.quantity);
                }
            }

        };
    }

})(angular.module('basketModule'));