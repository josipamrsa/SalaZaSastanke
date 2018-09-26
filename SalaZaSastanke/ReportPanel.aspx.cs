using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportPanel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["squery"] != null && Session["user_id"] != null)
        {
            string userRole = Session["user_role"].ToString();
            if (userRole == "report" || userRole == "admin")
            {
                lblTitle.Text = "<h1>Report panel</h1>";
                lblTimestamp.Text = "Podaci i statistika za <b>" + DateTime.Now.Date.ToString("dddd, dd. MMMM yyyy.") + "</b>";
            }

            else
            {
                lblTitle.Text = "Nemate ovlasti za pristup ovom dijelu aplikacije.";
                          
                tblContent.Attributes["hidden"] = "true";
                tblContent2.Attributes["hidden"] = "true";
            }

        }

        else { Response.Redirect("Login.aspx"); }

    }

    protected void btnLogOut_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("Login.aspx");
    }

}