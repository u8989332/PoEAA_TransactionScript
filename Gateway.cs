using CodeParadise.Money;
using System;
using System.Data;
using System.Data.SQLite;

namespace PoEAA_TransactionScript
{
    public class Gateway
    {
        private const string FindRecognitionsStatement =
            @"
                SELECT Amount FROM RevenueRecognitions 
                WHERE Contract = $contractId AND RecognizedOn <= $beforeDate
            ";

        private const string FindContractStatement =
            @"
                SELECT * FROM Contracts c, Products p 
                WHERE c.Id = $contractId AND c.product = p.Id
            ";

        private const string InsertRecognitionsStatement =
            @"
                INSERT INTO RevenueRecognitions VALUES ($contractId, $amount, $recognizedOn)
            ";

        public DataTable FindRecognitionsFor(int contractId, DateTime beforeDate)
        {
            var result = new DataTable();
            using var connection = DbManager.CreateConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = FindRecognitionsStatement;
            command.Parameters.AddWithValue("$contractId", contractId);
            command.Parameters.AddWithValue("$beforeDate", beforeDate);

            using(var sqlDataAdapter = new SQLiteDataAdapter(command))
            {
                 sqlDataAdapter.Fill(result);
            }
           
            return result;
        }

        public DataTable FindContract(int contractId)
        {
            var result = new DataTable();
            using var connection = DbManager.CreateConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = FindContractStatement;
            command.Parameters.AddWithValue("$contractId", contractId);

            using(var sqlDataAdapter = new SQLiteDataAdapter(command))
            {
                 sqlDataAdapter.Fill(result);
            }
           
            return result;
        }

        public void InsertRecognitions(int contractId, Money amount, DateTime recognizedOn)
        {
            using var connection = DbManager.CreateConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = InsertRecognitionsStatement;
            command.Parameters.AddWithValue("$contractId", contractId);
            command.Parameters.AddWithValue("$amount", amount.Amount);
            command.Parameters.AddWithValue("$recognizedOn", recognizedOn);
            command.ExecuteNonQuery();
        }
    }
}