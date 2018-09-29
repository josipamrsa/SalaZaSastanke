<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Profil.aspx.cs" Inherits="Profil" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml"  ng-app="myApp">
<head runat="server">
    <title>Profil korisnika</title>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.7.2/angular.min.js"></script>
    <script src="angularjs/profile.js"></script>
    <link rel="stylesheet" type="text/css" href="css/profile.css" />
   
</head>
<body>
          
    <form id="form1" runat="server">       
        
        <div class="side-content">

        <a ng-click="visible=!visible;" type="button" id="menu-button">                  
            <div ng-if="visible">Sakrij izbornik &#9650;</div>
            <div ng-if="!visible">Prikaži izbornik &#9660;</div>
        </a>

        <div ng-class="{'is-visible':visible}" class="menu" ng-controller="mainController">

            <input type="button" id="btnPoc" value="Početna" onclick="window.location='Pocetna.aspx'"/>
            <input type="button" id="btnRez" value="Rezervacija dvorane" onclick="window.location='Rezervacija.aspx'"/>
            <input type="button" id="adminPanel" value="Admin panel" onclick="window.location='AdminPanel.aspx'" />
            <input type="button" id="reportPanel" value="Report panel" onclick="window.location='ReportPanel.aspx'"/>
            <br /><br /> 
            <asp:Button ID="btnLogOut" runat="server" OnClick="btnLogOut_Click" Text="Odjava" />           
            
            
        </div> 
            </div> 
             
        <div class="user-profile" ng-controller="userController">
            <h1>Profil</h1><br />
            <h2>Vaši korisnički podaci</h2>
            <table id="tblInfo">            
                    <tr>
                        <th>Korisničko ime: </th>
                        <td>{{userName}}</td>                       
                    </tr>
                
                    <tr>
                        <th>Ime: </th>
                        <td>{{firstName}}</td>  
                    </tr>

                    <tr>
                        <th>Prezime: </th>
                        <td>{{lastName}}</td>  
                    </tr>

                    <tr>
                        <th>Titula: </th>
                        <td>{{jobTitle}}</td>  
                    </tr>

                    <tr>
                        <th>Email adresa: </th>
                        <td ng-if='emailAddress!=""'>{{emailAddress}}</td> 
                        <td ng-if='emailAddress==""'><b style="color: #c8111e;"><mark style="background: #fef7c6">Da bi vas drugi korisnici mogli pozivati na sastanke, molimo vas da od administratora zatražite postavljanje email adrese.</mark></b></td> 
                    </tr>

                    <tr>
                        <th>Kontakt telefon: </th>
                        <td ng-if='telephoneNum!=""'>{{telephoneNum}}</td> 
                        <td ng-if='telephoneNum==""'><b style="color: #c8111e;"><mark style="background: #fef7c6">Nije postavljen - za postavljanje broja telefona molimo obratite se administratoru.</mark></b></td>                        
                    </tr>
                
            </table>
            <br />

            <h2>Pregled aktivnosti</h2> <br />
            
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="PotvrdaID" DataSourceID="UserEventInfo" AllowPaging="True" GridLines="None" Height="16px" Width="1106px">
                <Columns>
                    <asp:BoundField DataField="PeriodOd" HeaderText="Početak" SortExpression="PeriodOd" />
                    <asp:BoundField DataField="PeriodDo" HeaderText="Završetak" SortExpression="PeriodDo" />
                    <asp:BoundField DataField="OpisDogadjaja" HeaderText="Kratak opis događaja" SortExpression="OpisDogadjaja" />
                    <asp:BoundField DataField="NazivDvorane" HeaderText="Dvorana" SortExpression="NazivDvorane" />
                    <asp:BoundField DataField="Adresa" HeaderText="Adresa" SortExpression="Adresa" />
                    <asp:BoundField DataField="Lokacija" HeaderText="Lokacija" SortExpression="Lokacija" />
                    <asp:TemplateField HeaderText="Vaš odgovor">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("KorisnikDolazi") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:SqlDataSource ID="UserEventInfo" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT r.PeriodOd, r.PeriodDo, r.OpisDogadjaja, d.Lokacija, d.Adresa, d.NazivDvorane, p.PotvrdaID, p.KorisnikDolazi FROM Rezervacija AS r INNER JOIN Potvrda AS p ON r.RezervacijaID = p.IDRezervacija INNER JOIN Dvorana AS d ON r.IDDvorana = d.DvoranaID WHERE (p.Email = (SELECT EmailAdresa FROM Korisnik WHERE (KorisnickoIme = @Korisnicko))) AND (p.KorisnikJeOdgovorio = 1)">
                <SelectParameters>
                    <asp:SessionParameter Name="Korisnicko" SessionField="user_id" />
                </SelectParameters>
            </asp:SqlDataSource>

            <h2>Vaši događaji</h2><br />

            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="RezervacijaID" DataSourceID="UserOrganizedInfo" AllowPaging="True" GridLines="None" Height="109px" Width="1026px">
                <Columns>
                    <asp:BoundField DataField="PeriodOd" HeaderText="Početak" SortExpression="PeriodOd" />
                    <asp:BoundField DataField="PeriodDo" HeaderText="Završetak" SortExpression="PeriodDo" />
                    <asp:BoundField DataField="OpisDogadjaja" HeaderText="Opis događaja" SortExpression="OpisDogadjaja" />
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                    <asp:BoundField DataField="Adresa" HeaderText="Adresa" SortExpression="Adresa" />
                    <asp:BoundField DataField="Lokacija" HeaderText="Lokacija" SortExpression="Lokacija" />
                    <asp:BoundField DataField="NazivDvorane" HeaderText="Naziv dvorane" SortExpression="NazivDvorane" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="UserOrganizedInfo" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT Rezervacija.RezervacijaID, Rezervacija.PeriodOd, Rezervacija.PeriodDo, Rezervacija.OpisDogadjaja, Rezervacija.Status, Rezervacija.IDKorisnik, Rezervacija.IDDvorana, Dvorana.Adresa, Dvorana.Lokacija, Dvorana.NazivDvorane FROM Rezervacija INNER JOIN Dvorana ON Rezervacija.IDDvorana = Dvorana.DvoranaID WHERE (Rezervacija.IDKorisnik = (SELECT KorisnikID FROM Korisnik WHERE (KorisnickoIme = @Korisnicko))) AND (Rezervacija.Status = 'active')">
                <SelectParameters>
                    <asp:SessionParameter Name="Korisnicko" SessionField="user_id" />
                </SelectParameters>
            </asp:SqlDataSource>
            <br />
        </div>                      
    </form>

    
    
</body>
</html>
