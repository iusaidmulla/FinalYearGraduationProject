using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

namespace FinalYearProject
{
    public partial class UpdateUser : Form
    {
        public UpdateUser()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        IDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
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
                    using (con = new SqlConnection(ConStr))
                    {
                        using (com = new SqlCommand("select Email,Type,Question,Answer,photo from Users where UserId='" + txtUserId.Text.Trim() + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            using (reader = com.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    txtEmail.Text = reader[0].ToString();
                                    cmbType.Text = reader[1].ToString();
                                    cmbQuestion.Text = reader[2].ToString();
                                    txtAnswer.Text = reader[3].ToString();

                                    byte[] img = (byte[])(reader["photo"]);
                                    MemoryStream mstream = new MemoryStream(img);
                                    UserPictureBox.Image = System.Drawing.Image.FromStream(mstream);
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

            panel3.Enabled = true;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text.Length == 0)
            {
                errorProvider1.SetError(txtEmail, "User Id");
                MessageBox.Show("Email is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtEmail, "");
                txtEmail.Focus();
                return;
            }
            else if (cmbType.SelectedIndex == 0)
            {
                errorProvider1.SetError(cmbType, "User Id");
                MessageBox.Show("Select user type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(cmbType, "");
                cmbType.Focus();
                return;
            }
            else if (cmbQuestion.SelectedIndex == 0)
            {
                errorProvider1.SetError(cmbQuestion, "User Id");
                MessageBox.Show("Select question", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(cmbQuestion, "");
                cmbQuestion.Focus();
                return;
            }
            else if (txtAnswer.Text.Length == 0)
            {
                errorProvider1.SetError(txtAnswer, "User Id");
                MessageBox.Show("Answer is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtAnswer, "");
                txtAnswer.Focus();
                return;
            }
            else if (UserPictureBox.Image == null)
            {
                errorProvider1.SetError(UserPictureBox, "User Id");
                MessageBox.Show("Choose user image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(UserPictureBox, "");
                UserPictureBox.Focus();
                return;
            }
            else
            {
                if (txtPassword.Text != txtRePassword.Text)
                {
                    MessageBox.Show("Password and confirm password do not matched !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    try
                    {
                        byte[] imgBytes = null;
                        ImageConverter imgConverter = new ImageConverter();
                        imgBytes = (System.Byte[])imgConverter.ConvertTo(UserPictureBox.Image, Type.GetType("System.Byte[]"));
                        

                        using (con = new SqlConnection(ConStr))
                        {
                            using (com = new SqlCommand("update Users set Email='" + txtEmail.Text.Trim() + "',Type='" + cmbType.SelectedItem + "',Question='" + cmbQuestion.SelectedItem + "',Answer='" + txtAnswer.Text.Trim() + "', photo=@IMG where UserId='"+txtUserId.Text.Trim()+"'", con))
                            {
                                com.Parameters.AddWithValue("@IMG", imgBytes);
                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                if (com.ExecuteNonQuery() > 0)
                                {
                                    MessageBox.Show("User updated successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ClearControls();
                                }
                                else
                                {
                                    MessageBox.Show("User updation unsuccessfull !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            ClearControls();
            panel3.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void ClearControls()
        {
            txtUserId.Clear();
            txtPassword.Clear();
            txtRePassword.Clear();
            txtEmail.Clear();
            cmbQuestion.SelectedIndex = 0;
            cmbType.SelectedIndex = 0;
            txtAnswer.Clear();
            UserPictureBox.Image = null;
            txtUserId.Focus();
            panel3.Enabled = false;
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPG |*.jpg|JPEG |*.jpeg|PNG |*.png|All Files |*.*";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                UserPictureBox.ImageLocation = openFileDialog1.FileName;

            }
            else
            {
                return;
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                             @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                             @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(txtEmail.Text.Trim()))
                return;
            else
                MessageBox.Show("Invalid email !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
