<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

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

                <asp:Label ID="lblKIme" runat="server" Text="Korisničko ime: "></asp:Label>        
                <asp:TextBox ID="korisnickoIme" runat="server"></asp:TextBox>
                <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Log in" Height="29px" Width="91px" />

                <asp:RequiredFieldValidator ID="rfVal1" runat="server" ControlToValidate="korisnickoIme" ErrorMessage="Unesite vaše korisničko ime!">*</asp:RequiredFieldValidator>   
                <asp:RegularExpressionValidator ID="reVal1" runat="server" ControlToValidate="korisnickoIme" ErrorMessage="Neispravan oblik korisničkog imena!" ValidationExpression="^[a-z0-9\.]*@tommy$">*</asp:RegularExpressionValidator>
                <asp:CustomValidator ID="cVal1" runat="server" ControlToValidate="korisnickoIme" ErrorMessage="Nepostojeći korisnik - ukoliko zelite unijeti novog korisnika, kliknite &quot;Registracija&quot;.">*</asp:CustomValidator>  

            </div>

            <br />

            <div id="registration-area">

                <asp:Button ID="btnReg" runat="server" CausesValidation="False" OnClick="btnReg_Click" Text="Želite li registrirati novog člana? Kliknite ovdje!" />        
            
            </div>

            <br />

            <div id="error-area">

                <asp:Label ID="lblInfo" runat="server"></asp:Label>
                <asp:ValidationSummary ID="valSumm1" runat="server" DisplayMode="SingleParagraph" />

            </div>        
        </form>
    </body>
</html>
