using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Log : Form
    {
        public Log()
        {
            InitializeComponent();
        }
        private db_conf conf = new db_conf();
        private void Log_Load(object sender, EventArgs e)
        {
            conf.Get(dataGridView1,"sp_GetLog");
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            object name = textBox7.Text;
            conf.GetSearch(name, "sp_GetSearchLog", dataGridView1);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;

        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                object name = textBox7.Text;
                conf.GetSearch(name, "sp_GetSearchLog", dataGridView1);
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
            }
        }
    }
}
