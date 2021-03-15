using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.IO;

namespace FinalYearProject
{
    public partial class AuthenticationForm : Form
    {
        public AuthenticationForm(string Name)
        {
            InitializeComponent();
            lblUserName.Text = Name;
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

        private void AuthenticationForm_Load(object sender, EventArgs e)
        {
            try
            {
                using (con = new SqlConnection(ConStr))
                {
                    using (com = new SqlCommand("select Question,photo from Users where UserId='" + lblUserName.Text + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        using (reader = com.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                lblSecurityQuestion.Text = reader[0].ToString();
                                byte[] img = (byte[])(reader["photo"]);
                                MemoryStream mstream = new MemoryStream(img);
                                userPictureBox.Image = System.Drawing.Image.FromStream(mstream);
                               
                            }
                           
                        }
                    }
                }
            }
            catch (Exception e1)
            {
               // MessageBox.Show(e1.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtAnswer.Text.Length == 0)
            {
                errorProvider1.SetError(txtAnswer, "User Id");
                MessageBox.Show("Answer is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtAnswer, "");
                txtAnswer.Focus();
                return;
            }
            else
            {
                try
                {
                    using (con = new SqlConnection(ConStr))
                    {
                        using (com = new SqlCommand("select Password from Users where Question='" + lblSecurityQuestion.Text + "' and Answer='"+txtAnswer.Text.Trim()+"'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            using (reader = com.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    MessageBox.Show("Your Password : "+reader[0], "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Authentication failed !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
