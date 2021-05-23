using System;

namespace PoEAA_TransactionScript
{
    class Program
    {
        
        static void Main(string[] args)
        {
            using (var connection = DbManager.CreateConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    DROP TABLE IF EXISTS Products;
                    DROP TABLE IF EXISTS Contracts; 
                    DROP TABLE IF EXISTS RevenueRecognitions;
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE Products (Id int primary key, Name TEXT, Type TEXT);
                    CREATE TABLE Contracts (Id int primary key, Product int, Revenue decimal, DateSigned date);
                    CREATE TABLE RevenueRecognitions (Contract int, Amount decimal, RecognizedOn date, PRIMARY KEY(Contract, RecognizedOn));
                ";
                command.ExecuteNonQuery();

                command.CommandText =
                @"
                   
                INSERT INTO Products
                    VALUES (1, 'Code Paradise Database', 'D');

                INSERT INTO Products
                    VALUES (2, 'Code Paradise Spreadsheet', 'S');

                INSERT INTO Products
                    VALUES (3, 'Code Paradise Word Processor', 'W');

                INSERT INTO Contracts
                    VALUES (1, 1, 9999, date('2020-01-01'));

                INSERT INTO Contracts
                    VALUES (2, 2, 1000, date('2020-03-15'));

                INSERT INTO Contracts
                    VALUES (3, 3, 24000, date('2020-07-25'));
                ";
                command.ExecuteNonQuery();
            }

            RecognitionService service = new RecognitionService();

            // database product
            service.CalculateRevenueRecognitions(1);
            var databaseRevenue = service.RecognizedRevenue(1, new System.DateTime(2020, 1, 25));
            Console.WriteLine($"database revenue before 2020-01-25 = {databaseRevenue.Amount}");

            // spreadsheet product
            service.CalculateRevenueRecognitions(2);
            var spreadsheetRevenue = service.RecognizedRevenue(2, new System.DateTime(2020, 6, 1));
            Console.WriteLine($"spreadsheet revenue before 2020-06-01 = {spreadsheetRevenue.Amount}");

             // word processor product
            service.CalculateRevenueRecognitions(3);
            var wordProcessorRevenue = service.RecognizedRevenue(3, new System.DateTime(2020, 9, 30));
            Console.WriteLine($"word processor revenue before 2020-09-30 = {wordProcessorRevenue.Amount}");
        }
    }
}
