using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Cryptography;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Web.Script.Serialization;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;


public partial class Login : System.Web.UI.Page
{
    public static string NewToken()
    {
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] randomBuffer = new byte[16];
            rng.GetBytes(randomBuffer);

            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(randomBuffer);

                StringBuilder sBuilder = new StringBuilder();
                foreach (byte byt in hashBytes)
                {
                    sBuilder.Append(byt.ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e) { }

    protected void btnReg_Click(object sender, EventArgs e)
    {
        Response.Redirect("Registracija.aspx");
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {    
        try
        {           
            bool checkDB = CheckForUserDB(korisnickoIme.Text, lblInfo);
            PrincipalContext ADserv = new PrincipalContext(ContextType.Domain, "192.168.252.4", "sso.apprezervacije", "Nak0nN0ciD0laziDan");
            UserPrincipal byUName = UserPrincipal.FindByIdentity(ADserv, IdentityType.UserPrincipalName, korisnickoIme.Text);
                     
            if (checkDB)
            {
                Session["squery"] = NewToken();
                Session["user_id"] = korisnickoIme.Text;
                Response.Redirect("Pocetna.aspx");
                return;               
            }          
            
            if (byUName != null)
            {
                bool userExistsInDB = CheckForUserAD(byUName, lblInfo);

                if (!userExistsInDB)
                {
                    try {
                        InputNewUser(byUName, lblInfo);
                        Session["squery"] = NewToken();
                        Session["user_id"] = korisnickoIme.Text; // izmijeniti da dobavljamo samo korisnicko ime, a ne korisnicki ID u bazi
                        Response.Redirect("Pocetna.aspx");
                    }
                    catch (Exception ec) { lblInfo.Text = "Pogreska pri unosu u bazu: "+ec.Message; }                                                    
                }

                else
                {
                    Session["squery"] = NewToken();
                    Session["user_id"] = korisnickoIme.Text;
                    Response.Redirect("Pocetna.aspx");
                }
            }

            else { lblInfo.Text += "Nepostojeći korisnik - ukoliko zelite unijeti novog korisnika, kliknite \"Registracija\"."; }
            
        }
        catch (Exception ex) { lblInfo.Text = ex.Message; }
    }

    public string getUserID(UserPrincipal uName, Label lblMessage)
    {
        string uid = "";
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        string sqlQuery = "SELECT KorisnikID FROM Korisnik WHERE KorisnickoIme = @UserName";
        SqlConnection connection = new SqlConnection(connectionString);

        SqlCommand cmd = new SqlCommand(sqlQuery, connection);
        cmd.Parameters.AddWithValue("@UserName", uName.UserPrincipalName);
        try
        {
            cmd.Connection.Open();
            uid = Convert.ToString(cmd.ExecuteScalar());
            lblMessage.Text = uid;       
        }
        catch (Exception ex) { lblMessage.Text = ex.Message; }
        finally { cmd.Connection.Close(); }

        return uid;
    }

    public bool CheckForUserDB(string username, Label lblMessage)
    {
        int userCount = 0;
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        string sqlQuery = "SELECT COUNT(1) FROM Korisnik WHERE KorisnickoIme = @UserName";
        SqlConnection connection = new SqlConnection(connectionString);

        SqlCommand cmd = new SqlCommand(sqlQuery, connection);
        cmd.Parameters.AddWithValue("@UserName", username);
        try
        {
            cmd.Connection.Open();
            userCount = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception ex) { lblMessage.Text = ex.Message; }
        finally { cmd.Connection.Close(); }

        if (userCount == 1) return true;
        else return false;
    }

    public bool CheckForUserAD(UserPrincipal uName, Label lblMessage)
    {
        int userCount = 0;
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        string sqlQuery = "SELECT COUNT(1) FROM Korisnik WHERE KorisnickoIme = @UserName";
        SqlConnection connection = new SqlConnection(connectionString);
        
        SqlCommand cmd = new SqlCommand(sqlQuery, connection);
        cmd.Parameters.AddWithValue("@UserName", uName.UserPrincipalName);
        try {
            cmd.Connection.Open();
            userCount = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception ex) { lblMessage.Text = ex.Message; }
        finally { cmd.Connection.Close(); }
        
        if (userCount == 1) return true;
        else return false;

    }
    public static void InputNewUser(UserPrincipal uName, Label lblMessage)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("DataStorageUser", connection);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@KorisnickoIme", uName.UserPrincipalName);
        cmd.Parameters.AddWithValue("@Ime", uName.GivenName);
        cmd.Parameters.AddWithValue("@Prezime", uName.Surname);
        cmd.Parameters.AddWithValue("@EmailAdresa", uName.EmailAddress);
        cmd.Parameters.AddWithValue("@Titula", uName.Description);
        cmd.Parameters.AddWithValue("@Telefon", uName.VoiceTelephoneNumber);
        cmd.Parameters.AddWithValue("@Razina", 5);

        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();

    }
    
}