using Mubbi.Marketplace.Register.Domain.Enums;
using Mubbi.Marketplace.Shared.DomainObjects;

namespace Mubbi.Marketplace.Register.Domain.ValueObjects
{
    public class Document : ValueObject
    {
        public Document(string number, EDocumentType documentType)
        {
            Number = number;
            DocumentType = documentType;

            Validate();
        }

        public string Number { get; private set; }
        public EDocumentType DocumentType { get; private set; }

        public override void Validate()
        {
            switch(DocumentType)
            {
                case EDocumentType.CPF: 
                    EntityConcerns.IsNotMatch(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$|^\d{11}$", Number, "The field Number is not a CPF");
                    break;
                case EDocumentType.CNPJ:
                    EntityConcerns.IsNotMatch(@"^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$|^\d{14}$", Number, "The field Number is not a CNPJ");
                    break;
            }
        }
    }
}
