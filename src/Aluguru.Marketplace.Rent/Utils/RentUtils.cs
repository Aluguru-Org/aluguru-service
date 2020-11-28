using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Dtos;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Rent.Utils
{
    public static class RentUtils
    {
        public static decimal CalculateProductPrice(AddOrderItemDTO orderItem, Product product)
        {
            decimal price = 0;

            switch (product.RentType)
            {
                case ERentType.Fixed:
                    price = product.Price.GetPeriodRentPrice(orderItem.SelectedRentPeriod.Value);
                    break;
                case ERentType.Indefinite:
                    price = orderItem.RentDays * product.Price.GetDailyRentPrice();
                    break;
            }

            return price;
        }

        public static List<DomainNotification> ValidateProduct(string messageType, AddOrderItemDTO orderItem, Product product)
        {
            List<DomainNotification> notifications = new List<DomainNotification>();

            if (!product.CheckValidRentStartDate(orderItem.RentStartDate))
            {
                notifications.Add(new DomainNotification(messageType, $"The product {product.Id} does not have a valid rent start date"));
            }

            switch (product.RentType)
            {
                case ERentType.Indefinite:
                    if (!product.CheckValidRentDays(orderItem.RentDays))
                    {
                        notifications.Add(new DomainNotification(messageType, $"The product {product.Id} have invalid rent days."));
                    }
                    break;
            }

            return notifications;
        }
    }
}
