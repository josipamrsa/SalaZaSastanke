﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

    <head runat="server">

        <title>Login - Tommy</title>  
        <link rel="stylesheet" type="text/css" href="css/login.css" />

    </head>

    <body>

        <form id="loginForm" runat="server">

            <div id="login-area">

                <img src="img/tommy_logo.jpg" id="tommy-logo"/><br /><br /> 
               
                <asp:Label ID="lblKIme" runat="server" Text="Korisničko ime: "></asp:Label>&nbsp;  
                <asp:TextBox ID="korisnickoIme" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="lblPass" runat="server" Text="Lozinka: "></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                <input id="passWord" type="password" runat="server"/><br />

                <br /><br />
                <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Prijava" Height="29px" Width="91px" />

                <asp:RequiredFieldValidator ID="rfVal1" runat="server" ControlToValidate="korisnickoIme" ErrorMessage="Unesite vaše korisničko ime!">*</asp:RequiredFieldValidator>   
                <asp:CustomValidator ID="cVal1" runat="server" ControlToValidate="korisnickoIme" ErrorMessage="Nepostojeći korisnik - ukoliko zelite unijeti novog korisnika, kliknite &quot;Registracija&quot;.">*</asp:CustomValidator>  

                <asp:RequiredFieldValidator ID="rfVal2" runat="server" ControlToValidate="passWord" ErrorMessage="Molimo unesite lozinku!">*</asp:RequiredFieldValidator>

            </div>

            <br />
            
            <div id="error-area">

                <asp:Label ID="lblInfo" runat="server"></asp:Label>
                <asp:ValidationSummary ID="valSumm1" runat="server" DisplayMode="SingleParagraph" />

            </div>        
        </form>
    </body>
</html>
