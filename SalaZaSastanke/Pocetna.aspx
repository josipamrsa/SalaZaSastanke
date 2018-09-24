<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pocetna.aspx.cs" Inherits="Pocetna" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Početna - Rezervacija sale za sastanak</title>
        <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.7.2/angular.min.js"></script>    
        <script src="angularjs/notify.js"></script>
        <link rel="stylesheet" type="text/css" href="css/dash.css" /> 
    </head>

    <body ng-app="myApp">
       
        <form id="form1" runat="server">                     
            <div class="side-content">
                <asp:Label ID="loginInfo" runat="server" Text=""></asp:Label>                          
            </div>   
                  
            <button ng-click="visible=!visible;" type="button" id="menu-button">Izbornik</button>           
            <div ng-class="{'is-visible':visible}" class="menu"  ng-controller="mainController">                   
                    <input type="button" id="btnProf" value="Pregled profila" onclick="window.location='Profil.aspx'" />
                    <input type="button" id="btnRez" value="Rezervacija dvorane" onclick="window.location='Rezervacija.aspx'" />
                    <asp:Button ID="btnLogOut" runat="server" OnClick="btnLogOut_Click" Text="Odjava" />  
                
                    <br /><br />  

                    <h2>Admin/report paneli</h2>
                    <input type="button" id="adminPanel" value="Admin panel" onclick="window.location='AdminPanel.aspx'"/>
                    <input type="button" id="reportPanel" value="Report panel" onclick="window.location='ReportPanel.aspx'"/>                                                              
                </div>                   
            <div> 
                
                <h1><asp:Label ID="lblWelcome" runat="server" Text="test"></asp:Label></h1>
               
            </div>

            <div id="notificationArea" runat="server" ng-controller="inviteController">
                              
                <div id="resNotifications" runat="server" ng-repeat="x in userInvite">  
                                         
                    <table id="tblEvents" runat="server">     
                                
                        <tr>
                            <th colspan="2" style="text-align: center">
                                <span style="font-size: 25px; color: #7d060e">
                                    <b>Obavijest o događaju</b>
                                </span>
                                <br />
                            </th>
                        </tr>

                        <tr>
                            <th>Datum: </th>
                            <td>{{x.dateEvent}}</td>
                        </tr>

                        <tr>
                            <th>Mjesto: </th>
                            <td>{{x.hallName}} - {{x.address}}, {{x.location}}</td>
                        </tr>

                         <tr>
                            <th>Termin: </th>
                            <td>{{x.beginPeriod}} - {{x.endPeriod}}</td>    
                         </tr>
                                               
                         <tr>
                            <th>Opis događaja: </th>
                            <td>{{x.eventInfo}}<br /></td>
                        </tr>                       
                   
                        <tr>                                                                                                                                     
                            <td colspan="4" style="text-align:center">
                                <br />
                                <asp:Button ID="btnPotvrdi" runat="server" Text="Potvrdi dolazak" OnClick="btnPotvrdi_Click"/>&nbsp;                       
                                <asp:Button ID="btnOdbij" runat="server" Text="Odbij pozivnicu" OnClick="btnOdbij_Click"/>                                                          
                            </td>                                                                             
                        </tr>
              
                    </table>                 
                    <input id="ModalId" type="hidden" runat="server" value="{{x.confId}}"/> 
                    <br />

                </div>   

                <br />
            <asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>  
                                                               
            </div>     
        </form>
    </body>
</html>
