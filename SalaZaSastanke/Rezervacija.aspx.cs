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
        par3.Visible = false;
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

                string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("UserConfirmation", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", p);
                cmd.Parameters.AddWithValue("@IDRez", rezID);

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

        Response.Redirect("Pocetna.aspx");
               
    }


    private void sendConfMail(int rID)
    {       
        List<Participant> userInvitations = new List<Participant>();       
        string sqlQuery = @"SELECT PeriodOd, PeriodDo,d.Lokacija, d.Adresa, d.NazivDvorane, p.PotvrdaID, p.Email, r.OpisDogadjaja
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
                ps.confId = Convert.ToInt32(rdr["PotvrdaID"]);
                userInvitations.Add(ps);
            }
        }       

        SmtpClient client = new SmtpClient();
        client.UseDefaultCredentials = false;
        client.Host = "smtp.gmail.com";
        client.Port = 587;
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential("whatusernameisalloweddarn@gmail.com", "a1b2c3d4@");   
                    
        foreach (Participant p in userInvitations)
        {
            try
            {
                LinkButton confirm = new LinkButton();
                LinkButton cancel = new LinkButton();

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("user@tommy.hr");

                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
                string replyToken = NewToken();

                confirm.PostBackUrl = baseUrl + "/Potvrda.aspx?replyToken=" + replyToken + "&userReplied=1&userAttending=1&confirmationId=" + p.confId;
                cancel.PostBackUrl = baseUrl + "/Potvrda.aspx?replyToken=" + replyToken + "&userReplied=1&userAttending=0&confirmationId=" + p.confId;

                mm.To.Add(new MailAddress(p.userName));
                mm.Subject = "Obavijest o pozivu na sastanak";
                mm.IsBodyHtml = true;
                mm.Body = "Poštovanje,<br><br>Pozivamo vas na sastanak na dan " + p.dateEvent + ", u terminu od " + p.beginPeriod + " do " + p.endPeriod + ".<br>"
                            + "Mjesto sastanka je " + p.address + ", " + p.location + ", u dvorani " + p.hallName + ".<br>"
                            + "Tema sastanka je " + p.eventInfo + ".<br>"
                            + "Da biste prihvatili ili odbili poziv, molimo Vas da kliknete jednu od poveznica ispod.<br><br>"
                            + "<a href='" + baseUrl + "/Potvrda.aspx?replyToken=" + replyToken + "&userReplied=1&userAttending=1&confirmationId=" + p.confId + "'>Potvrdi dolazak</a>"
                            + " | <a href='" + baseUrl + "/Potvrda.aspx?replyToken=" + replyToken + "&userReplied=1&userAttending=0&confirmationId=" + p.confId + "'>Odbij pozivnicu</a>";

                client.Send(mm);
            }

            catch
            {               
                                                    
            }
                           
        }
        
    }

    
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Pocetna.aspx");
    }
    
}
