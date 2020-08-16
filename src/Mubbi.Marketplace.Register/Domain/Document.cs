using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Text.RegularExpressions;
using static PampaDevs.Utils.Helpers.IdHelper;


namespace Mubbi.Marketplace.Register.Domain
{
    public class Document : Entity
    {
        private Document() : base(NewId()) { }
        public Document(string number, EDocumentType documentType)
            : base(NewId())
        {
            Number = number;
            DocumentType = documentType;

            ValidateEntity();
        }
        public Guid UserId { get; private set; }
        public string Number { get; private set; }
        public EDocumentType DocumentType { get; private set; }
        public virtual User User { get; set; }
        
        public void AssignUser(Guid userId)
        {
            UserId = userId;
        }

        internal void UpdateDocument(EDocumentType documentType, string number)
        {
            Number = number;
            DocumentType = documentType;
        }

        public override string ToString()
        {
            return Number;
        }

        protected override void ValidateEntity()
        {
            switch (DocumentType)
            {
                case EDocumentType.CPF:
                    Ensure.That(new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$|^\d{11}$").IsMatch(Number), "The field Number is not a CPF");
                    break;
                case EDocumentType.CNPJ:
                    Ensure.That(new Regex(@"^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$|^\d{14}$").IsMatch(Number), "The field Number is not a CNPJ");
                    break;
            }
        }
    }
}
