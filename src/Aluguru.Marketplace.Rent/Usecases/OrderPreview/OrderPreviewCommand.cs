using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Dtos;
using System;

namespace Aluguru.Marketplace.Rent.Usecases.OrderPreview
{
    public class OrderPreviewCommand : Command<OrderPreviewCommandResponse>
    {
        public OrderPreviewCommand(OrderPreviewDTO dto)
        {
            Preview = dto;
        }

        public OrderPreviewDTO Preview { get; set; }
    }

    public class OrderPreviewCommandResponse
    {
        public OrderPreviewDTO Order { get; set; }
    }
}
