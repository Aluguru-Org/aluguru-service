namespace Aluguru.Marketplace.Rent.Domain
{
    public enum EOrderStatus
    {
        Draft = 0,
        Initiated,
        PaymentConfirmed,
        Finished,        
        Canceled
    }
}
