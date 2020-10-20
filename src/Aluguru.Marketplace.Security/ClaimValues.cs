using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aluguru.Marketplace.Security
{
    public enum ClaimValues
    {
        None = 0,
        Read = 10,
        Write = 20
    }

    public static class ClaimValuesHelper
    {
        public static IEnumerable<string> ToPolicy(ClaimValues values)
        {
            return values.ToString().Split(',').Select(r => r.Trim());
        }

        public static ClaimValues Parse(string value)
        {
            switch(value)
            {
                case "Read": return ClaimValues.Read;
                case "Write": return ClaimValues.Write;
                default: return ClaimValues.None;
            }
        }
    }
}
