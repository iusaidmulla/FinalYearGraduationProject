using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace FinalYearProject
{
    public partial class UpdateFees : Form
    {
        public UpdateFees()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        DataTable dt = null;
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void UpdateFees_Load(object sender, EventArgs e)
        {
            getdata();
            cmb_installment.SelectedIndex = 0;
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

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length == 0)
            {
                errorProvider1.SetError(txtSearch, "Student ID");
                MessageBox.Show("Enter Student ID !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                errorProvider1.SetError(txtSearch, "");
            }
            else
            {
                    try
                        {
                            using (con = new SqlConnection(ConStr))
                            {
                                using (com = new SqlCommand("select StdName,CrsId,total_fees,balance_fees,photo from StudentFees where StdId='" + txtSearch.Text.Trim() + "'", con))
                                {
                                    if (con.State == ConnectionState.Closed)
                                        con.Open();
                                    using (reader = com.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            txtName.Text = reader[0].ToString();
                                            txtCourse.Text = reader[1].ToString();
                                            lbl_total.Text = reader[2].ToString();
                                            txtFees.Text = reader[3].ToString();

                                            byte[] img = (byte[])(reader["photo"]);
                                            MemoryStream mstream = new MemoryStream(img);
                                            studentpictureBox.Image = System.Drawing.Image.FromStream(mstream);
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

        private void txtCourse_TextChanged(object sender, EventArgs e)
        {
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
                lbl_remaining.Text = Convert.ToString(fees);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        public void ClearControls()
        {
            txtSearch.Clear();
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
            lbl_remaining.Text = "..."; 
            txtSearch.Focus();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
             if (txtSearch.Text.Length == 0)
            {
                errorProvider1.SetError(txtSearch, "Student ID");
                MessageBox.Show("Enter Student ID !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                errorProvider1.SetError(txtSearch, "");
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
                 try
                 {
                     string value = "0";
                     string emptydate = "-";
                     if (cmb_installment.Text == "Second Installment")
                     {
                         using (SqlConnection con = new SqlConnection(ConStr))
                         {
                             using (SqlCommand com = new SqlCommand("update StudentFees set second_inst_date='" + dop.Value.ToShortDateString() + "',second_inst_amount='" + txtAmount.Text.Trim() + "',last_paid_amount='" + lbl_paid.Text + "',balance_fees='" + lbl_balance.Text + "',type='" + cmb_installment.SelectedItem + "' where StdId='" + txtSearch.Text.Trim() + "'", con))
                             {
                                 if (con.State == ConnectionState.Closed)
                                     con.Open();
                                 if (com.ExecuteNonQuery() > 0)
                                 {
                                     MessageBox.Show("Fees Addedd successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                             using (SqlCommand com = new SqlCommand("update StudentFees set third_inst_date='" + dop.Value.ToShortDateString() + "',third_inst_amount='" + txtAmount.Text + "',last_paid_amount='" + lbl_paid.Text + "',balance_fees='" + lbl_balance.Text + "',type='" + cmb_installment.SelectedItem + "' where StdId='" + txtSearch.Text.Trim() + "'", con))
                             {
                                 if (con.State == ConnectionState.Closed)
                                     con.Open();
                                 if (com.ExecuteNonQuery() > 0)
                                 {
                                     MessageBox.Show("Fees Addedd successfully !","Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                     getdata();
                                     ClearControls();
                                 }
                             }
                         }
                     }
                 }
                 catch (Exception e1)
                 {
                     MessageBox.Show(e1.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
             }
        }

        private void cmb_installment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_installment.Text == "Second Installment")
            {
                checksecondintallment();
                return;

            }
            else
            {
                checkthirdinstallment();
                return;
            }
        }

        public void checksecondintallment()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select second_inst_amount from StudentFees where StdId='" + txtSearch.Text.Trim() + "'", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader[0].ToString() != "0" )
                            {
                                MessageBox.Show("Second Installment Already Done !" + "\n" + "Please Choose another payment type !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                                //clearcontrols();
                            }
                        }
                    }
                }
            }
        }
        public void checkthirdinstallment()
        {
           
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select balance_fees from StudentFees where StdId='" + txtSearch.Text.Trim() + "'", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader[0].ToString() == "0")
                            {
                                MessageBox.Show("Fees Fully Paid !", "Message Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // clearcontrols();
                            }
                        }
                    }

                }
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
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

    }
}
