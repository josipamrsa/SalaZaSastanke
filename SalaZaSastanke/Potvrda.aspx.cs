using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/*
       
    Event (PageLoad) - Provjerava jesu svi QueryStringovi za update baze prisutni. Ako jesu, parsira dobivene podatke
    i ažurira korisnički odgovor za dobiveni token odgovora.

    checkAllQueryStrings() - Provjerava sve QueryStringove, vraća true ako nisu prisutni.

    updateConfirmation() - Ažurira korisnički odgovor.
     
     
     */
public partial class Potvrda : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool flag = CheckAllQueryStrings();
        if (flag == false)
        {
            string replyToken = Request.QueryString["replyToken"].ToString();         
            int userAttending = Convert.ToInt32(Request.QueryString["userAttending"]);                                  
            updateConfirmation(replyToken, userAttending, 1);
            lblInfo.Text = "<h1>Vaš odgovor je zabilježen.</h1>";
        }

        else
        {
            lblInfo.Text = "Greška!";
        }
        
    }

    private bool CheckAllQueryStrings()
    {
        bool missingQueryString = false;

        if (Request.QueryString["replyToken"] == null) { missingQueryString = true; }        
        if (Request.QueryString["userAttending"] == null) { missingQueryString = true; }
       
        return missingQueryString;
    }

    private void updateConfirmation(string replyToken, int userAttending, int userReplied)
    {       
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("UserReply", connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@KorisnikDolazi", userAttending);
            cmd.Parameters.AddWithValue("@KorisnikJeOdgovorio", userReplied);
            cmd.Parameters.AddWithValue("@TokenOdgovora", replyToken);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
        
        catch (Exception ex)
        {
            lblInfo.Text = "Greska! " + ex.Message;
        }
    }

}