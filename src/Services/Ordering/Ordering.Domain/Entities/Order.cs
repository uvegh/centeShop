

using Microsoft.Extensions.Options;
using Ordering.Domain.Common;
using Ordering.Domain.Events;
using Shared.Library.Model;
using System.Net;

namespace Ordering.Domain.Entities;

public  class Order : AggregateRoot
{
    public Guid UserId { get; private set; }
    public List<OrderItem> Items {get; private set;}
   
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; } = "Pending";

    public Order(Guid userId,List<OrderItem> items, decimal totalAmount, string status)
    {

        UserId = userId;
        Items = items;
        TotalAmount = totalAmount;
        Status = status;

       
    }

    public static Order Create( Guid userId, decimal totalAmount, List<OrderItem>? items=null)
    {
        var order = new Order(userId= Guid.NewGuid(), items, totalAmount, "Pending");
        //automatically add new order domain event
      
     order.AddDomainEvent(new OrderCreatedDomainEvent(order.Id, userId, totalAmount));
        
        return order;
    }
    public void MaskAsPaid()
    {
        Status = "Paid";
    }
}
