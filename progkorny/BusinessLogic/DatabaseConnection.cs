using System.Data.OleDb;

namespace progkorny
{
    public class DatabaseConnection
    {
        /// <summary>
        /// Felépíti a kapcsolatot az adatbázissal
        /// </summary>
        /// <returns>OleDbConnection objektum</returns>
        public OleDbConnection OpenConnection()
        {
            OleDbConnection connection = new OleDbConnection();

            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Balint\source\repos\progkorny_master_push\progkorny\todo.accdb;Persist Security Info=True";

            connection.ConnectionString = connString;

            return connection;
        }
    }
}
