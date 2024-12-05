namespace Ordering.Orders.Dtos;
public record OrderDto(
    Guid Id,
    Guid CustomerId,
    string orderName,
    AddressDto ShhippingAddress,
    AddressDto BillingAddress,
    PaymentDto Payment,
    List<OrderItemDto> Items
    );