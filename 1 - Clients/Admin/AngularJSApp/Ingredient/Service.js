//Service to get data from ingredients mvc controller
sandwichapp.service('ingredientService', function ($http) {

    this.list = function () {

        return $http.get("http://localhost:54591/SandwichsService.svc/json/IngredientList");
    };

    this.save = function (Ingredient) {
        var request = $http({
            method: 'post',
            url: 'http://localhost:54591/SandwichsService.svc/json/IngredientCreate',
            data: Ingredient
        });

        return request;
    };

    this.update = function (Ingredient) {
        var updaterequest = $http({
            method: 'post',
            url: 'http://localhost:54591/SandwichsService.svc/json/IngredientUpdate',
            data: Ingredient
        });
        return updaterequest;
    }

    this.delete = function (Ingredient) {
        var request = $http({
            method: 'post',
            url: 'http://localhost:54591/SandwichsService.svc/json/IngredientDelete',
            data: Ingredient
        });

        return request;
    }
});