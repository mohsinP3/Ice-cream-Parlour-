namespace Ice_Cream_Parlour_Eproject.Areas.Models.Enums
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Completed,
        Cancelled,
        Refunded
    }

    
    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        PayPal,
        CashOnDelivery,
        BankTransfer
    }
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Refunded
    }
}