using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductShop
{
    class Options
    {
        string connectionString =
            @"Data Source=DESKTOP-FPQ69BF\SQLSERVER;Initial Catalog=ProductShop;Integrated Security=True";

        private DataSet ds;
        private SqlDataAdapter adapter;
        private SqlCommand cmd;


        public decimal GetA_I(decimal id, string expression)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                cmd = new SqlCommand(expression, connection);
                connection.Open();
                id = (decimal) cmd.ExecuteScalar();
                connection.Close();
                return id;
            }
        }
        public void Get(DataGridView table, string expression)
        {
            //string expression = "sp_GetSellers";
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

        public void Delete(DataGridView table, string expression, string getExpression)
        {
            //const string expression = "sp_DeleteProduct";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                cmd = new SqlCommand(expression, connection) {CommandType = CommandType.StoredProcedure};
                var idParameter = new SqlParameter()
                {
                    ParameterName = "@id",
                    Value = table.CurrentRow.Cells[0].Value
                };
                cmd.Parameters.Add(idParameter);
                cmd.ExecuteNonQuery();
                connection.Close();
                Get(table, getExpression);
                //GetProducts();
            }
        }

        public void GetMN(Object id, string expression, DataGridView tableOwner)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand(expression,connection);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adapter.SelectCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    ds = new DataSet();
                    adapter.Fill(ds);
                    tableOwner.DataSource = ds.Tables[0];
                }
            }
        }

        public void SoftDeleteMN(Object id, string expression,
            string[] parameters,Object[] values, string getExpression, DataGridView tableOwner)
        {
            //const string expression = "sp_UpdateUnit";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                cmd = new SqlCommand(expression, connection) { CommandType = CommandType.StoredProcedure };
                SqlParameter[] param = new SqlParameter[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    param[i] = new SqlParameter()
                    {
                        ParameterName = parameters[i],
                        Value = values[i]
                    };
                    cmd.Parameters.Add(param[i]);
                }
                cmd.ExecuteNonQuery();
                connection.Close();
                GetMN(id, getExpression, tableOwner);
            }
        }

        public void CRUD(DataGridView table, string expression, string[] parameters, Object[] values,string getExpression)
        {
            //const string expression = "sp_InsertProduct";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                cmd = new SqlCommand(expression, connection) { CommandType = CommandType.StoredProcedure };
                SqlParameter[] param = new SqlParameter[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    param[i] = new SqlParameter()
                    {
                        ParameterName = parameters[i],
                        Value = values[i]
                    };
                    cmd.Parameters.Add(param[i]);
                }
                cmd.ExecuteNonQuery();
                connection.Close();

                Get(table, getExpression);
            }
        }

        public void InsertMN(Object id, string expression, string[] parameters, Object[] values, string getExpression, DataGridView tableOwner)
        {
            //const string expression = "sp_InsertProduct";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                cmd = new SqlCommand(expression, connection) { CommandType = CommandType.StoredProcedure };
                SqlParameter[] param = new SqlParameter[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    param[i] = new SqlParameter()
                    {
                        ParameterName = parameters[i],
                        Value = values[i]
                    };
                    cmd.Parameters.Add(param[i]);
                }
                cmd.ExecuteNonQuery();
                connection.Close();

                GetMN(id, getExpression, tableOwner);
            }
        }
    }
}
