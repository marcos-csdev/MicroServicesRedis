using AutoMapper;
using Discount.gRPC.Models;
using Discount.gRPC.Protos;
using Discount.gRPC.Repositories;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Discount.gRPC.Services
{

    public class DiscountService(IDiscountRepository repository, Serilog.ILogger logger, IMapper mapper) : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository = repository;
        private readonly Serilog.ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;

        private void DisplayError(string message, StatusCode statusCode)
        {
            _logger.Error(message);
            throw new RpcException(
                new Status(statusCode, message)
            );
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            if (request == null) return null!;
            var coupon = await _repository.GetDiscountAsync(request.ProductName);
            if (coupon == null)
            {
                var message = $"Discount with Product {request.ProductName} was not found";
                DisplayError(message, StatusCode.NotFound);

            }

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;

        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            if (request == null) return null!;
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var isCreated = await _repository.CreateDiscountAsync(coupon);

            if (isCreated == false)
            {
                var message = $"Could not create coupon {coupon.ProductName} ";
                DisplayError(message, StatusCode.Internal);
            }

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;

        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            if (request == null) return null!;

            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var isUpdated = await _repository.UpdateDiscountAsync(coupon);

            if (isUpdated == false)
            {
                var message = $"Could not update coupon {coupon.ProductName} ";
                DisplayError(message, StatusCode.Internal);
            }

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            if (request == null) return null!;

            var isDeleted = await _repository.DeleteDiscountAsync(request.ProductName);

            if (isDeleted == false)
            {
                var message = $"Could not update coupon {request.ProductName} ";
                DisplayError(message, StatusCode.Internal);
            }

            var response = new DeleteDiscountResponse
            {
                Success = isDeleted
            };

            return response;
        }
    }
}
