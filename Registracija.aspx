<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Registracija.aspx.cs" Inherits="Registracija" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Registracija korisnika</h1><br />
        <label>Korisnicko ime: </label>
        
        <asp:TextBox ID="kImeTbx" runat="server"></asp:TextBox> @tommy
        
        <br />
        <label>Ime: </label>
        
        <asp:TextBox ID="imeTbx" runat="server"></asp:TextBox>
        
        <br />
        <label>Prezime: </label>
        
        <asp:TextBox ID="prezimeTbx" runat="server"></asp:TextBox>
        
        <br />
        <label>Email adresa: </label>
        
        <asp:TextBox ID="mailTbx" runat="server"></asp:TextBox>
        
        <br />
        <label>Telefon: </label>
        
        <asp:TextBox ID="telTbx" runat="server"></asp:TextBox>
        
        <br />
        <label>Titula: </label>
        
        <asp:TextBox ID="ttlTbx" runat="server"></asp:TextBox>
        
        <br />
        <br />
        <asp:Button ID="btnRegs" runat="server" OnClick="btnRegs_Click" Text="Registriraj se" />
        
        <br />        <br />    
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
    </form>
</body>
</html>
