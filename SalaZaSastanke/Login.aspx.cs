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


/*
 
    NewToken() - Generira token za prijavu na sustav - čisto da se zna je li korisnik ulogiran ili ne. Koristi se u kombinaciji s 
    imenom korisnika.

    Event (btnLogIn) - Provjerava postoji li korisnik na serveru te provjerava podudaraju li se podaci s onima na serveru. Ako ne postoji, 
    a postoji na serveru, tada sa servera vuče podatke u bazu lokalno kod prve prijave.

    GetUserId() - dohvaća ID korisnika u bazi

    CheckForUserAD() - provjerava postoji li korisnik lokalno putem podataka iz AD servera

    InputNewUser() - ukoliko se korisnik prvi puta prijavljuje u aplikaciju, onda se pokreće ova metoda za dohvaćanje i spremanje
    podataka s AD servera lokalno u bazu.    
     
*/

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
        lblInfo.Text = "";
        try
        {                      
            PrincipalContext ADserv = new PrincipalContext(ContextType.Domain, "", "", ""); // login data
            UserPrincipal byUName = UserPrincipal.FindByIdentity(ADserv, IdentityType.SamAccountName, korisnickoIme.Text);

            bool matchingCredentials = ADserv.ValidateCredentials(korisnickoIme.Text, passWord.Value);

            if (byUName != null && matchingCredentials)
            {
                bool userExistsInDB = CheckForUserAD(byUName, lblInfo);

                if (!userExistsInDB)
                {
                    try {
                        InputNewUser(byUName, lblInfo);
                        Session["squery"] = NewToken();
                        Session["user_id"] = korisnickoIme.Text; 
                        Response.Redirect("Pocetna.aspx");
                    }
                    catch (Exception ec) { lblInfo.Text = "Pogreška pri unosu u bazu: "+ec.Message; }                                                    
                }

                else
                {
                    Session["squery"] = NewToken();
                    Session["user_id"] = korisnickoIme.Text;
                    Response.Redirect("Pocetna.aspx");
                }
            }

            else if (byUName != null && !matchingCredentials)
            {
                lblInfo.Text += "Korisničko ime i lozinka se ne podudaraju.";
            }

            else { lblInfo.Text += "Nepostojeći korisnik - Za pristup aplikaciji morate se registrirati."; }
            
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
        cmd.Parameters.AddWithValue("@UserName", uName.SamAccountName);
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
  
    public bool CheckForUserAD(UserPrincipal uName, Label lblMessage)
    {
        int userCount = 0;
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        string sqlQuery = "SELECT COUNT(1) FROM Korisnik WHERE KorisnickoIme = @UserName";
        SqlConnection connection = new SqlConnection(connectionString);
        
        SqlCommand cmd = new SqlCommand(sqlQuery, connection);
        cmd.Parameters.AddWithValue("@UserName", uName.SamAccountName);
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
        cmd.Parameters.AddWithValue("@KorisnickoIme", uName.SamAccountName);
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
