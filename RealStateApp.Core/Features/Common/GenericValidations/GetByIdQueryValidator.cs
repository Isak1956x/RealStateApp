using FluentValidation;
using RealStateApp.Core.Application.Features.Common.GennericQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Common.GenericValidations
{
    public class GetByIdQueryValidator<Tid, Tdto> : AbstractValidator<GetByIdQuery<Tid, Tdto>>
    {
        public GetByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id cannot be empty.")
                .NotNull().WithMessage("Id cannot be null.");
        }
    }
}
