using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Catalog.Usecases.GetCategories
{
    public class GetCategoriesCommand : Command<GetCategoriesCommandResponse>
    {

    }

    public class GetCategoriesCommandResponse
    {
        public List<CategoryViewModel> Categories { get; set; }
    }
}
