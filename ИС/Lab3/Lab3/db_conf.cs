using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    class db_conf
    {
        string connectionString =
            @"Data Source=DESKTOP-FPQ69BF\SQLSERVER;Initial Catalog=BookRegister;Integrated Security=True";

        private DataSet ds;
        private SqlDataAdapter adapter;
        private SqlCommand cmd;
        private static int ID_Client;
        public void Get(DataGridView table, string expression)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(expression, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                table.DataSource = ds.Tables[0];
            }

        }

        public void GetCombobox(ComboBox box, string expression, string valueMember, string displayMember)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(expression, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                box.DataSource = ds.Tables[0];
                box.ValueMember = valueMember;
                box.DisplayMember = displayMember;
            }
        }

        public void CRUD(DataGridView table, string expression, Dictionary<string,object> paramDictionary, string getExpression)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                cmd = new SqlCommand(expression, connection) { CommandType = CommandType.StoredProcedure };
                SqlParameter[] param = new SqlParameter[paramDictionary.Count];
                int i = 0;
                foreach (var parameters in paramDictionary)
                {
                    param[i] = new SqlParameter()
                    {
                        ParameterName = parameters.Key,
                        Value = parameters.Value
                    };
                    cmd.Parameters.Add(param[i]);
                    i++;
                }
                cmd.ExecuteNonQuery();
                connection.Close();
                Get(table, getExpression);
            }
        }
        public void GetMN(Object id, string expression, DataGridView tableOwner)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand(expression, connection);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adapter.SelectCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    ds = new DataSet();
                    adapter.Fill(ds);
                    tableOwner.DataSource = ds.Tables[0];
                }
            }
        }
        public void CRUD_MN(Object id, DataGridView tableOwner, string expression, Dictionary<string, object> paramDictionary, string getExpression )
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                cmd = new SqlCommand(expression, connection) { CommandType = CommandType.StoredProcedure };
                SqlParameter[] param = new SqlParameter[paramDictionary.Count];
                int i = 0;
                foreach (var parameters in paramDictionary)
                {
                    param[i] = new SqlParameter()
                    {
                        ParameterName = parameters.Key,
                        Value = parameters.Value
                    };
                    cmd.Parameters.Add(param[i]);
                    i++;
                }
                cmd.ExecuteNonQuery();
                connection.Close();

                GetMN(id, getExpression, tableOwner);
            }
        }
        public decimal GetA_I(decimal id, string expression)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                cmd = new SqlCommand(expression, connection);
                connection.Open();
                id = (decimal)cmd.ExecuteScalar();
                connection.Close();
                return id;
            }
        }
        public void GetSearch(Object name, string expression, DataGridView tableOwner)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand(expression, connection);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adapter.SelectCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                    ds = new DataSet();
                    adapter.Fill(ds);
                    tableOwner.DataSource = ds.Tables[0];
                }
            }
        }
        public void GetFullSearch(Object[] objects, string expression, DataGridView tableOwner)
        {
            
            using (var connection = new SqlConnection(connectionString))
            {
                using (adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand(expression, connection);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adapter.SelectCommand.Parameters.Add("@name", SqlDbType.VarChar).Value =
                        (string) objects[0] != "" ? objects[0] : "";

                    adapter.SelectCommand.Parameters.Add("@publish", SqlDbType.VarChar).Value = objects[1] ;

                    adapter.SelectCommand.Parameters.Add("@genre", SqlDbType.VarChar).Value =  objects[2] ;

                    adapter.SelectCommand.Parameters.Add("@typebook", SqlDbType.VarChar).Value = objects[3] ;

                    adapter.SelectCommand.Parameters.Add("@udk", SqlDbType.VarChar).Value = objects[4] ;

                    adapter.SelectCommand.Parameters.Add("@year", SqlDbType.VarChar).Value = objects[5];

                    ds = new DataSet();
                    adapter.Fill(ds);
                    tableOwner.DataSource = ds.Tables[0];
                }
            }
        }
        public string[] GetAuth(string login,string password, string expression)
        {
            var result = new string[6];
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand(expression, connection);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                  
                    adapter.SelectCommand.Parameters.Add("@login", SqlDbType.VarChar).Value = login;

                    adapter.SelectCommand.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
                    SqlDataReader reader = adapter.SelectCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        result[0] = reader[0].ToString();
                        result[1] = reader[1].ToString();
                        result[2] = reader[2].ToString();
                        result[3] = reader[3].ToString();
                        result[4] = reader[4].ToString();
                    }
                    result[5] = result[1] != null ? "Успех" : "Провал";
                    connection.Close();
                    ID_Client = Convert.ToInt32(result[0]);
                    return result;
                }
            }
        }
        public string[] CheckAuth(string login,string expression)
        {
            var result = new string[6];
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand(expression, connection);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                    adapter.SelectCommand.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
                    SqlDataReader reader = adapter.SelectCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        result[0] = reader[0].ToString();
                        result[1] = reader[1].ToString();
                        result[2] = reader[2].ToString();
                        result[3] = reader[3].ToString();
                        result[4] = reader[4].ToString();
                    }
                    result[5] = result[1] != null ? "Успех" : "Провал";
                    connection.Close();
                    ID_Client = Convert.ToInt32(result[0]);
                    return result;
                }
            }
        }
        public void InsertLog(string expression, Dictionary<string, object> paramDictionary)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                cmd = new SqlCommand(expression, connection) { CommandType = CommandType.StoredProcedure };
                SqlParameter[] param = new SqlParameter[paramDictionary.Count+1];
                int i = 0;
                param[i] = new SqlParameter()
                {
                    ParameterName = "@client",
                    Value = ID_Client
                };
                cmd.Parameters.Add(param[i]);
                i++;
                foreach (var parameters in paramDictionary)
                {
                    param[i] = new SqlParameter()
                    {
                        ParameterName = parameters.Key,
                        Value = parameters.Value
                    };
                    cmd.Parameters.Add(param[i]);
                    i++;
                }
                
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
