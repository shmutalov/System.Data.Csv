#region Copyright

/*
	Copyright (c) Sherzod Mutalov, 2015
	mailto:shmutalov@gmail.com
*/

#endregion

using System.Collections.Generic;
using System.Data.Csv.Extensions;
using System.Data.Csv.Helpers;
using System.Data.Csv.Models;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace System.Data.Csv.Storage
{
    /// <summary>
    /// SQLite storage
    /// </summary>
    internal class SqliteStorage : IStorage
    {
        private const string DatabaseNameTemplate = "{0}.db3";
        private const string ConnectionStringTemplate = "Data Source={0};Version=3;";

        private const int BatchSize = 10000;

        public string GetConnectionString(string database, string user, string password)
        {
            return string.Format(ConnectionStringTemplate, database);
        }

        public IDbConnection GetConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }

        public string GetDatabaseName(string excelFileName, string storageDir)
        {
            return Path.Combine(
                storageDir,
                string.Format(
                    DatabaseNameTemplate,
                    Path.GetFileName(excelFileName)));
        }

        public void CreateDatabase(string database, string storageDir)
        {
            SQLiteConnection.CreateFile(Path.Combine(storageDir, database));
        }

        public void DropDatabase(string database, string storageDir)
        {
            try
            {
                File.Delete(Path.Combine(storageDir, database));
            }
            catch (Exception)
            {
                // ignore
            }
        }

        public bool DatabaseExists(string database, string storageDir)
        {
            return File.Exists(Path.Combine(storageDir, database));
        }

        private void Upload(CsvTable table, IDbConnection storageConnection, object[][] values)
        {
            var sb = new StringBuilder();

            #region SQLite upload settings

            sb.AppendLine(@"PRAGMA synchronous=OFF;");
            sb.AppendLine(@"PRAGMA count_changes=OFF;");
            sb.AppendLine(@"PRAGMA journal_mode=MEMORY;");
            sb.AppendLine(@"PRAGMA temp_store=MEMORY;");
            sb.AppendLine(@"PRAGMA encoding=""UTF-8"";");

            #endregion

            sb.AppendLine(@"BEGIN;");

            #region INSERT INTO statement

            sb.AppendFormat(
                @"INSERT INTO `{0}`({1})",
                table.Name,
                string.Join(",", table.Columns.Select(
                    column => string.Format("`{0}`", column.Name))));

            sb.AppendLine();

            sb.AppendFormat(
                " VALUES {0}",
                string.Join(",\n", values.Select(
                    value => string.Format("({0})", string.Join(",", value)))));

            sb.AppendLine(";");

            #endregion

            sb.AppendLine(@"COMMIT;");

            using (var cmd = storageConnection.CreateCommand())
            {
                cmd.CommandText = sb.ToString();
                cmd.ExecuteNonQuery();
            }
        }

        public void ImportData(CsvReader sourceReader, CsvConnectionParameters parameters, IDbConnection storageConnection)
        {
            List<object[]> preloadedValues;
            var reader = (IDataReader)sourceReader;

            var table = CsvHelper.GetTable(parameters.Database, reader, parameters.FirstRowIsHeader, parameters.AnalysisMethod, parameters.RowsToAnalyse, out preloadedValues);

            CreateTable((SQLiteConnection)storageConnection, table);

            if (preloadedValues == null ||
                preloadedValues.Count == 0)
                preloadedValues = new List<object[]>();

            // format preloaded values
            foreach (var values in preloadedValues)
            {
                for (var columnId = 0; columnId < table.Columns.Count; columnId++)
                {
                    values[columnId] = ToStorageValue(table.Columns[columnId].DataType, values[columnId]);
                }
            }

            while (reader.Read())
            {
                var values = new object[table.Columns.Count];

                reader.GetValues(values);

                for (var columnId = 0; columnId < table.Columns.Count; columnId++)
                {
                    values[columnId] = ToStorageValue(table.Columns[columnId].DataType,
                        values[columnId]);
                }

                preloadedValues.Add(values);

                if (preloadedValues.Count < BatchSize)
                    continue;

                Upload(table, storageConnection, preloadedValues.ToArray());
                preloadedValues.Clear();
            }

            // upload last rows if exists
            if (preloadedValues.Count > 0)
                Upload(table, storageConnection, preloadedValues.ToArray());
        }

        public string GetStorageDataType(Type type)
        {
            string result;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    result = "INTEGER";
                    break;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    result = "BIGINT";
                    break;
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    result = "DOUBLE";
                    break;
                case TypeCode.DateTime:
                    result = "DATETIME";
                    break;
                default:
                    result = "TEXT";
                    break;
            }

            return result;
        }

        private static object ToStorageValue(Type type, object value)
        {
            object result;

            if (value == null ||
                value == DBNull.Value)
                return "NULL";

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    result = value;
                    break;
                case TypeCode.DateTime:
                    result = string.Format("'{0}'", Convert.ToDateTime(value).ToString("yyyy-MM-dd hh:mm:ss"));
                    break;
                default:
                    result = string.Format("'{0}'", value.ToString().Replace("'", "''"));
                    break;
            }

            return result;
        }

        /// <summary>
        /// Create storage table form excel table
        /// </summary>
        /// <param name="conenction"></param>
        /// <param name="table"></param>
        private void CreateTable(SQLiteConnection conenction, CsvTable table)
        {
            try
            {
                var sb = new StringBuilder();

                sb.AppendLine(string.Format("CREATE TABLE IF NOT EXISTS `{0}`", table.Name));
                sb.AppendLine("(");

                // columns
                for (var columnId = 0; columnId < table.Columns.Count; columnId++)
                {
                    if (columnId > 0)
                        sb.AppendLine(",");

                    var column = table.Columns[columnId];

                    var storageDataType = GetStorageDataType(column.DataType);

                    // NOTE: Excel columns always nullable
                    sb.AppendFormat("\t`{0}` {1} NULL", column.Name, storageDataType);
                }

                sb.AppendLine();
                sb.AppendLine(")");

                using (var cmd = conenction.CreateCommand())
                {
                    cmd.CommandText = sb.ToString();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new CsvException(ex, "Cannot create storage table '{0}'", table.Name);
            }
        }
    }
}
