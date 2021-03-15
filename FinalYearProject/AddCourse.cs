using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace FinalYearProject
{
    public partial class AddCourse : Form
    {
        public AddCourse()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        string id;
        DataTable dt;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtCourse.Text.Length == 0)
            {
                errorProvider1.SetError(txtCourse, "Course");
                MessageBox.Show("Course is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtCourse, "");
                txtCourse.Focus();
                return;
            }
            else if (txtFees.Text.Length == 0)
            {
                errorProvider1.SetError(txtFees, "Course");
                MessageBox.Show("Password is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorProvider1.SetError(txtFees, "");
                txtFees.Focus();
                return;
            }
            else
            {
                try
                {
                    using (con = new SqlConnection(ConStr))
                    {
                        using (com = new SqlCommand("insert into Courses values('" + txtCourse.Text.Trim() + "','" + txtFees.Text.Trim() + "')", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Course Added successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearControls();
                                getdata();
                            }
                            else
                            {
                                MessageBox.Show("Course added unsuccessfull !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show("this course already addedd !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void txtCourse_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (char.IsLetter(e.KeyChar) == false && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Only characters is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Only digit is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        public void ClearControls()
        {
            txtCourse.Text = txtFees.Text = string.Empty;
            txtCourse.Focus();
        }

        private void AddCourse_Load(object sender, EventArgs e)
        {
            getdata();
        }

        public void getdata()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select CourseId as 'COURSE ID',CourseName as'COURSE NAME',CourseFees as'COURSE FEES' from Courses", con))
                {
                    com.CommandType = CommandType.Text;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                    {
                        dt = new DataTable();
                        adapter.Fill(dt);
                        BindingSource bsource = new BindingSource();
                        bsource.DataSource = dt;
                        dataGridViewCourse.DataSource = bsource;
                    }
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
