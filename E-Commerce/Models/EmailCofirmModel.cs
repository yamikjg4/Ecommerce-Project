﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class EmailCofirmModel
    {
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
        public bool EmailSent { get; set; }
        public bool EmailVerified { get; set; }
    }
}
