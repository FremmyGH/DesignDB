﻿using System;
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
    public partial class Publishment : Form
    {
        public Publishment()
        {
            InitializeComponent();
        }
        db_conf conf = new db_conf();
        private bool editMode;
        private void Add_Click(object sender, EventArgs e)
        {
            groupBox1.Text = "Добавление";
            Add.Enabled = false;
            Update.Enabled = false;
            Delete.Enabled = false;
            dataGridView1.Visible = false;
            label1.Visible = false;
            groupBox1.Visible = true;
            textBox1.Text = "";
            editMode = false;
        }

        private void Update_Click(object sender, EventArgs e)
        {
            groupBox1.Text = "Редактирование";
            Add.Enabled = false;
            Update.Enabled = false;
            Delete.Enabled = false;
            dataGridView1.Visible = false;
            label1.Visible = false;
            groupBox1.Visible = true;
            editMode = true;
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();

        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {


                Add.Enabled = true;
                Update.Enabled = true;
                Delete.Enabled = true;
                dataGridView1.Visible = true;
                label1.Visible = true;
                groupBox1.Visible = false;

                if (!editMode)
                {
                    Dictionary<string, object> paramDictionary = new Dictionary<string, object>
                    {
                        ["@name"] = textBox1.Text
                    };

                    conf.CRUD(dataGridView1, "sp_InsertPublishment", paramDictionary, "sp_GetPublishment");
                    Dictionary<string, object> paramDictionaryLog = new Dictionary<string, object>
                    {
                        ["@datetime"] = DateTime.Now,
                        ["object"] = "Издательство: " + textBox1.Text,
                        ["action"] = "Добавление"
                    };
                    conf.InsertLog("sp_InsertLog", paramDictionaryLog);
                }
                else
                {
                    if (dataGridView1.CurrentRow != null)
                    {
                        Dictionary<string, object> paramDictionaryLog = new Dictionary<string, object>
                        {
                            ["@datetime"] = DateTime.Now,
                            ["object"] = "Издательство: " + dataGridView1.CurrentRow.Cells[1].Value,
                            ["action"] = "Редактирование"
                        };
                        conf.InsertLog("sp_InsertLog", paramDictionaryLog);
                    }

                    Dictionary<string, object> paramDictionary = new Dictionary<string, object>
                    {
                        ["@id"] = dataGridView1.CurrentRow.Cells[0].Value,
                        ["@name"] = textBox1.Text
                    };
                    conf.CRUD(dataGridView1, "sp_UpdatePublishment", paramDictionary, "sp_GetPublishment");
                  

                }
            }
            else
            {
                MessageBox.Show("Проверьте корректность введенных данных.");
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Add.Enabled = true;
            Update.Enabled = true;
            Delete.Enabled = true;
            dataGridView1.Visible = true;
            label1.Visible = true;
            groupBox1.Visible = false;
        }

        private void Publishment_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (groupBox1.Visible != true) return;
            MessageBox.Show("Проверьте корректность данных");
            e.Cancel = true;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Owner is Book book)
            {
                if (dataGridView1.CurrentRow != null && book.Transition)
                {
                    book.ID_Publish = Convert.ToInt32(value: dataGridView1.CurrentRow.Cells[0].Value);
                    book.dblclck = true;
                    Close();
                }
            }
        }

        private void Publishment_Load(object sender, EventArgs e)
        {
            conf.Get(dataGridView1, "sp_GetPublishment");
            dataGridView1.Columns["ID_Publishment"].Visible = false;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>
            {
                ["@id"] = dataGridView1.CurrentRow.Cells[0].Value
            };

            conf.CRUD(dataGridView1, "sp_SoftDeletePublishment", paramDictionary, "sp_GetPublishment");
            Dictionary<string, object> paramDictionaryLog = new Dictionary<string, object>
            {
                ["@datetime"] = DateTime.Now,
                ["object"] = "Издательство: " + dataGridView1.CurrentRow.Cells[1].Value,
                ["action"] = "Удаление"
            };
            conf.InsertLog("sp_InsertLog", paramDictionaryLog);
        }
    }
}
