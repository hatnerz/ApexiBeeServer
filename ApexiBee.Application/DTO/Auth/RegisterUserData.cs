﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.DTO
{
    public class RegisterUserData
    {
        public string? Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
    }
}
