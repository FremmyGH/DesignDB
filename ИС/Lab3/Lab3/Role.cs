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
    public partial class Role : Form
    {
        public Role()
        {
            InitializeComponent();
        }

        private bool editMode;
        db_conf conf = new db_conf();
        private void Add_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            groupBox1.Text = "Добавление";
            Add.Visible = false;
            Update.Visible = false;
            Delete.Visible = false;
            dataGridView1.Visible = false;
            label1.Visible = false;
            groupBox1.Visible = true;
            editMode = false;
        }

        private void Update_Click(object sender, EventArgs e)
        {
            groupBox1.Text = "Редактирование";
            Add.Visible = false;
            Update.Visible = false;
            Delete.Visible = false;
            dataGridView1.Visible = false;
            label1.Visible = false;
            groupBox1.Visible = true;
            editMode = true;
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>
            {
                ["@id"] = dataGridView1.CurrentRow.Cells[0].Value
            };

            conf.CRUD(dataGridView1, "sp_SoftDeleteRole", paramDictionary, "sp_GetRole");

            Dictionary<string, object> paramDictionaryLog = new Dictionary<string, object>
            {
                ["@datetime"] = DateTime.Now,
                ["object"] = "Роль: " + dataGridView1.CurrentRow.Cells[1].Value,
                ["action"] = "Удаление"
            };
            conf.InsertLog("sp_InsertLog", paramDictionaryLog);
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Add.Visible = true;
                Update.Visible = true;
                Delete.Visible = true;
                dataGridView1.Visible = true;
                label1.Visible = true;
                groupBox1.Visible = false;
                if (!editMode)
                {
                    Dictionary<string, object> paramDictionary = new Dictionary<string, object>
                    {
                        ["@name"] =textBox1.Text
                    };
                    conf.CRUD(dataGridView1, "sp_InsertRole",paramDictionary,"sp_GetRole");
                    Dictionary<string, object> paramDictionaryLog = new Dictionary<string, object>
                    {
                        ["@datetime"] = DateTime.Now,
                        ["object"] = "Роль: " + textBox1.Text,
                        ["action"] = "Добавление"
                    };
                    conf.InsertLog("sp_InsertLog", paramDictionaryLog);
                }
                else
                {
                    Dictionary<string, object> paramDictionaryLog = new Dictionary<string, object>
                    {
                        ["@datetime"] = DateTime.Now,
                        ["object"] = "Роль: " + dataGridView1.CurrentRow.Cells[1].Value,
                        ["action"] = "Редактирование"
                    };
                    conf.InsertLog("sp_InsertLog", paramDictionaryLog);
                    Dictionary<string, object> paramDictionary = new Dictionary<string, object>
                    {
                        ["@id"] = dataGridView1.CurrentRow.Cells[0].Value,
                        ["@name"] = textBox1.Text
                    };
                    conf.CRUD(dataGridView1, "sp_UpdateRole", paramDictionary, "sp_GetRole");
                   
                }
            }
            
            else
            {
                MessageBox.Show("Проверьте корректность введенных данных");
            }

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Add.Visible = true;
            Update.Visible = true;
            Delete.Visible = true;
            dataGridView1.Visible = true;
            label1.Visible = true;
            groupBox1.Visible = false;
        }

        private void Role_Load(object sender, EventArgs e)
        {
            conf.Get(dataGridView1,"sp_GetRole");
            dataGridView1.Columns[0].Visible = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Owner is Client client)
            {
                if (dataGridView1.CurrentRow != null && client.Transition)
                {
                    client.ID_Role = Convert.ToInt32(value: dataGridView1.CurrentRow.Cells[0].Value);
                    client.dblclck = true;
                    Close();
                }
            }

        }
    }
}
