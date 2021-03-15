using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace FinalYearProject
{
    public partial class UpdateCourse : Form
    {
        public UpdateCourse()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        string id;
        DataTable dt;

        private void UpdateCourse_Load(object sender, EventArgs e)
        {
            getdata();
        }

        public void getdata()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select CourseName as'COURSE NAME',CourseFees as'COURSE FEES' from Courses", con))
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewCourse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnUpdate.Enabled = true;
                DataGridViewRow row = dataGridViewCourse.Rows[e.RowIndex];

                txtCourse.Text = row.Cells["COURSE NAME"].Value.ToString();
                txtFees.Text = row.Cells["COURSE FEES"].Value.ToString();
            }
        }

        private void dataGridViewCourse_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void ClearControls()
        {
            txtCourse.Text = txtFees.Text = string.Empty;
            txtCourse.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            ClearControls();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
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
                        using (com = new SqlCommand("Update Courses set CourseFees='" + txtFees.Text.Trim() + "' where CourseName='" + txtCourse.Text.Trim() + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Course updated successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearControls();
                                getdata();
                                btnUpdate.Enabled = false;
                            }
                            else
                            {
                                MessageBox.Show("Updation unsuccessfull !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
