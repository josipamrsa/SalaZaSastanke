<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pocetna.aspx.cs" Inherits="Pocetna" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" ng-app="myApp">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.7.2/angular.min.js"></script>    
    <script src="angularjs/notify.js"></script>
    <link rel="stylesheet" type="text/css" href="css/dash.css" />
   
</head>
<body>
           
    <form id="form1" runat="server">       
        <button ng-click="visible=!visible;" type="button">Click me to open the menu!</button>
        <div ng-class="{'is-visible':visible}" class="menu"  ng-controller="mainController">
            <ul>
                <li><input type="button" value="Pregled profila" onclick="window.location='Profil.aspx'"/></li>
                <li><input type="button" value="Rezervacija dvorane" onclick="window.location='Rezervacija.aspx'"/></li>
                <li><asp:Button ID="btnLogOut" runat="server" OnClick="btnLogOut_Click" Text="Odjava" /></li>                
            </ul>
        </div>
                        
    <div> 
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>   
    &nbsp;
    </div>

        <div id="notification-area" ng-controller="inviteController">
            {{userInvite[0]}}
        </div>
   
    </form>

</body>
</html>
