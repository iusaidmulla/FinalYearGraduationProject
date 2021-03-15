using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace FinalYearProject
{
    public partial class SearchStaff : Form
    {
        public SearchStaff()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
       
        DataTable dt;

        private void SearchStaff_Load(object sender, EventArgs e)
        {
            getdata();
        }

        public void getdata()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select StaffId as'ID',FirstName as'First_Name',LastName as 'Last Name',gender as 'Gender',email as 'Email',address as 'Address',contact as 'Contact',Qualification as 'Qualification',doa as 'Date of admission',photo as 'Photo' from Staff", con))
                {
                    com.CommandType = CommandType.Text;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                    {
                        dt = new DataTable();
                        adapter.Fill(dt);
                        BindingSource bsource = new BindingSource();
                        bsource.DataSource = dt;
                        dataGridStaff.DataSource = bsource;
                    }
                }
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false && radioButton4.Checked == false)
            {
                MessageBox.Show("Choose search option", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Handled = true;
                return;
            }
            else
            {
                if (radioButton1.Checked == true)
                {
                    if (!char.IsDigit(ch) && ch != 8 && ch != 48 && ch != 32)
                    {
                        MessageBox.Show("Only digit is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Handled = true;
                        return;
                    }
                }
                if (radioButton2.Checked == true)
                {
                    if (!char.IsLetter(ch) && ch != 8 && ch != 48 && ch != 32)
                    {
                        MessageBox.Show("Only characters is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Handled = true;
                        return;
                    }
                    else if (txtSearch.Text.Length > 14 && ch != 8 && ch != 48 && ch != 32)
                    {
                        MessageBox.Show("Name not more than 15 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Handled = true;
                        return;
                    }
                }
                if (radioButton3.Checked == true)
                {
                    if (!char.IsDigit(ch) && ch != 8 && ch != 48 && ch != 32)
                    {
                        MessageBox.Show("Only digit is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Handled = true;
                        return;
                    }
                    else if (txtSearch.Text.Length > 9 && ch != 8 && ch != 48 && ch != 32)
                    {
                        MessageBox.Show("Contact not more than 10 digites", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Handled = true;
                        return;
                    }
                }

            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                DataView dv = new DataView(dt);
                dv.RowFilter = string.Format("ID LIKE '%" + txtSearch.Text + "%'");
                dataGridStaff.DataSource = dv;
            }
            else if (radioButton2.Checked == true)
            {
                DataView dv = new DataView(dt);
                dv.RowFilter = string.Format("First_Name LIKE '%" + txtSearch.Text + "%'");
                dataGridStaff.DataSource = dv;
            }
            else if (radioButton3.Checked == true)
            {
                DataView dv = new DataView(dt);
                dv.RowFilter = string.Format("Contact LIKE '%" + txtSearch.Text + "%'");
                dataGridStaff.DataSource = dv;
            }
            else if (radioButton4.Checked == true)
            {
                DataView dv = new DataView(dt);
                dv.RowFilter = string.Format("Email LIKE'%" + txtSearch.Text + "%'");
                dataGridStaff.DataSource = dv;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
