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
});

