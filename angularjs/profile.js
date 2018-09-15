var app = angular.module("myApp", []);
var userData;
app.controller("userController", function ($scope, $http) {
    
    $http({
            method : "GET",
            url: "UserData.asmx/fetchDataUser",
            headers: {
                'Content-Type': "application/json"
            }
        })
     
         .then(function (response) {           
             $scope.userData = response.data;
           
             $scope.userName = $scope.userData[0].userName;
             $scope.firstName = $scope.userData[0].firstName;
             $scope.lastName = $scope.userData[0].lastName;
             $scope.emailAddress = $scope.userData[0].emailAddress;
             $scope.telephoneNum = $scope.userData[0].telephoneNumber;
             $scope.jobTitle = $scope.userData[0].jobTitle;          
         });
});

app.controller('mainController', function ($scope) {    
});


