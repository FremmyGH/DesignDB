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
    public partial class Product : Form
    {
        private bool editMode = false;
        public int ID_Unit;
        public int Kolvo;
        public string nameProduct;
        public bool transition;
        public bool transitionDop;
        private bool anyForm;
        private Options option = new Options();
        public bool error = true;
        public bool errorMN = false;
        public int foolKolvo;
        public Product()
        {
            InitializeComponent();
        }

        private void add_Click(object sender, EventArgs e)
        {
            editMode = false;
            editMode = false;
            dataGridView1.Visible = false;
            groupBox1.Visible = true;
            groupBox1.Text = "Добавление";
            add.Enabled = false;
            update.Enabled = false;
            delete.Enabled = false;
            label1.Visible = false;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.SelectedIndex = -1;
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
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            comboBox1.SelectedValue = Convert.ToInt32(dataGridView1.CurrentRow.Cells[6].Value);
        }

        private void delete_Click(object sender, EventArgs e)
        {
            Object[] objects = { dataGridView1.CurrentRow.Cells[0].Value };
            string[] param = { "@id" };
            option.CRUD(dataGridView1, "sp_SoftDeleteProduct", param, objects, "sp_GetProducts");
        }

        private void Product_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Owner is Purchase pr && error)
            {
                pr.errorMN = true;
            }
            if (groupBox1.Visible != true) return;
            MessageBox.Show("Проверьте корректность данных");
            
            e.Cancel = true;
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && double.TryParse(textBox2.Text, out var price)
                                    && int.TryParse(textBox3.Text, out var kol)
                                    && comboBox1.SelectedValue != null
                                    && textBox4.Text != "")
            {
                dataGridView1.Visible = true;
                groupBox1.Visible = false;
                label1.Visible = true;
                add.Enabled = true;
                update.Enabled = true;
                delete.Enabled = true;
          
                if (!editMode)
                {
                    Object[] objects = { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, comboBox1.SelectedValue };
                    string[] param = { "@name", "@price", "@kolvo", "@article", "@unit" };
                    option.CRUD(dataGridView1, "sp_InsertProduct", param, objects, "sp_GetProducts");
                }
                else
                {
                    Object[] objects = { dataGridView1.CurrentRow.Cells[0].Value, textBox1.Text, Convert.ToDecimal(textBox2.Text), textBox3.Text, textBox4.Text, comboBox1.SelectedValue };
                    string[] param = { "@id", "@name", "@price", "@kolvo", "@article", "@unit" };
                    option.CRUD(dataGridView1, "sp_UpdateProduct", param, objects, "sp_GetProducts");
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

        private void Product_Load(object sender, EventArgs e)
        {
            option.Get(dataGridView1,"sp_GetProducts");
            dataGridView1.Columns["ID_Product"].Visible = false;
            dataGridView1.Columns["Unit_ID"].Visible = false;
            option.GetCombobox(comboBox1, "sp_GetUnit", "ID_Unit", "Короткое название");
            comboBox1.SelectedIndex = -1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            transition = true;
            var unit = new Unit
            {
                Owner = this
            };
            unit.ShowDialog();
            option.GetCombobox(comboBox1, "sp_GetUnit", "ID_Unit", "Короткое название");
            comboBox1.SelectedValue = ID_Unit;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Owner is Purchase pr)
            {
                error = false;
                errorMN = false;
                anyForm = pr.transitionMN;
                try
                {
                    if (dataGridView1.CurrentRow != null && anyForm)
                    {
                        pr.ID_Product = Convert.ToInt32(value: dataGridView1.CurrentRow.Cells[0].Value);
                        transitionDop = true;
                        var dopForm = new dopForm()
                        {
                            Owner = this
                        };
                        nameProduct = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                        foolKolvo = Convert.ToInt32(dataGridView1.CurrentRow.Cells[3].Value);
                        dopForm.ShowDialog();
                        pr.Kolvo = Kolvo;
                        int ostatok = Convert.ToInt32(dataGridView1.CurrentRow.Cells[3].Value) - Kolvo;
                        pr.dgwProductId = dataGridView1.CurrentRow.Cells[0].Value;
                        pr.ostatok = ostatok;
                        if (!errorMN)
                        {
                            Close();
                        }
                    }
                }
                catch 
                {
                    
                }
                
            }
        }
    }
}
