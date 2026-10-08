using System.Threading;
using System;
namespace Application.UseCases;

using Application.Interfaces;
using Domain.Entities;
using Domain.Services;

public class CreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderUseCase (IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Order Execute(string customer, string product, int qty, decimal price)
    {
        var order = OrderService.CreateOrder(customer, product, qty, price);

        _orderRepository.Create(order);
        
        return order;
    }
}
