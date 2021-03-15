using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace FinalYearProject
{
    public partial class UpdateStaffSalary : Form
    {
        public UpdateStaffSalary()
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

        private void UpdateStaffSalary_Load(object sender, EventArgs e)
        {
            cmb_type.SelectedIndex = 0;
            getdata();
            getid();
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
                    using (SqlCommand com = new SqlCommand("select name,Qualification,photo,type,amount,cheque_no from StaffPayment where staff_id='" + txtId.Text + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        using (reader = com.ExecuteReader())
                        {
                            if (reader.Read() == true)
                            {
                                txtName.Text = reader[0].ToString();
                                txtQualification.Text = reader[1].ToString();
                                cmb_type.Text = reader[3].ToString();
                                if (cmb_type.Text == "Cash")
                                {
                                    txtAmount.Enabled = true;
                                    txtAmount.Text = reader[4].ToString();
                                }
                                else
                                {
                                    txtChequeNo.Enabled = true;
                                    txtChequeNo.Text = reader[5].ToString();
                                    txtAmount.Enabled = true;
                                    txtAmount.Text = reader[4].ToString();
                                }
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

        private void cmb_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_type.Text == "Cash")
            {
                txtAmount.Enabled = true;
                txtChequeNo.Enabled = false;
                txtChequeNo.Clear();
            }
            else if (cmb_type.Text == "--Select--")
            {
                txtAmount.Enabled = false;
                txtChequeNo.Enabled = false;
                txtAmount.Clear();
                txtChequeNo.Clear();
            }
            else
            {
                txtAmount.Enabled = true;
                txtChequeNo.Enabled = true;
            }
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
                    using (SqlCommand com = new SqlCommand("update StaffPayment set type='" + cmb_type.Text + "',amount='" + txtAmount.Text.Trim() + "',cheque_no='"+cheque+"',date='" + dop.Value.ToShortDateString() + "' where staff_id='" + txtId.Text.Trim() + "'", con))
                    {
                        com.Parameters.Add("@IMG", imgbyte);
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Payment Updated successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            getid();
                            getdata();
                        }
                    }
                }
                else
                {
                    using (SqlCommand com = new SqlCommand("update StaffPayment set type='" + cmb_type.Text + "',cheque_no='" + txtChequeNo.Text.Trim() + "',amount='" + txtAmount.Text.Trim() + "',date='" + dop.Value.ToShortDateString() + "' where staff_id='" + txtId.Text.Trim() + "'", con))
                    {
                        com.Parameters.Add("@IMG", imgbyte);
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        if (com.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Payment Updated successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            getid();
                            getdata();
                        }
                    }
                }
                clearcontrols();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
