describe('API Caller Service Test', function () {

    var apiCallerService = {};
    var expectedData = 'data:[{"Id":1,"Name":"Apples","Description":"Fruit","Price":2.50},{"Id":2,"Name":"Bread","Loaf":"Fruit","Price":1.50}]';
    var errorData = 'Internal server error';
    var $httpBackend, $location, $rootScope;
    var url = 'http://localhost/ShoppingCart.Api/api/Values';
    var response;
    var params;

    var onSuccessCallback = function (data) {
        response = data;
    }

    var onFailureCallback = function (error) {
        response = error;
    }

    window.beforeEach(function () {

        module('serviceModule');
        inject(function (_apiCallerService_, _$httpBackend_, _$rootScope_, _$location_) {

            apiCallerService = _apiCallerService_;
            $httpBackend = _$httpBackend_;
            $location = _$location_;
            $rootScope = _$rootScope_;

        });
    });


    window.it('should return the data', function () {

        $httpBackend.when('GET', url).respond(200, expectedData);
        apiCallerService.get(url, params, onSuccessCallback, onFailureCallback);
        $httpBackend.flush();

        expect(response.data).toEqual(expectedData);
        expect(response.status).toEqual(200);
        expect(response.data).not.toEqual('not expectedData');
    });

    window.it('should return the error data when get', function () {

        $httpBackend.when('GET', url).respond(500, errorData);
        apiCallerService.get(url, params, onSuccessCallback, onFailureCallback);
        $httpBackend.flush();

        expect(response.data).toEqual(errorData);
        expect(response.status).toEqual(500);
        expect(response.data).not.toEqual('not errorData');
    });

    window.it('should return the auth failure when get', function () {

        $httpBackend.when('GET', url).respond(401, errorData);
        apiCallerService.get(url, params, onSuccessCallback, onFailureCallback);
        $httpBackend.flush();

        expect($location.path()).toEqual("/login");
        expect($location.path()).not.toEqual("login");
        expect($rootScope.previousState).toEqual('/');
        expect($rootScope.previousState).not.toEqual('');
    });

    window.it('should return posted after post', function () {

        $httpBackend.when('POST', url).respond(200, expectedData);
        apiCallerService.post(url, params, onSuccessCallback, onFailureCallback);
        $httpBackend.flush();

        expect(response.data).toEqual(expectedData);
        expect(response.status).toEqual(200);
        expect(response.data).not.toEqual('not expectedData');
    });

    window.it('should return the error data when post', function () {

        $httpBackend.when('POST', url).respond(500, errorData);
        apiCallerService.post(url, params, onSuccessCallback, onFailureCallback);
        $httpBackend.flush();

        expect(response.data).toEqual(errorData);
        expect(response.status).toEqual(500);
        expect(response.data).not.toEqual('not errorData');
    });

    window.it('should return the auth failure whe post', function () {

        $httpBackend.when('POST', url).respond(401, errorData);
        apiCallerService.post(url, params, onSuccessCallback, onFailureCallback);
        $httpBackend.flush();

        expect($location.path()).toEqual("/login");
        expect($location.path()).not.toEqual("login");
        expect($rootScope.previousState).toEqual('/');
        expect($rootScope.previousState).not.toEqual('');
    });

});
