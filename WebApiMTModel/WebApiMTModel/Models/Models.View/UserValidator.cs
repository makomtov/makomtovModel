using FluentValidation;
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
            RuleFor(x => x.UserFirstName).NotEmpty().WithErrorCode("Empty");
            //RuleFor(x => x.UserFirstName).NotEmpty().WithMessage("שם פרטי אינו יכול להיות ריק")
            //                            .Length(0, 20).WithMessage("שם פרטי אינו יכול להיות יותר מ 20 תווים");

            RuleFor(x => x.UserLastName).NotEmpty().WithErrorCode("Empty"); //.WithMessage("שם משפחה אינו יכול להיות ריק");
           // RuleFor(x => x.UserEmail).Must(checkUserExist).WithErrorCode("112");
            RuleFor(x => x.UserEmail).EmailAddress();
            RuleFor(x => x.UserLastName).NotEmpty();
            RuleFor(x => x.UserCityName).Must(checkCityExist);
            RuleFor(x => x.UserPhone1).NotEmpty().Matches(@"0\d{8}");
            RuleFor(x => x.UserPhone2).Matches(@"0\d{8}");
            RuleFor(x => x.UserAddress).NotEmpty();
            RuleFor(x => x.UserPaswrd).NotEmpty();
            RuleFor(x => x.UserStatus).NotEmpty();
            RuleFor(x => x.VeterinarName).NotEmpty();
            RuleFor(x => x.VeterinarPhone1).NotEmpty().Matches(@"^ 0\d{8}$|^0[5,7]{1}\d{8}$");

           // RuleFor(x => x.UserPaswrd).Matches();
          // RuleFor(x => x.).LessThan(DateTime.Today).WithMessage("You cannot enter a birth date in the future.");

            //RuleFor(x => x.Username).Length(8, 999).WithMessage("The user name must be at least 8 characters long.");
        }

        private bool checkUserExist(UserDetailsView user,string mail)
        {
            Userservice userservice = new Userservice();
            return !userservice.GetUserByMail(mail);

        }
        private bool checkCityExist(UserDetailsView user, string City)
        {
            XmlService xmlService = new XmlService();
            return !xmlService.CheckCity(City);

        }
    }
   
    public class LoginValidator : AbstractValidator<LoginView>
    {



        public LoginValidator()
        {
            
            RuleFor(x => x.UserEmail).NotEmpty().WithMessage("מייל אינו יכול להיות ריק").EmailAddress().WithMessage("מייל לא חוקי");
            RuleFor(x => x.UserPassword);                          

            //RuleFor(x => x.LastName).NotEmpty().WithMessage("The Last Name cannot be blank.");

            //RuleFor(x => x.BirthDate).LessThan(DateTime.Today).WithMessage("You cannot enter a birth date in the future.");

            //RuleFor(x => x.Username).Length(8, 999).WithMessage("The user name must be at least 8 characters long.");
        }
    }

    public class UserdogsValidator : AbstractValidator<DogsForManagerView>
    {



        public UserdogsValidator()
        {

            RuleFor(x => x.UserDogs).NotEmpty();
            
            //RuleFor(x => x.LastName).NotEmpty().WithMessage("The Last Name cannot be blank.");

            //RuleFor(x => x.BirthDate).LessThan(DateTime.Today).WithMessage("You cannot enter a birth date in the future.");

            //RuleFor(x => x.Username).Length(8, 999).WithMessage("The user name must be at least 8 characters long.");
        }
    }
    public class dogValidator : AbstractValidator<DogDetailsView>
    {
        public dogValidator()
        {
            RuleFor(x => x.DogBirthDate).NotEmpty().LessThan(DateTime.Today);
            RuleFor(x => x.DogFriendlyWith).NotEmpty();
            RuleFor(x => x.DogGender).Must(checkGenderValues);
            RuleFor(x => x.DogName).NotEmpty();
            RuleFor(x => x.DogRabiesVaccine).NotEmpty().LessThan(DateTime.Today);
            RuleFor(x => x.DogStatus).NotEmpty();
            RuleFor(x => x.DogType).NotEmpty();
            RuleFor(x => x.DogUserID).Must(checkUserExist);
           
        }
        private bool checkGenderValues(DogDetailsView dog, string gender)
        {
            return (gender == "זכר" || gender == "נקבה");
                

        }
        private bool checkUserExist(DogDetailsView dog, int  userid)
        {
            Userservice userservice = new Userservice();
            return userservice.GetUser(userid)!=null;

        }
    }

}