using System;
using System.Collections.Generic;

using oht.Entities;

namespace ConsoleTestOHT
{
    class Program
    {
        static void Main(string[] args)
        {
            var _api = new oht.OhtApi("e42086528b31f89260929a2c8c0b798e", "3xTNDt7h9CfqLbWF2HmK", true);

            List<string> resourcesList = new List<string>();
            resourcesList.Add("rsc-58b7c9c2c31203-03874442");
            var result = (Quote)_api.GetQuote(resourcesList, 0, "en-us", "it-it");
            Console.WriteLine(string.Format("Quote received. Currency: {0}, word count: {1}, price: {2}.", result.Currency, result.Total.Wordcount, result.Total.Price));
            Console.ReadLine();

        }
    }
}
