using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace FinalYearProject
{
    public partial class CourseWiseReports : Form
    {
        public CourseWiseReports()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        DataTable dt = null;
        SqlDataReader reader = null;
        SqlDataAdapter adapter = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        private void CourseWiseReports_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            getCourses();
            if (comboBox1.SelectedIndex == 0)
            {
                crystalReportViewer1.ReportSource = null;
            }
            
        }

        public void getCourses()
        {

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand com = new SqlCommand("select * from Courses", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    using (reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader[1].ToString());
                        }
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
             if (comboBox1.SelectedIndex == 0)
             {
                 crystalReportViewer1.ReportSource = null;
             }
            else
             {
                using (con = new SqlConnection(ConStr))
                {
                    using (com = new SqlCommand("select * from Student where CourseName='" + comboBox1.Text + "'", con))
                    {
                        using (adapter = new SqlDataAdapter(com))
                        {
                            dt = new DataTable();
                            adapter.Fill(dt);
                            CourseWiseCrystalReport cwr = new CourseWiseCrystalReport();
                            cwr.SetDataSource(dt);
                            crystalReportViewer1.ReportSource = cwr;
                        }
                    }

                }
             }
               
            
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        
    }
}
