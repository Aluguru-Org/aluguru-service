using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

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
                    price = orderItem.RentDays.Value * product.Price.GetDailyRentPrice();
                    break;
                case ERentType.None:
                    price = product.Price.GetSellPrice();
                    break;
            }

            return price;
        }

        public static List<DomainNotification> ValidateProduct(string messageType, AddOrderItemDTO orderItem, Product product)
        {
            List<DomainNotification> notifications = new List<DomainNotification>();

            if (product.RentType != ERentType.None)
            {
                if (!product.CheckValidRentStartDate(orderItem.RentStartDate.Value))
                {
                    notifications.Add(new DomainNotification(messageType, $"The product {product.Id} does not have a valid rent start date"));
                }
            }
            

            switch (product.RentType)
            {
                case ERentType.Indefinite:
                    if (!product.CheckValidRentDays(orderItem.RentDays.Value))
                    {
                        notifications.Add(new DomainNotification(messageType, $"The product {product.Id} have invalid rent days."));
                    }
                    break;
            }

            return notifications;
        }

        public static int GetRentDays(List<RentPeriod> rentPeriods, AddOrderItemDTO orderItem, Product product)
        {
            switch(product.RentType)
            {
                case ERentType.Indefinite:
                    return orderItem.RentDays.Value;
                case ERentType.Fixed:
                    return rentPeriods.FirstOrDefault(x => x.Id == orderItem.SelectedRentPeriod.Value).Days;
                default:
                case ERentType.None:
                    return 0;
            }
        }

        internal static decimal CalculateProductFreigthPrice(Product product, double distance)
        {
            return product.Price.FreightPriceKM * (int)Math.Ceiling(distance/1000);
        }
    }
}
