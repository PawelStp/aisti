﻿using System;

namespace SPA.Pkb.Exceptions
{
    public class ParsingException : Exception
    {
        public ParsingException()
        {
        }

        public ParsingException(string message)
            : base(message)
        {
        }

        public ParsingException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}