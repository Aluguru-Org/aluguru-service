using Mubbi.Marketplace.Domain.Core.ValueObjects;
using Mubbi.Marketplace.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Domain.ValueObjects
{
    public class Document : ValueObject
    {
        public Document(int number, DocumentType documentType)
        {
            Number = number;
            DocumentType = documentType;
        }

        public int Number { get; private set; }
        public DocumentType DocumentType { get; private set; }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
