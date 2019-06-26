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

namespace OtdelKadrov
{
    public partial class Dolznost : Form
    {
        private bool editMode = false;
        private DataSet ds;
        private SqlDataAdapter adapter;
        private SqlCommandBuilder commandBuilder;
        private string connectionString =
            @"Data Source=DESKTOP-FPQ69BF\SQLSERVER;Initial Catalog=OtdelKadrov;Integrated Security=True";
        private string sql = "SELECT ID_Dolznost AS 'Код должности', Name AS 'Название', Oklad AS 'Оклад (руб.)' FROM Dolznost";

        //CAST(Oklad AS DEC(12,2))
        private bool anyForm = false;
        public Dolznost()
        {
            InitializeComponent();
        }

        private void UpdateDS()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(sql, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            editMode = false;
            groupBox1.Text = "Добавление";
            groupBox1.Visible = true;
            dataGridView1.Visible = false;
            label1.Visible = false;
            add.Enabled = false;
            update.Enabled = false;
            delete.Enabled = false;
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void confirm_Click(object sender, EventArgs e)
        {
           
            if (decimal.TryParse(textBox2.Text, result: out var okl) && textBox1.Text != "")
            {
                groupBox1.Visible = false;
                dataGridView1.Visible = true;
                label1.Visible = true;
                add.Enabled = true;
                update.Enabled = true;
                delete.Enabled = true;
                if (!editMode)
                {
                    //if (decimal.TryParse(textBox2.Text, result: out var okl) && textBox1.Text != "")
                    //{
                        var insertSql = $"INSERT INTO Dolznost (Name, Oklad) VALUES ('{textBox1.Text}', (@oklad))";
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            var cmd = new SqlCommand(insertSql, connection);
                            cmd.Parameters.AddWithValue("@oklad", okl);
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }

                        UpdateDS();
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Проверьте корректность введенных данных");
                    //}
                }
                else
                {

                    var updateSql =
                        $"UPDATE Dolznost SET Name='{textBox1.Text}', Oklad=(@oklad) WHERE ID_Dolznost='{dataGridView1.CurrentRow.Cells[0].Value}'";
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var cmd = new SqlCommand(updateSql, connection);
                        cmd.Parameters.AddWithValue("@oklad", okl);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }

                    UpdateDS();

                }
            }
            else
                {
                    MessageBox.Show("Проверьте корректность введенных данных");
                }
            }

        private void cancel_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            dataGridView1.Visible = true;
            label1.Visible = true;
            add.Enabled = true;
            update.Enabled = true;
            delete.Enabled = true;
        }

        private void update_Click(object sender, EventArgs e)
        {
            editMode = true;
            groupBox1.Text = "Редактирование";
            groupBox1.Visible = true;
            dataGridView1.Visible = false;
            label1.Visible = false;
            add.Enabled = false;
            update.Enabled = false;
            delete.Enabled = false;

            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        }

        private void Dolznost_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (groupBox1.Visible != true) return;
            MessageBox.Show("Закончите работу с данными");
            e.Cancel = true;
        }

        private void Dolznost_Load(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(sql, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns["Код должности"].Visible = false;
            }

        }

        private void delete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
                using (var connection = new SqlConnection(connectionString))
                {
                    adapter = new SqlDataAdapter(sql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.Update(ds);
                    ds.Clear();
                    adapter.Fill(ds);
                }
            }
            catch
            {
                MessageBox.Show("Данные по должности связаны с 1 или несколькими сотрудниками. Удаление невозможно");
                using (var connection = new SqlConnection(connectionString))
                {
                    adapter = new SqlDataAdapter(sql, connection);
                    ds.Clear();
                    adapter.Fill(ds);
                }
            }

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Owner is Sotrudnik sotr)
            {
                anyForm = sotr.transition;
                if (dataGridView1.CurrentRow != null && anyForm)
                {
                    sotr.ID_Dolznost = Convert.ToInt32(value: dataGridView1.CurrentRow.Cells[0].Value);
                    Close();
                }

                //sotr.comboBox1.SelectedIndex = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value) - 1;
            }
            //var test = new Options();
            //var formSotr = new Sotrudnik();

        }
    }
}
