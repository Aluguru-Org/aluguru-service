using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Catalog.Usecases.GetCategories
{
    public class GetCategoriesCommand : Command<GetCategoriesCommandResponse>
    {

    }

    public class GetCategoriesCommandResponse
    {
        public List<CategoryViewModel> Categories { get; set; }
    }
}
