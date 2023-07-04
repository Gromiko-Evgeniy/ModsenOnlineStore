using AutoMapper;
using Microsoft.AspNetCore.Http;
using ModsenOnlineStore.Common;
using ModsenOnlineStore.Common.Interfaces;
using ModsenOnlineStore.Store.Application.Interfaces.OrderInterfaces;
using ModsenOnlineStore.Store.Application.Interfaces.OrderPaymentConfirmationInterfaces;
using ModsenOnlineStore.Store.Domain.DTOs.OrderDTOs;
using ModsenOnlineStore.Store.Domain.Entities;
using System.Net.Http.Json;

namespace ModsenOnlineStore.Store.Application.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IMapper mapper;
        private readonly IOrderRepository orderRepository;
        private readonly IRabbitMQMessagingService rabbitMQMessagingService;
        private readonly IOrderPaymentConfirmationRepository orderPaymentConfirmationRepository;

        public OrderService(
            IMapper mapper, IOrderRepository repository,
            IRabbitMQMessagingService rabbitMQMessagingService,
            IOrderPaymentConfirmationRepository orderPaymentConfirmationRepository
        )
        {
            this.mapper = mapper;
            this.orderRepository = repository;
            this.rabbitMQMessagingService = rabbitMQMessagingService;
            this.orderPaymentConfirmationRepository = orderPaymentConfirmationRepository;
        }

        public async Task<DataResponseInfo<List<GetOrderDTO>>> GetAllOrders(int pageNumber, int pageSize)
        {
            var orders = await orderRepository.GetAllOrders(pageNumber, pageSize);
            var orderDTOs = orders.Select(p => mapper.Map<GetOrderDTO>(p)).ToList();

            return new DataResponseInfo<List<GetOrderDTO>>(data: orderDTOs, success: true, message: "all orders");
        }

        public async Task<DataResponseInfo<GetOrderDTO>> GetSingleOrder(int id)
        {
            var order = await orderRepository.GetSingleOrderAsync(id);

            if (order is null)
            {
                return new DataResponseInfo<GetOrderDTO>(data: null, success: false, message: "no such order");
            }

            return new DataResponseInfo<GetOrderDTO>(data: mapper.Map<GetOrderDTO>(order), success: true, message: "order");
        }

        public async Task<ResponseInfo> UpdateOrderAsync(UpdateOrderDTO updateOrder)
        {
            var oldOrder = await orderRepository.GetSingleOrderAsync(updateOrder.Id);

            if (oldOrder is null)
            {
                return new ResponseInfo(success: false, message: "no such order");
            }

            var newOrder = mapper.Map<Order>(updateOrder);
            await orderRepository.UpdateOrderAsync(newOrder);

            return new ResponseInfo(success: true, message: "order updated");
        }

        public async Task<ResponseInfo> AddOrderAsync(AddOrderDTO addOrder)
        {
            var newOrder = mapper.Map<Order>(addOrder);
            await orderRepository.AddOrderAsync(newOrder);

            return new ResponseInfo(success: true, message: "order added");
        }

        public async Task<ResponseInfo> PayOrderAsync(int id, string confirmationEmail, HttpRequest httpRequest)
        {
            var order = await orderRepository.GetSingleOrderAsync(id);

            if (order is null)
            {
                return new ResponseInfo(success: false, message: "no such order");
            }

            var code = Guid.NewGuid().ToString();

            var orderPaymentConfirmation = new OrderPaymentConfirmation()
            {
                OrderId = order.Id,
                Code = code
            };

            await orderPaymentConfirmationRepository.AddOrderPaymentConfirmationAsync(orderPaymentConfirmation);

            var url = $"{httpRequest.Scheme}://{httpRequest.Host}/Orders/ConfirmOrderPayment?id={order.Id}&code={code}";
            var message = $"{confirmationEmail} {url}";

            rabbitMQMessagingService.PublishMessage("email-confirmation", message);

            return new ResponseInfo(success: true, message: "confirm payment");
        }

        public async Task<ResponseInfo> ConfirmOrderPaymentAsync(int id, string confirmationEmail, string code)
        {
            var order = await orderRepository.GetSingleOrderAsync(id);

            var orderPaymentConfirmation = await orderPaymentConfirmationRepository.GetOrderPaymentConfirmationAsync(order.Id, code);

            if (orderPaymentConfirmation.Code != code)
            {
                return new ResponseInfo(success: false, message: "wrong confirmation code");
            }

            var message = order.UserId + " " + order.TotalPrice.ToString() + " " + confirmationEmail;

            rabbitMQMessagingService.PublishMessage("user-payment", message);

            return new ResponseInfo(success: true, message: "await for bank response");
        }

        public async Task<ResponseInfo> DeleteOrderAsync(int id)
        {
            var order = await orderRepository.GetSingleOrderAsync(id);

            if (order is null)
            {
                return new ResponseInfo(success: false, message: "no such order");
            }

            await orderRepository.DeleteOrderAsync(id);

            return new ResponseInfo(success: true, message: "order deleted");
        }

        public async Task<DataResponseInfo<List<GetOrderDTO>>> GetAllOrdersByUserIdAsync(int id, int pageNumber, int pageSize)
        {
            var userOrders = await orderRepository.GetAllOrdersByUserId(id, pageNumber, pageSize);
            var orderDTOs = userOrders.Select(mapper.Map<GetOrderDTO>).ToList();

            return new DataResponseInfo<List<GetOrderDTO>>(data: orderDTOs, success: true, message: "all orders");
        }
    }
}
