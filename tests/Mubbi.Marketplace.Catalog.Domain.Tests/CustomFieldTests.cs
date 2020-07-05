using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mubbi.Marketplace.Catalog.Domain.UnitTests
{
    public class CustomFieldTests
    {
        [Fact]
        public void CreateCustomField_WhenNull_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new CustomField(null));
        }

        [Fact]
        public void CreateCustomField_WhenEmptyValueAsString_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new CustomField(""));
        }

        [Fact]
        public void CreateCustomField_WhenEmptyOptions_AndFieldTypeCheckbox_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new CustomField(EFieldType.Checkbox, new List<string>()));
        }

        [Fact]
        public void CreateCustomField_WhenEmptyOptions_AndFieldTypeRadio_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new CustomField(EFieldType.Radio, new List<string>()));
        }
    }
}
