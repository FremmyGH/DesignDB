using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductShop
{
    public partial class Main : Form
    {
        Options option = new Options();
        public Main()
        {
            InitializeComponent();
        }

        private void пользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var user = new User();
            user.ShowDialog();
        }

        private void покупкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var purchase = new Purchase();
            purchase.ShowDialog();
            option.Get(dataGridView1, "sp_GetProducts");
        }

        private void продуктыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var product = new Product();
            product.ShowDialog();
        }

        private void едИзмToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var unit= new Unit();
            unit.ShowDialog();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            option.Get(dataGridView1, "sp_GetProducts");
            dataGridView1.Columns["ID_Product"].Visible = false;
            dataGridView1.Columns["Unit_ID"].Visible = false;
        }
    }
}
