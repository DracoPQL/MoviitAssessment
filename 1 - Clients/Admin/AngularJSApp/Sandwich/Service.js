//Service to get data from sandwiches mvc controller
sandwichapp.service('sandwichService', function ($http) {

    this.list = function () {

        return $http.get("http://localhost:54591/SandwichsService.svc/json/SandwichList");
    };

    this.save = function (Sandwich) {
        var request = $http({
            method: 'post',
            url: 'http://localhost:54591/SandwichsService.svc/json/SandwichCreate',
            data: Sandwich
        });

        return request;
    };

    this.update = function (Sandwich) {
        var updaterequest = $http({
            method: 'post',
            url: 'http://localhost:54591/SandwichsService.svc/json/SandwichUpdate',
            data: Sandwich
        });
        return updaterequest;
    }

    this.delete = function (Sandwich) {
        var request = $http({
            method: 'post',
            url: 'http://localhost:54591/SandwichsService.svc/json/SandwichDelete',
            data: Sandwich
        });

        return request;
    }
});