using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.IO;


namespace FinalYearProject
{
    public partial class DeleteStudent : Form
    {
        public DeleteStudent()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        DataTable dt;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUserId_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Only characters is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;

            }
            else if (txtSearch.Text.Length > 34 && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Name not more than 35 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;

            }
        }


        private void DeleteStudent_Load(object sender, EventArgs e)
        {
            getdata();
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = string.Format("First_Name LIKE '%" + txtSearch.Text + "%'");
            dataGridStudent.DataSource = dv;
        }

        private void dataGridStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridStudent.Rows[e.RowIndex];
                string id = row.Cells[0].Value.ToString();


                DialogResult dr = MessageBox.Show("Are you sure want to delete !", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        using (con = new SqlConnection(ConStr))
                        {
                            using (com = new SqlCommand("delete from Student where std_id='" + id + "'", con))
                            {
                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                if (com.ExecuteNonQuery() > 0)
                                {
                                    MessageBox.Show("Student deleted successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dataGridStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
