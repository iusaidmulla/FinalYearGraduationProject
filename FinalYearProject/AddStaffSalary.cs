using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using System.Drawing;


namespace FinalYearProject
{
    public partial class AddStaffSalary : Form
    {
        public AddStaffSalary()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        DataTable dt = null;
        int tNo;
        byte[] imgbyte;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddStaffSalary_Load(object sender, EventArgs e)
        {
            getid();
            getdata();
            cmb_type.SelectedIndex = 0;
        }

        public void getdata()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select no as 'Sr.No',staff_id as 'STAFF ID',name as 'NAME',type as 'TYPE',Qualification as 'QUALIFICATION',amount as 'AMOUNT',cheque_no as 'CHEQUE NUMBER',date as 'DATE',photo as 'PHOTO' from StaffPayment", con))
                {
                    com.CommandType = CommandType.Text;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                    {
                        dt = new DataTable();
                        con.Open();
                        adapter.Fill(dt);
                        BindingSource bsource = new BindingSource();
                        bsource.DataSource = dt;
                        dataGridStaff.DataSource = bsource;
                    }
                }
            }
        }

        public void getid()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select max(no) from StaffPayment", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read() == true)
                        {
                            if (reader[0] == DBNull.Value)
                            {
                                txtNo.Text = "1";
                                tNo = 1;
                            }
                            else
                            {
                                tNo = int.Parse(reader[0].ToString());
                                tNo++;
                                txtNo.Text = tNo.ToString();
                            }
                        }
                    }

                }
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Length == 0)
            {
                errorProvider1.SetError(txtId, "Student ID");
                MessageBox.Show("Enter Student ID !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                errorProvider1.SetError(txtId, "");
            }
            else
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    using (SqlCommand com = new SqlCommand("select FirstName,LastName,Qualification,photo from Staff where StaffId='" + txtId.Text + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        using (reader = com.ExecuteReader())
                        {
                            if (reader.Read() == true)
                            {
                                txtName.Text = reader[0].ToString() + " " + reader[1].ToString();
                                txtQualification.Text = reader[2].ToString();

                                byte[] img = (byte[])(reader["photo"]);
                                MemoryStream mstream = new MemoryStream(img);
                                studentpictureBox.Image = System.Drawing.Image.FromStream(mstream);
                               
                            }
                            else
                            {
                                MessageBox.Show("Staff not found ! ", "Information Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                       
                    }
                }
            }
        }

        private void cmb_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_type.Text == "Cash")
            {
                txtAmount.Enabled = true;
            }
            else if (cmb_type.Text == "--Select--")
            {
                txtAmount.Enabled = false;
                txtChequeNo.Enabled = false;
            }
            else
            {
                txtAmount.Enabled = true;
                txtChequeNo.Enabled = true;
            }
        }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Only digit is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Only digit is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void txtChequeNo_KeyPress(object sender, KeyPressEventArgs e)
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
            clearcontrols();
        }

        public void clearcontrols()
        {

            txtId.Clear();
            txtName.Clear();
            txtAmount.Clear();
            txtChequeNo.Clear();
            cmb_type.SelectedIndex = 0;
            txtQualification.Clear();
            studentpictureBox.Image = null;
            txtId.Focus();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    imgbyte = null;
                    ImageConverter imgConverter = new ImageConverter();
                    imgbyte = (System.Byte[])imgConverter.ConvertTo(studentpictureBox.Image, Type.GetType("System.Byte[]"));

                    string cheque = "-";
                    if (cmb_type.SelectedIndex == 1)
                    {
                        using (SqlCommand com = new SqlCommand("insert into StaffPayment values('" + txtNo.Text.Trim() + "','" + txtId.Text.Trim() + "','" + txtName.Text.Trim() + "','" + cmb_type.SelectedItem + "','"+txtQualification.Text.Trim()+"','" + txtAmount.Text.Trim() + "','" + cheque + "','" + dop.Value.ToShortDateString() + "',@IMG)", con))
                        {
                            com.Parameters.Add("@IMG", imgbyte);
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Payment Addedd successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                getid();
                                getdata();
                            }
                        }
                    }
                    else
                    {
                        using (SqlCommand com = new SqlCommand("insert into StaffPayment values('" + txtNo.Text.Trim() + "','" + txtId.Text.Trim() + "','" + txtName.Text.Trim() + "','" + cmb_type.SelectedItem + "','" + txtQualification.Text.Trim() + "','" + txtAmount.Text.Trim() + "','" + txtChequeNo.Text.Trim() + "','" + dop.Value.ToShortDateString() + "',@IMG)", con))
                        {
                            com.Parameters.Add("@IMG", imgbyte);
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            if (com.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Payment Addedd successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                getid();
                                getdata();
                            }
                        }
                    }
                    clearcontrols();
                }
            
        }
    }
}
