//ingredient controller
sandwichapp.controller('ingredient-controller', function ($scope, ingredientService) {

    //Loads all Ingredients when page loads
    list();

    function list() {
        var IngredientsRecords = ingredientService.list();
        IngredientsRecords.then(function (d) {
            //success
            $scope.Ingredients = d.data;
        },
        //error
        function () {
            alert("Error occured while fetching ingredients list...");
        });
    }

    //save ingredient data
    $scope.save = function () {
        var Ingredient = {
            request: {
                Name: $scope.Name,
                MachineStackIndex: $scope.MachineStackIndex
            }
        };
        var saverecords = ingredientService.save(Ingredient);
        saverecords.then(function (d) {
            if (d.status === 200) {
                loadIngredients();
                alert("Ingredient added successfully");
                $scope.resetSave();
            }
            else { alert("Ingredient not added."); }
        },
        function () {
            alert("Error occurred while adding ingredient.");
        });
    }
    //reset controls after save operation
    $scope.resetSave = function () {
        $scope.Name = '';
        $scope.MachineStackIndex = '';
    }

    //update ingredient data
    $scope.update = function () {
        var Ingredient = {
            request: {
                Id: $scope.UpdateId,
                Name: $scope.UpdateName,
                MachineStackIndex: $scope.UpdateMachineStackIndex
            }
        };
        var updaterecords = ingredientService.update(Ingredient);
        updaterecords.then(function (d) {
            if (d.status === 200) {
                loadIngredients();
                alert("Ingredient updated successfully");
                $scope.resetUpdate();
            }
            else {
                alert("Ingredient not updated.");
            }
        },
        function () {
            alert("Error occured while updating ingredient record");
        });
    }
    //reset controls after update
    $scope.resetUpdate = function () {
        $scope.UpdateId = '';
        $scope.UpdateName = '';
        $scope.UpdateMachineStackIndex = '';
    }

    //delete ingredient record
    $scope.delete = function (IngredientId) {

        var Ingredient = {
            ingredientId: IngredientId
        };

        var deleterecord = ingredientService.delete(Ingredient);
        deleterecord.then(function (d) {
            if (d.status === 200) {
                loadIngredients();
                alert("Ingredient deleted successfully");
            }
            else {
                alert("Ingredient not deleted.");
            }
        });
    }

    //get single record by ID
    $scope.getForUpdate = function (Ingredient) {
        $scope.UpdateId = Ingredient.Id;
        $scope.UpdateName = Ingredient.Name;
        $scope.UpdateMachineStackIndex = Ingredient.Type.Id;
    }

    //get data for delete confirmation
    $scope.getForDelete = function (Ingredient) {
        $scope.UpdateId = Ingredient.Id;
        $scope.UpdateName = Ingredient.Name;
    }
});