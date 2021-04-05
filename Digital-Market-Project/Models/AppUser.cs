﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Digital_Market.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        public int Age { get; set; }
    }
}
