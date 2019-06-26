using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductShop
{
    public partial class User : Form
    {
        private bool editMode = false;
        //string connectionString = @"Data Source=DESKTOP-FPQ69BF\SQLSERVER;Initial Catalog=ProductShop;Integrated Security=True";
        //private DataSet ds;
        //private SqlDataAdapter adapter;
        //private SqlCommand cmd;
        public bool anyForm;
        private Options option;
        public User()
        {
            InitializeComponent();
        }

    
        private void add_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            editMode = false;
            dataGridView1.Visible = false;
            groupBox1.Visible = true;
            groupBox1.Text = "Добавление";
            add.Enabled = false;
            update.Enabled = false;
            delete.Enabled = false;
            label2.Visible = false;
        }

        private void update_Click(object sender, EventArgs e)
        {
            editMode = true;
            dataGridView1.Visible = false;
            groupBox1.Visible = true;
            groupBox1.Text = "Редактирование";
            add.Enabled = false;
            update.Enabled = false;
            delete.Enabled = false;
            label2.Visible = false;
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            //option.SoftDelete(dataGridView1, "sp_SoftDeleteSeller", "sp_GetSellers");
            Object[] objects = { dataGridView1.CurrentRow.Cells[0].Value };
            string[] param = { "@id" };
            option.CRUD(dataGridView1, "sp_SoftDeleteSeller", param, objects, "sp_GetSellers");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text !="")
            {
                dataGridView1.Visible = true;
                groupBox1.Visible = false;
                add.Enabled = true;
                update.Enabled = true;
                delete.Enabled = true;
                label2.Visible = true;
            
                if (!editMode)
                {

                    Object[] objects = { textBox1.Text };
                    string[] param = { "@fio" };
                    option.CRUD(dataGridView1, "sp_InsertSeller", param, objects, "sp_GetSellers");
                }
                else
                {
                    Object[] objects = { dataGridView1.CurrentRow.Cells[0].Value, textBox1.Text };
                    string[] param = { "@id", "@fio" };
                    option.CRUD(dataGridView1, "sp_UpdateSeller", param, objects, "sp_GetSellers");
                }
            }
            else
            {
                MessageBox.Show("Проверьте корректность введенных данных.");
            }
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            groupBox1.Visible = false;
            add.Enabled = true;
            update.Enabled = true;
            delete.Enabled = true;
            label2.Visible = true;
        }

        private void User_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (groupBox1.Visible != true) return;
            MessageBox.Show("Закончите работу с данными");
            e.Cancel = true;
        }

        private void User_Load(object sender, EventArgs e)
        {
            option = new Options();
            option.Get(dataGridView1, "sp_GetSellers");
            dataGridView1.Columns["ID_Seller"].Visible = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Owner is Purchase pr)
            {
                anyForm = pr.transition;
                if (dataGridView1.CurrentRow != null && anyForm)
                {
                    pr.ID_Seller = Convert.ToInt32(value: dataGridView1.CurrentRow.Cells[0].Value);
                    Close();
                }
            }
        }
    }
}
