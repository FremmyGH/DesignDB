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

namespace Auth_Lab5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        private DataSet ds;
        private SqlDataAdapter adapter;
        private SqlCommand cmd;
        public List<string> list;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                var login = textBox1.Text;
                var password = textBox2.Text;
                string connectionString =
                    $@"Data Source=DESKTOP-FPQ69BF\SQLSERVER;Initial Catalog=Auth_lab5;User ID={login};Password={password}";
                string expression = "sp_GetUser";
                string parameter = "@name";
                try
                {
                    list = Get(connectionString, expression, parameter, login);
                    var main = new Main
                    {
                        Owner = this
                    };
                    main.ShowDialog();
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.Message);
                }
                
            }
           
        }

        List<string> Get(string connectionString, string expression, string parameter, string value)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                List<string> list =new List<string>();
                connection.Open();
                cmd = new SqlCommand(expression, connection) { CommandType = CommandType.StoredProcedure };
                SqlParameter param = new SqlParameter();
                    param = new SqlParameter()
                    {
                        ParameterName = parameter,
                        Value = value
                    };
                    cmd.Parameters.Add(param);
                var dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        list.Add(dataReader.GetValue(0).ToString());
                        list.Add(dataReader.GetValue(1).ToString());
                    }
                }
                connection.Close();
                return list;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';

            }
        }
    }
}
