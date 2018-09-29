using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Net;

using System.Security.Cryptography;
using System.Text;

/*
 
     Event (PageLoad) - Provjerava je li korisnik ulogiran. Skriva određene funkcionalnosti dok korisnik ne ispuni tražene podatke
     Postavlja minimalnu vrijednost DatePickera na današnji datum.

     NewToken() - Ovdje generira random tokene za zabilježavanje pozivnica korisnicima.

     Event (ProvjeraDvorana) - Provjerava unose korisnika. Sprema uneseno vrijeme početka i završetka te šalje dalje u
     SQL upit. Otkriva skrivene kontrole vezane za daljnji nastavak unosa podataka.

     getUserId() - Dohvaća ID korisnika iz baze za potrebe rezervacije dvorane

     getUserEmail() - Dohvaća email adresu korisnika iz baze za potrebe stvaranja pozivnice sudioniku

     usersToInvite() - Dohvaća listu korisničkih imena iz GridViewa koje je korisnik na formi označio za slanje poziva

     sendInvitationConfirmation() - Pomoću liste korisničkih imena dohvaća email adrese svih označenih korisnika, te dohvaća
     i email adrese koje je korisnik sam unio za slanje korisnicima koji nisu dio Tommyjeve mreže (tipa vanjski suradnici).
     Tu se stvara i sama potvrda koja se sprema u bazu podataka, skupa sa tokenom odgovora.

     Event (btnEnd) - Stvara se rezervacija, stvaraju se vezane potvrde te se šalju mailovi korisnicima koji su pozvani na sastanak

     sendConfMail() - Dohvaća sve potvrde koje su stvorene prethodno, te uzima mail adresu svakog od sudionika (putem metode iz
     WebService objekta) i šalje poruku s generiranim podacima te linkovima za odgovor. Kad korisnik klikne na jedan od linkova, 
     vodi ga se na stranicu potvrde gdje se zabilježava njegov odgovor. Klikanjem više puta linkova u mailu korisnik može promijeniti
     svoj odgovor, iako to nije iskodirano pri odgovoru na pozivnicu iz same aplikacije.

     Event (btnBack) - Vraća na početnu stranicu.
     
     
     
     */
public partial class Rezervacija : System.Web.UI.Page
{  
    public bool Flag { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (Session["squery"] == null && Session["user_id"] == null)
        {
            Response.Redirect("Login.aspx");
            return;   
        }

        Flag = false;
        DropDownList1.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        btnEnd.Visible = false;
        par.Visible = false;
        par2.Visible = false;
        par3.Visible = false;
        emailInvite.Visible = false;

        eventDate.Attributes["min"] = DateTime.Today.Date.ToString("yyyy-MM-dd");                    
    }

    private static string NewToken()
    {
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] randomBuffer = new byte[32];
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
    protected void btnProvjeriDvorane_Click(object sender, EventArgs e)
    {
        Label1.Text = "";
        
        if (eventDate.Value == "")
        {
            Label1.Text = "Molimo odaberite datum.";
            return;
        }

        if (timeBegin.Value == "")
        {
            Label1.Text = "Molimo odaberite vrijeme početka.";
            return;
        }

        if (timeEnd.Value == "")
        {
            Label1.Text = "Molimo odaberite vrijeme završetka.";
            return;
        }

        if (DateTime.Parse(timeBegin.Value) > DateTime.Parse(timeEnd.Value))
        {
            Label1.Text = "Vrijeme završetka ne može biti ranije od vremena početka.";
            return;
        }

        GridView1.DataBind();                       

        string timestampBegin = eventDate.Value + " " + timeBegin.Value;
        string timestampEnd = eventDate.Value + " " + timeEnd.Value;       
        
        Session["time-begin"] = timestampBegin;
        Session["time-end"] = timestampEnd;

        DropDownList1.Visible = true;
        GridView1.Visible = true;
        GridView2.Visible = true;
        btnEnd.Visible = true;
        par.Visible = true;
        par2.Visible = true;
        par3.Visible = true;
        emailInvite.Visible = true;              
    }

    public string getUserID(string username)
    {
        string uid = "";
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        string sqlQuery = "SELECT KorisnikID FROM Korisnik WHERE KorisnickoIme = @UserName";
        SqlConnection connection = new SqlConnection(connectionString);

        SqlCommand cmd = new SqlCommand(sqlQuery, connection);
        cmd.Parameters.AddWithValue("@UserName", username);
        try
        {
            cmd.Connection.Open();
            uid = Convert.ToString(cmd.ExecuteScalar());          
        }
        catch (Exception ex) { Label1.Text = ex.Message; }
        finally { cmd.Connection.Close(); }

        return uid;
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
        catch (Exception ex) { Label1.Text = ex.Message; }
        finally { cmd.Connection.Close(); }

        return umail;
    }

   
    public List<String> usersToInvite(GridView g, Label l)
    {     
        int rNum = g.Rows.Count;
        List<String> userNames = new List<string>();
        for (int i = 0; i < rNum; i++)
        {
            CheckBox cb = g.Rows[i].Cells[0].FindControl("CheckBox1") as CheckBox;
            if (cb.Checked)
            {              
                userNames.Add(cb.Text);
            }
        }
        return userNames;                
    }

   
    public void sendInvitationConfirmation(int rezID)
    {
        List<String> users = usersToInvite(GridView2, Label1);
        
        List<String> otherParticipants = new List<string>();
        List<String> allParticipants = new List<string>();

        if (emailInvite.Text!="")
        {
            string[] vanjskiKorisnici = emailInvite.Text.Split(',');
            foreach (string v in vanjskiKorisnici) { otherParticipants.Add(v); }
        }

        foreach(string u in users)
        {           
            string uEmail = getUserEmail(u);
            allParticipants.Add(uEmail);           
        }
       
        foreach (string v in otherParticipants)
        {
            allParticipants.Add(v);
            
        }
       
        if (allParticipants.Count > 0)
        {
            foreach (string p in allParticipants)
            {
                string id = NewToken();
                string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("UserConfirmation", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", p);
                cmd.Parameters.AddWithValue("@IDRez", rezID);
                cmd.Parameters.AddWithValue("@TokenOdgovora", id);

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
                }
            }
        }
      
    }

    protected void btnEnd_Click(object sender, EventArgs e)
    {                 
        int reservationID = 0;
        if (DropDownList1.SelectedIndex > -1)
        {
            int userID = int.Parse(getUserID(Session["user_id"].ToString()));
            string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("HallReservation", connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PeriodOd", Session["time-begin"]);
            cmd.Parameters.AddWithValue("@PeriodDo", Session["time-end"]);
            cmd.Parameters.AddWithValue("@OpisDogadjaja", eventDesc.Text);
            cmd.Parameters.AddWithValue("@Status", "active");
            cmd.Parameters.AddWithValue("@IDKorisnik", userID);
            cmd.Parameters.AddWithValue("@IDDvorana", DropDownList1.SelectedItem.Value);
            SqlParameter idRez = new SqlParameter("@IDRez", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(idRez);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                reservationID = Convert.ToInt32(idRez.Value);
            }

            catch (Exception ex)
            {
                Label1.Text = ex.Message;
            }

            finally
            {
                cmd.Connection.Close();
            }
        }

        sendInvitationConfirmation(reservationID);
        sendConfMail(reservationID);
        //Label1.Text = "Dvorana rezervirana. Pozivnice su poslane.";
        Response.Redirect("Pocetna.aspx");              
    }


    private void sendConfMail(int rID)
    {       
        List<Participant> userInvitations = new List<Participant>();       
        string sqlQuery = @"SELECT PeriodOd, PeriodDo,d.Lokacija, d.Adresa, d.NazivDvorane, p.TokenOdgovora, p.Email, r.OpisDogadjaja
                                FROM Rezervacija r, Dvorana d, Potvrda p
                                WHERE r.RezervacijaID = @IDRez AND r.IDDvorana = d.DvoranaID 
                                AND r.RezervacijaID = p.IDRezervacija AND p.KorisnikJeOdgovorio = 0";

        string CS = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        using (SqlConnection con = new SqlConnection(CS))
        {

            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@IDRez", rID);
           
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Participant ps = new Participant();                
                ps.userName = rdr["Email"].ToString();                
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
                ps.replyToken = rdr["TokenOdgovora"].ToString();
                userInvitations.Add(ps);
            }
        }

        /*

       Inicijalne postavke

       Smtp server: mail.tommy.hr
       Server port: 465
       Authentication type: SSL
       E-mail: robot.aplikacije@tommy.hr
       Username: robot.aplikacije@tommy.hr
       Password: WN#6mVX4 
       
       TODO - provjeriti zasto SMTPS port ne radi (465) na ovoj masini        

        */

        SmtpClient client = new SmtpClient();
        client.UseDefaultCredentials = false;
        client.Host = "mail.tommy.hr";       
        client.Port = 25; 
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential("robot.aplikacije@tommy.hr", "WN#6mVX4");    

        foreach (Participant p in userInvitations)
        {
            try
            {
                LinkButton confirm = new LinkButton();
                LinkButton decline = new LinkButton();

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("robot.aplikacije@tommy.hr");

                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
                string replyToken = p.replyToken;

                confirm.PostBackUrl = baseUrl + "/Potvrda.aspx?replyToken=" + replyToken + "&userAttending=1";
                decline.PostBackUrl = baseUrl + "/Potvrda.aspx?replyToken=" + replyToken + "&&userAttending=0";

                mm.To.Add(new MailAddress(p.userName));
                mm.Subject = "Obavijest o pozivu na sastanak";
                mm.IsBodyHtml = true;
                mm.Body = "Poštovanje,<br><br>Pozivamo vas na sastanak na dan " + p.dateEvent + ", u terminu od " + p.beginPeriod + " do " + p.endPeriod + ".<br>"
                            + "Mjesto sastanka je " + p.address + ", " + p.location + ", u dvorani " + p.hallName + ".<br>"
                            + "Tema sastanka je " + p.eventInfo + ".<br>"
                            + "Da biste prihvatili ili odbili poziv, molimo Vas da kliknete jednu od poveznica ispod.<br><br>"
                            + "<a href='" + baseUrl + "/Potvrda.aspx?replyToken=" + replyToken + "&userAttending=1'>Potvrdi dolazak</a>"
                            + " | <a href='" + baseUrl + "/Potvrda.aspx?replyToken=" + replyToken + "&&userAttending=0'>Odbij pozivnicu</a>";

                client.Send(mm);
            }

            catch {}                          
        }       
    }
  
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Pocetna.aspx");
    }
    
}
