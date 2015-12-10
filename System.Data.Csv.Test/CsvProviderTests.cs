using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace System.Data.Csv.Test
{
    [TestFixture]
    public class CsvProviderTests
    {
        private const string SmallCsvFile = @"C:\Sample - Superstore.csv";
        private const string LargeCsvFile = @"C:\AWLArge_FactInternetSales_1M.csv";

        private const string ConnectionStringCsvTemplate = "Database={0};Password=;FirstRowIsHeader=True;StorageDir=;ForceStorageReload=True;AnalysisMethod=BestMatch;RowsToAnalyse=50;Delimiter=,;FieldWrapper=\";Escape=\"";

        [Test]
        public void TestExcelConnectionToSmallCsvFile()
        {
            var sw = new Stopwatch();

            sw.Start();

            using (var connection = new CsvConnection(string.Format(ConnectionStringCsvTemplate, SmallCsvFile)))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT 1 FROM `Sample - Superstore` LIMIT 1");
                    var value = cmd.ExecuteScalar();

                    Assert.AreEqual(1, value);
                }

                connection.Close();
            }

            sw.Stop();

            Console.WriteLine("Test finished in {0} ms.", sw.ElapsedMilliseconds);
        }

        [Test]
        public void TestExcelConnectionToLargeFile()
        {
            var sw = new Stopwatch();

            sw.Start();

            using (var connection = new CsvConnection(string.Format(ConnectionStringCsvTemplate, LargeCsvFile)))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT 1 FROM `AWLArge_FactInternetSales_1M` LIMIT 1";
                    var value = cmd.ExecuteScalar();

                    Assert.AreEqual(1, value);
                }

                connection.Close();
            }

            sw.Stop();

            Console.WriteLine("Test finished in {0} ms.", sw.ElapsedMilliseconds);
        }
    }
}
