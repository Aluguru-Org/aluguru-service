using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Mubbi.Marketplace.Catalog.Domain
{
    public class CustomField : Entity
    {
        private readonly List<string> _valueAsOptions;

        private CustomField() : base(NewId())
        {
            _valueAsOptions = new List<string>();
        }

        public CustomField(string value) : this(EFieldType.Text)
        {
            ValueAsString = value;
            ValidateCreation();
        }

        public CustomField(int value) : this(EFieldType.Number)
        {
            ValueAsInt = value;
            ValidateCreation();
        }

        public CustomField(EFieldType fieldType, List<string> values) : this(fieldType)
        {
            _valueAsOptions = values;
            ValidateCreation();
        }

        private CustomField(EFieldType fieldType) : base(NewId())
        {
            FieldType = fieldType;
        }

        public EFieldType FieldType { get; private set; }
        public string ValueAsString { get; private set; }
        public int? ValueAsInt { get; private set; }
        public IReadOnlyCollection<string> ValueAsOptions { get; private set; }
        public bool Active { get; private set; }
        public Guid ProductId { get; private set; }

        // EF Relational
        public virtual Product Product { get; set; }


        protected override void ValidateCreation()
        {
            switch(FieldType)
            {
                case EFieldType.Text:
                    Ensure.That<DomainException>(!string.IsNullOrEmpty(ValueAsString));
                    break;
                case EFieldType.Number:
                    Ensure.That<DomainException>(ValueAsInt.HasValue);
                    break;
                case EFieldType.Checkbox:
                case EFieldType.Radio:
                    Ensure.That<DomainException>(ValueAsOptions != null && ValueAsOptions.Count >= 1);
                    break;
            }
        }
    }
}
