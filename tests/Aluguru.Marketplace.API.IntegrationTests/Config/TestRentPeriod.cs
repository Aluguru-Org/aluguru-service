using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.API.IntegrationTests.Config
{
    public class TestRentPeriod
    {
        public TestRentPeriod(string name, int days)
        {
            Name = name;
            Days = days;
        }

        public Guid Id { get; set; }
        public string Name { get; }
        public int Days { get; }
    }
}
