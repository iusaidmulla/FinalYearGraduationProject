using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace FinalYearProject
{
    public partial class AllStaffPaymentReports : Form
    {
        public AllStaffPaymentReports()
        {
            InitializeComponent();
        }

        SqlConnection con = null;
        SqlCommand com = null;
        DataTable dt = null;
        SqlDataAdapter adapter = null;
        string ConStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        private void AllStaffPaymentReports_Load(object sender, EventArgs e)
        {
            using (con = new SqlConnection(ConStr))
            {
                using (adapter = new SqlDataAdapter("select * from StaffPayment", con))
                {
                    dt = new DataTable();
                    adapter.Fill(dt);
                    AllStaffPaymentCrystalReport asr = new AllStaffPaymentCrystalReport();
                    asr.SetDataSource(dt);
                    crystalReportViewer1.ReportSource = asr;
                }
            }
        }


    }
}
