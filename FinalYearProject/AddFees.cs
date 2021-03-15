using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace FinalYearProject
{
    public partial class AddFees : Form
    {
        public AddFees()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        int rNo;
        DataTable dt;
        int i;

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddFees_Load(object sender, EventArgs e)
        {
            getid();
            cmb_installment.SelectedIndex = 0;
            getdata();
        }

        public void getdata()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select no as'Rec.No',StdId as'Student ID',CrsId as 'Course Name',full_pay_date as 'FULL PAY DATE',full_amount as 'Amount',first_inst_date as 'FIRST INSTALLMENT DATE',first_inst_amount as 'Amount',second_inst_date as 'SECOND INSTALLMENT DATE',second_inst_amount as 'Amount',third_inst_date as 'THIRD INSTALLMENT DATE',third_inst_amount as 'Amount',total_fees as 'Total Fees',last_paid_amount as 'Last paid Amount',balance_fees as 'Balance Fees' from StudentFees", con))
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

        public void getid()
        {

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select max(no) from StudentFees", con))
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
                                txtNo.Text = "1";
                                rNo = 1;
                            }
                            else
                            {
                                rNo = int.Parse(reader[0].ToString());
                                rNo++;
                                txtNo.Text =rNo.ToString();
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
                        try
                        {
                            using (con = new SqlConnection(ConStr))
                            {
                                using (com = new SqlCommand("select first_name,last_name,CourseName,photo from Student where std_id='" + txtId.Text.Trim() + "'", con))
                                {
                                    if (con.State == ConnectionState.Closed)
                                        con.Open();
                                    using (reader = com.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            txtName.Text = reader[0].ToString() + " " + reader[1].ToString();
                                            byte[] img = (byte[])(reader["photo"]);
                                            MemoryStream mstream = new MemoryStream(img);
                                            studentpictureBox.Image = System.Drawing.Image.FromStream(mstream);
                                            txtCourse.Text = reader[2].ToString();
                                          

                                        }
                                        else
                                        {
                                            MessageBox.Show("Student not found ! ", "Information Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }

                                    }
                                }
                            }
                        }
                        catch (Exception e1)
                        {
                            // MessageBox.Show(e1.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

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

        private void txtCourse_TextChanged(object sender, EventArgs e)
        {
            if (txtCourse.Text.Length != 0)
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    using (SqlCommand com = new SqlCommand("select CourseFees from Courses where CourseName='" + txtCourse.Text.Trim() + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        using (reader = com.ExecuteReader())
                        {
                            if (reader.Read() == true)
                            {

                                txtFees.Text = reader[0].ToString();

                            }
                            else
                            {
                                MessageBox.Show("Student not found ! ", "Information Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                        }


                    }
                }
            }
            else
            {
                txtCourse.Clear();
            }
           
        }

        private void cmb_installment_SelectedIndexChanged(object sender, EventArgs e)
        {
                if (cmb_installment.Text == "Full Payment")
                {

                    txtAmount.Text = txtFees.Text;
                    txtAmount.Enabled = false;
                    checkfullpayment();
                }
                else
                {
                    txtAmount.Enabled = true;
                    checkfirstintallment();
                }
            
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtAmount.Text.Length == 0)
            {
                lbl_total.Text = "...";
                lbl_paid.Text = "...";
                lbl_balance.Text = "...";
            }
            else
            {
                int fees = int.Parse(txtFees.Text);
                int amount = int.Parse(txtAmount.Text);
                lbl_total.Text = Convert.ToString(fees);
                lbl_paid.Text = Convert.ToString(amount);
                lbl_balance.Text = Convert.ToString(fees - amount);
                if (amount > fees)
                {
                    MessageBox.Show("Amount not more than fees amount !", "MInformation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAmount.Clear();
                    lbl_total.Text = "...";
                    lbl_paid.Text = "...";
                    lbl_balance.Text = "...";
                }
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

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Length == 0)
            {
                errorProvider1.SetError(txtId, "Student ID");
                MessageBox.Show("Enter Student ID !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                errorProvider1.SetError(txtId, "");
            }
            else if (dop.Value.Date > DateTime.Today)
            {
                errorProvider1.SetError(dop, "Date ");
                MessageBox.Show("Date should be less than current date !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                errorProvider1.SetError(dop, "");
            }
            else if (cmb_installment.SelectedIndex == 0)
            {
                errorProvider1.SetError(cmb_installment, "Date ");
                MessageBox.Show("Choose Fees Type !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                errorProvider1.SetError(cmb_installment, "");
            }
            else
            {
                byte[] imgBytes = null;
                ImageConverter imgConverter = new ImageConverter();
                imgBytes = (System.Byte[])imgConverter.ConvertTo(studentpictureBox.Image, Type.GetType("System.Byte[]"));
                


                string value = "0";
                string emptydate = "-";
                if (cmb_installment.Text == "Full Payment")
                {
                    using (SqlConnection con = new SqlConnection(ConStr))
                    {
                        using (SqlCommand com = new SqlCommand("insert into StudentFees values('" + txtNo.Text + "','" + txtId.Text + "','" + txtName.Text.Trim() + "','" + txtCourse.Text + "','" + dop.Value.ToShortDateString() + "','" + txtAmount.Text + "','" + emptydate + "','" + value + "','" + emptydate + "','" + value + "','" + emptydate + "','" + value + "','" + lbl_total.Text + "','" + lbl_paid.Text + "','" + lbl_balance.Text + "',@IMG,'" + cmb_installment.SelectedItem + "')", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            com.Parameters.AddWithValue("@IMG", imgBytes);
                            if (com.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Fees Addedd successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                getid();
                                getdata();
                                ClearControls();
                            }
                        }
                    }
                }
                else
                {
                   
                    using (SqlConnection con = new SqlConnection(ConStr))
                    {
                        using (SqlCommand com = new SqlCommand("insert into StudentFees values('" + txtNo.Text + "','" + txtId.Text + "','" + txtName.Text.Trim() + "','" + txtCourse.Text + "','" + emptydate + "','" + value + "','" + dop.Value.ToShortDateString() + "','" + txtAmount.Text + "','" + emptydate + "','" + value + "','" + emptydate + "','" + value + "','" + lbl_total.Text + "','" + lbl_paid.Text + "','" + lbl_balance.Text + "',@IMG,'" + cmb_installment.SelectedItem + "')", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            com.Parameters.AddWithValue("@IMG", imgBytes);
                            if (com.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Fees Addedd successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                getid();
                                getdata();
                                ClearControls();
                            }
                            
                        }
                    }
                }
                
            }
        }

        public void checkfirstintallment()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select first_inst_amount from StudentFees where StdId='" + txtId.Text + "'", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    using (reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader[0].ToString() != "0")
                            {
                                MessageBox.Show("First Installment Already Done !" + "\n" + "Please Choose another payment type !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //ClearControls();
                            }
                        }
                    }
                }
            }
        }

        public void checkfullpayment()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select full_amount from StudentFees where StdId='" + txtId.Text + "'", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    using (reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader[0].ToString() != "0")
                            {
                                MessageBox.Show("Full Payment Already Done !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //ClearControls();
                            }
                        }
                    }

                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            getid();
        }

        public void ClearControls()
        {
            txtId.Clear();
            txtName.Clear();
            txtCourse.Clear();
            txtFees.Clear();
            txtAmount.Clear();
            cmb_installment.SelectedIndex = 0;
            dop.Value = DateTime.Today;
            studentpictureBox.Image = null;
            lbl_total.Text = "...";
            lbl_paid.Text = "...";
            lbl_balance.Text = "...";
            txtId.Focus();
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAmount_TextChanged_1(object sender, EventArgs e)
        {
            if (txtAmount.Text.Length == 0)
            {
                lbl_total.Text = "...";
                lbl_paid.Text = "...";
                lbl_balance.Text = "...";
            }
            else
            {
                int fees = int.Parse(txtFees.Text);
                int amount = int.Parse(txtAmount.Text);
                lbl_total.Text = Convert.ToString(fees);
                lbl_paid.Text = Convert.ToString(amount);
                lbl_balance.Text = Convert.ToString(fees - amount);
                if (amount > fees)
                {
                    MessageBox.Show("Amount not more than fees amount !", "MInformation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAmount.Clear();
                    lbl_total.Text = "...";
                    lbl_paid.Text = "...";
                    lbl_balance.Text = "...";
                }
            }
        }

        private void txtAmount_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Only digit is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }
    }
}
