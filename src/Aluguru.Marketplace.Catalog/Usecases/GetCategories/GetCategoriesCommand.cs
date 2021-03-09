﻿using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Catalog.Usecases.GetCategories
{
    public class GetCategoriesCommand : Command<GetCategoriesCommandResponse>
    {
        public GetCategoriesCommand(PaginateCriteria paginateCriteria)
        {
            PaginateCriteria = paginateCriteria;
        }

        public PaginateCriteria PaginateCriteria { get; private set; }
    }

    public class GetCategoriesCommandResponse
    {
        public PaginatedItem<CategoryDTO> Categories { get; set; }
    }
}
