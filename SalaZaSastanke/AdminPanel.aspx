<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminPanel.aspx.cs" Inherits="AdminPanel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" ng-app="myApp">
<head runat="server">
    <title>Admin panel</title>
    <link rel="stylesheet" type="text/css" href="css/admin.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.7.2/angular.min.js"></script>    
    <script src="angularjs/sidebar.js"></script>
    <style type="text/css">
        .auto-style1 {
            height: 28px;
        }
        .auto-style2 {
            width: 268435456px;
        }
    </style>
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
                    <input type="button" id="btnProf" value="Pregled profila" onclick="window.location='Profil.aspx'" />
                    <input type="button" id="btnRez" value="Rezervacija dvorane" onclick="window.location='Rezervacija.aspx'" />
                    <input type="button" id="reportPanel" value="Report panel" onclick="window.location='ReportPanel.aspx'"/>
                    <br /><br />    
                    <asp:Button ID="btnLogOut" runat="server" OnClick="btnLogOut_Click" Text="Odjava" />                                                                     
                </div>    
                                
            </div>         

       
        <asp:Label ID="lblTitle" runat="server"></asp:Label>         
              
        <asp:Label ID="lblAdminInfo" runat="server" Text=""></asp:Label>
        <br />
        
        <div id="hall-area">

        <table id="hall-upd">

            <tr>
                <th class="auto-style1">
                    <asp:Label ID="lblAzuriraj" runat="server" Text=""><h2>Ažurirajte dvoranu</h2></asp:Label>
                </th>
            </tr>

            <tr>
                <td>
                    <asp:DropDownList ID="azuriranjeDvorane" runat="server" AutoPostBack="True" DataSourceID="AzuriranjeDvorana" DataTextField="NazivDvorane" DataValueField="DvoranaID" OnSelectedIndexChanged="azuriranjeDvorane_SelectedIndexChanged" OnLoad="azuriranjeDvorane_SelectedIndexChanged" AppendDataBoundItems="True">
                        <asp:ListItem>Odaberi dvoranu...</asp:ListItem>
                        
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="AzuriranjeDvorana" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT [DvoranaID], [NazivDvorane] FROM [Dvorana]"></asp:SqlDataSource>
                </td>              
            </tr>

            <tr>
                <th>Naziv dvorane: <asp:Label ID="dvorana" runat="server" Text=""></asp:Label></th>                
                <td><asp:TextBox ID="azurNaziv" runat="server" MaxLength="50" ValidationGroup="dvorana"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="azurNaziv" ErrorMessage="Polje naziva je prazno!" ValidationGroup="dvorana">*</asp:RequiredFieldValidator>
                </td>
            </tr>

            <tr>
                <th>Lokacija: <asp:Label ID="lokacija" runat="server" Text=""></asp:Label></th>                
                <td><asp:TextBox ID="azurLok" runat="server" MaxLength="150" ValidationGroup="dvorana"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="azurLok" ErrorMessage="Polje lokacije je prazno!" ValidationGroup="dvorana">*</asp:RequiredFieldValidator>
                </td>
            </tr>

            <tr>
                <th>Adresa: <asp:Label ID="adresa" runat="server" Text=""></asp:Label></th>               
                <td><asp:TextBox ID="azurAddr" runat="server" MaxLength="150" ValidationGroup="dvorana"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="azurAddr" ErrorMessage="Polje adrese je prazno!" ValidationGroup="dvorana">*</asp:RequiredFieldValidator>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Button ID="btnAzuriraj" runat="server" Text="Ažuriraj podatke za dvoranu" OnClick="btnAzuriraj_Click" ValidationGroup="dvorana" />
                </td>
            </tr>

        </table> 

        <table id="hall-add">
            <tr>
                <th colspan="2">
                    <asp:Label ID="lblNovaDvorana" runat="server"><h2>Dodajte novu dvoranu</h2></asp:Label>
                </th>                                        
            </tr>

            <tr>
                <th colspan="2">Naziv dvorane: </th>
                <td colspan="2"><asp:TextBox ID="tbxNaziv" runat="server" MaxLength="50" ValidationGroup="nova"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbxNaziv" ErrorMessage="Unesite naziv dvorane!" ValidationGroup="nova">*</asp:RequiredFieldValidator>
                </td>     
            </tr>

            <tr>
                <th colspan="2">Lokacija: </th>
                <td colspan="2"><asp:TextBox ID="tbxLok" runat="server" MaxLength="150" ValidationGroup="nova"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbxLok" ErrorMessage="Unesite lokaciju!" ValidationGroup="nova">*</asp:RequiredFieldValidator>
                </td>
                           
            </tr>

            <tr>
                <th colspan="2">Adresa: </th>
                <td colspan="2"><asp:TextBox ID="TbxAddr" runat="server" MaxLength="150" ValidationGroup="nova"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="TbxAddr" ErrorMessage="Unesite adresu!" ValidationGroup="nova">*</asp:RequiredFieldValidator>
                </td>
            </tr>

            <tr>                
                <td>
                    <br />
                    <asp:Button ID="btnUnosDvorane" runat="server" Text="Unesi novu dvoranu" OnClick="btnUnosDvorane_Click" ValidationGroup="nova" />
                </td>                  
            </tr> 
        </table>

        <br />

        <table id="val-summ">
            <tr>
                <td>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="dvorana" />
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="nova" />
                </td>
            </tr>
        </table>

        <table id="hall-del">
            <tr>
                <th>                  
                    <asp:Label ID="lblObrisiDvoranu" runat="server"><h2>Obrišite postojeću dvoranu</h2></asp:Label>
                </th>                
            </tr>   
            
            <tr>                                                                                                            
                <td>
                    <br />
                    <asp:DropDownList ID="ddDvorane" runat="server" AutoPostBack="True" DataSourceID="BrisanjeDvorane" DataTextField="NazivDvorane" DataValueField="DvoranaID" OnLoad="azuriranjeDvorane_SelectedIndexChanged" AppendDataBoundItems="True">
                        <asp:ListItem>Odaberite dvoranu...</asp:ListItem>
                       
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="BrisanjeDvorane" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT DvoranaID, NazivDvorane FROM Dvorana"></asp:SqlDataSource>
                </td>                            
            </tr> 
            
            <tr>
                <td><asp:Button ID="btnObrisi" runat="server" Text="Obriši dvoranu" OnClick="btnObrisi_Click" /></td>
            </tr>  
                              
        </table>    
        
        </div>
        
        <br />

        <div id="user-area">

            <table id="val-summ2">
                <tr>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="korisnik" />
                    </td>
                </tr>
            </table>

            <table id="user-edit">
                <tr>
                    <th colspan="2"><asp:Label ID="lblIzmijeniPodatke" runat="server" Text=""><h2>Ažurirajte podatke za postojećeg korisnika</h2></asp:Label></th>
                    
                </tr>

                <tr>
                    <td colspan="2">
                        <asp:DropDownList ID="ddKorisnici" runat="server" DataSourceID="azuriranjeKorisnika" DataTextField="KorisnickoIme" DataValueField="KorisnikID" AutoPostBack="True" OnSelectedIndexChanged="ddKorisnici_SelectedIndexChanged" OnLoad="ddKorisnici_SelectedIndexChanged" AppendDataBoundItems="True">
                            <asp:ListItem>Odaberite korisnika...</asp:ListItem>
                            
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="azuriranjeKorisnika" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT [KorisnickoIme], [KorisnikID] FROM [Korisnik]"></asp:SqlDataSource>
                    </td>
                </tr>

                <tr>
                        <th colspan="2"><label>Ime: <asp:Label ID="lblI" runat="server" Text=""></asp:Label></label> </th>
                        <td colspan="2" class="auto-style2">
                              
                            <asp:TextBox ID="imeTbx" runat="server" ValidationGroup="korisnik" MaxLength="50"></asp:TextBox>      
                            <asp:RequiredFieldValidator ID="rfVal2" runat="server" ControlToValidate="imeTbx" ErrorMessage="Unesite ime!" ValidationGroup="korisnik">*</asp:RequiredFieldValidator>
                        </td>
                </tr>

                <tr>
                        <th colspan="2"><label>Prezime: <asp:Label ID="lblP" runat="server" Text=""></asp:Label></label>     </th>
                        <td colspan="2" class="auto-style2">
                              
                            <asp:TextBox ID="prezimeTbx" runat="server" ValidationGroup="korisnik" MaxLength="70"></asp:TextBox>    
                            <asp:RequiredFieldValidator ID="rfVal3" runat="server" ControlToValidate="prezimeTbx" ErrorMessage="Unesite prezime!" ValidationGroup="korisnik">*</asp:RequiredFieldValidator>
                        </td>
                </tr>

                <tr>
                        <th colspan="2"><label>Email adresa: <asp:Label ID="lblE" runat="server" Text=""></asp:Label></label> </th>
                        <td colspan="2" class="auto-style2">
                                  
                            <asp:TextBox ID="mailTbx" runat="server" ValidationGroup="korisnik" MaxLength="255"></asp:TextBox>        
                            <asp:RequiredFieldValidator ID="rfVal4" runat="server" ControlToValidate="mailTbx" ErrorMessage="Unesite email adresu!" ValidationGroup="korisnik">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="reVal1" runat="server" ControlToValidate="mailTbx" ErrorMessage="Neispravan oblik email adrese!" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="korisnik">*</asp:RegularExpressionValidator>                    
                        </td>
                </tr>

                <tr>
                        <th colspan="2"><label>Telefon: <asp:Label ID="lblT" runat="server" Text=""></asp:Label></label> </th>
                        <td colspan="2" class="auto-style2">
                                   
                            <asp:TextBox ID="telTbx" runat="server" ValidationGroup="korisnik" MaxLength="25"></asp:TextBox>   
                            <asp:RequiredFieldValidator ID="rfVal5" runat="server" ControlToValidate="telTbx" ErrorMessage="Unesite kontakt telefon!" ValidationGroup="korisnik">*</asp:RequiredFieldValidator>
        
                        </td>
                </tr>

                <tr>
                        <th colspan="2"><label>Titula: <asp:Label ID="lblJ" runat="server" Text=""></asp:Label></label> </th>
                        <td colspan="2" class="auto-style2">
                                
                            <asp:TextBox ID="ttlTbx" runat="server" ValidationGroup="korisnik" MaxLength="150"></asp:TextBox>     
                        </td>
                </tr>

                <tr>
                    <td colspan="2"><br />
                        <b>Ažuriraj podatke:</b> <br />
                        <asp:RadioButton ID="rMan" runat="server" GroupName="updateType" OnCheckedChanged="rMan_CheckedChanged" />ručno<br />
                        <asp:RadioButton ID="rAd" runat="server" GroupName="updateType" OnCheckedChanged="rAd_CheckedChanged" />putem AD-a (nije potrebno unositi podatke)
                    </td>

                    
                </tr>

                <tr>
                    <td colspan="2">
                        <br />
                        <asp:Button ID="btnAzurirajKorisnik" runat="server" Text="Ažuriraj podatke za korisnika" ValidationGroup="korisnik" OnClick="btnAzurirajKorisnik_Click" CausesValidation="False" />
                    </td>
                </tr>

            </table>

            

        </div>     
        
        
        
    </form>
</body>
</html>
