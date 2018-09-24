<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Registracija.aspx.cs" Inherits="Registracija" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registracija novog člana</title>
    <link rel="stylesheet" type="text/css" href="css/register.css" />
</head>
<body>
    <form id="form1" runat="server">
        <h1>Registracija korisnika</h1><br />
        <label>Korisničko ime: </label>
        
        <asp:TextBox ID="kImeTbx" runat="server"></asp:TextBox> @tommy
        
        <asp:RequiredFieldValidator ID="rfVal1" runat="server" ControlToValidate="kImeTbx" ErrorMessage="Unesite korisničko ime!">*</asp:RequiredFieldValidator>
        
        <br />
        <label>Ime: </label>
        
        <asp:TextBox ID="imeTbx" runat="server"></asp:TextBox>
        
        <asp:RequiredFieldValidator ID="rfVal2" runat="server" ControlToValidate="imeTbx" ErrorMessage="Unesite ime!">*</asp:RequiredFieldValidator>
        
        <br />
        <label>Prezime: </label>
        
        <asp:TextBox ID="prezimeTbx" runat="server"></asp:TextBox>
        
        <asp:RequiredFieldValidator ID="rfVal3" runat="server" ControlToValidate="prezimeTbx" ErrorMessage="Unesite prezime!">*</asp:RequiredFieldValidator>
        
        <br />
        <label>Email adresa: </label>
        
        <asp:TextBox ID="mailTbx" runat="server"></asp:TextBox>
        
        <asp:RequiredFieldValidator ID="rfVal4" runat="server" ControlToValidate="mailTbx" ErrorMessage="Unesite email adresu!">*</asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="reVal1" runat="server" ControlToValidate="mailTbx" ErrorMessage="Neispravan oblik email adrese!" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
        
        <br />
        <label>Telefon: </label>
        
        <asp:TextBox ID="telTbx" runat="server"></asp:TextBox>
        
        <asp:RequiredFieldValidator ID="rfVal5" runat="server" ControlToValidate="telTbx" ErrorMessage="Unesite kontakt telefon!">*</asp:RequiredFieldValidator>
        
        <br />
        <label>Titula: </label>
        
        <asp:TextBox ID="ttlTbx" runat="server"></asp:TextBox>
        
        <br />
        <br />
        <asp:Button ID="btnRegs" runat="server" OnClick="btnRegs_Click" Text="Registriraj se" /> &nbsp;
        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Povratak na login" CausesValidation="False" />
        
        <br />        <br />    
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
        <asp:ValidationSummary ID="valSumm1" runat="server" DisplayMode="List" />
    </form>
</body>
</html>
