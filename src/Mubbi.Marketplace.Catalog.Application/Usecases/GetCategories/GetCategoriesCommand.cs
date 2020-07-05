using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Catalog.Application.Usecases.GetCategories
{
    public class GetAllCategoriesCommand : Command<GetAllCategoriesCommandResponse>
    {

    }

    public class GetAllCategoriesCommandResponse
    {
        public List<CategoryViewModel> Categories { get; set; }
    }
}
