using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Aluguru.Marketplace.Catalog.Domain
{
    public class CustomField : Entity
    {
        private readonly List<string> _valueAsOptions;

        private CustomField() : base(NewId())
        {
            _valueAsOptions = new List<string>();
        }

        public CustomField(EFieldType fieldType, string fieldName) : this(fieldType)
        {
            FieldName = fieldName;
            ValidateEntity();
        }

        public CustomField(EFieldType fieldType, List<string> values) : this(fieldType)
        {
            _valueAsOptions = values;
            ValidateEntity();
        }

        private CustomField(EFieldType fieldType) : base(NewId())
        {
            FieldType = fieldType;
        }
        public string FieldName { get; set; }
        public EFieldType FieldType { get; private set; }
        public IReadOnlyCollection<string> ValueAsOptions { get { return _valueAsOptions; } }
        public bool Active { get; private set; }
        public Guid ProductId { get; private set; }

        // EF Relational
        public virtual Product Product { get; set; }
        protected override void ValidateEntity()
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(FieldName));

            switch (FieldType)
            {
                case EFieldType.Checkbox:
                case EFieldType.Radio:
                    Ensure.That<DomainException>(ValueAsOptions != null && ValueAsOptions.Count >= 1);
                    break;
            }
        }
    }
}
