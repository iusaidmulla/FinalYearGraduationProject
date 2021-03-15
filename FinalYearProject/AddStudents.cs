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
    public partial class AddStudents : Form
    {
        public AddStudents()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        SqlDataAdapter adapter = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        int id;
        DataTable dt;
        int i;

        private void AddStudents_Load(object sender, EventArgs e)
        {
            getid();
            getdata();
            getCourses();
            cmbGender.SelectedIndex = 0;
           cmbCourse.SelectedIndex = 0;
        }

        public void getid()
        {
            
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select max(std_id) from Student", con))
                {
                    com.CommandType = CommandType.Text;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read() == true)
                        {
                            if (reader[0] == DBNull.Value)
                            {
                                lblId.Text = 'S' + "1";
                                id = 1;
                            }
                            else
                            {
                                id = int.Parse(reader[0].ToString());
                                id++;
                                lblId.Text = 'S' + id.ToString();
                            }
                         }
                    }
                }
            }
        }

        public void getCourses()
        {

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select * from Courses", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    using (reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbCourse.Items.Add(reader[1].ToString());
                        }
                    }
                }
            }
        }

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

        public void getstudentdata()
        {
            lblId.Text = "S"+dt.Rows[i][0].ToString();
            txtFirstName.Text = dt.Rows[i][1].ToString();
            txtLastName.Text = dt.Rows[i][2].ToString();
            cmbGender.Text = dt.Rows[i][3].ToString();
            txtEmail.Text = dt.Rows[i][4].ToString();
            txtAddress.Text = dt.Rows[i][5].ToString();
            txtContact.Text = dt.Rows[i][6].ToString();
            dateTimePicker1.Value = Convert.ToDateTime(dt.Rows[i][7]);
            cmbCourse.Text = dt.Rows[i][8].ToString();
            byte[] img = (byte[])(dt.Rows[i]["photo"]);
            MemoryStream mstream = new MemoryStream(img);
            studentPictureBox.Image = System.Drawing.Image.FromStream(mstream);

        }

        private void txtFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) && ch != 8 && ch != 48 && ch != 8)
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

        private void btnCreate_Click(object sender, EventArgs e)
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

                try
                {
                    using (SqlConnection con = new SqlConnection(ConStr))
                    {
                        using (SqlCommand com = new SqlCommand("insert into Student values('" + id + "','" + txtFirstName.Text.Trim() + "','" + txtLastName.Text.Trim() + "','" + cmbGender.SelectedItem + "','" + txtEmail.Text + "','" + txtAddress.Text.Trim() + "','" + txtContact.Text.Trim() + "','" + cmbCourse.SelectedItem + "','" + dateTimePicker1.Value.ToShortDateString()+ "',@IMG)", con))
                        {
                            com.CommandType = CommandType.Text;
                            com.Parameters.AddWithValue("@IMG", imgBytes);
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Student Addedd successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearControls();
                                getid();
                                getdata();
                            }
                            else
                            {
                                MessageBox.Show("Student Addedd unsuccessfull !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public void getdata()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select std_id as'ID',first_name as'First Name',last_name as 'Last Name',gender as 'Gender',email as 'Email',address as 'Address',contact as 'Contact',doa as 'Date of Addmission',CourseName as 'Course',photo as 'Photo' from Student", con))
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

        private void ClearControls()
        {
            txtFirstName.Text = txtLastName.Text = txtEmail.Text = txtAddress.Text = txtContact.Text = string.Empty;
            cmbGender.SelectedIndex = cmbCourse.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Today;
            studentPictureBox.Image = null;
            cmbCourse.SelectedIndex = 0;
            cmbGender.SelectedIndex = 0;
            txtFirstName.Focus();
        }



        private void txtAddress_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            getid();
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void studentPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            i = 0;
            getstudentdata();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                i = i + 1;
                getstudentdata();
            }
            catch (Exception e1)
            {
                MessageBox.Show("This is last record of the table !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                i = i - 1;
                getstudentdata();
            }
            catch (Exception e1)
            {
                MessageBox.Show("This is first record of the table !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            i = dt.Rows.Count-1;
            getstudentdata();
        }

        private void dataGridStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblId_Click(object sender, EventArgs e)
        {
             
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

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

        private void cmbCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
