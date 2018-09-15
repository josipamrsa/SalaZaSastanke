﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pocetna : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {      
        if (Session["squery"] != null && Session["user_id"] != null)
        {           
            string u = Session["user_id"].ToString();
            Label1.Text = "Trenutno ste ulogirani kao: " + u + "<br>";         
        }

        else { Response.Redirect("Login.aspx"); }
    }

    protected void btnLogOut_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("Login.aspx");
    }
}