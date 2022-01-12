namespace SuperStarTrek
{
    internal static class StringExtensions
    {
        internal static string Pluralize(this string singular, int quantity) => singular + (quantity > 1 ? "s" : "");
    }
}