using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace FinalYearProject
{
    public partial class DeleteCourse : Form
    {
        public DeleteCourse()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        string id;
        DataTable dt;
        DataTable dt1;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeleteCourse_Load(object sender, EventArgs e)
        {
            getdata();
            CheckData();
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dt1);
            dv.RowFilter = string.Format("CourseName LIKE '%" + txtSearch.Text + "%'");
            dataGridViewCourse.DataSource = dv;
        }

        public void CheckData()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select * from Courses", con))
                {
                    com.CommandType = CommandType.Text;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                    {
                        dt1 = new DataTable();
                        adapter.Fill(dt1);
                    }
                }
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {

            char ch = e.KeyChar;
            if (!char.IsLetter(ch) && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Only characters is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;

            }
        }

        private void dataGridViewCourse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewCourse.Rows[e.RowIndex];
                string CourseName = row.Cells[0].Value.ToString();


                DialogResult dr = MessageBox.Show("Are you sure want to delete !", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        using (con = new SqlConnection(ConStr))
                        {
                            using (com = new SqlCommand("delete from Courses where CourseName='" + CourseName + "'", con))
                            {
                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                if (com.ExecuteNonQuery() > 0)
                                {
                                    MessageBox.Show("Course deleted successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    getdata();
                                }
                                else
                                {
                                    MessageBox.Show("Deletion unsuccessfull !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }
}
