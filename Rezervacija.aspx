<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Rezervacija.aspx.cs" Inherits="Rezervacija" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.7.2/angular.min.js"></script>
    <!--<link rel="stylesheet" type="text/css" href="css/dash.css" />-->
        
</head>
<body ng-app="MyApp">
   
    <form id="form1" runat="server">  
         
    <h1>Rezervacija dvorane za sastanak</h1>
    Unesite datum sastanka: <input type="date" runat="server" id="eventDate" /><br />
    Unesite termin (od-do): 
        <ul>
            <li>Pocetak: <input type="time" runat="server" id="timeBegin"/></li>
            <li>Zavrsetak: <input type="time" runat="server" id="timeEnd" /> </li>
        </ul> 
    
        <p>
        <asp:Button ID="btnProvjeriDvorane" runat="server" OnClick="btnProvjeriDvorane_Click" Text="Provjeri dostupne dvorane" />
        </p>
    
    Unesite kratki opis: 
        <asp:TextBox ID="eventDesc" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="DostupneDvorane" DataTextField="NazivDvorane" DataValueField="DvoranaID" Height="16px">
        </asp:DropDownList>
&nbsp;<p>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="DvoranaID" DataSourceID="DostupneDvorane">
                <Columns>
                    <asp:BoundField DataField="Lokacija" HeaderText="Lokacija" SortExpression="Lokacija" />
                    <asp:BoundField DataField="Adresa" HeaderText="Adresa" SortExpression="Adresa" />
                    <asp:BoundField DataField="NazivDvorane" HeaderText="NazivDvorane" SortExpression="NazivDvorane" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="DostupneDvorane" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT DvoranaID, Lokacija, Adresa, NazivDvorane FROM Dvorana WHERE (DvoranaID NOT IN (SELECT IDDvorana FROM Rezervacija WHERE (PeriodOd &gt;= @PeriodOd) AND (PeriodDo &lt;= @PeriodDo)))">
                <SelectParameters>
                    <asp:SessionParameter DefaultValue="" Name="PeriodOd" SessionField="time-begin" />
                    <asp:SessionParameter DefaultValue="" Name="PeriodDo" SessionField="time-end" />
                </SelectParameters>
            </asp:SqlDataSource>



        <p>
            Pozovite sudionike na sastanak:<p>
            <asp:GridView ID="GridView2" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="KorisnikID" DataSourceID="DostupniKorisnici">
                <Columns>
                    <asp:TemplateField HeaderText="Korisničko ime">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Text='<%# Eval("KorisnickoIme") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Ime" HeaderText="Ime" SortExpression="Ime" />
                    <asp:BoundField DataField="Prezime" HeaderText="Prezime" SortExpression="Prezime" />
                    <asp:BoundField DataField="EmailAdresa" HeaderText="EmailAdresa" SortExpression="EmailAdresa" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="DostupniKorisnici" runat="server" DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="RezervacijeTableAdapters.KorisnikTableAdapter" UpdateMethod="Update">
                <DeleteParameters>
                    <asp:Parameter Name="Original_KorisnikID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="KorisnickoIme" Type="String" />
                    <asp:Parameter Name="Ime" Type="String" />
                    <asp:Parameter Name="Prezime" Type="String" />
                    <asp:Parameter Name="EmailAdresa" Type="String" />
                    <asp:Parameter Name="Titula" Type="String" />
                    <asp:Parameter Name="Telefon" Type="String" />
                    <asp:Parameter Name="IDRazina" Type="Int32" />
                </InsertParameters>
                <SelectParameters>
                    <asp:SessionParameter Name="UserID" SessionField="user_id" Type="String" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="KorisnickoIme" Type="String" />
                    <asp:Parameter Name="Ime" Type="String" />
                    <asp:Parameter Name="Prezime" Type="String" />
                    <asp:Parameter Name="EmailAdresa" Type="String" />
                    <asp:Parameter Name="Titula" Type="String" />
                    <asp:Parameter Name="Telefon" Type="String" />
                    <asp:Parameter Name="IDRazina" Type="Int32" />
                    <asp:Parameter Name="Original_KorisnikID" Type="Int32" />
                </UpdateParameters>
            </asp:ObjectDataSource>
        <p>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>



        <p>
            <asp:Button ID="btnEnd" runat="server" OnClick="btnEnd_Click" style="width: 49px" Text="Zavrsi" />

    </form>
    </body>
</html>
