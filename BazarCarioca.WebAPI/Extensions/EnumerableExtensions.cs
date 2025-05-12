namespace BazarCarioca.WebAPI.Extensions
{
    public static class EnumerableExtensions
    {
        // <summary>
        /// Retorna true se a sequência for null ou não contiver elementos.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? Source)
        {
            return Source == null || !Source.Any();
        }
    }
}
