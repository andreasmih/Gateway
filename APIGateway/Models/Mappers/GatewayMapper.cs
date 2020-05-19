using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Models.DB;
using AutoMapper;

namespace APIGateway.Models.Mappers
{
    public class GatewayMapper : Profile
    {
        public GatewayMapper()
        {
            CreateMap<PaymentDetails, Payment>()
                .ForMember(o => o.Amount, s => s.MapFrom(o => o.Amount))
                .ForMember(o => o.CardNumber, s => s.MapFrom(o => o.CardNumber))
                .ForMember(o => o.Currency, s => s.MapFrom(o => o.Currency))
                .ForMember(o => o.Cvv, s => s.MapFrom(o => o.Cvv))
                .ForMember(o => o.ExpiryDate, s => s.MapFrom(o => o.ExpiryDate))
                .ForPath(o => o.MerchantId, s => s.MapFrom(o => o.MerchantId));

            CreateMap<Payment, PaymentMerchantResponse>()
                .ForMember(o => o.BankReference, s => s.MapFrom(o => o.BankUniqueId))
                .ForMember(o => o.Status, s => s.MapFrom(o => o.Status))
                .ForMember(o => o.Id, s => s.MapFrom(o => o.Id));

            CreateMap<Payment, PaymentEnquiryResponse>()
                .ForMember(o => o.Status, s => s.MapFrom(o => o.Status))
                .ForMember(o => o.Amount, s => s.MapFrom(o => o.Amount))
                .ForMember(o => o.Currency, s => s.MapFrom(o => o.Currency))
                .ForMember(o => o.Cvv, s => s.MapFrom(o => o.Cvv))
                .ForMember(o => o.ExpiryDate, s => s.MapFrom(o => o.ExpiryDate))
                .ForMember(o => o.MaskedCardNumber, s => s.MapFrom(o => MaskCardNumber(o.CardNumber)));

        }

        private string MaskCardNumber(string cardNo)
        {
            var result = "";
            var length = cardNo.Length;
            var digindex = 0;
            foreach (var dig in cardNo)
            {
                if (digindex + 4 < length)
                    result += "*";
                else
                {
                    result += dig;
                }

                digindex++;
            }

            return result;
        }
    }
}
