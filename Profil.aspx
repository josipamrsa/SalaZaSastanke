<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Profil.aspx.cs" Inherits="Profil" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml"  ng-app="myApp">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.7.2/angular.min.js"></script>
    <script src="angularjs/profile.js"></script>
    <link rel="stylesheet" type="text/css" href="css/dash.css" />
   
</head>
<body>
          
    <form id="form1" runat="server">       
        <button ng-click="visible=!visible;" type="button">Click me to open the menu!</button>
        <div ng-class="{'is-visible':visible}" class="menu" ng-controller="mainController">
            <ul>
                <li><input type="button" value="Pocetna" onclick="window.location='Pocetna.aspx'"/></li>
                <li><input type="button" value="Rezervacija dvorane" onclick="window.location='Rezervacija.aspx'"/></li>
                <li><asp:Button ID="btnLogOut" runat="server" OnClick="btnLogOut_Click" Text="Odjava" /></li> 
            </ul>
        </div>  
             
        <div class="user-profile" ng-controller="userController">
            <table>
               
                    <tr>
                        <th>Korisnicko ime: </th>
                        <th>{{userName}}</th>                       
                    </tr>
                
                    <tr>
                        <th>Ime: </th>
                        <th>{{firstName}}</th>  
                    </tr>

                    <tr>
                        <th>Prezime: </th>
                        <th>{{lastName}}</th>  
                    </tr>

                    <tr>
                        <th>Titula: </th>
                        <th>{{jobTitle}}</th>  
                    </tr>

                    <tr>
                        <th>Email adresa: </th>
                        <th>{{emailAddress}}</th>  
                    </tr>

                    <tr>
                        <th>Kontakt telefon: </th>
                        <th>{{telephoneNum}}</th>  
                    </tr>
                
            </table>

        </div>                      
    </form>

    
    
</body>
</html>
