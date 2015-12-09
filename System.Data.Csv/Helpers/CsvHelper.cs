#region Copyright

/*
	Copyright (c) Sherzod Mutalov, 2015
	mailto:shmutalov@gmail.com
*/

#endregion

using System.Collections.Generic;
using System.Data.Csv.Enums;
using System.Data.Csv.Models;
using System.Linq;
using JetBrains.Annotations;
using LumenWorks.Framework.IO.Csv;

namespace System.Data.Csv.Helpers
{
    internal static class CsvHelper
    {
        /// <summary>
        /// Choose best data type between of two
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        private static Type Choose(Type type1, Type type2)
        {
            switch (Type.GetTypeCode(type1))
            {
                case TypeCode.Boolean:
                    {
                        switch (Type.GetTypeCode(type2))
                        {
                            case TypeCode.Boolean:
                                return typeof(bool);
                            default:
                                return typeof(string);
                        }
                    }
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
                    {
                        switch (Type.GetTypeCode(type2))
                        {
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
                                return typeof(double);
                            default:
                                return typeof(string);
                        }
                    }
                case TypeCode.DateTime:
                    {
                        switch (Type.GetTypeCode(type2))
                        {
                            case TypeCode.DateTime:
                                return typeof(DateTime);
                            default:
                                return typeof(string);
                        }
                    }
                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// Returns best data type for the given column by parsing rows
        /// </summary>
        /// <param name="method"></param>
        /// <param name="dataTypes"></param>
        /// <param name="rows"></param>
        /// <param name="columnId"></param>
        /// <returns></returns>
        private static Type GetColumnDataType(CsvColumnDataTypeAnalysisMethod method, object[][] dataTypes, int rows, int columnId)
        {
            var typesDict = new Dictionary<Type, int>();
            var result = typeof(string);

            if (rows == 0 || columnId >= dataTypes[0].Length)
                return result;

            for (var rowId = 0; rowId < rows; rowId++)
            {
                var value = dataTypes[rowId][columnId];

                if (value == null)
                    continue;

                var type = value.GetType();

                if (!typesDict.ContainsKey(type))
                    typesDict[type] = 0;

                typesDict[type]++;
            }

            switch (method)
            {
                case CsvColumnDataTypeAnalysisMethod.MostFrequent:
                    if (typesDict.Count > 0)
                        result = typesDict.Aggregate((l, r) => l.Value > r.Value ? l : r).Key ?? typeof(string);
                    break;
                case CsvColumnDataTypeAnalysisMethod.BestMatch:
                    var lastType = (Type)null;

                    foreach (var type in typesDict.Keys)
                    {
                        if (lastType == null)
                        {
                            lastType = type;
                            continue;
                        }

                        lastType = Choose(type, lastType);
                    }

                    result = lastType ?? typeof(string);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }

            return result;
        }

        /// <summary>
        /// Builds table from reader's result set
        /// </summary>
        /// <param name="database"></param>
        /// <param name="reader"></param>
        /// <param name="firstRowIsHeader">Is first row contains column names?</param>
        /// <param name="method"></param>
        /// <param name="rowsToAnalyse">How much rows we want to analyse?</param>
        /// <param name="preloadedValues">Preloaded rows after analysis</param>
        /// <returns></returns>
        public static CsvTable GetTable(
            [NotNull] string database,
            CsvReader.RecordEnumerator reader, 
            bool firstRowIsHeader, 
            CsvColumnDataTypeAnalysisMethod method, 
            int rowsToAnalyse, 
            out List<object[]> preloadedValues)
        {
            reader.Reset();
            reader.MoveNext();
            
            var table = new CsvTable(database);
            var columnsCount = reader.Current.Length;

            if (firstRowIsHeader)
            {
                for (var columnId = 0; columnId < columnsCount; columnId++)
                {
                    var columnName = reader.Current[columnId] ?? string.Format("Column {0}", columnId + 1);

                    table.Columns.Add(new CsvColumn(table, columnName));
                }
            }
            else
            {
                for (var columnId = 0; columnId < columnsCount; columnId++)
                {
                    table.Columns.Add(new CsvColumn(table, string.Format("Column {0}", columnId)));
                }

                reader.Reset();
            }

            // preloaded data list
            var preloadedDataList = new List<object[]>();

            if (rowsToAnalyse < 1)
                rowsToAnalyse = 1;

            for (var rowId = 0; rowId < rowsToAnalyse; rowId++)
            {
                if (!reader.MoveNext())
                    break;

                var values = new object[columnsCount];

                for (var columnId = 0; columnId < columnsCount; columnId++)
                {
                    values[columnId] = reader.Current[columnId];
                }

                preloadedDataList.Add(values);
            }

            // calculate columns data types

            var data = preloadedDataList.ToArray();

            for (var columnId = 0; columnId < columnsCount; columnId++)
            {
                table.Columns[columnId].DataType = GetColumnDataType(method, data, preloadedDataList.Count, columnId);
            }

            preloadedValues = preloadedDataList;

            return table;
        }
    }
}
