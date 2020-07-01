using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Catalog.Application.Usecases.CreateProduct
{
    public class CreateProductCommand : Command<CreateProductCommandResponse>
    {
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }

    public class CreateProductCommandResponse
    {

    }
}
