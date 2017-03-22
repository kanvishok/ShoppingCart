
(function (checkoutModule) {

    checkoutModule.directive('itemsInBasket', itemsInBasket);

    function itemsInBasket() {

        return {
            controller: checkoutModule.checkoutController,
            templateUrl: './Scripts/app/checkout/itemsInBasket.template.html',
            link: function (scope, element, attrs, ngModel) {

                //scope.addToBasket = function () {
                //    scope.$parent.addToBasket(scope.product.Id, scope.quantity);
                //};

                //scope.editBasketQuantity = function () {
                //    scope.$parent.editBasketQuantity(scope.product.Id, scope.quantity);
                //}
            }

        };
    }

})(angular.module('checkoutModule'));