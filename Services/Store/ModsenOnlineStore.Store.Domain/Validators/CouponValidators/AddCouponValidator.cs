﻿using FluentValidation;
using ModsenOnlineStore.Store.Domain.DTOs.CouponDTO;

namespace ModsenOnlineStore.Store.Domain.Validators.CouponValidators
{
    public class AddCouponValidator : AbstractValidator<AddCouponDTO>
    {
        public AddCouponValidator()
        {
            RuleFor(x => x.Discount).ExclusiveBetween(0, 100);
        }
    }
}
