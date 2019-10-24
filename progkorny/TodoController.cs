using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace progkorny
{
    public static class TodoController
    {
        private static OleDbConnection dbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Lenovo\source\repos\progkorny_beadando\progkorny\todo.accdb;Persist Security Info=True");
        private static OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
        private static OleDbCommand command;

        public static void LoadTodos(DataTable dt)
        {
            try
            {
                dbConn.Open();
                string query = "SELECT * FROM [todos]";
                command = new OleDbCommand(query, dbConn);
                dataAdapter.SelectCommand = command;
                dataAdapter.SelectCommand.ExecuteNonQuery();
                dataAdapter.Fill(dt);
                dbConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt: " + ex.Message);
            }
        }

        public static int InsertTodo(string title, string body, string author, string created_at)
        {
            if(!TodoHelper.IsEmptyOrNull(title, body, author, created_at))
            {
                try
                {
                    dbConn.Open();
                    string query = "INSERT INTO [todos] (todo_title, todo_body, todo_author, todo_created_at) VALUES('" + title + "', '" + body + "','" + author + "', '" + created_at + "')";
                    command = new OleDbCommand(query, dbConn);
                    dataAdapter.InsertCommand = command;

                    if (dataAdapter.InsertCommand.ExecuteNonQuery() == 1)
                    {
                        dbConn.Close();
                        return 1;
                    }
                    else
                    {
                        dbConn.Close();
                        return 0;
                    }
                    
                }
                catch (Exception ex)
                {
                    dbConn.Close();
                    MessageBox.Show(ex.Message);
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public static int UpdateTodo(string id, string title, string body, string author, string created_at)
        {
            if (!TodoHelper.IsEmptyOrNull(id, title, body, author, created_at))
            {
                try
                {
                    dbConn.Open();
                    string query = "UPDATE [todos] SET todo_title = '"+title+"', todo_body = '"+body+"', todo_author = '"+author+ "', todo_created_at = #" + created_at + "# WHERE todo_id = " + id;
                    command = new OleDbCommand(query, dbConn);
                    dataAdapter.UpdateCommand = command;

                    if (dataAdapter.UpdateCommand.ExecuteNonQuery() == 1)
                    {
                        dbConn.Close();
                        return 1;
                    }
                    else
                    {
                        dbConn.Close();
                        return 0;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt: " + ex.Message);
                    dbConn.Close();
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public static int DeleteTodo(string id)
        {
            if (!TodoHelper.IsEmptyOrNull(id))
            {
                try
                {
                    dbConn.Open();
                    string query = "DELETE FROM [todos] WHERE todo_id = " +id;
                    command = new OleDbCommand(query, dbConn);
                    dataAdapter.DeleteCommand = command;

                    if (dataAdapter.DeleteCommand.ExecuteNonQuery() == 1)
                    {
                        dbConn.Close();
                        return 1;
                    }
                    else
                    {
                        dbConn.Close();
                        return 0;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt: " + ex.Message);
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public static List<string> LoadDates()
        {
            List<string> dates = new List<string>();
            try
            {
                dbConn.Open();
                string query = "SELECT * FROM [todos]";
                command = new OleDbCommand(query, dbConn);
                dataAdapter.SelectCommand = command;
                int rowsAffected = dataAdapter.SelectCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dates.Add(reader["todo_created_at"].ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("nincs dátum");
                }

                return dates;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt: " + ex.Message);
                return dates;
            }
        }
    }
}
