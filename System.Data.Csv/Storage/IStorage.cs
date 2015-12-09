#region Copyright

/*
	Copyright (c) Sherzod Mutalov, 2015
	mailto:shmutalov@gmail.com
*/

#endregion

using System.Data.Csv.Models;
using JetBrains.Annotations;
using LumenWorks.Framework.IO.Csv;

namespace System.Data.Csv.Storage
{
    /// <summary>
    /// Storage interface
    /// </summary>
    internal interface IStorage
    {
        /// <summary>
        /// Gets storage specific connection string
        /// </summary>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        string GetConnectionString(
            [NotNull] string database,
            string user,
            string password);

        /// <summary>
        /// Gets storage specific connection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        IDbConnection GetConnection([NotNull] string connectionString);

        /// <summary>
        /// Gets storage specific database name
        /// </summary>
        /// <param name="excelFileName"></param>
        /// <param name="storageDir"></param>
        /// <returns></returns>
        string GetDatabaseName([NotNull] string excelFileName, [NotNull] string storageDir);

        /// <summary>
        /// Creates storage database
        /// </summary>
        /// <param name="database"></param>
        /// <param name="storageDir"></param>
        void CreateDatabase([NotNull] string database, [NotNull] string storageDir);

        /// <summary>
        /// Deletes storage database
        /// </summary>
        /// <param name="database"></param>
        /// <param name="storageDir"></param>
        void DropDatabase([NotNull] string database, [NotNull] string storageDir);

        /// <summary>
        /// Checks whether storage database already exists
        /// </summary>
        /// <param name="database"></param>
        /// <param name="storageDir"></param>
        /// <returns></returns>
        bool DatabaseExists([NotNull] string database, [NotNull] string storageDir);

        /// <summary>
        /// Imports data from Excel to storage
        /// </summary>
        /// <param name="sourceReader"></param>
        /// <param name="parameters"></param>
        /// <param name="storageConnection"></param>
        void ImportData(
            [NotNull] CsvReader sourceReader, 
            [NotNull] CsvConnectionParameters parameters, 
            [NotNull] IDbConnection storageConnection);

        /// <summary>
        /// Convert excel data type to storage specific data type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetStorageDataType(Type type);
    }
}
