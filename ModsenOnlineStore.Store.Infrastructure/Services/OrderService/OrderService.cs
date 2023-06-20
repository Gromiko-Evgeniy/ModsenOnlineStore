﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModsenOnlineStore.Common;
using ModsenOnlineStore.Login.Application.Interfaces;
using ModsenOnlineStore.Login.Domain.Entities;
using ModsenOnlineStore.Store.Application.Interfaces.OrderInterfaces;
using ModsenOnlineStore.Store.Domain.DTOs.OrderDTOs;
using ModsenOnlineStore.Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModsenOnlineStore.Store.Infrastructure.Services.OrderService
{
        public class OrderService : IOrderService
        {
            private IMapper mapper;
            private IOrderRepository orderRepository;

            public OrderService(IMapper mapper, IOrderRepository repository)
            {
                this.mapper = mapper;
                this.orderRepository = repository;
            }

            public async Task<ResponseInfo<List<GetOrderDTO>>> GetAllOrders()
            {
                var orders = await orderRepository.GetAllOrders();
                var orderDTOs = orders.Select(p => mapper.Map<GetOrderDTO>(p)).ToList();
                if (orderDTOs.Count == 0)
                {
                    return new ResponseInfo<List<GetOrderDTO>>(orderDTOs, true, "no orders yet");
                }
                return new ResponseInfo<List<GetOrderDTO>>(orderDTOs, true, "all orders");
            }

            public async Task<ResponseInfo<GetOrderDTO>> GetSingleOrder(int id)
            {
                var order = await orderRepository.GetSingleOrder(id);
                if (order is null)
                {
                    return new ResponseInfo<GetOrderDTO>(null, false, "order not found");
                }
                return new ResponseInfo<GetOrderDTO>(mapper.Map<GetOrderDTO>(order), true, $"order with id {id}");
            }

            public async Task<ResponseInfo<string>> AddOrder(AddOrderDTO addOrder)
            {
                var newOrder = mapper.Map<Order>(addOrder);
                await orderRepository.AddOrder(newOrder);
                return new ResponseInfo<string>(data: "added successfully", success: true, message: "order");
            }

            public async Task<ResponseInfo<string>> UpdateOrder(UpdateOrderDTO updateOrder)
            {
                var newOrder = mapper.Map<Order>(updateOrder);
                await orderRepository.UpdateOrder(newOrder);
                return new ResponseInfo<string>(data: "updated successfully", success: true, message: "order");
            }


            public async Task<ResponseInfo<string>> DeleteOrder(int id)
            {
                await orderRepository.DeleteOrder(id);
                return new ResponseInfo<string>(data: "deleted successfully", success: true, message: "order");
            }
        }

}
