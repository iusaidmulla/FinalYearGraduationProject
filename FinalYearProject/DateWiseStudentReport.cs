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
    public partial class DateWiseStudentReport : Form
    {
        public DateWiseStudentReport()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        DataTable dt = null;
        SqlDataAdapter adapter = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        private void DateWiseStudentReport_Load(object sender, EventArgs e)
        {
            using (con = new SqlConnection(ConStr))
            {
                using (adapter = new SqlDataAdapter("select * from Student", con))
                {
                    dt = new DataTable();
                    adapter.Fill(dt);
                    DateWiseStudentCrystalReport dwr = new DateWiseStudentCrystalReport();
                    dwr.SetDataSource(dt);
                    crystalReportViewer1.ReportSource = dwr;
                }
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            using (con = new SqlConnection(ConStr))
            {
                using (com = new SqlCommand("select * from Student where doa between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "'", con))
                {
                    using (adapter = new SqlDataAdapter(com))
                    {
                        dt = new DataTable();
                        adapter.Fill(dt);
                        DateWiseStudentCrystalReport dwr = new DateWiseStudentCrystalReport();
                        dwr.SetDataSource(dt);
                        crystalReportViewer1.ReportSource = dwr;
                    }
                }

            }
        }
    }
}
