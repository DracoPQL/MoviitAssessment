//sandwich controller
sandwichapp.controller('sandwich-controller', function ($scope, sandwichService) {

    //Loads all Sandwiches when page loads
    list();

    function list() {
        var SandwichesRecords = sandwichService.list();
        SandwichesRecords.then(function (d) {
            //success
            $scope.Sandwiches = d.data;
        },
        //error
        function () {
            alert("Error occured while fetching sandwiches list...");
        });
    }

    //save sandwich data
    $scope.save = function () {
        var Sandwich = {
            request: {
                Ingredients: [{
                    Id: 2,
                }],
                Name: $scope.Name,
                Price: $scope.Price,
                Type: {
                    Id: $scope.TypeId
                }
            }
        };
        var saverecords = sandwichService.save(Sandwich);
        saverecords.then(function (d) {
            if (d.status === 200) {
                loadSandwiches();
                alert("Sandwich added successfully");
                $scope.resetSave();
            }
            else { alert("Sandwich not added."); }
        },
        function () {
            alert("Error occurred while adding sandwich.");
        });
    }
    //reset controls after save operation
    $scope.resetSave = function () {
        $scope.Ingredients = '';
        $scope.Name = '';
        $scope.Price= '';
        $scope.TypeId = '';
    }

    //update sandwich data
    $scope.update = function () {
        var Sandwich = {
            request: {
                Id: $scope.UpdateId,
                Ingredients: [{
                    Id: 2,
                }],
                Name: $scope.UpdateName,
                Price: $scope.UpdatePrice,
                Type: {
                    Id: $scope.UpdateTypeId
                }
            }
        };
        var updaterecords = sandwichService.update(Sandwich);
        updaterecords.then(function (d) {
            if (d.status === 200) {
                loadSandwiches();
                alert("Sandwich updated successfully");
                $scope.resetUpdate();
            }
            else {
                alert("Sandwich not updated.");
            }
        },
        function () {
            alert("Error occured while updating sandwich record");
        });
    }
    //reset controls after update
    $scope.resetUpdate = function () {
        $scope.UpdateId = '';
        $scope.UpdateName = '';
        $scope.UpdatePrice = '';
        $scope.UpdateTypeId = '';
    }

    //delete Sandwich record
    $scope.delete = function (SandwichId) {

        var Sandwich = {
            sandwichId: SandwichId
        };

        var deleterecord = sandwichService.delete(Sandwich);
        deleterecord.then(function (d) {
            if (d.status === 200) {
                loadSandwiches();
                alert("Sandwich deleted successfully");
            }
            else {
                alert("Sandwich not deleted.");
            }
        });
    }

    //get single record by ID
    $scope.getForUpdate = function (Sandwich) {
        $scope.UpdateId = Sandwich.Id;
        $scope.UpdateName = Sandwich.Name;
        $scope.UpdatePrice = Sandwich.Price;
        $scope.UpdateTypeId = Sandwich.Type.Id;
    }

    //get data for delete confirmation
    $scope.getForDelete = function (Sandwich) {
        $scope.UpdateId = Sandwich.Id;
        $scope.UpdateName = Sandwich.Name;
    }
});