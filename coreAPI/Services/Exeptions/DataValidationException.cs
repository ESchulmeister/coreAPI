﻿using System;

namespace coreAPI.Services
{
    public class DataValidationException : Exception
    {
        public DataValidationException()
        {

        }
        public DataValidationException(string sMessage) : base(sMessage)
        {

        }
        public DataValidationException(Exception oException) : base(oException.Message, oException)
        {

        }
    }
}
