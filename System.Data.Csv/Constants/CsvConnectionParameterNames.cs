#region Copyright

/*
	Copyright (c) Sherzod Mutalov, 2015
	mailto:shmutalov@gmail.com
*/

#endregion

namespace System.Data.Csv.Constants
{
    /// <summary>
    /// Excel datasource connection string parameter names
    /// </summary>
    internal static class CsvConnectionParameterNames
    {
        /// <summary>
        /// Database name (document file name)
        /// </summary>
        public const string Database = "DATABASE";

        /// <summary>
        /// Directory where internal storage must initialized
        /// </summary>
        public const string StorageDirectory = "STORAGEDIR";

        /// <summary>
        /// Password to decrypt document
        /// </summary>
        public const string Password = "PASSWORD";

        /// <summary>
        /// First row of tables is header
        /// </summary>
        public const string FirstRowIsHeader = "FIRSTROWISHEADER";

        /// <summary>
        /// Field delimiter
        /// </summary>
        public const string Delimiter = "DELIMITER";

        /// <summary>
        /// Field wrapper character. 
        /// For example "Some text" wrapped with double quote characters.
        /// </summary>
        public const string FieldWrapper = "FIELDWRAPPER";

        /// <summary>
        /// Escape character
        /// </summary>
        public const string Escape = "ESCAPE";

        /// <summary>
        /// Forces internal storage to reinitialize
        /// </summary>
        public const string ForceStorageReload = "FORCESTORAGERELOAD";

        /// <summary>
        /// Column data type analysis method
        /// </summary>
        public const string AnalysisMethod = "ANALYSISMETHOD";

        /// <summary>
        /// Rows count to analyse
        /// </summary>
        public const string RowsToAnalyse = "ROWSTOANALYSE";

    }
}
