#region Copyright

/*
	Copyright (c) Sherzod Mutalov, 2015
	mailto:shmutalov@gmail.com
*/

#endregion

using LumenWorks.Framework.IO.Csv;

namespace System.Data.Csv.Extensions
{
    /// <summary>
    /// Excel data reader extension methods
    /// </summary>
    internal static class ReaderExt
    {
        /// <summary>
        /// IExcelData reader doesn't implement GetValues method,
        /// we implement it here
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int _GetValues(this CsvReader.RecordEnumerator enumerator, object[] values)
        {
            if (enumerator.Current.Length <= 0)
            {
                return enumerator.Current.Length;
            }

            for (var fieldId = 0; fieldId < enumerator.Current.Length; fieldId++)
            {
                values[fieldId] = enumerator.Current[fieldId];
            }

            return enumerator.Current.Length;
        }

        /// <summary>
        /// Resets reader to initial position
        /// </summary>
        /// <param name="reader"></param>
        public static void Reset(this IDataReader reader)
        {
            var csv = reader as CsvReader;

            csv?.MoveTo(-1L);
        }
    }
}
