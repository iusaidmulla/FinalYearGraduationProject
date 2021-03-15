using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;

namespace FinalYearProject
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
           
        }

        private void SignUp_Load(object sender, EventArgs e)
        {
            cmbType.SelectedIndex = 0;
            cmbQuestion.SelectedIndex = 0;
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
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void txtUserId_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (txtUserId.Text.Length > 9 && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("User id not more than 10 characters","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (txtPassword.Text.Length > 7 && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Password not more than 8 characters","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void txtRePassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (txtRePassword.Text.Length > 7 && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Re-Password not more than 8 characters","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void txtAnswer_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (char.IsLetter(e.KeyChar) == false && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Only characters is required","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
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
            else if (txtRePassword.Text.Length == 0)
            {
                errorProvider1.SetError(txtRePassword, "User Id");
                MessageBox.Show("Re-Password is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtRePassword, "");
                txtRePassword.Focus();
                return;
            }
            else if (txtEmail.Text.Length == 0)
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
                        byte[] img = null;
                        FileStream fstream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fstream);
                        img = br.ReadBytes((int)fstream.Length);

                        using (con = new SqlConnection(ConStr))
                        {
                            using (com = new SqlCommand("insert into Users values('" + txtUserId.Text.Trim() + "','" + txtPassword.Text.Trim() + "','" + txtRePassword.Text.Trim() + "','" + txtEmail.Text.Trim() + "','" + cmbType.SelectedItem + "','" + cmbQuestion.SelectedItem + "','" + txtAnswer.Text.Trim() + "',@IMG)", con))
                            {
                                com.Parameters.AddWithValue("@IMG", img);
                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                if (com.ExecuteNonQuery() > 0)
                                {
                                    MessageBox.Show("User created successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ClearControls();
                                }
                                else
                                {
                                    MessageBox.Show("User creation unsuccessfull !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
