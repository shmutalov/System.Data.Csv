#region Copyright

/*
	Copyright (c) Sherzod Mutalov, 2015
	mailto:shmutalov@gmail.com
*/

#endregion

namespace System.Data.Csv.Models
{
    /// <summary>
    /// Represents Excel table column
    /// </summary>
    internal class CsvColumn
    {
        public CsvColumn()
        {
            DataType = typeof(string);
        }

        public CsvColumn(string name)
            : this()
        {
            Name = name;
        }

        public CsvColumn(CsvTable table, string name)
            : this(name)
        {
            Table = table;
        }

        /// <summary>
        /// Column name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Column data type
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// Column's parent table
        /// </summary>
        public CsvTable Table { get; set; }
    }
}
