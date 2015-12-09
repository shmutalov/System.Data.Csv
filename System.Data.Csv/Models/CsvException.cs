#region Copyright

/*
	Copyright (c) Sherzod Mutalov, 2015
	mailto:shmutalov@gmail.com
*/

#endregion

using JetBrains.Annotations;

namespace System.Data.Csv.Models
{
    /// <summary>
    /// Excel exception
    /// </summary>
    public class CsvException : Exception
    {
        public CsvException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public CsvException(string message)
            : base(message)
        {

        }

        [StringFormatMethod("format")]
        public CsvException(string format, params object[] args)
            : base(string.Format(format, args))
        {

        }

        [StringFormatMethod("format")]
        public CsvException(Exception innerException, string format,  params object[] args)
            : base(string.Format(format, args), innerException)
        {

        }
    }
}
