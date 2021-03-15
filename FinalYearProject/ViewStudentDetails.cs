using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.IO;

namespace FinalYearProject
{
    public partial class ViewStudentDetails : Form
    {
        string StudentId;
        public ViewStudentDetails(string id)
        {
            InitializeComponent();
            StudentId = id;
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        DataTable dt;

        private void ViewStudentDetails_Load(object sender, EventArgs e)
        {
            getdata();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void getdata()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select s.std_id,s.first_name,s.last_name,s.gender,s.email,s.contact,s.CourseName,s.address,s.doa,f.total_fees,f.balance_fees,f.last_paid_amount,f.full_pay_date,f.first_inst_date,f.second_inst_date,f.third_inst_date,f.full_amount,f.first_inst_amount,f.second_inst_amount,f.third_inst_amount,s.photo,f.type from Student s,StudentFees f where f.StdId='" + StudentId + "' and s.std_id='" + StudentId + "'", con))
                {
                    com.CommandType = CommandType.Text;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    using (reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblId.Text = StudentId;
                            lblName.Text = reader[1].ToString() +" "+ reader[2].ToString();
                            lblGender.Text = reader[3].ToString();
                            lblEmail.Text = reader[4].ToString();
                            lblContact.Text = reader[5].ToString();
                            lblCourse.Text = reader[6].ToString();
                            lblAddress.Text = reader[7].ToString();
                            lblDOA.Text = reader[8].ToString();
                            lblTotalFees.Text = reader[9].ToString();
                            lblBalanceFees.Text = reader[10].ToString();
                            lblLastPaid.Text = reader[11].ToString();

                            int totalPaidamount = Convert.ToInt32(reader[16]) + Convert.ToInt32(reader[17]) + Convert.ToInt32(reader[18]) + Convert.ToInt32(reader[19]);
                            lblPaidFees.Text = totalPaidamount.ToString();



                            byte[] img = (byte[])(reader[20]);
                            MemoryStream mstream = new MemoryStream(img);
                            StudentPictureBox.Image = System.Drawing.Image.FromStream(mstream);

                            if (reader[21].ToString() == "Full Payment")
                            {
                                lblDate.Text = reader[12].ToString();
                                lbltype.Text = reader[21].ToString();
                                return;
                            }
                            else if (reader[21].ToString() == "First Installment")
                            {
                                lblDate.Text = reader[13].ToString();
                                lbltype.Text = reader[21].ToString();
                                return;
                            }
                            else if (reader[21].ToString() == "Second Installment")
                            {
                                lblDate.Text = reader[14].ToString();
                                lbltype.Text = reader[21].ToString();
                                return;
                            }
                            else if (reader[21].ToString() == "Third Installment")
                            {
                                lblDate.Text = reader[15].ToString();
                                lbltype.Text = reader[21].ToString();
                                return;
                            }
                           

                            
                           
                        }
                    }
                }
            }
        }
    }
}
