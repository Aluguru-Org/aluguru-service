using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.API.IntegrationTests.Config
{
    public class TestCategory
    {
        public TestCategory(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }

        public string Name { get; }
        public string Uri { get; }
        public Guid MainCategoryId { get; set; }
        public Guid Id { get; set; }
    }
}
