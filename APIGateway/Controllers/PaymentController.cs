using System.Threading.Tasks;
using APIGateway.Interfaces;
using APIGateway.Models;
using APIGateway.Models.DB;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentValidationService _validation;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        public PaymentController(IPaymentValidationService validation, IPaymentService paymentService, IMapper mapper)
        {
            _validation = validation;
            _paymentService = paymentService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment([FromBody]PaymentDetails paymentDetails)
        {
            if (await _validation.ValidatePaymentDetails(paymentDetails))
            {
                var payment = _mapper.Map<PaymentDetails, Payment>(paymentDetails);
                await _paymentService.ForwardPayment(payment);
                var response = _mapper.Map<Payment, PaymentMerchantResponse>(payment);
                return Ok(response);
            }
            
            return BadRequest("Payment validation failed");
        }

        [HttpGet]
        public async Task<IActionResult> GetPayment([FromQuery]int id)
        {
            var payment = await _paymentService.GetPayment(id);
            if (payment == null)
                return BadRequest($"Id {id} does not exist");
            var paymentResponse = _mapper.Map<Payment, PaymentEnquiryResponse>(payment);

            return Ok(paymentResponse);
        }
    }
}