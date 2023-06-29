﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModsenOnlineStore.LogService.Application.Interfaces;
using ModsenOnlineStore.LogService.Domain.Entities;
using ModsenOnlineStore.LogService.Infrastructure.Services;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModsenOnlineStore.LogService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogger logger;
        public LogController(ILoggerFactory loggerFactory, ILogRepository repository)
        {
            loggerFactory.AddProvider(new DBLoggerProvider(repository));

            logger = loggerFactory.CreateLogger("");
        }

        [HttpPost]
        public IActionResult Index( int eventId, string context) { // exception not in use now
            Func<string, Exception?, string> formatter = delegate (string state, Exception? exception)
            {
                var message = $"context: {context},";
                if (exception == null) message += "no errors";
                else message += $"error: {exception.Message}";

                return message;
            };

            LogLevel logLevel = LogLevel.Information;

            logger.Log(logLevel, eventId, context, null, formatter);

            return Ok("log added");
        }
    }
}
