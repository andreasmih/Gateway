﻿using System.Threading.Tasks;
using APIGateway.Models;
using APIGateway.Models.DB;

namespace APIGateway.Interfaces
{
    public interface IBankPayment
    {
        Task<BankPaymentResponse> SendPayment(Payment payment);
    }
}
