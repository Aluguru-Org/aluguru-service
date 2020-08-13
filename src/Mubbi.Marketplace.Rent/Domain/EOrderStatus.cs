namespace Mubbi.Marketplace.Rent.Domain
{
    public enum EOrderStatus
    {
        Draft = 0,
        Initiated,
        PaymentConfirmed,
        Delivered,
        Canceled
    }
}
