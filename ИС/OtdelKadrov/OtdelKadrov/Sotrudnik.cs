using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace OtdelKadrov
{
    public partial class Sotrudnik : Form
    {
        private bool editMode = false;
        private DataSet ds;
        private SqlDataAdapter adapter;
        //private SqlCommandBuilder commandBuilder;
        private string connectionString = @"Data Source=DESKTOP-FPQ69BF\SQLSERVER;Initial Catalog=OtdelKadrov;Integrated Security=True";

        private string sql =
            "SELECT ID_Sotrudnik AS 'Код сотрудника', FIO AS 'ФИО', Seria AS 'Серия паспорта' , Nomer AS 'Номер паспорта'," +
            " KemVidan AS 'Кем выдан', Adress AS 'Адрес', Name AS 'Должность', Dolznost_ID FROM Sotrudnik INNER JOIN Dolznost ON Dolznost_ID = ID_Dolznost";

        private string sqlDolznost = "SELECT ID_Dolznost, Name FROM Dolznost";
        public int ID_Dolznost;
        public bool transition;
        public Sotrudnik()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

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
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            comboBox1.SelectedIndex = -1;


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

            if (dataGridView1.CurrentRow == null) return;
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            comboBox1.SelectedValue = Convert.ToInt32(dataGridView1.CurrentRow.Cells[7].Value);
            textBox4.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox6.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        }

        private void confirm_Click(object sender, EventArgs e)
        {
            if (textBox4.Text!="" && 
                textBox3.Text!="" &&
                comboBox1.SelectedValue != null &&
                textBox1.Text != "" &&
                textBox5.Text != "" &&
                textBox6.Text != "")
            {
                groupBox1.Visible = false;
                dataGridView1.Visible = true;
                label1.Visible = true;
                add.Enabled = true;
                update.Enabled = true;
                delete.Enabled = true;
                if (!editMode)
                {
                    var sqlInsert = $"INSERT INTO Sotrudnik (FIO, Seria, Nomer, KemVidan, Adress, Dolznost_ID) VALUES" +
                                    $" ('{textBox1.Text}', '{textBox4.Text}', '{textBox3.Text}'," +
                                    $"'{textBox5.Text}','{textBox6.Text}','{Convert.ToInt32(comboBox1.SelectedValue)}')";
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var command = new SqlCommand(sqlInsert, connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        //adapter = new SqlDataAdapter(sql, connection);
                        //ds = new DataSet();
                        //adapter.Fill(ds);
                        //dataGridView1.DataSource = ds.Tables[0];
                    }

                    UpdateDS();
                }
                else
                {
                    var sqlUpdate =
                        "UPDATE Sotrudnik SET FIO='" + textBox1.Text + "', Seria='" + textBox4.Text + "', Nomer='" +
                        textBox3.Text + "'," + " KemVidan='" + textBox5.Text + "', Adress='" + textBox6.Text + "'," +
                        " Dolznost_ID='" + Convert.ToInt32(comboBox1.SelectedValue) + "' WHERE ID_Sotrudnik = '" +
                        dataGridView1.CurrentRow.Cells[0].Value + "'";

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var command = new SqlCommand(sqlUpdate, connection);
                        command.ExecuteNonQuery();
                        connection.Close();

                        //adapter = new SqlDataAdapter(sql, connection);
                        //ds = new DataSet();
                        //adapter.Fill(ds);
                        //dataGridView1.DataSource = ds.Tables[0];
                    }

                    UpdateDS();
                }
            }
            else
            {
                MessageBox.Show("Проверьте корректность введеных данных");
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

        private void Sotrudnik_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (groupBox1.Visible != true) return;
            MessageBox.Show("Закончите работу с данными");
            e.Cancel = true;
        }

        private void Sotrudnik_Load(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(sqlDolznost, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "ID_Dolznost";
                comboBox1.DisplayMember = "Name";
                comboBox1.SelectedIndex = -1;

                adapter = new SqlDataAdapter(sql, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns["Dolznost_ID"].Visible = false;
                dataGridView1.Columns["Код сотрудника"].Visible = false;
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sqlDelete = "DELETE FROM Sotrudnik WHERE ID_Sotrudnik='" + dataGridView1.CurrentRow.Cells[0].Value + "'";
                var cmd = new SqlCommand(sqlDelete, connection);
                cmd.ExecuteNonQuery();
                connection.Close();

                //adapter = new SqlDataAdapter(sql, connection);
                //ds = new DataSet();
                //adapter.Fill(ds);
                //dataGridView1.DataSource = ds.Tables[0];
            }
            UpdateDS();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            transition = true;
            Form dolznost = new Dolznost();
            dolznost.Owner = this;
            dolznost.ShowDialog();
            //Dolznost dol = this.Owner as Dolznost;
            using (var connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(sqlDolznost, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "ID_Dolznost";
                comboBox1.DisplayMember = "Name";
                comboBox1.SelectedValue = ID_Dolznost;
                //comboBox1.SelectedIndex = -1;
            }
            //comboBox1.Items.Clear();

        }
    }
}
