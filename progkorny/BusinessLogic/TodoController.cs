using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Windows;

namespace progkorny
{
    public class TodoController : DatabaseConnection
    {       
        private OleDbDataAdapter dataAdapter;

        /// <summary>
        /// Betölti az összes Todo-t az adatbázisból dátum és sürgősség szerinti csökkenő sorrendben
        /// </summary>
        /// <param name="dt">Feltöltendő DataTable</param>
        public void LoadTodos(DataTable dt)
        {
            OleDbConnection dbConn = OpenConnection();
            try
            {
                dbConn.Open();
                string query = "SELECT * FROM [todos] ORDER BY todo_deadline DESC, todo_priority DESC";
                OleDbCommand command = new OleDbCommand(query, dbConn);

                dataAdapter.SelectCommand = command;
                dataAdapter.SelectCommand.ExecuteNonQuery();
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt a betöltéskor: " + ex.Message);
            }
            finally
            {
                dbConn.Close();
            }
        }

        /// <summary>
        /// Hozzáadja az adatbázishoz az új Todo-t, aminek adatai a paraméterekben érkeznek
        /// </summary>
        /// <param name="insertTodo">Todo objektum</param>
        /// <returns>int: -1 - Failed, 1 - Success</returns>
        public int InsertTodo(Todo insertTodo)
        {
            OleDbConnection dbConn = OpenConnection();
            try
            {
                dbConn.Open();
                string query = "INSERT INTO [todos] (todo_title, todo_body, todo_author, todo_deadline, todo_priority) VALUES('" + insertTodo.Todo_Title + "', '" + insertTodo.Todo_Body + "','" + insertTodo.Todo_Author + "', '" + insertTodo.Todo_Deadline + "','" + insertTodo.Todo_Priority.ToString().ToLower() + "')";

                OleDbCommand command = new OleDbCommand(query, dbConn);

                dataAdapter.InsertCommand = command;
                int result = dataAdapter.InsertCommand.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt az adatfelvitelkor: " + ex.Message);
                return -1;
            }
            finally
            {
                dbConn.Close();
            }
        }

        /// <summary>
        /// Frissíti a paraméterben kapott ID-jű Todo-t
        /// </summary>
        /// <param name="updateTodo">Todo objektum</param>
        /// <returns>int: -1 - Failed, 1 - Success</returns>
        public int UpdateTodo(Todo updateTodo)
        {
            OleDbConnection dbConn = OpenConnection();
            try
            {
                dbConn.Open();
                string query = "UPDATE [todos] SET todo_title = '" + updateTodo.Todo_Title + "', todo_body = '" + updateTodo.Todo_Body + "', todo_author = '" + updateTodo.Todo_Author + "', todo_deadline = #" + updateTodo.Todo_Deadline + "#, todo_priority = '" + updateTodo.Todo_Priority.ToString().ToLower() + "' WHERE todo_id = " + updateTodo.Todo_ID;
                OleDbCommand command = new OleDbCommand(query, dbConn);

                dataAdapter.UpdateCommand = command;
                int result = dataAdapter.UpdateCommand.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt a frissítéskor: " + ex.Message);
                return -1;
            }
            finally
            {
                dbConn.Close();
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
                OleDbConnection dbConn = OpenConnection();
                try
                {
                    dbConn.Open();
                    string query = "DELETE FROM [todos] WHERE todo_id = " + id;
                    OleDbCommand command = new OleDbCommand(query, dbConn);
                    
                    dataAdapter.DeleteCommand = command;
                    int result = dataAdapter.DeleteCommand.ExecuteNonQuery();

                    return result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt a törléskor: " + ex.Message);
                    return -1;
                }
                finally
                {
                    dbConn.Close();
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Betölti az összes létező Todo dátumát egy listába, amit a naptárnál tölt be
        /// </summary>
        /// <returns>List<string> dátumok listája</returns>
        public List<string> LoadDates()
        {
            List<string> dates = new List<string>();

            OleDbConnection dbConn = OpenConnection();
            try
            {
                dbConn.Open();
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
                MessageBox.Show("Hiba történt a dátumok betöltésekor: " + ex.Message);
                return dates;
            }
            finally
            {
                dbConn.Close();
            }
        }

        public TodoController()
        {
            dataAdapter = new OleDbDataAdapter();
        }
    }
}
