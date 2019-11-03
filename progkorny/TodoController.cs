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
        private static OleDbConnection dbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Balint\source\repos\progkorny_beadando-master\progkorny\todo.accdb;Persist Security Info=True");
        private static OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
        private static OleDbCommand command;

        /// <summary>
        /// Betölti az összes Todo-t az adatbázisból dátum szerinti csökkenő sorrendben
        /// </summary>
        /// <param name="dt">DataTable</param>
        public static void LoadTodos(DataTable dt)
        {
            try
            {
                dbConn.Open();
                string query = "SELECT * FROM [todos] ORDER BY todo_created_at DESC, todo_priority DESC";
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

        /// <summary>
        /// Hozzáadja az adatbázishoz az új Todo-t, aminek adatai a paraméterekben érkeznek
        /// </summary>
        /// <param name="title">Todo Title</param>
        /// <param name="body">Todo Body</param>
        /// <param name="author">Todo Author</param>
        /// <param name="priority">Todo Priority</param>
        /// <param name="created_at">Todo Created at</param>
        /// <returns>int: -1 - Failed, 1 - Success</returns>
        public static int InsertTodo(string title, string body, string author, string created_at, string priority)
        {
            if(!TodoHelper.IsEmptyOrNull(title, body, author, created_at, priority))
            {
                try
                {
                    dbConn.Open();
                    string query = "INSERT INTO [todos] (todo_title, todo_body, todo_author, todo_created_at, todo_priority) VALUES('" + title + "', '" + body + "','" + author + "', '" + created_at + "','" + priority + "')";
                    command = new OleDbCommand(query, dbConn);
                    dataAdapter.InsertCommand = command;
                    int result = dataAdapter.InsertCommand.ExecuteNonQuery();
                    dbConn.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba: " + ex.Message);
                    return -1;
                }
            }
            else
            {
                MessageBox.Show("Tölts ki minden mezőt!");
                return -1;
            }
        }

        /// <summary>
        /// Frissíti a paraméterben kapott ID-jű Todo-t
        /// </summary>
        /// <param name="id">Todo ID</param>
        /// <param name="title">Todo Title</param>
        /// <param name="body">Todo Body</param>
        /// <param name="author">Todo Author</param>
        /// <param name="created_at">Todo Created at</param>
        /// <param name="priority">Todo Priority</param>
        /// <returns>int: -1 - Failed, 1 - Success</returns>
        public static int UpdateTodo(string id, string title, string body, string author, string created_at, string priority)
        {
            if (!TodoHelper.IsEmptyOrNull(id, title, body, author, created_at, priority))
            {
                try
                {
                    dbConn.Open();
                    string query = "UPDATE [todos] SET todo_title = '"+title+"', todo_body = '"+body+"', todo_author = '"+author+ "', todo_created_at = #" + created_at + "#, todo_priority = '"+ priority +"' WHERE todo_id = " + id;
                    command = new OleDbCommand(query, dbConn);
                    dataAdapter.UpdateCommand = command;
                    int result = dataAdapter.UpdateCommand.ExecuteNonQuery();
                    dbConn.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt: " + ex.Message);
                    return -1;
                }
            }
            else
            {
                MessageBox.Show("Tölts ki minden mezőt!");
                return -1;
            }
        }

        /// <summary>
        /// Törli a paraméterben kapott ID-jű Todo-t az adatbázisból
        /// </summary>
        /// <param name="id">Todo ID</param>
        /// <returns>int: -1 - Failed, 1 - Success</returns>
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
                    int result = dataAdapter.DeleteCommand.ExecuteNonQuery();
                    dbConn.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt: " + ex.Message);
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Betölti az összes létező Todo dátumát egy listába, amit a naptárnál tudunk betölteni
        /// </summary>
        /// <returns>List<string> dátumok</returns>
        public static List<string> LoadDates()
        {
            List<string> dates = new List<string>();
            try
            {
                dbConn.Open();
                using (command = new OleDbCommand("SELECT todo_created_at FROM [todos]", dbConn))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime date = reader.GetDateTime(0);
                            string convertedDate = date.ToString("yyyy-MM-dd");
                            dates.Add(convertedDate);
                        }
                    }
                }
                dbConn.Close();
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
