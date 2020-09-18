using System;

namespace eShopSolution.Utilities.Exceptions
{
    public class eShopSolutionException : Exception
    {
        public eShopSolutionException() { }

        public eShopSolutionException(string message) : base(message) { }

        public eShopSolutionException(string message, Exception inner) : base(message, inner) { }
    }
}
