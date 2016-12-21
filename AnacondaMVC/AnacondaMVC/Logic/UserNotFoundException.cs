﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnacondaMVC.Logic
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message) : base(message)
        {
        }

        public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}