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
                Label1.Text = "Dobrodošli u report panel";
            }

            else
            {
                Label1.Text = "Nemate ovlasti za pristupiti ovom dijelu aplikacije.";
                LinkButton returnBtn = new LinkButton();
                returnBtn.PostBackUrl = "Pocetna.aspx";
                returnBtn.Text = "Povratak na početnu...";
                form1.Controls.Add(returnBtn);
            }

        }

        else { Response.Redirect("Login.aspx"); }
    }
}