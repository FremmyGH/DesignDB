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
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private bool editMode;
        db_conf conf = new db_conf();
        public bool Transition;
        public int ID_Role;
        public string[] resultAuth;
        public bool dblclck;
        private string oldLogin;
        private void Add_Click(object sender, EventArgs e)
        {
            groupBox1.Text = "Редактирование";
            Add.Visible = false;
            Update.Visible = false;
            Delete.Visible = false;
            dataGridView1.Visible = false;
            label1.Visible = false;
            groupBox1.Visible = true;
            editMode = false;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox2.SelectedIndex = -1;
            oldLogin = "";
        }

        private void Update_Click(object sender, EventArgs e)
        {


            if (Convert.ToInt32(resultAuth[4]) == 2 && Convert.ToInt32(dataGridView1.CurrentRow.Cells[4].Value) == 2)
            {
                MessageBox.Show("Вы не можете редактировать других администраторов");
            }
            else
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
                textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                comboBox2.SelectedValue = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                oldLogin = textBox2.Text;
            }

        }

        private void Delete_Click(object sender, EventArgs e)
        {

            if (Convert.ToInt32(resultAuth[4]) == 2 && Convert.ToInt32(dataGridView1.CurrentRow.Cells[4].Value) == 2)
            {
                MessageBox.Show("Вы не можете удалять других администраторов");
            }
            else
            {
                Dictionary<string, object> paramDictionaryLog = new Dictionary<string, object>
                {
                    ["@datetime"] = DateTime.Now,
                    ["object"] = "Пользователь: " + dataGridView1.CurrentRow.Cells[2].Value,
                    ["action"] = "Удаление"
                };
                conf.InsertLog("sp_InsertLog", paramDictionaryLog);
                Dictionary<string, object> paramDictionary = new Dictionary<string, object>
                {
                    ["@id"] = dataGridView1.CurrentRow.Cells[0].Value
                };

                conf.CRUD(dataGridView1, "sp_SoftDeleteClient", paramDictionary, "sp_GetClient");
               
            }

        }

        private void Confirm_Click(object sender, EventArgs e)
        {
           
            if (textBox1.Text != "")
            {
                var result = conf.CheckAuth(textBox2.Text, "sp_CheckAuth");
                if (result[5] == "Успех" && oldLogin != textBox2.Text)
                {
                    MessageBox.Show("Такой логин уже занят");
                }
                else
                {
                    if (Convert.ToInt32(resultAuth[4]) == 2 && Convert.ToInt32(comboBox2.SelectedValue) == 2)
                    {
                        MessageBox.Show("Вы не можете добавлять другого администратора");
                    }
                    else
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
                                ["@name"] = textBox1.Text,
                                ["@login"] = textBox2.Text,
                                ["@password"] = textBox3.Text,
                                ["@role"] = comboBox2.SelectedValue
                            };
                            conf.CRUD(dataGridView1, "sp_InsertClient", paramDictionary, "sp_GetClient");
                            Dictionary<string, object> paramDictionaryLog = new Dictionary<string, object>
                            {
                                ["@datetime"] = DateTime.Now,
                                ["object"] = "Пользователь: " + textBox2.Text,
                                ["action"] = "Добавление"
                            };
                            conf.InsertLog("sp_InsertLog", paramDictionaryLog);
                        }
                        else
                        {
                            Dictionary<string, object> paramDictionaryLog = new Dictionary<string, object>
                            {
                                ["@datetime"] = DateTime.Now,
                                ["object"] = "Пользователь: " + dataGridView1.CurrentRow.Cells[2].Value,
                                ["action"] = "Редактирование"
                            };
                            conf.InsertLog("sp_InsertLog", paramDictionaryLog);
                            Dictionary<string, object> paramDictionary = new Dictionary<string, object>
                            {
                                ["@id"] = dataGridView1.CurrentRow.Cells[0].Value,
                                ["@name"] = textBox1.Text,
                                ["@login"] = textBox2.Text,
                                ["@password"] = textBox3.Text,
                                ["@role"] = comboBox2.SelectedValue
                            };
                            conf.CRUD(dataGridView1, "sp_UpdateClient", paramDictionary, "sp_GetClient");
                          
                        }
                    }
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

        private void Role_Click(object sender, EventArgs e)
        {
            dblclck = false;
            Transition = true;
            var oldValue = comboBox2.SelectedValue;
            var role = new Role()
            {
                Owner = this
            };
            role.ShowDialog();
            conf.GetCombobox(comboBox2, "sp_GetRole", "ID_Role", "Наименование");
            if (dblclck)
            {
                comboBox2.SelectedValue = ID_Role;
            }
            else
            {
                comboBox2.SelectedValue = oldValue;
            }
        }

        private void Client_Load(object sender, EventArgs e)
        {
            conf.Get(dataGridView1, "sp_GetClient");
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            conf.GetCombobox(comboBox2, "sp_GetRole", "ID_Role", "Наименование");
            comboBox2.SelectedIndex = -1;
            if (Owner is Form1 main)
            {
                resultAuth = main.resultAuth;
            }
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (groupBox1.Visible != true) return;
            MessageBox.Show("Проверьте корректность данных");
            e.Cancel = true;
        }
    }
}
