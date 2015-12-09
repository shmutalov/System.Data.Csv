#region Copyright

/*
	Copyright (c) Sherzod Mutalov, 2015
	mailto:shmutalov@gmail.com
*/

#endregion

using System.Data.Csv.Constants;
using System.Data.Csv.Enums;
using System.Data.Csv.Extensions;

namespace System.Data.Csv.Models
{
    /// <summary>
    /// Excel connection parameters
    /// </summary>
    internal class CsvConnectionParameters
    {
        /// <summary>
        /// Database name (document name)
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Storage directory
        /// </summary>
        public string StoregeDirectory { get; set; }

        /// <summary>
        /// Password to database
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// First row of tables are table column names
        /// </summary>
        public bool FirstRowIsHeader { get; set; }

        /// <summary>
        /// Field delimiter in CSV data
        /// </summary>
        public char Delimiter { get; set; }

        /// <summary>
        /// Field wrapper
        /// </summary>
        public char FieldWrapper { get; set; }

        /// <summary>
        /// Escape character in values
        /// </summary>
        public char Escape { get; set; }

        /// <summary>
        /// Forces reload all data of internal storage
        /// </summary>
        public bool ForceStorageReload { get; set; }

        /// <summary>
        /// Column data type analysis method
        /// </summary>
        public CsvColumnDataTypeAnalysisMethod AnalysisMethod { get; set; }

        /// <summary>
        /// Rows count to analyse
        /// </summary>
        public int RowsToAnalyse { get; set; }

        /// <summary>
        /// Build connection parameters by parsing connection string
        /// </summary>
        /// <param name="connectionString">Connection string to parse</param>
        /// <returns></returns>
        public static CsvConnectionParameters FromConnectionString(string connectionString)
        {
            var parameters = new CsvConnectionParameters
            {
                AnalysisMethod = CsvColumnDataTypeAnalysisMethod.BestMatch,
                RowsToAnalyse = 100,
                ForceStorageReload = false,
                FirstRowIsHeader = true,
            };

            var splitted = connectionString.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            if (splitted.Length == 0)
                return parameters;

            foreach (var entry in splitted)
            {
                var splittedKeyVal = entry.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                if (splittedKeyVal.Length == 2)
                {
                    switch (splittedKeyVal[0].ToUpper())
                    {
                        case CsvConnectionParameterNames.Database:
                            parameters.Database = splittedKeyVal[1];
                            break;
                        case CsvConnectionParameterNames.StorageDirectory:
                            parameters.Database = splittedKeyVal[1];
                            break;
                        case CsvConnectionParameterNames.Password:
                            parameters.Password = splittedKeyVal[1];
                            break;
                        case CsvConnectionParameterNames.FirstRowIsHeader:
                            parameters.FirstRowIsHeader = splittedKeyVal[1].ToBool();
                            break;
                        case CsvConnectionParameterNames.Delimiter:
                            parameters.Delimiter = ','; // default delimiter

                            if (!string.IsNullOrEmpty(splittedKeyVal[1]))
                                parameters.Delimiter = splittedKeyVal[1][0];
                            break;
                        case CsvConnectionParameterNames.FieldWrapper:
                            parameters.FieldWrapper = '"'; // default field wrapper

                            if (!string.IsNullOrEmpty(splittedKeyVal[1]))
                                parameters.FieldWrapper = splittedKeyVal[1][0];
                            break;
                        case CsvConnectionParameterNames.Escape:
                            parameters.Escape = '\\'; // default field wrapper

                            if (!string.IsNullOrEmpty(splittedKeyVal[1]))
                                parameters.Escape = splittedKeyVal[1][0];
                            break;
                        case CsvConnectionParameterNames.ForceStorageReload:
                            parameters.ForceStorageReload = splittedKeyVal[1].ToBool();
                            break;
                        case CsvConnectionParameterNames.AnalysisMethod:
                            CsvColumnDataTypeAnalysisMethod method;

                            if (!Enum.TryParse(splittedKeyVal[1], out method))
                            {
                                parameters.AnalysisMethod = CsvColumnDataTypeAnalysisMethod.BestMatch;
                            }

                            parameters.AnalysisMethod = method;
                            break;
                        case CsvConnectionParameterNames.RowsToAnalyse:
                            int rowsToAnalyse;

                            if (!int.TryParse(splittedKeyVal[1], out rowsToAnalyse))
                            {
                                parameters.RowsToAnalyse = 100;
                            }

                            if (rowsToAnalyse < 1)
                                rowsToAnalyse = 1;

                            parameters.RowsToAnalyse = rowsToAnalyse;
                            break;
                        default:
                            continue;
                    }
                }
            }

            return parameters;
        }

        public static string ToConnectionString(CsvConnectionParameters parameters)
        {
            return string.Format(
                "{0}={1};{2}={3};{4}={5};{6}={7};{8}={9};{10}={11};{12}={13};{14}={15}",
                CsvConnectionParameterNames.Database, parameters.Database,
                CsvConnectionParameterNames.StorageDirectory, parameters.StoregeDirectory,
                CsvConnectionParameterNames.Password, parameters.Password,
                CsvConnectionParameterNames.FirstRowIsHeader, parameters.FirstRowIsHeader,
                CsvConnectionParameterNames.Delimiter, parameters.Delimiter,
                CsvConnectionParameterNames.FieldWrapper, parameters.FieldWrapper,
                CsvConnectionParameterNames.Escape, parameters.Escape,
                CsvConnectionParameterNames.AnalysisMethod, parameters.AnalysisMethod,
                CsvConnectionParameterNames.RowsToAnalyse, parameters.RowsToAnalyse
                );
        }
    }
}