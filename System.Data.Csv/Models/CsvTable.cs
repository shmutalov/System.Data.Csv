#region Copyright

/*
	Copyright (c) Sherzod Mutalov, 2015
	mailto:shmutalov@gmail.com
*/

#endregion

using System.Collections.Generic;
using System.ComponentModel;

namespace System.Data.Csv.Models
{
    /// <summary>
    /// Represents Excel table
    /// </summary>
    [DefaultProperty("Name")]
    internal class CsvTable
    {
        public CsvTable()
        {
            Columns = new List<CsvColumn>();
        }

        public CsvTable(string name)
            :this()
        {
            Name = name;
        }

        /// <summary>
        /// Table name
        /// </summary>
        public string Name { get; set; }

        public List<CsvColumn> Columns { get; }
    }
}
