﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModsenOnlineStore.Login.Application.Interfaces;
using ModsenOnlineStore.Login.Domain.DTOs.UserDTOs;

namespace ModsenOnlineStore.Login.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService service;
        private IEncryptionService encryption;

        public LoginController(ILoginService service, IEncryptionService encryption)
        {
            this.service = service;
            this.encryption = encryption;
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> LoginAsync(LoginData data)
        {
            data.Password = encryption.HashPassword(data.Password);
            var response = await service.GetTokenAsync(data);

            if (response.Data is null) return Unauthorized();

            return Ok(new { access_token = response.Data });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var response = await service.GetAllUsersAsync();
            
            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSingleUserAsync(int id)
        {
            var response = await service.GetUserByIdAsync(id);
            
            if (!response.Success)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        [Route("/Register")]
        public async Task<IActionResult> RegisterUserAsync(AddUserDTO user)
        {
            user.Password = encryption.HashPassword(user.Password);

            var response = await service.RegisterUserAsync(user);

            if (!response.Success)
            {
                return BadRequest();
            }
            
            return Ok(response.Message);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserDTO user)
        {
            user.Password = encryption.HashPassword(user.Password);

            var response = await service.UpdateUserAsync(user);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var response = await service.DeleteUserAsync(id);

            if (!response.Success)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Message);
        }
    }
}
