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

public partial class Rezervacija : System.Web.UI.Page
{  
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (Session["squery"] == null && Session["user_id"] == null)
        {
            Response.Redirect("Login.aspx");
            return;   
        }

        DropDownList1.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        btnEnd.Visible = false;
        par.Visible = false;
        par2.Visible = false;
        emailInvite.Visible = false;

        eventDate.Attributes["min"] = DateTime.Today.Date.ToString("yyyy-MM-dd");                    
    }

    protected void btnProvjeriDvorane_Click(object sender, EventArgs e)
    {
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
        foreach(string u in users)
        {           
            string uEmail = getUserEmail(u);
            string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("UserConfirmation", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", uEmail);
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
        Response.Redirect("Pocetna.aspx");
    }


    private void sendConfMail(TextBox email, int rez, Label l)
    {
        if (email.Text != "")
        {
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress("sso.rezervacije@tommy.hr");
            string[] mails = email.Text.Split(',');

            foreach(string m in mails)
            {
                mm.To.Add(new MailAddress(m));
                mm.Subject = "Poziv na sastanak";
                mm.Body = @"Poštovanje, 
                            Pozivamo vas na sastanak.";

            }         
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            //client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            //client.Credentials = new NetworkCredential("sso.apprezervacije", "Nak0nN0ciD0laziDan");
            client.Send(mm);
            Label1.Text = "poslano";
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Pocetna.aspx");
    }
}
