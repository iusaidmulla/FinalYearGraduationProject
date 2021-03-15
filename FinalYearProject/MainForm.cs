using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.IO;


namespace FinalYearProject
{
    public partial class MainForm : Form
    {
        public MainForm(string Type,string UserId)
        {
            InitializeComponent();
            lblType.Text = Type;
            lblUserId.Text = UserId;
            lblUser.Text = UserId;
            lblUserType.Text = Type;
            if (lblType.Text == "Admin")
            {
                //createUserToolStripMenuItem.Visible = false;
                //loginDetailsToolStripMenuItem.Visible = false;
                //updateUserToolStripMenuItem.Visible = false;
                //courcesToolStripMenuItem.Visible = false;
                //staffSalaryToolStripMenuItem.Visible = false;
            }
            else
            {
                createUserToolStripMenuItem.Enabled = false;
                updateUserToolStripMenuItem.Enabled = false;
                courcesToolStripMenuItem.Visible = false;
                staffSalaryToolStripMenuItem.Visible = false;
                courcesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
                staffSalaryToolStripMenuItem.ForeColor = System.Drawing.Color.White;
                panel5.Enabled = false;
            }
        }

        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        DataTable dt;
        DataTable dt1;

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
           DialogResult dr= MessageBox.Show("Are you sure want to exit !", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
           if (dr == DialogResult.Yes)
           {
               try
               {
                   using (con = new SqlConnection(ConStr))
                   {
                       using (com = new SqlCommand("insert into LoginDetails values('" + lblUser.Text + "','" + lblLoginTime.Text + "','" + lblCurrentTime.Text + "','" + lblUserType.Text + "','" + lblDate.Text + "')", con))
                       {
                           if (con.State == ConnectionState.Closed)
                               con.Open();
                           if (com.ExecuteNonQuery() > 0)
                           {
                               this.Hide();
                               LoginForm lf = new LoginForm();
                               lf.Show();
                               //MessageBox.Show("User created successfully !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                           }
                           else
                           {
                               //MessageBox.Show("User creation unsuccessfull !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                           }
                       }
                   }
               }
               catch (Exception e1)
               {
                   MessageBox.Show(e1.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               }

              
           }
           else
           {
               return;
           }
        }

        private void calculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }

        private void notePadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe");
        }

        private void browserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("www.google.com");
        }

        private void addStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddStudents As =new AddStudents();
            As.ShowDialog();
        }

        private void updateStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateStudent us = new UpdateStudent();
            us.ShowDialog();
        }

        private void deleteStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteStudent ds = new DeleteStudent();
            ds.ShowDialog();
        }

        private void createUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SignUp sf = new SignUp();
            sf.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword cp = new ChangePassword();
            cp.ShowDialog();
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            studentsToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void addTeacherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddStaff As = new AddStaff();
            As.ShowDialog();
        }

        private void updateTeacherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateStaff us = new UpdateStaff();
            us.ShowDialog();
        }

        private void deleteTeacherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteStaff ds = new DeleteStaff();
            ds.ShowDialog();
        }

        public void getdata()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select std_id as'ID',first_name as'First Name',last_name as 'Last Name',gender as 'Gender',email as 'Email',address as 'Address',contact as 'Contact',CourseName as 'Course',doa as 'Date of admission',photo as 'Photo' from Student", con))
                {
                    com.CommandType = CommandType.Text;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                    {
                        dt = new DataTable();
                        adapter.Fill(dt);
                        BindingSource bsource = new BindingSource();
                        bsource.DataSource = dt;
                        dataGridView1.DataSource = bsource;
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                using (con = new SqlConnection(ConStr))
                {
                    using (com = new SqlCommand("select photo from Users where UserId='" + lblUserId.Text + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        using (reader = com.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                byte[] img = (byte[])(reader["photo"]);
                                MemoryStream mstream = new MemoryStream(img);
                                UserPictureBox.Image = System.Drawing.Image.FromStream(mstream);
                                pictureBox2.Image = System.Drawing.Image.FromStream(mstream);
                             }

                        }
                    }
                }
            }
            catch (Exception e1)
            {
                // MessageBox.Show(e1.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            lblLoginTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToShortDateString();

            getdata();
            CheckData();
        }

        private void loginDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (groupBox1.Visible == false)
            {
                groupBox1.Visible = true;
                button1.Text = "Hide Login Details";

               
            }
            else
            {
                groupBox1.Visible = false;
                button1.Text = "Show Login Details";
            }
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            AddStudents st = new AddStudents();
            st.ShowDialog();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            UpdateStudent us = new UpdateStudent();
            us.ShowDialog();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            AddStaff sf = new AddStaff();
            sf.ShowDialog();
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            UpdateStaff ust = new UpdateStaff();
            ust.ShowDialog();
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            AddFees af = new AddFees();
            af.ShowDialog();
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            AddStaffSalary ass = new AddStaffSalary();
            ass.ShowDialog();
        }

        private void txtUserId_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (char.IsLetter(e.KeyChar) == false && ch != 8 && ch != 48 && ch != 32)
            {
                MessageBox.Show("Only characters is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }          
        }

        private void txtUserId_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dt1);
            dv.RowFilter = string.Format("first_name LIKE '%" + txtUserId.Text + "%'");
            dataGridView1.DataSource = dv;
        }

        public void CheckData()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select * from Student", con))
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

        private void lblType_Click(object sender, EventArgs e)
        {

        }

        private void addCourseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCourse ac = new AddCourse();
            ac.ShowDialog();
        }

        private void updateCourseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateCourse uc = new UpdateCourse();
            uc.ShowDialog();
        }

        private void deleteCourseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteCourse dc = new DeleteCourse();
            dc.ShowDialog();
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void courcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            courcesToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            adminToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void teachersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            teachersToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            searchToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void feesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            feesToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void staffSalaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            staffSalaryToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reportsToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void applicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            applicationToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutUsToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void fileToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
        }

        private void adminToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            adminToolStripMenuItem.ForeColor = System.Drawing.Color.White;
        }

        private void teachersToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            teachersToolStripMenuItem.ForeColor = System.Drawing.Color.White;
        }

        private void courcesToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            courcesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
        }

        private void searchToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            searchToolStripMenuItem.ForeColor = System.Drawing.Color.White;
        }

        private void feesToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            feesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
        }

        private void staffSalaryToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            staffSalaryToolStripMenuItem.ForeColor = System.Drawing.Color.White;
        }

        private void reportsToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            reportsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
        }

        private void applicationToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            applicationToolStripMenuItem.ForeColor = System.Drawing.Color.White;
        }

        private void aboutUsToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            aboutUsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
        }

        private void studentsToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            studentsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
        }

        private void fileToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            fileToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void adminToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            adminToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void studentsToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            studentsToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void teachersToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            teachersToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void courcesToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            courcesToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void searchToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            searchToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void feesToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            feesToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void staffSalaryToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            staffSalaryToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void reportsToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            reportsToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void applicationToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            applicationToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void aboutUsToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            aboutUsToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void studentsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SearchStudents ss = new SearchStudents();
            ss.ShowDialog();
        }

        private void staffsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchStaff sst = new SearchStaff();
            sst.ShowDialog();
        }

        private void disableApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableApp da = new DisableApp();
            da.ShowDialog();
        }

        private void updateUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateUser uu = new UpdateUser();
            uu.ShowDialog();
        }

        private void addFeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFees af = new AddFees();
            af.ShowDialog();
        }

        private void updateFeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateFees uf = new UpdateFees();
            uf.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex; 
            string id;
            if (e.RowIndex >= 0)
            {
               
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                 id= row.Cells[0].Value.ToString();
                 ViewStudentDetails vsd = new ViewStudentDetails(id);
                 vsd.ShowDialog();
            }

            
        }

        private void addSalaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddStaffSalary ass = new AddStaffSalary();
            ass.ShowDialog();
        }

        private void updateSalaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateStaffSalary uss = new UpdateStaffSalary();
            uss.ShowDialog();
        }

        private void allStudentReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllStudentReports asr = new AllStudentReports();
            asr.Show();
        }

        private void courseWiseStudentReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CourseWiseReports cwr = new CourseWiseReports();
            cwr.Show();
        }

        private void dateWiseStudentReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateWiseStudentReport dsr = new DateWiseStudentReport();
            dsr.Show();
        }

        private void staffReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllStaffReport asr = new AllStaffReport();
            asr.Show();
        }

        private void paidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void feesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AllFeesReports afr = new AllFeesReports();
            afr.Show();
        }

        private void paymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllStaffPaymentReports aspr = new AllStaffPaymentReports();
            aspr.Show();
        }

        private void developerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDevelopers ad = new AboutDevelopers();
            ad.Show();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblUser_Click(object sender, EventArgs e)
        {

        }
    }
}
