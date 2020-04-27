﻿using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class ProgramValidator : AbstractValidator<ProgramV1DTO>
    {
        public ProgramValidator()
        {
            //RuleFor(x => x.ProgramID).NotNull();
            RuleFor(x => x.SponsorName).NotNull().MaximumLength(20);
            RuleFor(x => x.OfferType).NotNull().MaximumLength(20);
        }
    }
}
