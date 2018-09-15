using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;


public partial class Registracija : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public bool checkForTakenUsername(string userName, Label lblMessage)
    {
        int userCount = 0;
        bool userExists;
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        string sqlQuery = "SELECT COUNT(1) FROM Korisnik WHERE KorisnickoIme = @UserName";
        SqlConnection connection = new SqlConnection(connectionString);

        SqlCommand cmd = new SqlCommand(sqlQuery, connection);
        cmd.Parameters.AddWithValue("@UserName", userName);
        try
        {
            cmd.Connection.Open();
            userCount = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception ex) { lblMessage.Text = ex.Message; }
        finally { cmd.Connection.Close(); }

        if (userCount == 1) { userExists = true; }
        else { userExists = false; }
        return userExists;
    }

    protected void btnRegs_Click(object sender, EventArgs e)
    {
        bool userNameExists = checkForTakenUsername(kImeTbx.Text, Label1);

        if (userNameExists)
        {
            Label1.Text = "Uneseno korisnicko ime je zauzeto. Molimo vas da odaberete drukcije ime!";
            return;
        }

        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("DataStorageUser", connection);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@KorisnickoIme", kImeTbx.Text+"@tommy");
        cmd.Parameters.AddWithValue("@Ime", imeTbx.Text);
        cmd.Parameters.AddWithValue("@Prezime", prezimeTbx.Text);
        cmd.Parameters.AddWithValue("@EmailAdresa", mailTbx.Text);
        cmd.Parameters.AddWithValue("@Titula", ttlTbx.Text);
        cmd.Parameters.AddWithValue("@Telefon", telTbx.Text);
        cmd.Parameters.AddWithValue("@Razina", 5);

        try
        {
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            Label1.Text = ex.Message;
        }

        finally
        {
            cmd.Connection.Close();
            Response.Redirect("Login.aspx");
        }
    }
}