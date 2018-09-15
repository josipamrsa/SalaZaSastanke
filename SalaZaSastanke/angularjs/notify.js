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
             //TODO

             // Izlistati podatke u tablicu/neku strukturu dobivene iz response
             // Kreirati 2 nove forme - checkbox/link/button za prihvaćanje/odbijanje 
             // Ovisno o tome koju korisnik označi/odabere, vrijednost za prihvat se sprema, skupa sa odgovorom i tokenom, u potvrdu
             // Kasnije se s profila moze naknadno mijenjati zeli li korisnik doći na sastanak

         });
});

app.controller('mainController', function ($scope) {
});

