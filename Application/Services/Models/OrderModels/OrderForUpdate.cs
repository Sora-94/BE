﻿using Application.Services.Models.OrderModels.Base;
using FluentValidation;
using System.Text.Json.Serialization;

namespace Application.Services.Models.OrderModels
{
    public class OrderForUpdate : OrderBaseDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }
    public class OrderForUpdateValidator : AbstractValidator<OrderForUpdate>
    {
        public OrderForUpdateValidator()
        {
            RuleFor(x => x.Status)
                .NotNull().WithMessage("Trạng Thái Không Được Để Trống")
                .NotEmpty().WithMessage("Tên Trạng Thái Là Bắt Buộc");
            RuleFor(x => x.OrderDate)
                .NotNull().WithMessage("Ngày Đặt Hàng Không Được Để Trống")
                .NotEmpty().WithMessage("Ngày Đặt Hàng Là Bắt Buộc");
        }
    }
}
  