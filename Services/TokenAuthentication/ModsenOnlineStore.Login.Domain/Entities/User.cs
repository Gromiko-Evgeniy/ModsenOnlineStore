﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ModsenOnlineStore.Login.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
      
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public decimal Money { get; set; }
      
        public bool IsEmailConfirmed { get; set; } = false;

        public Role Role { get; set; } = Role.User;
    }

    public enum Role
    {
        User,
        Admin
    }
}
