﻿using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    [Validator(typeof(LoginValidator))]
    public class LoginView
    {
       
            public string UserEmail { get; set; }

            public string UserPassword { get; set; }

        }
    }
