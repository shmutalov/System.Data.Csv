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
    /// Represents Excel table column
    /// </summary>
    internal class CsvColumn
    {
        public CsvColumn()
        {
            DataType = typeof(string);
        }

        public CsvColumn([NotNull] string name)
            : this()
        {
            Name = name;
        }

        public CsvColumn([NotNull] CsvTable table, [NotNull] string name)
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

        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, DataType.Name);
        }
    }
}
