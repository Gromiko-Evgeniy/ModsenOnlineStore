﻿using ModsenOnlineStore.Login.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModsenOnlineStore.Login.Infrastructure.Services
{
    public class EncryptionService : IEncryptionService
    {
        public string HashPassword(string password)
        {
            var sha = System.Security.Cryptography.SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);

            return Convert.ToBase64String(hashedPassword);
        }
    }
}