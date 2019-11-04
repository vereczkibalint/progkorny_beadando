using System.Linq;

namespace progkorny
{
    public static class TodoHelper
    {
        /// <summary>
        /// Helper függvény, vizsgálja, hogy bármelyik paraméterben kapott érték null-e vagy üres-e
        /// </summary>
        /// <param name="items">értékek</param>
        /// <returns>bool</returns>
        public static bool IsEmptyOrNull(params string[] items)
        {
            return items.Any(x => x == null || x == "");
        }
    }
}
