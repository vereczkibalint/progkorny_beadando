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
            bool returnValue = false;

            foreach (string item in items)
            {
                if(item == null || item == "")
                {
                    returnValue = true;
                    break;
                }
            }

            return returnValue;
        }
    }
}
