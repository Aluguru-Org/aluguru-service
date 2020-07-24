using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.UnitTests
{
    public static class Populator
    {
        public static IEnumerable<object[]> Populate(int count, Func<object> populator)
        {
            var items = new List<object[]>();
            for (int i = 0; i < count; i++)
            {
                items.Add(new object[] { populator() });
            }

            return items;
        }
    }
}