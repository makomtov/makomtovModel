﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class UserValidator: AbstractValidator<UserDetailsView>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserFirstName).NotEmpty().WithMessage("שם פרטי אינו יכול להיות ריק")
                                        .Length(0, 20).WithMessage("שפ פרטי אינו יכול להיות יותר מ 20 תווים");

            //RuleFor(x => x.LastName).NotEmpty().WithMessage("The Last Name cannot be blank.");

            //RuleFor(x => x.BirthDate).LessThan(DateTime.Today).WithMessage("You cannot enter a birth date in the future.");

            //RuleFor(x => x.Username).Length(8, 999).WithMessage("The user name must be at least 8 characters long.");
        }
    }

    public class LoginValidator : AbstractValidator<LoginView>
    {



        public LoginValidator()
        {
            RuleFor(x => x.UserEmail).NotEmpty().WithMessage("מייל אינו יכול להיות ריק");
                                        //.Length(0, 20).WithMessage("שפ פרטי אינו יכול להיות יותר מ 20 תווים");

            //RuleFor(x => x.LastName).NotEmpty().WithMessage("The Last Name cannot be blank.");

            //RuleFor(x => x.BirthDate).LessThan(DateTime.Today).WithMessage("You cannot enter a birth date in the future.");

            //RuleFor(x => x.Username).Length(8, 999).WithMessage("The user name must be at least 8 characters long.");
        }
    }
}