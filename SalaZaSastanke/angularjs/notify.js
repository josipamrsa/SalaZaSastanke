var app = angular.module("myApp", []);
var userInvite;

app.controller("inviteController", function ($scope, $http) {    
    $http({
        method: "POST",
        url: "UserData.asmx/loadUserInvitations",
        headers: {
            'Content-Type': "application/json"
        }
    })

         .then(function (response) {
             $scope.userInvite = response.data;
             
         });
            
});


app.controller('mainController', function ($scope) {
    if (localStorage.getItem("visible") != null) {
        $scope.isVisible = localStorage.getItem("visible");
    }

    else {
        $scope.isVisible = false;
    }
    
});

app.controller('toggleController', function ($scope) {
    $scope.message = "Prikaži izbornik";
    $scope.changeMessage = function (value) {
        if (value === "Prikaži izbornik") {
            $scope.message = "Sakrij izbornik";
        } else {
            $scope.message = "Prikaži izbornik";
        }

    };
});