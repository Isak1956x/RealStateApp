using FluentValidation;
using RealStateApp.Core.Application.Features.Common.GenericCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Common.GenericValidations
{
    public class CreateCommandValidation<TComm> : AbstractValidator<CreateResourceCommand<TComm>> 
    {
        public CreateCommandValidation()
        {
            RuleFor(x => x).NotNull().WithMessage("El objeto no puede ser nulo");
            RuleFor(x => x.Name).NotNull().WithMessage("El nombre no puede ser nulo")
                                .NotEmpty().WithMessage("El nombre no puede estar vacío")
                                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");
            RuleFor(x => x.Description).NotNull().WithMessage("La descripción no puede ser nula")
                                        .NotEmpty().WithMessage("La descripción no puede estar vacía")
                                        .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres");
        }
    }
}
