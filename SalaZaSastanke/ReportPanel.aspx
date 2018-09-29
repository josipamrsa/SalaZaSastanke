<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportPanel.aspx.cs" Inherits="ReportPanel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" ng-app="myApp">
<head runat="server">
    <title>Reporting panel i statistika</title>
    <link rel="stylesheet" type="text/css" href="css/report.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.7.2/angular.min.js"></script>    
    <script src="angularjs/sidebar.js"></script>
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
                    <input type="button" id="adminPanel" value="Admin panel" onclick="window.location='AdminPanel.aspx'" />
                    <br /><br />    
                    <asp:Button ID="btnLogOut" runat="server" OnClick="btnLogOut_Click" Text="Odjava" />                                                                     
                </div>    
                                
            </div>         

    <div>   
        <asp:Label ID="lblTitle" runat="server"></asp:Label>         
        <asp:Label ID="lblTimestamp" runat="server" Text=""></asp:Label>
        <br /><br />
    </div>
        
        <table cellpadding="5" id="tblContent" runat="server">
            <tr>
                <th>
                    Statistika za korisnika:
                    <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="DostupniKorisnici" DataTextField="KorisnickoIme" DataValueField="KorisnikID" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="DostupniKorisnici" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT [KorisnikID], [KorisnickoIme] FROM [Korisnik]"></asp:SqlDataSource>          
                </th>              

                <th>
                    Statistika za dvorane:
                    <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" DataSourceID="DostupneDvorane" DataTextField="NazivDvorane" DataValueField="DvoranaID">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="DostupneDvorane" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT [DvoranaID], [NazivDvorane] FROM [Dvorana]"></asp:SqlDataSource>       
                </th>
            </tr>

            <tr>
                <td>               
                    <h2>Aktivnost za sastanke</h2>
                    
                     <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataSourceID="DolaznostSastanci" GridLines="None" Height="16px" Width="170px">
                        <Fields>
                            <asp:BoundField DataField="Dolazi" HeaderText="Broj potvrda dolazaka: " ReadOnly="True" SortExpression="Dolazi" />
                        </Fields>
                    </asp:DetailsView>
                    <asp:SqlDataSource ID="DolaznostSastanci" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT COUNT(PotvrdaID) AS 'Dolazi' FROM Potvrda WHERE (Email = (SELECT EmailAdresa FROM Korisnik WHERE (KorisnikID = @Korisnik))) AND (KorisnikDolazi = 'true')">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList1" Name="Korisnik" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                    <asp:DetailsView ID="DetailsView4" runat="server" AutoGenerateRows="False" DataSourceID="BrojOdbijenih" Height="16px" Width="164px" GridLines="None">
                        <Fields>
                            <asp:BoundField DataField="Broj odbijenih poziva" HeaderText="Broj odbijenih poziva:" ReadOnly="True" SortExpression="Broj odbijenih poziva" />
                        </Fields>
                    </asp:DetailsView>
                    <asp:SqlDataSource ID="BrojOdbijenih" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT COUNT(PotvrdaID) AS [Broj odbijenih poziva] FROM Potvrda WHERE (Email = (SELECT EmailAdresa FROM Korisnik WHERE (KorisnikID = @Korisnik))) AND (KorisnikDolazi = 'false')">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList1" Name="Korisnik" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                    <asp:DetailsView ID="DetailsView2" runat="server" AutoGenerateRows="False" DataSourceID="OrganiziraniSastanci" GridLines="None" Height="16px" Width="209px">
                        <Fields>
                            <asp:BoundField DataField="BrojOrganiziranihSastanaka" HeaderText="Broj organiziranih sastanaka:" ReadOnly="True" SortExpression="BrojOrganiziranihSastanaka" />
                        </Fields>
                    </asp:DetailsView>
                    <asp:SqlDataSource ID="OrganiziraniSastanci" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT COUNT(RezervacijaID) AS BrojOrganiziranihSastanaka FROM Rezervacija WHERE (IDKorisnik = @Korisnik)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList1" Name="Korisnik" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <br />
                </td>

                <td>         
                    <h2>Zastupljenost dvorana</h2>
                    
                    <asp:DetailsView ID="DetailsView3" runat="server" AutoGenerateRows="False" DataSourceID="NajzastupljenijaDvorana" GridLines="None">
                        <Fields>
                            <asp:BoundField DataField="BrojRezervacija" HeaderText="Broj aktivnih rezervacija za ovu dvoranu: " ReadOnly="True" SortExpression="BrojRezervacija" />
                        </Fields>
                    </asp:DetailsView>
                    <asp:SqlDataSource ID="NajzastupljenijaDvorana" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT COUNT(RezervacijaID) AS BrojRezervacija FROM Rezervacija WHERE IDDvorana =  @Dvorana">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList2" Name="Dvorana" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>                         
                </td>
            </tr>
        </table>
              
        <br />
        <table cellpadding="5" id="tblContent2" runat="server">
            <tr>
                <th>
                     Statistika za sastanke:
                    <asp:DropDownList ID="DropDownList3" runat="server" DataSourceID="Sastanci" DataTextField="OpisDogadjaja" DataValueField="RezervacijaID" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="Sastanci" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT [RezervacijaID], [OpisDogadjaja] FROM [Rezervacija]"></asp:SqlDataSource>
                    
                </th>

                <th>
                    Lista sudionika koji dolaze na ovaj sastanak:
                </th>
            </tr>

            <tr>
                <td>
                    <asp:DetailsView ID="DetailsView5" runat="server" AutoGenerateRows="False" DataSourceID="BrojSudionika" Height="16px" Width="182px" GridLines="None">
                        <Fields>
                            <asp:BoundField DataField="BrojPozvanihSudionika" HeaderText="Broj pozvanih sudionika:" ReadOnly="True" SortExpression="BrojPozvanihSudionika" />
                        </Fields>
                    </asp:DetailsView>
                    <asp:SqlDataSource ID="BrojSudionika" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT COUNT(PotvrdaID) AS BrojPozvanihSudionika FROM Potvrda WHERE (IDRezervacija = @Rezervacija)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList3" Name="Rezervacija" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:DetailsView ID="DetailsView6" runat="server" AutoGenerateRows="False" DataSourceID="OdazivSudionika" Height="16px" Width="198px" GridLines="None">
                        <Fields>
                            <asp:BoundField DataField="Column1" HeaderText="Broj sudionika koji dolaze: " ReadOnly="True" SortExpression="Column1" />
                        </Fields>
                    </asp:DetailsView>
                    <asp:SqlDataSource ID="OdazivSudionika" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT COUNT(p.PotvrdaID) FROM POTVRDA p WHERE p.KorisnikDolazi = 'true' AND p.IDRezervacija = @Rezervacija">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList3" Name="Rezervacija" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <br />
                </td>


                <td>
                    <asp:ListView ID="ListView1" runat="server" DataSourceID="ListaSudionika">
                        <AlternatingItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("Email") %>' />
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <EditItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
                                </td>
                                <td>
                                    <asp:TextBox ID="EmailTextBox" runat="server" Text='<%# Bind("Email") %>' />
                                </td>
                            </tr>
                        </EditItemTemplate>
                        <EmptyDataTemplate>
                            <table runat="server" style="">
                                <tr>
                                    <td>Nema sudionika.</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <InsertItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
                                </td>
                                <td>
                                    <asp:TextBox ID="EmailTextBox" runat="server" Text='<%# Bind("Email") %>' />
                                </td>
                            </tr>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("Email") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <table runat="server">
                                <tr runat="server">
                                    <td runat="server">
                                        <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                            <tr runat="server" style="">
                                                <th runat="server">Email sudionika koji dolaze</th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server">
                                    <td runat="server" style="">
                                        <asp:DataPager ID="DataPager1" runat="server">
                                            <Fields>
                                                <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True" ButtonCssClass="prev-next" />
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <SelectedItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("Email") %>' />
                                </td>
                            </tr>
                        </SelectedItemTemplate>
                    </asp:ListView>
                    <br />
                    <asp:SqlDataSource ID="ListaSudionika" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT Email FROM Potvrda AS P WHERE (IDRezervacija = @Rezervacija) AND (KorisnikDolazi = 'true')">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList3" Name="Rezervacija" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
        
    </form>
</body>
</html>
