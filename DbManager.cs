using System.Data.SQLite;

namespace PoEAA_TransactionScript
{
    public static class DbManager
    {
        public static SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection("Data Source=poeaa_transactionscript.db");
        }
    }
}