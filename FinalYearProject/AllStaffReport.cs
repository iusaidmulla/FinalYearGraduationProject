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
    public partial class AllStaffReport : Form
    {
        public AllStaffReport()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        DataTable dt = null;
        SqlDataAdapter adapter = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        private void AllStaffReport_Load(object sender, EventArgs e)
        {
            using (con = new SqlConnection(ConStr))
            {
                using (adapter = new SqlDataAdapter("select * from Staff", con))
                {
                    dt = new DataTable();
                    adapter.Fill(dt);
                    AllStaffCrystalReport asr = new AllStaffCrystalReport();
                    asr.SetDataSource(dt);
                    crystalReportViewer1.ReportSource = asr;
                }
            }
        }
    }
}
