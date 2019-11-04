using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace progkorny
{
    public class DatabaseConnection
    {
        public OleDbConnection OpenConnection()
        {
            OleDbConnection connection = new OleDbConnection();

            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Balint\source\repos\progkorny_master_push\progkorny\todo.accdb;Persist Security Info=True";

            connection.ConnectionString = connString;

            return connection;
        }
    }
}
