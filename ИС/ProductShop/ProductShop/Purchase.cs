using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using EasyDox;
//using DocumentFormat.OpenXml;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Wordprocessing;
//using Word = Microsoft.Office.Interop.Word;
//using System.Reflection;
using System.Drawing;
using Xceed.Words.NET;


namespace ProductShop
{
    public partial class Purchase : Form
    {
        string connectionString =
            @"Data Source=DESKTOP-FPQ69BF\SQLSERVER;Initial Catalog=ProductShop;Integrated Security=True";
        private bool editMode = false;
        public int ID_Seller;
        public int ID_Product;
        public int Kolvo;
        public bool transition = false;
        public bool transitionMN = false;
        public bool errorMN = false;
        private Options option = new Options();
        public decimal id;
        public decimal id2;
        int currentRow;
        public Object dgwProductId;
        public int ostatok;
        public Purchase()
        {
            InitializeComponent();
        }
        private void add_Click(object sender, EventArgs e)
        {
            label5.Text = "Сумма: ";
            editMode = false;
            dataGridView1.Visible = false;
            groupBox1.Visible = true;
            groupBox1.Text = "Добавление";
            add.Enabled = false;
            update.Enabled = false;
            delete.Enabled = false;
            button6.Enabled = false;
            label1.Visible = false;
            dateTimePicker1.ResetText();
            comboBox1.SelectedIndex = -1;

            Object[] objects = { };
            string[] param = { };
            option.CRUD(dataGridView1, "sp_InsertPurchaseMN", param, objects, "sp_GetPurchase");
            id = option.GetA_I(id, "GetAI");
            //MessageBox.Show(id.ToString());
            option.GetMN(id, "sp_GetPurProduct", dataGridView2);
            dataGridView2.Columns["Product_ID"].Visible = false;
            dataGridView2.Columns["Код покупки"].Visible = false;


            
        }

        private void update_Click(object sender, EventArgs e)
        {
            id = 0;
            editMode = true;
            dataGridView1.Visible = false;
            groupBox1.Visible = true;
            groupBox1.Text = "Редактирование";
            add.Enabled = false;
            update.Enabled = false;
            delete.Enabled = false;
            button6.Enabled = false;
            label1.Visible = false;
            dateTimePicker1.Value = (DateTime)dataGridView1.CurrentRow.Cells[1].Value;
            comboBox1.SelectedValue = Convert.ToInt32(dataGridView1.CurrentRow.Cells[4].Value);
            option.GetMN(dataGridView1.CurrentRow.Cells[0].Value, "sp_GetPurProduct", dataGridView2);
            dataGridView2.Columns["Product_ID"].Visible = false;
            dataGridView2.Columns["Код покупки"].Visible = false;


            
            string expression = "GetSum";
            SqlCommand cmd;
            currentRow = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                cmd = new SqlCommand(expression, connection) { CommandType = CommandType.StoredProcedure };
                var param = new SqlParameter()
                {
                    ParameterName = "id",
                    Value = dataGridView1.CurrentRow.Cells[0].Value
                };
                cmd.Parameters.Add(param);
                label5.Text ="Сумма: "+ cmd.ExecuteScalar().ToString() + " руб.";
                connection.Close();
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            Object[] objects = { dataGridView1.CurrentRow.Cells[0].Value };
            string[] param = { "@id" };
            option.CRUD(dataGridView1, "sp_SoftDeletePurchase", param, objects, "sp_GetPurchase");
        }

        private void Purchase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (groupBox1.Visible != true) return;
            MessageBox.Show("Проверьте корректность данных");
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                dataGridView1.Visible = true;
                groupBox1.Visible = false;
                add.Enabled = true;
                update.Enabled = true;
                delete.Enabled = true;
                label1.Visible = true;
                button6.Enabled = true;
                if (!editMode)
                {
                    Object[] objects = { id, dateTimePicker1.Value, Convert.ToInt32(comboBox1.SelectedValue) };
                    string[] param = { "@id", "@datetime", "@seller" };
                    option.CRUD(dataGridView1, "sp_UpdatePurchase", param, objects, "sp_GetPurchase");
                    dataGridView1.Columns["Seller_ID"].Visible = false;
                }
                else
                {
                    Object[] objects =
                    {
                        dataGridView1.CurrentRow.Cells[0].Value, dateTimePicker1.Value,
                        Convert.ToInt32(comboBox1.SelectedValue)
                    };
                    string[] param = { "@id", "@datetime", "@seller" };
                    option.CRUD(dataGridView1, "sp_UpdatePurchase", param, objects, "sp_GetPurchase");
                    dataGridView1.Columns["Seller_ID"].Visible = false;
                }
            }
            else
            {
                MessageBox.Show("Проверьте корректность введенных данных.");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (id != 0)
            {
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    string[] parameters2 = { "@purchase", "@product" };
                    Object[] objects2 = { dataGridView2.Rows[i].Cells[0].Value, dataGridView2.Rows[i].Cells[1].Value };
                    option.SoftDeleteMN(id, "sp_DeletePurProduct", parameters2, objects2, "sp_GetPurProduct", dataGridView2);
                }
                Object[] objects = { id };
                string[] param = { "@id" };
                option.CRUD(dataGridView1, "sp_DeletePurchase", param, objects, "sp_GetPurchase");
            }


            dataGridView1.Visible = true;
            groupBox1.Visible = false;
            add.Enabled = true;
            update.Enabled = true;
            delete.Enabled = true;
            button6.Enabled = true;
            label1.Visible = true;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Purchase_Load(object sender, EventArgs e)
        {
            option = new Options();
            option.Get(dataGridView1, "sp_GetPurchase");
            //dataGridView1.Columns["ID_Purchase"].Visible = false;
            dataGridView1.Columns["Seller_ID"].Visible = false;
            option.GetCombobox(comboBox1, "sp_GetSellers", "ID_Seller", "ФИО");
            comboBox1.SelectedIndex = -1;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy hh:mm";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            transition = true;
            var seller = new User
            {
                Owner = this
            };
            seller.ShowDialog();
            option.GetCombobox(comboBox1, "sp_GetSellers", "ID_Seller", "ФИО");
            comboBox1.SelectedValue = ID_Seller;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (!editMode)
            {
                transitionMN = true;
                errorMN = false;
                var product = new Product
                {
                    Owner = this
                };
                product.ShowDialog();
                //try
                //{
                    if (!errorMN)
                    {
                        Object[] objects = { ID_Product, id, Kolvo };
                        string[] param = { "@product", "@purchase", "@kolvo" };
                        option.InsertMN(id, "sp_InsertPurProduct", param, objects, "sp_GetPurProduct", dataGridView2);

                        Object[] objects2 = { dgwProductId, ostatok };
                        string[] param2 = { "@id", "@kolvo" };
                        option.CRUD(dataGridView1, "sp_UpdateProductMN", param2, objects2, "sp_GetProducts");

                        

                       
                        string expression = "GetSum";
                        SqlCommand cmd;
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            cmd = new SqlCommand(expression, connection) { CommandType = CommandType.StoredProcedure };
                            id2 = option.GetA_I(id2, "GetAI");
                          
                            var param3 = new SqlParameter()
                            {
                                ParameterName = "id",
                                Value = id2
                            };
                            cmd.Parameters.Add(param3);
                            label5.Text = "Сумма: " + cmd.ExecuteScalar().ToString() + " руб.";
                            connection.Close();
                        }
                    }
                    else
                    {
                        option.GetMN(dataGridView1.CurrentRow.Cells[0].Value, "sp_GetPurProduct", dataGridView2);
                    }

                //}
                //catch
                //{

                //}

            }
            else
            {
                transitionMN = true;
                errorMN = false;
                var product = new Product
                {
                    Owner = this
                };
                product.ShowDialog();
                try
                {
                    if (!errorMN)
                    {
                        Object[] objects = { ID_Product, currentRow, Kolvo };
                        string[] param = { "@product", "@purchase", "@kolvo" };
                        option.InsertMN(currentRow, "sp_InsertPurProduct", param, objects,
                            "sp_GetPurProduct", dataGridView2);

                        Object[] objects2 = { dgwProductId, ostatok };
                        string[] param2 = { "@id", "@kolvo" };
                        option.CRUD(dataGridView1, "sp_UpdateProductMN", param2, objects2, "sp_GetProducts");


                        string expression = "GetSum";
                        SqlCommand cmd;
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            cmd = new SqlCommand(expression, connection) { CommandType = CommandType.StoredProcedure };
                            var param3 = new SqlParameter()
                            {
                                ParameterName = "id",
                                Value = currentRow
                            };
                            cmd.Parameters.Add(param3);
                            label5.Text = "Сумма: " + cmd.ExecuteScalar().ToString() + " руб.";
                            connection.Close();

                        }
                    }
                    else
                    {
                        option.GetMN(dataGridView1.CurrentRow.Cells[0].Value, "sp_GetPurProduct", dataGridView2);
                    }

                }
                catch
                {

                }

            }
           

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dataGridView2.CurrentRow.Cells[3].Value.ToString());
            try
            {

          
            if (!editMode)
            {
                int res = ostatok + Convert.ToInt32(dataGridView2.CurrentRow.Cells[3].Value);
                Object[] objects2 = { dgwProductId, res };
                string[] param2 = { "@id", "@kolvo" };
                option.CRUD(dataGridView1, "sp_UpdateProductMN", param2, objects2, "sp_GetProducts");
            }
           
           

            string[] parameters = { "@purchase", "@product" };
            Object[] objects = { dataGridView2.CurrentRow.Cells[0].Value, dataGridView2.CurrentRow.Cells[1].Value };
            option.SoftDeleteMN(dataGridView2.CurrentRow.Cells[0].Value, "sp_DeletePurProduct", parameters, objects, "sp_GetPurProduct", dataGridView2);

            if (!editMode)
            {
                string expression = "GetSum";
                SqlCommand cmd;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    cmd = new SqlCommand(expression, connection) { CommandType = CommandType.StoredProcedure };
                    id2 = option.GetA_I(id2, "GetAI");
                    var param3 = new SqlParameter()
                    {
                        ParameterName = "id",
                        Value = id2
                    };
                    cmd.Parameters.Add(param3);
                    label5.Text = "Сумма: " + cmd.ExecuteScalar().ToString() + " руб.";
                    connection.Close();
                }
            }
            else
            {
                string expression = "GetSum";
                SqlCommand cmd;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    cmd = new SqlCommand(expression, connection) { CommandType = CommandType.StoredProcedure };
                    var param2 = new SqlParameter()
                    {
                        ParameterName = "id",
                        Value = currentRow
                    };
                    cmd.Parameters.Add(param2);
                    label5.Text = "Сумма: " + cmd.ExecuteScalar().ToString() + " руб.";
                    connection.Close();

                }
            }

            }
            catch
            {

            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            option.GetMN(dataGridView1.CurrentRow.Cells[0].Value, "sp_GetPurProduct", dataGridView2);
            dataGridView2.Columns["Product_ID"].Visible = false;
            dataGridView2.Columns["Код покупки"].Visible = false;
            var fieldValues = new Dictionary<string, string> {
                {"ДатаВремя", dataGridView1.CurrentRow.Cells[1].Value.ToString()},
                {"Номер чека", dataGridView1.CurrentRow.Cells[0].Value.ToString() },
                {"Имя продавца", dataGridView1.CurrentRow.Cells[2].Value.ToString() },
                {"Итог", dataGridView1.CurrentRow.Cells[3].Value.ToString() },
            };
            var engine = new Engine();
            engine.Merge("Шаблон.docx", fieldValues, "output.docx");
            // путь к документу
            string pathDocument = AppDomain.CurrentDomain.BaseDirectory + "output.docx";

            // создаём документ
            DocX document = DocX.Load(pathDocument);

            // создаём таблицу с 1 строкой и 3 столбцами
            Table table = document.AddTable(1, 3);
            // располагаем таблицу по центру
            table.Alignment = Alignment.center;
            // меняем стандартный дизайн таблицы
            table.Design = TableDesign.TableGrid;
            table.Rows[0].Cells[0].Paragraphs[0].Append("Продукт");
            table.Rows[0].Cells[1].Paragraphs[0].Append("Количество");
            table.Rows[0].Cells[2].Paragraphs[0].Append("Цена за шт. (Руб.)");
            // заполнение ячейки текстом
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                table.InsertRow();
                table.Rows[i + 1].Cells[0].Paragraphs[0].Append(dataGridView2.Rows[i].Cells[2].Value.ToString());
                table.Rows[i + 1].Cells[1].Paragraphs[0].Append(dataGridView2.Rows[i].Cells[3].Value.ToString());
                table.Rows[i + 1].Cells[2].Paragraphs[0].Append(dataGridView2.Rows[i].Cells[4].Value.ToString());

            }
            document.InsertParagraph().InsertTableAfterSelf(table);
            // сохраняем документ
            document.Save();



        }
    }
}
