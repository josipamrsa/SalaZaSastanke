<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>  

</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="Label1" runat="server" Text="Korisničko ime: "></asp:Label>
        <asp:TextBox ID="korisnickoIme" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="korisnickoIme" ErrorMessage="Unesite vaše korisničko ime!">*</asp:RequiredFieldValidator>
    
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="korisnickoIme" ErrorMessage="Neispravan oblik korisničkog imena!" ValidationExpression="^[a-z0-9\.]*@tommy$">*</asp:RegularExpressionValidator>
        <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="korisnickoIme" ErrorMessage="Nepostojeći korisnik - ukoliko zelite unijeti novog korisnika, kliknite &quot;Registracija&quot;.">*</asp:CustomValidator>
    
    </div>
        <p>
            <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" />
        &nbsp;<asp:Button ID="btnReg" runat="server" CausesValidation="False" OnClick="btnReg_Click" Text="Registracija" />
        &nbsp;</p>
        <asp:Label ID="Label2" runat="server"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    </form>
</body>
</html>
