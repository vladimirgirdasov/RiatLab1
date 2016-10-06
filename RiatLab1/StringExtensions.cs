namespace RiatLab1
{
    public static class StringExtensions
    {
        public static string RemoveEndLines(this string str)
        {
            return str.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
        }
    }
}
