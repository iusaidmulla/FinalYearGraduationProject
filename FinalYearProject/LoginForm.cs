using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;

namespace FinalYearProject
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            ForgotPasswordcs fp = new ForgotPasswordcs();
            fp.Show();
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
            else if (txtPassword.Text.Length == 0)
            {
                errorProvider1.SetError(txtPassword, "User Id");
                MessageBox.Show("Password is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtPassword, "");
                txtPassword.Focus();
                return;
            }
            else
            {
                try
                {
                    using (con = new SqlConnection(ConStr))
                    {
                        using (com = new SqlCommand("select  UserId,Password,Type from Users where UserId='" + txtUserId.Text.Trim() + "' and Password='" + txtPassword.Text.Trim() + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            using (reader = com.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (rememebermecheckBox.Checked == false)
                                    {
                                        Properties.Settings.Default.Username = null;
                                        Properties.Settings.Default.Password = null;
                                    }
                                        this.Hide();
                                        MainForm mf = new MainForm(reader[2].ToString(),txtUserId.Text.Trim());
                                        mf.Show();
                               }
                                else
                                {
                                    MessageBox.Show("Invalid user id or password !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtUserId.Clear();
            txtPassword.Clear();
            txtUserId.Focus();
        }

        private void chkRemember_CheckedChanged(object sender, EventArgs e)
        {
            if (rememebermecheckBox.Checked)
            {
                Properties.Settings.Default.Username = txtUserId.Text;
                Properties.Settings.Default.Password = txtPassword.Text;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Username != string.Empty)
            {
                txtUserId.Text = Properties.Settings.Default.Username;
                txtPassword.Text = Properties.Settings.Default.Password;
            }
            else
            {
                if (rememebermecheckBox.Checked==false)
                {
                    txtUserId.Clear();
                    txtPassword.Clear(); txtUserId.Focus();
                }
            }
        }
    }
}
