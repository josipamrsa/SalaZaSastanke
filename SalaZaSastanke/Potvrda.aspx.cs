using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class Potvrda : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool flag = CheckAllQueryStrings();
        if (flag == false)
        {
            string replyToken = Request.QueryString["replyToken"].ToString();
            int userReplied = Convert.ToInt32(Request.QueryString["userReplied"]);
            int userAttending = Convert.ToInt32(Request.QueryString["userAttending"]);
            int confirmationId = Convert.ToInt32(Request.QueryString["confirmationId"]);

            updateConfirmation(confirmationId, userAttending, userReplied, replyToken);
            lblInfo.Text = "Vaš odgovor je zabilježen.";
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
        if (Request.QueryString["userReplied"] == null) { missingQueryString = true; }
        if (Request.QueryString["userAttending"] == null) { missingQueryString = true; }
        if (Request.QueryString["confirmationId"] == null) { missingQueryString = true; }

        return missingQueryString;
    }

    private void updateConfirmation(int id, int userConf, int userReply, string replyToken)
    {       
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;

        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("UserReply", connection);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@PotvrdaId", id);
        cmd.Parameters.AddWithValue("@KorisnikDolazi", userConf);
        cmd.Parameters.AddWithValue("@KorisnikJeOdgovorio", userReply);
        cmd.Parameters.AddWithValue("@TokenOdgovora", replyToken);

        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
    }
}