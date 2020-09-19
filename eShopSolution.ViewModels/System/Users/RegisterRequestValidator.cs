using FluentValidation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class RegisterRequestValidator:AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required").MaximumLength(200).WithMessage("FirstName can not over 200 characters");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required").MaximumLength(200).WithMessage("LastName can not over 200 characters");
            RuleFor(x => x.DOB).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Birthday cannot greater than 100 years");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email is not valid");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required").MinimumLength(6).WithMessage("Password is at least 6 characters");
            RuleFor(x => x).Custom((request, context) => {
                if(request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Confirm Password is not match");
                }

            });
        }
    }
}
