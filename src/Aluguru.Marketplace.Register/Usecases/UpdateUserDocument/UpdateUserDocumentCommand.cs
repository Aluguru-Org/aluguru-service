using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;
using Aluguru.Marketplace.Register.Domain;

namespace Aluguru.Marketplace.Register.Usecases.UpadeUserDocument
{
    public class UpdateUserDocumentCommand : Command<bool>
    {
        public UpdateUserDocumentCommand(Guid userId, string number, string documentType)
        {
            UserId = userId;
            Number = number;
            DocumentType = documentType;
        }

        public Guid UserId { get; }
        public string Number { get; }
        public string DocumentType { get; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateUserDocumentCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateUserDocumentCommandValidator : AbstractValidator<UpdateUserDocumentCommand>
    {
        public UpdateUserDocumentCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.DocumentType).Must(x => x == EDocumentType.CNPJ.ToString() || x == EDocumentType.CPF.ToString())
                .WithMessage("DocumentType must be CNPJ or CPF");
        }
    }
}
