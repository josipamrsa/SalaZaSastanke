using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Profil : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["squery"] != null && Session["user_id"] != null)
        {
            string q = Session["squery"].ToString();
            string u = Session["user_id"].ToString();
        }

        else { Response.Redirect("Login.aspx"); }
        
        
        int rNum = GridView1.Rows.Count;
        for (int i = 0; i < rNum; i++)
        {
            CheckBox cb = GridView1.Rows[i].Cells[6].FindControl("CheckBox1") as CheckBox;

            if (cb.Checked)
            {
                GridView1.Rows[i].Cells[6].Text = "Dolazim";
            }

            else
            {
                GridView1.Rows[i].Cells[6].Text = "Ne dolazim";
            }
        }       
    }


    protected void btnLogOut_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("Login.aspx");
    }

   
}