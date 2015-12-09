#region Copyright

/*
	Copyright (c) Sherzod Mutalov, 2015
	mailto:shmutalov@gmail.com
*/

#endregion

namespace System.Data.Csv.Extensions
{
    /// <summary>
    /// String extension methods
    /// </summary>
    internal static class StringExt
    {
        /// <summary>
        /// Converts source string to boolean value
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool ToBool(this string source)
        {
            bool result;

            switch (source.ToLower())
            {
                case "true":
                case "t":
                case "1":
                case "yes":
                case "y":
                    result = true;
                    break;
                case "false":
                case "f":
                case "0":
                case "no":
                case "n":
                    result = false;
                    break;
                default:
                    throw new InvalidCastException(
                        string.Format("Cannot convert '{0}' string to boolean value", source));
            }

            return result;
        }
    }
}
