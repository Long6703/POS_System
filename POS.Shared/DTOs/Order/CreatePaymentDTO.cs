﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.Order
{
    public class CreatePaymentDTO
    {
        public Guid OrderId { get; set; }
        public int PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
    }
}
