﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Rezervacija.aspx.cs" Inherits="Rezervacija" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rezervacija dvorane</title>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.7.2/angular.min.js"></script>
    <link rel="stylesheet" type="text/css" href="css/reserve.css" />
        
</head>
<body>
   <div class="side-content">
       <img src="img/stock1.jpg" />
   </div>
    <form id="form1" runat="server">  
       
        <h1>Rezervacija dvorane za sastanak</h1>
        Unesite datum sastanka: <input type="date" runat="server" id="eventDate"/><br />
        Unesite termin (od-do): 
        <ul>
            <li>Početak: &nbsp;&nbsp;&nbsp;<input type="time" runat="server" id="timeBegin" max="20:00" min="08:00" /></li>
            <li>Završetak: <input type="time" runat="server" id="timeEnd" max="20:00" min="08:00" /> </li>
        </ul> 
    Unesite kratki opis: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="eventDesc" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfVal1" runat="server" ErrorMessage="Unesite kratki opis!" ControlToValidate="eventDesc"></asp:RequiredFieldValidator>
        <br />

        <p>
        <asp:Button ID="btnProvjeriDvorane" runat="server" OnClick="btnProvjeriDvorane_Click" Text="Provjeri dostupne dvorane" /> &nbsp;
        <asp:Button ID="btnBack" runat="server" Text="Povratak na početnu" CausesValidation="False" OnClick="btnBack_Click"/>
        </p>
    
        <asp:Label ID="Label1" runat="server"></asp:Label>

       <span id="par3" runat="server">Lista dostupnih dvorana za odabrani datum:</span><br /><br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="DvoranaID" DataSourceID="DostupneDvorane" GridLines="None" EmptyDataText="Nema dostupnih dvorana.">
                <Columns>
                    <asp:BoundField DataField="Lokacija" HeaderText="Lokacija" SortExpression="Lokacija" />
                    <asp:BoundField DataField="Adresa" HeaderText="Adresa" SortExpression="Adresa" />
                    <asp:BoundField DataField="NazivDvorane" HeaderText="Naziv dvorane" SortExpression="NazivDvorane" />
                </Columns>
                <RowStyle Wrap="True" />
            </asp:GridView>
            <asp:SqlDataSource ID="DostupneDvorane" runat="server" ConnectionString="<%$ ConnectionStrings:Rezervacija %>" SelectCommand="SELECT DvoranaID, Lokacija, Adresa, NazivDvorane FROM Dvorana WHERE (DvoranaID NOT IN (SELECT IDDvorana FROM Rezervacija WHERE (PeriodOd &gt;= @PeriodOd) AND (PeriodDo &lt;= @PeriodDo)))">
                <SelectParameters>
                    <asp:SessionParameter DefaultValue="" Name="PeriodOd" SessionField="time-begin" />
                    <asp:SessionParameter DefaultValue="" Name="PeriodDo" SessionField="time-end" />
                </SelectParameters>
            </asp:SqlDataSource>
        
            <br />
        <div id="userArea" runat="server"> 
            <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="DostupneDvorane" DataTextField="NazivDvorane" DataValueField="DvoranaID" AutoPostBack="True">
        </asp:DropDownList>
&nbsp;<p>   
            <span id="par" runat="server">Pozovite sudionike na sastanak:</span><p>
                
            <asp:GridView ID="GridView2" runat="server" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="KorisnikID" DataSourceID="DostupniKorisnici" GridLines="None">
                <Columns>
                    <asp:TemplateField HeaderText="Korisničko ime">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Text='<%# Eval("KorisnickoIme") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Ime" HeaderText="Ime" SortExpression="Ime" />
                    <asp:BoundField DataField="Prezime" HeaderText="Prezime" SortExpression="Prezime" />
                    <asp:BoundField DataField="EmailAdresa" HeaderText="Email adresa" SortExpression="EmailAdresa" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="DostupniKorisnici" runat="server" DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="RezervacijeTableAdapters.KorisnikTableAdapter" InsertMethod="Insert" UpdateMethod="Update">
                <DeleteParameters>
                    <asp:Parameter Name="Original_KorisnikID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="KorisnickoIme" Type="String" />
                    <asp:Parameter Name="Ime" Type="String" />
                    <asp:Parameter Name="Prezime" Type="String" />
                    <asp:Parameter Name="EmailAdresa" Type="String" />
                </InsertParameters>
                <SelectParameters>
                    <asp:SessionParameter Name="UserID" SessionField="user_id" Type="String" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="KorisnickoIme" Type="String" />
                    <asp:Parameter Name="Ime" Type="String" />
                    <asp:Parameter Name="Prezime" Type="String" />
                    <asp:Parameter Name="EmailAdresa" Type="String" />
                    <asp:Parameter Name="Original_KorisnikID" Type="Int32" />
                </UpdateParameters>
            </asp:ObjectDataSource>
        <p>
            
        <p>
            <br />

            <span id="par2" runat="server">Ili pozovite korisnike putem email adrese (adrese odvajajte zarezom):</span> <br />
            <asp:TextBox ID="emailInvite" runat="server" Height="22px" Width="331px" TextMode="MultiLine"></asp:TextBox>
            <br />
        <p>

        

        <p>
            <asp:Button ID="btnEnd" runat="server" OnClick="btnEnd_Click" Text="Završi" />
            </div>  

    </form>

    </body>
</html>
