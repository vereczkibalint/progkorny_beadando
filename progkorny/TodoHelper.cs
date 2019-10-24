using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace progkorny
{
    public static class TodoHelper
    {
        public static bool IsEmptyOrNull(params string[] items)
        {
            return items.Any(x => x == null || x == "");
        }
    }
}
