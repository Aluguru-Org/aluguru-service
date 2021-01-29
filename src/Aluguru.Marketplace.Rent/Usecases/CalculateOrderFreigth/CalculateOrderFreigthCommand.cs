using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Dtos;
using System;

namespace Aluguru.Marketplace.Rent.Usecases.CalculateOrderFreigth
{
    public class CalculateOrderFreigthCommand : Command<CalculateOrderFreigthCommandResponse>
    {
        public CalculateOrderFreigthCommand(Guid userId, Guid orderId, string number, string complement, string zipCode)
        {
            UserId = userId;
            OrderId = orderId;
            Number = number;
            Complement = complement;
            ZipCode = zipCode;
        }

        public Guid UserId { get; set; }
        public Guid OrderId {get; set;}
        public string Number { get; set; }
        public string Complement { get; set; }
        public string ZipCode { get; set; }
    }

    public class CalculateOrderFreigthCommandResponse
    {
        public OrderDTO Order { get; set; }
    }
}
