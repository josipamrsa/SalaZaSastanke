using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

/*
 
     Event (PageLoad) - Provjera je li se korisnik ulogirao, dohvat uloge korisnika za neke dijelove aplikacije, 
     pozdrav korisniku na temelju doba dana. Ukoliko neulogirani korisnik pokuša pristupiti, vraća ga se na login
     formu.

     Event (btnLogOut) - Odlogirava korisnika iz aplikacije.

     getTimeOfDay() - Dohvaća trenutno vrijeme te na temelju toga formira pozdrav korisniku.

     getUserRole() - Dohvaća ulogu korisnika na temelju trenutno ulogiranog.

     updateConfirmation() - Na početnoj stranici prikazuju se svi pozivi korisniku od drugih korisnika. Ova metoda
     služi za spremanje korisničkog odgovora na temelju koje je dugme pritisnuo (Prihvati/Odbij). Token odgovora dohvaća
     iz skrivene kontrole za svaku obavijest. Možda postoji bolji način za to dohvatiti?

     Events (btnPotvrdi, btnOdbij) - dohvaća token odgovora te ga šalje skupa s korisničkim odgovorom (Dolazi/Ne dolazi)
     
*/

public partial class Pocetna : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["squery"] != null && Session["user_id"] != null)
        {
            string uRole = getUserRole();
            if(uRole!="") { Session["user_role"] = uRole; }
           
            string userId = Session["user_id"].ToString();
            loginInfo.Text = userId + "<br>";
            getTimeOfDay(lblWelcome);
                      
        }

        else { Response.Redirect("Login.aspx"); }       
    }

    protected void btnLogOut_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("Login.aspx");
    }

    private void getTimeOfDay(Label l)
    {

        string time = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
        if (DateTime.Parse(time) > DateTime.Parse("06:00") && DateTime.Parse(time) < DateTime.Parse("12:00"))
        {
            l.Text = "Dobro jutro!";
        }

        else if (DateTime.Parse(time) > DateTime.Parse("12:00") && DateTime.Parse(time) < DateTime.Parse("18:00"))
        {
            l.Text = "Dobar dan!";
        }

        else
        {
            l.Text = "Dobra večer!";
        }
            
    }
   
    private string getUserRole()
    {
        string userId = Session["user_id"].ToString();
        string userRole = "";
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        string sqlQuery = @"SELECT NazivRazine FROM Razina 
                                            WHERE RazinaID = (SELECT IDRazina 
                                            FROM Korisnik 
                                            WHERE KorisnickoIme = @Korisnicko)";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Korisnicko", userId);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                userRole = rdr["NazivRazine"].ToString();
            }
        }

        return userRole;              

    }
    private void updateConfirmation(string id, int userConf, int userReply)
    {       
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("UserReply", connection);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@KorisnikDolazi", userConf);
        cmd.Parameters.AddWithValue("@KorisnikJeOdgovorio", userReply);
        cmd.Parameters.AddWithValue("@TokenOdgovora", id);

        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
    }
      
    protected void btnPotvrdi_Click(object sender, EventArgs e)
    {
        string id = ModalId.Value;
        updateConfirmation(id, 1, 1);
        lblInfo.Text = "Potvrdili ste dolazak. Vaš odgovor naknadno možete promijeniti sa profila.";
    }

    protected void btnOdbij_Click(object sender, EventArgs e)
    {
        string id = ModalId.Value;
        updateConfirmation(id, 0, 1);
        lblInfo.Text = "Odbili ste poziv. Vaš odgovor naknadno možete promijeniti sa profila.";
    }
   
}

