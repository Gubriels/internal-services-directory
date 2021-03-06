﻿using System.Data;
using FluentValidation;
using MultCo_ISD_API.Models;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class LocationTypeV1DTOValidator : AbstractValidator<LocationTypeV1DTO>
    {
        public LocationTypeV1DTOValidator()
        {
            RuleFor(x => x.LocationTypeId)
                .Empty().WithMessage("cannot specify LocationTypeId explicitly");

            RuleFor(x => x.LocationTypeName)
                .MaximumLength(255).WithMessage("Location type name cannot exceed 255 characters");
        }
    }
}
