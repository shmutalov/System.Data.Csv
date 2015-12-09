#region Copyright

/*
	Copyright (c) Sherzod Mutalov, 2015
	mailto:shmutalov@gmail.com
*/

#endregion

namespace System.Data.Csv.Enums
{
    /// <summary>
    /// Column data type analyse method
    /// </summary>
    internal enum CsvColumnDataTypeAnalysisMethod
    {
        /// <summary>
        /// The most frequent data type
        /// </summary>
        MostFrequent,

        /// <summary>
        /// Data type that best matches to
        /// </summary>
        BestMatch
    }
}
