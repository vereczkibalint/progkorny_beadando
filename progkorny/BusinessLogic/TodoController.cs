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
    public class TodoController : DatabaseConnection
    {
        /*private static OleDbConnection dbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Balint\source\repos\progkorny_beadando-master\progkorny\todo.accdb;Persist Security Info=True");
        private static OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
        private static OleDbCommand command;*/

        private OleDbConnection dbConn;
        private OleDbDataAdapter dataAdapter;

        /// <summary>
        /// Betölti az összes Todo-t az adatbázisból dátum szerinti csökkenő sorrendben
        /// </summary>
        /// <param name="dt">DataTable</param>
        public void LoadTodos(DataTable dt)
        {
            try
            {
                string query = "SELECT * FROM [todos] ORDER BY todo_deadline DESC, todo_priority DESC";
                OleDbCommand command = new OleDbCommand(query, dbConn);
                
                dataAdapter.SelectCommand = command;
                dataAdapter.SelectCommand.ExecuteNonQuery();
                dataAdapter.Fill(dt);
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
        public int InsertTodo(string title, string body, string author, string deadline, string priority)
        {
            if(!TodoHelper.IsEmptyOrNull(title, body, author, deadline, priority))
            {
                try
                {
                    string query = "INSERT INTO [todos] (todo_title, todo_body, todo_author, todo_deadline, todo_priority) VALUES('" + title + "', '" + body + "','" + author + "', '" + deadline + "','" + priority + "')";
                    OleDbCommand command = new OleDbCommand(query, dbConn);

                    dataAdapter.InsertCommand = command;
                    int result = dataAdapter.InsertCommand.ExecuteNonQuery();

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
        public int UpdateTodo(string id, string title, string body, string author, string deadline, string priority)
        {
            if (!TodoHelper.IsEmptyOrNull(id, title, body, author, deadline, priority))
            {
                try
                {
                    string query = "UPDATE [todos] SET todo_title = '" + title + "', todo_body = '" + body + "', todo_author = '" + author + "', todo_deadline = #" + deadline + "#, todo_priority = '" + priority + "' WHERE todo_id = " + id;
                    OleDbCommand command = new OleDbCommand(query, dbConn);

                    dataAdapter.UpdateCommand = command;
                    int result = dataAdapter.UpdateCommand.ExecuteNonQuery();

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
        public int DeleteTodo(string id)
        {
            if (!TodoHelper.IsEmptyOrNull(id))
            {
                try
                {
                    string query = "DELETE FROM [todos] WHERE todo_id = " + id;
                    OleDbCommand command = new OleDbCommand(query, dbConn);
                    
                    dataAdapter.DeleteCommand = command;
                    int result = dataAdapter.DeleteCommand.ExecuteNonQuery();

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
        public List<string> LoadDates()
        {
            List<string> dates = new List<string>();
            try
            {
                string query = "SELECT todo_deadline FROM[todos]";

                using (OleDbCommand command = new OleDbCommand(query,dbConn))
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
                return dates;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt: " + ex.Message);
                return dates;
            }
        }

        public TodoController()
        {
            dataAdapter = new OleDbDataAdapter();
            dbConn = OpenConnection();
        }
    }
}
