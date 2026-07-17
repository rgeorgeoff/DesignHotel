using System;
using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
    public class TestLinqScript
    {
        static void Main()
        {
            List<string> myCars = new List<string>
            {
                "Mercury Cougar",
                "Dodge Dart",
                "Ford Taurus SHO",
                "Dodge Charger",
                "Chevrolet Blazer",
                "Dodge Neon"
            };

            // ReSharper disable once SuggestVarOrType_Elsewhere
            var dodges = from someCars in myCars
                where someCars.Contains("Dodge")
                orderby someCars descending
                select someCars;

            foreach (var car in dodges)
            {
                Console.WriteLine(car);
            }

            Console.WriteLine("\nThe collection type is: \n{0}", dodges.GetType());
        }
    }
}