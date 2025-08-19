using FluentValidation;
using RealStateApp.Core.Application.Features.Properties.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Properties.Validations
{
    public class GetPropertyByCodeQueryValidator : AbstractValidator<GetPropertyByCodeQuery>
    {
        public GetPropertyByCodeQueryValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("The property code must not be empty.")
                .Length(1, 50).WithMessage("The property code must be between 1 and 50 characters long.");
        }
    }
}
