﻿using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mubbi.Marketplace.Catalog.UnitTests
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

        [Fact]
        public void CreateCustomField_WhenText_ShouldPass()
        {
            new CustomField("text");
        }

        [Fact]
        public void CreateCustomField_WhenNumber_ShouldPass()
        {
            new CustomField(50);
        }

        [Fact]
        public void CreateCustomField_WhenCheckbox_ShouldPass()
        {
            new CustomField(EFieldType.Checkbox, new List<string> { "test", "test" });
        }

        [Fact]
        public void CreateCustomField_WhenRadio_ShouldPass()
        {
            new CustomField(EFieldType.Radio, new List<string> { "test", "test" });
        }
    }
}
