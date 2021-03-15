using System;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace FinalYearProject
{
    public partial class ForgotPasswordcs : Form
    {
        public ForgotPasswordcs()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm lf = new LoginForm();
            lf.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUserId.Text.Length == 0)
            {
                errorProvider1.SetError(txtUserId, "User Id");
                MessageBox.Show("User id is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtUserId, "");
                txtUserId.Focus();
                return;
            }
            else
            {
                try
                {
                    using(con=new SqlConnection(ConStr))
                    {
                        using (com = new SqlCommand("select UserId from Users where UserId='"+txtUserId.Text.Trim()+"'",con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            using (reader = com.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    this.Hide();
                                    AuthenticationForm af = new AuthenticationForm(txtUserId.Text.Trim());
                                    af.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Invalid User !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               
            }
           
        }

        private void ForgotPasswordcs_Load(object sender, EventArgs e)
        {

        }
    }
}
