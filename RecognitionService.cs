using CodeParadise.Money;
using System;

namespace PoEAA_TransactionScript
{
    class RecognitionService
    {
        public Money RecognizedRevenue(int contractNumber, DateTime beforeDate)
        {
            Money result = Money.Dollars(0m);
            Gateway db = new Gateway();
            var dt = db.FindRecognitionsFor(contractNumber, beforeDate);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                var amount = (decimal) dt.Rows[i]["Amount"];
                result += Money.Dollars(amount);
            }

            return result;
        }

        public void CalculateRevenueRecognitions(int contractId)
        {
            Gateway db = new Gateway();
            var contracts = db.FindContract(contractId);
            Money totalRevenue = Money.Dollars((decimal) contracts.Rows[0]["Revenue"]);
            DateTime recognitionDate = (DateTime) contracts.Rows[0]["DateSigned"];
            string type = contracts.Rows[0]["Type"].ToString();

            if(type == "S")
            {
                Money[] allocation = totalRevenue.Allocate(3);
                db.InsertRecognitions(contractId, allocation[0], recognitionDate);
                db.InsertRecognitions(contractId, allocation[1], recognitionDate.AddDays(60));
                db.InsertRecognitions(contractId, allocation[2], recognitionDate.AddDays(90));
            }
            else if(type == "W")
            {
                db.InsertRecognitions(contractId, totalRevenue, recognitionDate);
            }
            else if(type == "D")
            {
                Money[] allocation = totalRevenue.Allocate(3);
                db.InsertRecognitions(contractId, allocation[0], recognitionDate);
                db.InsertRecognitions(contractId, allocation[1], recognitionDate.AddDays(30));
                db.InsertRecognitions(contractId, allocation[2], recognitionDate.AddDays(60));
            }
        }
    }
}
