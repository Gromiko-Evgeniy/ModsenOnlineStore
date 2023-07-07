using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ModsenOnlineStore.Store.Application.Interfaces.OrderInterfaces;
using ModsenOnlineStore.Store.Domain.DTOs.OrderDTOs;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ModsenOnlineStore.Store.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IConfiguration configuration;

        public OrdersController(IOrderService orderService, IConfiguration configuration)
        {
            this.configuration = configuration; 
            this.orderService = orderService;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var response = await orderService.GetAllOrders();
            
            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetSingleOrderAsync(int id)
        {
            var response = await orderService.GetSingleOrder(id);

            if (!response.Success)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Data);
        }

        
        [HttpPost]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> AddOrder(AddOrderDTO order) // send mail, save code to database
        {

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("https://localhost:7107/");
            //    var response = await client.PostAsJsonAsync("EmaiLoginlAuthentication", "egrom2002@gmail.com");
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var data = await response.Content.ReadAsStringAsync();

            //        order.PaymentConfirmationCode = data;
            //    }
            //}
            var response = await orderService.AddOrderAsync(order);

            if (!response.Success)
            {
                return NotFound(response.Message);
            }
            return Ok(response.Message);
        }

        [HttpPut("PayOrder/{id}")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> PayOrder(int id)
        {
            string userEmail = "egrom2002@gmail.com"; // �������� �� jwt

            var response = await orderService.PayOrderAsync(id, userEmail, HttpContext!.Request);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


        [HttpPost]
        [Route("ConfirmOrderPayment")]
        public async Task<IActionResult> ConfirmOrderPaymentAsync(int id, string code)
        {
            string userEmail = "egrom2002@gmail.com"; // �������� �� jwt

            var response = await orderService.ConfirmOrderPaymentAsync(id, userEmail, code);

            if (!response.Success)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Message);
        }

        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateOrderAsync(UpdateOrderDTO order)
        {
            var response = await orderService.UpdateOrderAsync(order);

            if (!response.Success)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Message);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            var response = await orderService.DeleteOrderAsync(id);

            if (!response.Success)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Message);
        }

        [HttpGet("byUser{id}")]
        [Authorize]
        public async Task<IActionResult> GetAllOrdersByUserIdAsync(int id)
        {
            var response = await orderService.GetAllOrdersByUserIdAsync(id);
                
            if (!response.Success)
            {
                return NotFound(response.Message);
            }
            
            return Ok(response.Data);
        }
    }
}
