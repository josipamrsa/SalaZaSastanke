using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

public partial class AdminPanel : System.Web.UI.Page
{
    /*
     
        Event (PageLoad) - Provjerava ulogiranost i nivo korisnika. Korisnik ne može pristupiti ovom dijelu aplikacije
        ukoliko nije razine "admin".
         
        Event (UnosDvorane) - Korisnik unosi podatke o novoj dvorani, a ova metoda ih dohvaća i sprema u bazu podataka.

        checkForDBRecords() - Dohvaća listu dvorana koje su prethodno ili trenutno rezervirane. Ukoliko vrati true, tada
        se ta dvorana ne može obrisati iz baze podataka.

        Event (btnObrisi) - Dohvaća odabranu dvoranu te provjerava pomoću prethodne metode ima li vezanih rezervacija. Ako
        ima, onda javlja grešku. U protivnom briše dvoranu iz baze.

        Event (azuriranjeDvorane) - Prema odabranoj stavci mijenja podatke u labelama kako bi korisnik imao ideju što treba
        oko te dvorane ažurirati. Moglo se riješiti unošenjem GridViewa sa Edit/Update metodama, ali programer ove aplikacije
        voli sebi otežavati stvari.

        Event (btnAzuriraj) - Ako je sve uneseno, ažurira podatke u bazi podataka vezano za odabranu dvoranu.

        Event (ddKorisnici) - Dohvaća podatke preko odabrane stavke, slično eventu azuriranja dvorane.

        Event (btnAzurirajKorisnik) - Ima dva načina rada. Ako je odabran unos preko AD-a, dohvaćaju se samo podaci sa AD servera
        te nije potrebno unositi podatke u polja jer ih se ionako ignorira. Ako je odabran ručni unos, ako su popunjena sva polja,
        ažuriraju se podaci o korisniku u bazi podataka. UnosAD() i UnosManual() su metode koje izvršavaju ažuriranje. Eventi kod
        RadioButtona samo mijenjaju podiže li dugme ažuriranja korisnika eventove za provjeru unosa.

         
         */
    protected void Page_Load(object sender, EventArgs e)
    {     
        lblAdminInfo.Text = "";
               
        if (Session["squery"] != null && Session["user_id"] != null)
        {
            string userRole = Session["user_role"].ToString();
            if (userRole == "admin")
            {
                lblTitle.Text = "<h1>Admin panel</h1>";                
            }

            else
            {
                lblTitle.Text = "Nemate ovlasti za pristup ovom dijelu aplikacije.";               
            }

        }

        else { Response.Redirect("Login.aspx"); }
    }

    protected void btnLogOut_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("Login.aspx");
    }

    protected void btnUnosDvorane_Click(object sender, EventArgs e)
    {
        lblAdminInfo.Text = "";
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("InsertNewHall", connection);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@NazivDvorane", tbxNaziv.Text);
        cmd.Parameters.AddWithValue("@Lokacija", tbxLok.Text);
        cmd.Parameters.AddWithValue("@Adresa", TbxAddr.Text);

        try
        {
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            lblAdminInfo.Text = "<h3 style=\"color:#358444\">Uspješan unos dvorane.</h3>";

            azuriranjeDvorane.Items.Clear();
            azuriranjeDvorane.Items.Add("Odaberite dvoranu...");

            ddDvorane.Items.Clear();
            ddDvorane.Items.Add("Odaberite dvoranu...");
            
            ddDvorane.DataBind();
            azuriranjeDvorane.DataBind();
            
        }

        catch (Exception ex)
        {
            lblAdminInfo.Text = "<h3>Nije moguće unijeti dvoranu (" + ex.Message + ")</h3>";
        }

        tbxNaziv.Text = "";
        tbxLok.Text = "";
        TbxAddr.Text = "";
    }

    private bool checkForDBRecords (int id)
    {
        int userCount = 0;
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        string sqlQuery = @"SELECT COUNT(1) FROM Dvorana WHERE DvoranaID = @DvoranaID
                            AND EXISTS (SELECT r.RezervacijaID FROM Rezervacija r WHERE r.IDDvorana = @DvoranaID)";
        SqlConnection connection = new SqlConnection(connectionString);

        SqlCommand cmd = new SqlCommand(sqlQuery, connection);
        cmd.Parameters.AddWithValue("@DvoranaID", id);
        try
        {
            cmd.Connection.Open();
            userCount = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception ex) { lblAdminInfo.Text = ex.Message; }
        finally { cmd.Connection.Close(); }

        if (userCount == 1) return true;
        else return false;
    }

    protected void btnObrisi_Click(object sender, EventArgs e)
    {
        lblAdminInfo.Text = "";
        
        if (ddDvorane.SelectedIndex > 0)
        {            
            int id = Convert.ToInt32(ddDvorane.SelectedItem.Value);
            bool hallHasRecords = checkForDBRecords(id);

            if (hallHasRecords)
            {
                lblAdminInfo.Text = "<h3>Dvorana se ne može obrisati jer je prethodno ili trenutno rezervirana za sastanak.</h3>";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("DeleteHall", connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DvoranaID", id);            

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                lblAdminInfo.Text = "<h3 style=\"color:#358444\">Dvorana uspješno obrisana.</h3>";

                azuriranjeDvorane.Items.Clear();
                azuriranjeDvorane.Items.Add("Odaberite dvoranu...");

                ddDvorane.Items.Clear();
                ddDvorane.Items.Add("Odaberite dvoranu...");

                ddDvorane.DataBind();
                azuriranjeDvorane.DataBind();
            }

            catch (Exception ex)
            {
                lblAdminInfo.Text = "<h3>Nije moguće obrisati dvoranu (" + ex.Message + ")</h3>";
            }
        }

        else
        {
            lblAdminInfo.Text = "<h3>Molimo odaberite dvoranu za brisanje.</h3>";
        }    

    }

    protected void azuriranjeDvorane_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (azuriranjeDvorane.SelectedIndex > 0)
        {
            int id = Convert.ToInt32(azuriranjeDvorane.SelectedItem.Value);
            string CS = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
            string sqlQuery = "SELECT NazivDvorane, Adresa, Lokacija FROM Dvorana WHERE DvoranaID = @Dvorana";
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Dvorana", id);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    dvorana.Text = rdr["NazivDvorane"].ToString();
                    lokacija.Text = rdr["Lokacija"].ToString();
                    adresa.Text = rdr["Adresa"].ToString();
                }
            }
        }
    }

    protected void btnAzuriraj_Click(object sender, EventArgs e)
    {
        lblAdminInfo.Text = "";
        if (azuriranjeDvorane.SelectedIndex > 0)
        {
            int id = Convert.ToInt32(azuriranjeDvorane.SelectedItem.Value);

            string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("UpdateHall", connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NazivDvorane", azurNaziv.Text);
            cmd.Parameters.AddWithValue("@Lokacija", azurLok.Text);
            cmd.Parameters.AddWithValue("@Adresa", azurAddr.Text);
            cmd.Parameters.AddWithValue("@DvoranaID", id);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                
                lblAdminInfo.Text = "<h3 style=\"color:#358444\">Uspješno ažuriranje dvorane.</h3>";

                azuriranjeDvorane.Items.Clear();
                azuriranjeDvorane.Items.Add("Odaberite dvoranu...");

                ddDvorane.Items.Clear();
                ddDvorane.Items.Add("Odaberite dvoranu...");

                ddDvorane.DataBind();
                azuriranjeDvorane.DataBind();
            }

            catch (Exception ex)
            {
                lblAdminInfo.Text = "<h3>Nije moguće ažurirati dvoranu (" + ex.Message + ")</h3>";
            }

            azurNaziv.Text = "";
            azurLok.Text = "";
            azurAddr.Text = "";
        }

        else
        {
            lblAdminInfo.Text = "<h3>Molimo odaberite dvoranu za ažuriranje.</h3>";
        }

    }


    protected void ddKorisnici_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddKorisnici.SelectedIndex > 0)
        {
            int id = Convert.ToInt32(ddKorisnici.SelectedItem.Value);
            string CS = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
            string sqlQuery = "SELECT Ime, Prezime, EmailAdresa, Titula, Telefon FROM Korisnik WHERE KorisnikID = @Korisnik";
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Korisnik", id);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lblI.Text = rdr["Ime"].ToString();
                    lblP.Text = rdr["Prezime"].ToString();
                    lblE.Text = rdr["EmailAdresa"].ToString();
                    lblT.Text = rdr["Telefon"].ToString();
                    lblJ.Text = rdr["Titula"].ToString();
                }
            }
        }
    }

   

    protected void btnAzurirajKorisnik_Click(object sender, EventArgs e)
    {               
        if (rAd.Checked == false && rMan.Checked == false) {
            lblAdminInfo.Text = "<h3>Molimo odaberite način ažuriranja podataka (ručno ili putem AD servera)!</h3>";
            return;
        }

        if (ddKorisnici.SelectedIndex > 0)
        {
            if (rAd.Checked)
            {                
                int id = Convert.ToInt32(ddKorisnici.SelectedItem.Value);
                string userName = ddKorisnici.SelectedItem.Text;
                unosAD(id, userName);
            }

            if (rMan.Checked)
            {             
                int id = Convert.ToInt32(ddKorisnici.SelectedItem.Value);
                string userName = ddKorisnici.SelectedItem.Text;
                unosManual(id);               
            }
        }

        else
        {
            lblAdminInfo.Text = "<h3>Molimo odaberite korisnika za ažuriranje.</h3>";
        }       
        
    }

    private void unosAD(int id, string username)
    {
        PrincipalContext ADserv = new PrincipalContext(ContextType.Domain, "192.168.252.4", "sso.apprezervacije", "Nak0nN0ciD0laziDan");
        UserPrincipal uName = UserPrincipal.FindByIdentity(ADserv, IdentityType.SamAccountName, username);

        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("UpdateUser", connection);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Ime", uName.GivenName);
        cmd.Parameters.AddWithValue("@Prezime", uName.Surname);
        cmd.Parameters.AddWithValue("@EmailAdresa", uName.EmailAddress);
        cmd.Parameters.AddWithValue("@Titula", uName.Description);
        cmd.Parameters.AddWithValue("@Telefon", uName.VoiceTelephoneNumber);
        cmd.Parameters.AddWithValue("@KorisnikID", id);

        try
        {
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            lblAdminInfo.Text = "<h3 style=\"color:#358444\">Uspješno ažuriran korisnik (podaci dohvaćeni sa servera).</h3>";

            ddKorisnici.Items.Clear();
            ddKorisnici.Items.Add("Odaberite korisnika...");
            ddKorisnici.DataBind();
        }

        catch (Exception ex)
        {
            lblAdminInfo.Text = "<h3>Podaci nisu ažurirani (" + ex.Message + ")</h3>";
        }

        finally
        {
            cmd.Connection.Close();
        }

        imeTbx.Text = "";
        prezimeTbx.Text = "";
        mailTbx.Text = "";
        telTbx.Text = "";
        ttlTbx.Text = "";

    }

    private void unosManual(int id)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Rezervacija"].ConnectionString;
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("UpdateUser", connection);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Ime", imeTbx.Text);
        cmd.Parameters.AddWithValue("@Prezime", prezimeTbx.Text);
        cmd.Parameters.AddWithValue("@EmailAdresa", mailTbx.Text);
        cmd.Parameters.AddWithValue("@Titula", ttlTbx.Text);
        cmd.Parameters.AddWithValue("@Telefon", telTbx.Text);
        cmd.Parameters.AddWithValue("@KorisnikID", id);

        try
        {
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            lblAdminInfo.Text = "<h3 style=\"color:#358444\">Uspješno ažuriran korisnik (podaci uneseni ručno).</h3>";

            ddKorisnici.Items.Clear();
            ddKorisnici.Items.Add("Odaberite korisnika...");
            ddKorisnici.DataBind();
        }

        catch (Exception ex)
        {
            lblAdminInfo.Text = "<h3>Podaci nisu ažurirani (" + ex.Message + ")</h3>";
        }

        finally
        {
            cmd.Connection.Close();           
        }

        imeTbx.Text = "";
        prezimeTbx.Text = "";
        mailTbx.Text = "";
        telTbx.Text = "";
        ttlTbx.Text = "";
    }


    protected void rMan_CheckedChanged(object sender, EventArgs e)
    {
        btnAzurirajKorisnik.CausesValidation = true;
        rMan.DataBind();
    }

    protected void rAd_CheckedChanged(object sender, EventArgs e)
    {
        btnAzurirajKorisnik.CausesValidation = false;
        rAd.DataBind();
    }
}