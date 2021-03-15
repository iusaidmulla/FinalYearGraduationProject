using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FinalYearProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Button btn = new Button();
            btn.Name = "btnInsert";
            btn.Width = 250;
            btn.Height = 90;
            btn.Text = "Insert";
            btn.Font = new Font("Times New Roman", 16f);
            btn.Location = new Point(300, 100);
          //  btn.Click += new EventHandler(this.btnInsert_Click);
            this.Controls.Add(btn);


            
        }

        private void btnInsert_Click(object sender, EventHandler e)
        {
            MessageBox.Show("Hello");
        }
    }
}
