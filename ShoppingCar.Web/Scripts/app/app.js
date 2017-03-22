(function () {

    'use strict';
    var shoppingCartApp = angular.module('shoppingCartApp', ['ngRoute', 'basketModule', 'checkoutModule', 'confirmationModule', 'serviceModule']).config(config).run(run);

    config.$inject = ['$routeProvider'];
    function config($routeProvider) {

        $routeProvider
            .when("/", { templateUrl: "Scripts/app/basket/index.html", controller: "basketController" })
            .when("/checkoutsummary", { templateUrl: "Scripts/app/checkout/checkoutsummary.html", controller: "checkoutController" })
            .when("/confirmation", { templateUrl: "Scripts/app/confirmation/index.html", controller: "confirmationController" })
            .otherwise({ redirectTo: "/" });
    }


    function run() {
        console.log("run method");
    }

})();