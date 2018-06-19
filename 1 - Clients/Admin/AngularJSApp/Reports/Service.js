//Service to get data from ingredients mvc controller
sandwichapp.service('reportService', function ($http) {

    this.list = function () {

        return $http.get("http://localhost:54591/SandwichsService.svc/json/TheOnlyReport");
    };

});