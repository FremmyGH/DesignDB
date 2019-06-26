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
    public partial class Unit : Form
    {
        private bool editMode = false;
        private bool anyForm = false;
        private Options option;
        public Unit()
        {
            InitializeComponent();
        }

        private void add_Click(object sender, EventArgs e)
        {
            editMode = false;
            textBox1.Text = "";
            textBox2.Text = "";
            dataGridView1.Visible = false;
            groupBox1.Visible = true;
            groupBox1.Text = "Добавление";
            add.Enabled = false;
            update.Enabled = false;
            delete.Enabled = false;
            label1.Visible = false;
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
            label1.Visible = false;
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            Object[] objects = { dataGridView1.CurrentRow.Cells[0].Value };
            string[] param = { "@id" };
            option.CRUD(dataGridView1, "sp_SoftDeleteUnit", param, objects, "sp_GetUnit");
        }

        private void button5_Click(object sender, EventArgs e)
        {
             if (textBox1.Text !="" && textBox2.Text != "")
             {
                dataGridView1.Visible = true;
                groupBox1.Visible = false;
                label1.Visible = true;
                add.Enabled = true;
                update.Enabled = true;
                delete.Enabled = true;
           
                if (!editMode)
                {
                    Object[] objects = { textBox1.Text, textBox2.Text };
                    string[] param = { "@nameS", "@nameF" };
                    option.CRUD(dataGridView1, "sp_InsertUnit", param, objects, "sp_GetUnit");
                }
                else
                {
                    Object[] objects = { dataGridView1.CurrentRow.Cells[0].Value, textBox1.Text, textBox2.Text };
                    string[] param = { "@id", "@nameS", "@nameF" };
                    option.CRUD(dataGridView1, "sp_UpdateUnit", param, objects, "sp_GetUnit");
                }
            }
            else
            {
                MessageBox.Show("Проверьте корректность введенных данных.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            groupBox1.Visible = false;
            label1.Visible = true;
            add.Enabled = true;
            update.Enabled = true;
            delete.Enabled = true;
        }

        private void Unit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (groupBox1.Visible != true) return;
            MessageBox.Show("Проверьте корректность данных");
            e.Cancel = true;
        }

        private void Unit_Load(object sender, EventArgs e)
        {
            option = new Options();
            option.Get(dataGridView1, "sp_GetUnit");
            dataGridView1.Columns["ID_Unit"].Visible = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Owner is Product pr)
            {
                anyForm = pr.transition;
                if (dataGridView1.CurrentRow != null && anyForm)
                {
                    pr.ID_Unit = Convert.ToInt32(value: dataGridView1.CurrentRow.Cells[0].Value);
                    Close();
                }
            }
        }
    }
}
