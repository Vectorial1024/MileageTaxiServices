using System;

namespace MileageTaxiServices
{
    public class StrangeTaxiFareException : Exception
    {
        public StrangeTaxiFareException(string message) : base(message)
        {
        }
    }
}
