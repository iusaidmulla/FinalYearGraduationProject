using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace FinalYearProject
{
    public partial class UpdateStudent : Form
    {
        public UpdateStudent()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        string id;
        DataTable dt;
        int i;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPG |*.jpg|JPEG |*.jpeg|PNG |*.png|All Files |*.*";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                studentPictureBox.ImageLocation = openFileDialog1.FileName;

            }
            else
            {
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        public void ClearControls()
        {
            txtFirstName.Text = txtLastName.Text = txtEmail.Text = txtAddress.Text = txtContact.Text = string.Empty;
            cmbGender.SelectedIndex = cmbCourse.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Today;
            txtFirstName.Focus();
            studentPictureBox.Image = null;
            lblId.Text = "ID";

            groupBox1.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void txtFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {

            char ch = e.KeyChar;
            if (!char.IsLetter(ch) && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Only characters is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;

            }
            else if (txtFirstName.Text.Length > 14 && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("First name not more than 15 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;

            }
        }

        private void txtLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (txtLastName.Text.Length > 14 && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Last name not more than 15 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
            else if (!char.IsLetter(ch) && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Only characters is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (txtContact.Text.Length > 9 && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Contact not more than 10 digites", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
            else if (!char.IsDigit(ch) && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Only digit is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtFirstName.Text.Length == 0)
            {
                errorProvider1.SetError(txtFirstName, "First Name");
                MessageBox.Show("Enter First Name !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtFirstName, "");
                return;
            }
            else if (txtLastName.Text.Length == 0)
            {
                errorProvider1.SetError(txtLastName, "Last Name");
                MessageBox.Show("Enter Last Name !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtLastName, "");
                return;
            }
            else if (cmbGender.SelectedIndex == 0)
            {
                errorProvider1.SetError(cmbGender, "Gender");
                MessageBox.Show("Select Student Gender !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(cmbGender, "");
                return;
            }
            else if (txtEmail.Text.Length == 0)
            {
                errorProvider1.SetError(txtEmail, "Email");
                MessageBox.Show("Enter Student Email !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtEmail, "");
                return;
            }
            else if (txtAddress.Text.Length == 0)
            {
                errorProvider1.SetError(txtAddress, "Address");
                MessageBox.Show("Enter Student Address !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtAddress, "");
                return;
            }
            else if (txtContact.Text.Length == 0)
            {
                errorProvider1.SetError(txtContact, "Contact");
                MessageBox.Show("Enter Student Contact !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtContact, "");
                return;
            }
            else if (cmbCourse.SelectedIndex == 0)
            {
                errorProvider1.SetError(cmbCourse, "Course");
                MessageBox.Show("Choose Student Course !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(cmbCourse, "");
                return;
            }
            else if (dateTimePicker1.Value.Date > DateTime.Today)
            {
                errorProvider1.SetError(dateTimePicker1, "Date");
                MessageBox.Show("Date should be less than current date !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(dateTimePicker1, "");
                return;
            }
            else if (studentPictureBox.Image == null)
            {
                errorProvider1.SetError(studentPictureBox, "Photo");
                MessageBox.Show("Choose Student Photo !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(studentPictureBox, "");
                return;
            }
            else
            {
                byte[] imgBytes = null;
                ImageConverter imgConverter = new ImageConverter();
                imgBytes = (System.Byte[])imgConverter.ConvertTo(studentPictureBox.Image, Type.GetType("System.Byte[]"));

                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    using (SqlCommand com = new SqlCommand("update Student set first_name='" + txtFirstName.Text + "',last_name='" + txtLastName.Text + "',gender='" + cmbGender.SelectedItem + "',email='" + txtEmail.Text + "',address='" + txtAddress.Text + "',contact='" + txtContact.Text + "',CourseName='" + cmbCourse.SelectedItem + "',doa='" + dateTimePicker1.Value.ToShortDateString() + "',photo=@IMG where std_id='" + id + "'", con))
                    {
                        com.CommandType = CommandType.Text;
                        com.Parameters.AddWithValue("@IMG", imgBytes);
                       
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Student Updated successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearControls();
                            getdata();
                        }
                    }
                }
            }
        }

        public void getdata()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select std_id as'ID',first_name as'First_Name',last_name as 'Last Name',gender as 'Gender',email as 'Email',address as 'Address',contact as 'Contact',CourseName as 'Course',doa as 'Date of admission',photo as 'Photo' from Student", con))
                {
                    com.CommandType = CommandType.Text;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                    {
                        dt = new DataTable();
                        adapter.Fill(dt);
                        BindingSource bsource = new BindingSource();
                        bsource.DataSource = dt;
                        dataGridStudent.DataSource = bsource;
                    }
                }
            }
        }

        private void UpdateStudent_Load(object sender, EventArgs e)
        {
            cmbGender.SelectedIndex = 0;
            cmbCourse.SelectedIndex = 0;
            getdata();
            getCourses();
        }

        private void dataGridStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                groupBox1.Enabled = true;
                btnUpdate.Enabled = true;
                DataGridViewRow row = dataGridStudent.Rows[e.RowIndex];
                lblId.Text = "S" + row.Cells["ID"].Value.ToString();
                id = row.Cells["ID"].Value.ToString();
                txtFirstName.Text = row.Cells["First_Name"].Value.ToString();
                txtLastName.Text = row.Cells["Last Name"].Value.ToString();
                cmbGender.Text = row.Cells["Gender"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                txtContact.Text = row.Cells["Contact"].Value.ToString();
                cmbCourse.Text = row.Cells["Course"].Value.ToString();
                dateTimePicker1.Text = row.Cells["Date of admission"].Value.ToString();
                byte[] img = (byte[])(dt.Rows[i]["Photo"]);
                MemoryStream mstream = new MemoryStream(img);
                studentPictureBox.Image = System.Drawing.Image.FromStream(mstream);
            }

        }

        public void getCourses()
        {

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select * from Courses", con))
                {
                    com.CommandType = CommandType.Text;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbCourse.Items.Add(reader[1].ToString());
                        }
                    }
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = string.Format("First_Name LIKE '%" + txtSearch.Text + "%'");
            dataGridStudent.DataSource = dv;
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

