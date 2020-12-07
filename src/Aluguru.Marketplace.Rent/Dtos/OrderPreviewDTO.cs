using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Rent.Dtos
{
    public class OrderPreviewDTO : IDto
    {
        public string ZipCode { get; set; }
        public List<ItemPreviewDTO> Items { get; set; }
        public decimal TotalFreigth { get; set; }
    }

    public class ItemPreviewDTO : IDto
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public decimal Freigth { get; set; }
    }
}
