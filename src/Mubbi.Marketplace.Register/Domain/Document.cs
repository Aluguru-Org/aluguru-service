using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mubbi.Marketplace.Register.Domain
{
    public class Document : ValueObject
    {
        public Document(string number, EDocumentType documentType)
        {
            Number = number;
            DocumentType = documentType;

            ValidateValueObject();
        }

        public string Number { get; private set; }
        public EDocumentType DocumentType { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
            yield return DocumentType;
        }

        protected override void ValidateValueObject()
        {
            switch(DocumentType)
            {
                case EDocumentType.CPF:
                    Ensure.That(new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$|^\d{11}$").IsMatch(Number), "The field Number is not a CPF");
                    break;
                case EDocumentType.CNPJ:
                    Ensure.That(new Regex(@"^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$|^\d{14}$").IsMatch(Number), "The field Number is not a CNPJ");
                    break;
            }
        }

        public override string ToString()
        {
            return Number;
        }
    }
}
