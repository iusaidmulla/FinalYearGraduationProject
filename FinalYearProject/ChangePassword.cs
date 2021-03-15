using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;

namespace FinalYearProject
{
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUserId_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (txtUserId.Text.Length > 9 && ch!=8 && ch!=48 && ch!=32)
            {
                MessageBox.Show("User id not more than 10 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void txtOldPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (txtOldPassword.Text.Length > 7 && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Password not more than 8 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void txtNewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (txtNewPassword.Text.Length > 7 && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Password not more than 8 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void txtRePassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (txtRePassword.Text.Length > 7 && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Password not more than 8 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (txtUserId.Text.Length == 0)
            {
                errorProvider1.SetError(txtUserId, "User Id");
                MessageBox.Show("User id is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtUserId, "");
                txtUserId.Focus();
                return;
            }
            else if (txtOldPassword.Text.Length == 0)
            {
                errorProvider1.SetError(txtOldPassword, "User Id");
                MessageBox.Show("Old password is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtOldPassword, "");
                txtOldPassword.Focus();
                return;
            }
            else if (txtNewPassword.Text.Length == 0)
            {
                errorProvider1.SetError(txtNewPassword, "User Id");
                MessageBox.Show("New password is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtNewPassword, "");
                txtNewPassword.Focus();
                return;
            }
            else if (txtRePassword.Text.Length == 0)
            {
                errorProvider1.SetError(txtRePassword, "User Id");
                MessageBox.Show("New password is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtRePassword, "");
                txtRePassword.Focus();
                return;
            }
            else
            {
                if (txtNewPassword.Text != txtRePassword.Text)
                {
                    MessageBox.Show("Password and confirm password do not matched !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    try
                    {
                        using (con = new SqlConnection(ConStr))
                        {
                            using (com = new SqlCommand("update Users set Password='" + txtNewPassword.Text.Trim() + "',RePassword='" + txtRePassword.Text.Trim() + "' where UserId='" + txtUserId.Text.Trim() + "' and Password='" + txtOldPassword.Text.Trim() + "'", con))
                            {
                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                if (com.ExecuteNonQuery() > 0)
                                {
                                    MessageBox.Show("Password changed successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ClearControls();
                                   
                                }
                                else
                                {
                                    MessageBox.Show("User id or password is incorrect !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.Message);
                        //MessageBox.Show("This user already exist !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        public void ClearControls()
        {
            txtNewPassword.Clear();
            txtOldPassword.Clear();
            txtRePassword.Clear();
            txtUserId.Clear();
            txtUserId.Focus();
        }

        private void txtUserId_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
