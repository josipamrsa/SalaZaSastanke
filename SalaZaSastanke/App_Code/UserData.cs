using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class WebService : System.Web.Services.WebService
{

    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(EnableSession = true)]
    public void fetchDataUser()
    {
        if (Session["user_id"] != null)
        {
            List<User> userDetails = new List<User>();
            string CS = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
            string sqlQuery = "SELECT KorisnickoIme, Ime, Prezime, EmailAdresa, Titula, Telefon FROM Korisnik WHERE KorisnickoIme = @Korisnicko";
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Korisnicko", Session["user_id"]);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    User us = new User();
                    us.userName = rdr["KorisnickoIme"].ToString();
                    us.firstName = rdr["Ime"].ToString();
                    us.lastName = rdr["Prezime"].ToString();
                    us.emailAddress = rdr["EmailAdresa"].ToString();
                    us.telephoneNumber = rdr["Telefon"].ToString();
                    us.jobTitle = rdr["Titula"].ToString();
                    userDetails.Add(us);
                }
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(userDetails));
        }

        else
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize("Error!"));
        }
    }

    public string getUserEmail(string username)
    {
        string umail = "";
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        string sqlQuery = "SELECT EmailAdresa FROM Korisnik WHERE KorisnickoIme = @UserName";
        SqlConnection connection = new SqlConnection(connectionString);

        SqlCommand cmd = new SqlCommand(sqlQuery, connection);
        cmd.Parameters.AddWithValue("@UserName", username);
        try
        {
            cmd.Connection.Open();
            umail = Convert.ToString(cmd.ExecuteScalar());
        }
        catch (Exception ex) { }
        finally { cmd.Connection.Close(); }

        return umail;
    }

    public List<int> getReservationID(string email)
    {
        List<int> rid = new List<int>();

        string CS = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        string sqlQuery = "SELECT IDRezervacija FROM Potvrda WHERE Email = @Email";

        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Email", email);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                rid.Add(Convert.ToInt32(rdr["IDRezervacija"]));              
            }
        }

        return rid;
    }

    [WebMethod(EnableSession = true)]
    public void loadUserInvitations()
    {
        if (Session["user_id"] != null)
        {
            List<Participant> userInvitations = new List<Participant>();
            string invitedName = getUserEmail(Session["user_id"].ToString());
            List<int> reservationID = getReservationID(invitedName);

            foreach (int i in reservationID)
            {
                string sqlQuery = @"SELECT PeriodOd, PeriodDo, d.Lokacija, d.Adresa, d.NazivDvorane, p.PotvrdaID, r.OpisDogadjaja
                                FROM Rezervacija r, Dvorana d, Potvrda p
                                WHERE r.RezervacijaID = @IDRez AND p.Email = @Mail AND r.IDDvorana = d.DvoranaID 
                                AND r.RezervacijaID = p.IDRezervacija AND p.KorisnikJeOdgovorio IS NULL";

                string CS = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
                using (SqlConnection con = new SqlConnection(CS))
                {

                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@IDRez", i);
                    cmd.Parameters.AddWithValue("@Mail", invitedName);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Participant ps = new Participant();
                        ps.userName = Session["user_id"].ToString();
                        string[] dateSplit = new string[4];
                        dateSplit = rdr["PeriodOd"].ToString().Split(' ');                      
                        ps.dateEvent = dateSplit[0];
                        ps.beginPeriod = dateSplit[1];
                        dateSplit = rdr["PeriodDo"].ToString().Split(' ');                        
                        ps.endPeriod = dateSplit[1];
                        ps.location = rdr["Lokacija"].ToString();
                        ps.address = rdr["Adresa"].ToString();
                        ps.hallName = rdr["NazivDvorane"].ToString();
                        ps.eventInfo = rdr["OpisDogadjaja"].ToString();
                        ps.confId = Convert.ToInt32(rdr["PotvrdaID"]);
                        userInvitations.Add(ps);
                    }
                }

                
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(userInvitations));
        }

        else
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize("Error!"));
        }
    }
}

  

