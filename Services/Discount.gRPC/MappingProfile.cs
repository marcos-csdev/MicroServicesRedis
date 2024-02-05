using AutoMapper;
using Discount.gRPC.Models;
using Discount.gRPC.Protos;

namespace Discount.gRPC
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
