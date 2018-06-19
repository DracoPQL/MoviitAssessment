//report controller
sandwichapp.controller('report-controller', function ($scope, reportService) {

    //Loads report when page loads
    loadReport();

    function loadReport() {
        var ReportRecords = reportService.list();
        ReportRecords.then(function (d) {
            //success
            console.log(d);
            $scope.Report = d.data;
        },
        //error
        function () {
            alert("Error occured while loading report...");
        });
    }
});