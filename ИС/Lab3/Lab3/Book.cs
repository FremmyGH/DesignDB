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
    public partial class Book : Form
    {
        public Book()
        {
            InitializeComponent();
        }

        public bool Transition;
        public int ID_Publish;
        public int ID_Genre;
        public int ID_TypeBook;
        public int ID_Autor;
        public string Autor;
        db_conf conf = new db_conf();
        private bool editMode;
        private Object id;
        //private DataSet ds;
        private DataTable dt;
        private string grpbx = "search";
        public bool dblclck = false; 
        private void Add_Click(object sender, EventArgs e)
        {
            groupBox1.Text = "Добавление";
            groupBox1.Top = 10;
            Add.Enabled = false;
            Update.Enabled = false;
            Delete.Enabled = false;
            dataGridView1.Visible = false;
            label1.Visible = false;
            groupBox1.Visible = true;
            editMode = false;
            groupBox2.Visible = false;
            groupBox3.Visible = false;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;

            //ds = new DataSet();
            dt = new DataTable();

            DataColumn Book_ID = new DataColumn();
            DataColumn Autor_ID = new DataColumn();
            DataColumn Autor = new DataColumn();
            dt.Columns.Add(Book_ID);
            dt.Columns.Add(Autor_ID);
            dt.Columns.Add(Autor);
            dt.Columns[2].ColumnName = "ФИО";
            dataGridView2.DataSource = dt;
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Visible = false;
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                groupBox1.Text = "Редактирование";
                groupBox1.Top = 10;
                Add.Enabled = false;
                Update.Enabled = false;
                Delete.Enabled = false;
                dataGridView1.Visible = false;
                label1.Visible = false;
                groupBox1.Visible = true;
                editMode = true;
                groupBox2.Visible = false;
                groupBox3.Visible = false;
                textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                comboBox1.SelectedValue = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                comboBox2.SelectedValue = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                comboBox3.SelectedValue = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                textBox5.Text = dataGridView1.CurrentRow.Cells[11].Value.ToString();
                textBox6.Text = dataGridView1.CurrentRow.Cells[12].Value.ToString();

                id = dataGridView1.CurrentRow.Cells[0].Value;
                conf.GetMN(id, "sp_GetBook_Autor", dataGridView2);
                dataGridView2.Columns["Book_ID"].Visible = false;
                dataGridView2.Columns["Autor_ID"].Visible = false;
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования");
            }
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" &
                int.TryParse(textBox2.Text, out var kol) &
                int.TryParse(textBox4.Text, out var kolp) &
                int.TryParse(textBox5.Text, out var kolc) &
                textBox6.Text != "" &
                comboBox1.SelectedIndex != -1 &
                comboBox2.SelectedIndex != -1 &
                comboBox3.SelectedIndex != -1)
            {


                Add.Enabled = true;
                Update.Enabled = true;
                Delete.Enabled = true;
                dataGridView1.Visible = true;
                label1.Visible = true;
                groupBox1.Visible = false;
                if (grpbx == "search")
                {
                    groupBox2.Visible = true;
                }
                else
                {
                    groupBox3.Visible = true;
                }


                if (!editMode)
                {
                    Dictionary<string, object> paramDictionary = new Dictionary<string, object>
                    {
                        ["@name"] = textBox1.Text,
                        ["@annotation"] = textBox3.Text,
                        ["@yearPublish"] = textBox2.Text,
                        ["@kolvo"] = textBox4.Text,
                        ["publish"] = comboBox1.SelectedValue,
                        ["genre"] = comboBox2.SelectedValue,
                        ["typeBook"] = comboBox3.SelectedValue,
                        ["@kolvoPage"] = textBox5.Text,
                        ["@udk"] = textBox6.Text
                    };
                    conf.CRUD(dataGridView1, "sp_InsertBook", paramDictionary, "sp_GetBook");

                    foreach (DataRow row in dt.Rows)
                    {
                        Dictionary<string, object> paramMNDictionary = new Dictionary<string, object>
                        {
                            ["@book"] = row[0],
                            ["@autor"] = row[1]
                        };
                        conf.CRUD_MN(id, dataGridView2, "sp_InsertBook_Autor", paramMNDictionary, "sp_GetBook_Autor");
                    }
                    Dictionary<string, object> paramDictionaryLog = new Dictionary<string, object>
                    {
                        ["@datetime"] = DateTime.Now,
                        ["object"] = "Книга: " + textBox1.Text,
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
                            ["object"] = "Книга: " + dataGridView1.CurrentRow.Cells[1].Value,
                            ["action"] = "Редактирование"
                        };
                        conf.InsertLog("sp_InsertLog", paramDictionaryLog);
                    }

                    Dictionary<string, object> paramDictionary = new Dictionary<string, object>
                    {
                        ["@id"] = dataGridView1.CurrentRow.Cells[0].Value,
                        ["@name"] = textBox1.Text,
                        ["@annotation"] = textBox3.Text,
                        ["@yearPublish"] = textBox2.Text,
                        ["@kolvo"] = textBox4.Text,
                        ["publish"] = comboBox1.SelectedValue,
                        ["genre"] = comboBox2.SelectedValue,
                        ["typeBook"] = comboBox3.SelectedValue,
                        ["@kolvoPage"] = textBox5.Text,
                        ["@udk"] = textBox6.Text
                    };

                    conf.CRUD(dataGridView1, "sp_UpdateBook", paramDictionary, "sp_GetBook");
                   
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
            if (grpbx == "search")
            {
                groupBox2.Visible = true;
            }
            else
            {
                groupBox3.Visible = true;
            }

        }

        private void Book_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (groupBox1.Visible != true) return;
            MessageBox.Show("Проверьте корректность данных");
            e.Cancel = true;
        }

     

        private void Book_Load(object sender, EventArgs e)
        {
            //0-id, 5 - Publishment_ID, 6 - Genre_ID, 7 - TypeBook_ID
            conf.Get(dataGridView1,"sp_GetBook");
            dataGridView1.Columns["ID_Book"].Visible = false;
            dataGridView1.Columns["Publishment_ID"].Visible = false;
            dataGridView1.Columns["Genre_ID"].Visible = false;
            dataGridView1.Columns["TypeBook_ID"].Visible = false;

            conf.GetCombobox(comboBox1, "sp_GetPublishment", "ID_Publishment", "Издательство");
            comboBox1.SelectedIndex = -1;
            conf.GetCombobox(comboBox6, "sp_GetPublishment", "ID_Publishment", "Издательство");
            comboBox6.SelectedIndex = -1;

            conf.GetCombobox(comboBox2, "sp_GetGenre", "ID_Genre", "Жанр");
            comboBox2.SelectedIndex = -1;
            conf.GetCombobox(comboBox5, "sp_GetGenre", "ID_Genre", "Жанр");
            comboBox5.SelectedIndex = -1;

            conf.GetCombobox(comboBox3, "sp_GetTypeBook", "ID_TypeBook", "Тип книги");
            comboBox3.SelectedIndex = -1;
            conf.GetCombobox(comboBox4, "sp_GetTypeBook", "ID_TypeBook", "Тип книги");
            comboBox4.SelectedIndex = -1;

        }
        private void Publish_Click(object sender, EventArgs e)
        {
            dblclck = false;
            var oldValue = comboBox1.SelectedValue;
            Transition = true;
            var publishment = new Publishment()
            {
                Owner = this
            };
            publishment.ShowDialog();
            conf.GetCombobox(comboBox1, "sp_GetPublishment", "ID_Publishment", "Издательство");

            if (dblclck)
            {
                comboBox1.SelectedValue = ID_Publish;
            }
            else
            {
                comboBox1.SelectedValue = oldValue;
            }
        }

        private void Genre_Click(object sender, EventArgs e)
        {
            dblclck = false;
            var oldValue = comboBox2.SelectedValue;
            Transition = true;
            var genre = new Genre()
            {
                Owner = this
            };
            genre.ShowDialog();
 

            conf.GetCombobox(comboBox2, "sp_GetGenre", "ID_Genre", "Жанр");

            if (dblclck)
            {
                comboBox2.SelectedValue = ID_Genre;
            }
            else
            {
                comboBox2.SelectedValue = oldValue;
            }
        }

        private void TypeBook_Click(object sender, EventArgs e)
        {
            dblclck = false;
            var oldValue = comboBox3.SelectedValue;
            Transition = true;
            var typeBook = new TypeBook()
            {
                Owner = this
            };
            typeBook.ShowDialog();
            
            conf.GetCombobox(comboBox3, "sp_GetTypeBook", "ID_TypeBook", "Тип книги");

            if (dblclck)
            {
                comboBox3.SelectedValue = ID_TypeBook;
            }
            else
            {
                comboBox3.SelectedValue = oldValue;
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>
            {
                ["@id"] = dataGridView1.CurrentRow.Cells[0].Value
            };

            conf.CRUD(dataGridView1, "sp_SoftDeleteBook", paramDictionary, "sp_GetBook");

            Dictionary<string, object> paramDictionaryLog = new Dictionary<string, object>
            {
                ["@datetime"] = DateTime.Now,
                ["object"] = "Книга: " + dataGridView1.CurrentRow.Cells[1].Value,
                ["action"] = "Удаление"
            };
            conf.InsertLog("sp_InsertLog", paramDictionaryLog);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Transition = true;
            var autor = new Author()
            {
                Owner = this
            };
            autor.ShowDialog();

            if (!editMode)
            {
                if (dataGridView2.RowCount != 0)
                {
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        if (Convert.ToInt32(dataGridView2.Rows[i].Cells[1].Value) != ID_Autor)
                        {
                            id = conf.GetA_I(Convert.ToDecimal(id), "sp_GetAI") + 1;
                            //MessageBox.Show(id.ToString());
                            dt.Rows.Add(id, ID_Autor, Autor);
                            break;
                        }
                    }
                }
                else
                {
                    id = conf.GetA_I(Convert.ToDecimal(id), "sp_GetAI") + 1;
                    //MessageBox.Show(id.ToString());
                    dt.Rows.Add(id, ID_Autor, Autor);
                }
                
                
            }
            else
            {
                Dictionary<string, object> paramDictionary = new Dictionary<string, object>
                {
                    ["@book"] = id,
                    ["@autor"] = ID_Autor
                };
                conf.CRUD_MN(id, dataGridView2, "sp_InsertBook_Autor", paramDictionary, "sp_GetBook_Autor");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>
            {
                ["@book"] = dataGridView2.CurrentRow.Cells[0].Value,
                ["autor"] = dataGridView2.CurrentRow.Cells[1].Value
            };
            conf.CRUD_MN(id, dataGridView2, "sp_DeleteBook_Autor", paramDictionary, "sp_GetBook_Autor");
        }

        private void Filters_Click(object sender, EventArgs e)
        {
            grpbx = "fullSearch";
            //var groupBox1Location = groupBox1.Location;
            //groupBox1Location.Y = 100;
            //var dgw = dataGridView1.Location;
            //dgw.Y = 100;
            groupBox3.Visible = true;
            groupBox2.Visible = false;
            dataGridView1.Top = 250;
            label1.Top = 230;
            dataGridView1.Height = 300;
            //groupBox1.Top = 200;
            conf.Get(dataGridView1, "sp_GetBook");
            dataGridView1.Columns["ID_Book"].Visible = false;
            dataGridView1.Columns["Publishment_ID"].Visible = false;
            dataGridView1.Columns["Genre_ID"].Visible = false;
            dataGridView1.Columns["TypeBook_ID"].Visible = false;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            object name = textBox7.Text;
            conf.GetSearch(name, "sp_GetSearch", dataGridView1);
            dataGridView1.Columns["ID_Book"].Visible = false;
            dataGridView1.Columns["Publishment_ID"].Visible = false;
            dataGridView1.Columns["Genre_ID"].Visible = false;
            dataGridView1.Columns["TypeBook_ID"].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox7.Text = "";
            conf.Get(dataGridView1, "sp_GetBook");
            dataGridView1.Columns["ID_Book"].Visible = false;
            dataGridView1.Columns["Publishment_ID"].Visible = false;
            dataGridView1.Columns["Genre_ID"].Visible = false;
            dataGridView1.Columns["TypeBook_ID"].Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            comboBox6.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            grpbx = "search";
            dataGridView1.Top = 120;
            dataGridView1.Height = 490;
            label1.Top = 100;
            //groupBox1.Top = 120;
            groupBox2.Visible = true;
            groupBox3.Visible = false;

            textBox7.Text = "";
            conf.Get(dataGridView1, "sp_GetBook");
            dataGridView1.Columns["ID_Book"].Visible = false;
            dataGridView1.Columns["Publishment_ID"].Visible = false;
            dataGridView1.Columns["Genre_ID"].Visible = false;
            dataGridView1.Columns["TypeBook_ID"].Visible = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dblclck = false;
            var oldValue = comboBox6.SelectedValue;
            Transition = true;
            var publishment = new Publishment()
            {
                Owner = this
            };
            publishment.ShowDialog();
           
            conf.GetCombobox(comboBox6, "sp_GetPublishment", "ID_Publishment", "Издательство");

            if (dblclck)
            {
                comboBox6.SelectedValue = ID_Publish;
            }
            else
            {
                comboBox6.SelectedValue = oldValue;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dblclck = false;
            var oldValue = comboBox5.SelectedValue;
            Transition = true;
            var genre = new Genre()
            {
                Owner = this
            };
        
            conf.GetCombobox(comboBox5, "sp_GetGenre", "ID_Genre", "Жанр");
         
            genre.ShowDialog();
            if (dblclck)
            {
                comboBox5.SelectedValue = ID_Genre;
            }
            else
            {
                comboBox5.SelectedValue = oldValue;
            }
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dblclck = false;
            var oldValue = comboBox4.SelectedValue;
            Transition = true;
            var typeBook = new TypeBook()
            {
                Owner = this
            };
            typeBook.ShowDialog();
            
            conf.GetCombobox(comboBox4, "sp_GetTypeBook", "ID_TypeBook", "Тип книги");
            if (dblclck)
            {
                comboBox4.SelectedValue = ID_TypeBook;
            }
            else
            {
                comboBox4.SelectedValue = oldValue;
            }
        }

        private void FullSearch_Click(object sender, EventArgs e)
        {
            object[] objects = new object[6];
            objects[0] = textBox8.Text;
            if (comboBox6.SelectedValue != null)
            {
                objects[1] = comboBox6.SelectedValue;
            }
            else
            {
                objects[1] = "";
            }

            if (comboBox5.SelectedValue != null)
            {
                objects[2] = comboBox5.SelectedValue;
            }
            else
            {
                objects[2] = "";
            }

            if (comboBox4.SelectedValue != null)
            {
                objects[3] = comboBox4.SelectedValue;
            }
            else
            {
                objects[3] = "";
            }
            objects[4] = textBox9.Text;
            objects[5] = textBox10.Text;
            conf.GetFullSearch(objects, "sp_FullSearch", dataGridView1);
            dataGridView1.Columns["ID_Book"].Visible = false;
            dataGridView1.Columns["Publishment_ID"].Visible = false;
            dataGridView1.Columns["Genre_ID"].Visible = false;
            dataGridView1.Columns["TypeBook_ID"].Visible = false;
        }

        private void ClearFullSearch_Click(object sender, EventArgs e)
        {
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            comboBox6.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            conf.Get(dataGridView1, "sp_GetBook");
            dataGridView1.Columns["ID_Book"].Visible = false;
            dataGridView1.Columns["Publishment_ID"].Visible = false;
            dataGridView1.Columns["Genre_ID"].Visible = false;
            dataGridView1.Columns["TypeBook_ID"].Visible = false;
        }

        private void Book_KeyPress(object sender, KeyPressEventArgs e)
        {

            
        }

        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
      
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                object name = textBox7.Text;
                conf.GetSearch(name, "sp_GetSearch", dataGridView1);
                dataGridView1.Columns["ID_Book"].Visible = false;
                dataGridView1.Columns["Publishment_ID"].Visible = false;
                dataGridView1.Columns["Genre_ID"].Visible = false;
                dataGridView1.Columns["TypeBook_ID"].Visible = false;
            }
        }
    }
}
