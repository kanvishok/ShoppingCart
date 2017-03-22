(function (serviceModule) {

    'use strict';

    serviceModule.factory('apiCallerService', apiCallerService);

    apiCallerService.$inject = ['$http', '$rootScope', '$location'];
    function apiCallerService($http, $rootScope, $location) {
       var serviceFactory = {};
       var errorCode = {
            Unauthorized: 401
        }

        var _get = function (url, params, onSuccessCallback, onFailureCallback) {

            return $http.get(url, params)
                        .then(function (result) {
                    if (onSuccessCallback != null)
                        onSuccessCallback(result);
                }, function (error) {
                            if (error.status == errorCode.Unauthorized) {
                                _whenAuthFailed();
                            } else if (onFailureCallback != null) {
                                onFailureCallback(error);
                            }
                        });
        }

        var _post = function (url, params, onSuccessCallback, onFailurecallback) {

            return $http.post(url, params)
                        .then(function (result) {
                            if (onSuccessCallback != null) {
                                onSuccessCallback(result);
                            }
                        }, function (error) {
                            if (error.status == errorCode.Unauthorized) {
                                _whenAuthFailed();
                            } else if (onFailurecallback != null)

                                onFailurecallback(error);

                        });

        }

        var _whenAuthFailed = function () {

            $rootScope.previousState = $location.path();
            $location.path('/login');
        }

        serviceFactory.get = _get;
        serviceFactory.post = _post;
        return serviceFactory;
    }


})(angular.module('serviceModule'));