using System;

namespace SPA.Pkb.Exceptions
{
    public class PkbException : Exception
    {
        public PkbException()
        {
        }

        public PkbException(string message)
            : base(message)
        {
        }

        public PkbException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}