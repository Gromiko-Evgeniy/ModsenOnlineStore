﻿using Microsoft.EntityFrameworkCore;
using ModsenOnlineStore.LogService.Application.Interfaces;
using ModsenOnlineStore.LogService.Domain.Entities;

namespace ModsenOnlineStore.LogService.Infrastructure.Data
{
    public class LogRepository : ILogRepository
    {
        private readonly DataContext context;
        public LogRepository(DataContext context) { 
            this.context = context;
        }

        public async Task AddLogAsync(Log log)
        {
            context.Logs.Add(log);
            await context.SaveChangesAsync();
        }

        public async Task<List<Log>> GetAllLogsAsync() =>
            await context.Logs.AsNoTracking().ToListAsync();

        public async Task<Log> GetLogByIdAsync(int id) =>
            await context.Logs.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }
}
