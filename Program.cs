using System;
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCacheExample
{
    class Program
    {
        static void Main(string[] args)
        {
            ExampleSimpleCacheMemory();

        }

        private static void ExampleSimpleCacheMemory()
        {
            var employees = Employees.GetAllEmployees();
            var activeEmployees = employees.Where(x => x.Active.Equals(true)).ToList();
            var inactiveeEmployees = employees.Where(x => x.Active == false).ToList();

            Console.WriteLine("Active Employees:");
            foreach (var item in activeEmployees)
                Console.WriteLine(item.FullName);

            Console.WriteLine();
            Console.WriteLine("Inactive Employees:");
            foreach (var item in inactiveeEmployees)
                Console.WriteLine(item.FullName);

            Console.WriteLine();
            Console.WriteLine("Add both List to Memory Cache");


            //**********************ACTIVE EMPLOYEES************************

            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            string keyActiveEmployees = "ActiveEmpl";
            if (!cache.TryGetValue(keyActiveEmployees, out List<Employees> activeEmployeesCache))
            {
                MemoryCacheEntryOptions options = new()
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(60.0)
                };

                cache.Set(keyActiveEmployees, activeEmployees, options);
            }

            if (cache.TryGetValue(keyActiveEmployees, out List<Employees> _))
                Console.WriteLine("Active Employees List Added to Cache");
            else
                Console.WriteLine("Active Employees List: Error saving in Cache");

            activeEmployeesCache = cache.Get<List<Employees>>(keyActiveEmployees).ToList();

            Console.WriteLine();
            Console.WriteLine("Active Employees from Cache");

            foreach (var item in activeEmployeesCache)
                Console.WriteLine(item.FullName);

            Console.WriteLine();

            //**********************INACTIVE EMPLOYEES************************

            string keyInactiveEmployees = "InactiveEmpl";

            if (!cache.TryGetValue(keyInactiveEmployees, out List<Employees> inactiveEmployeesCache))
            {
                MemoryCacheEntryOptions options = new()
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(60.0)
                };

                cache.Set(keyInactiveEmployees, inactiveeEmployees, options);

            }

            if (cache.TryGetValue(keyInactiveEmployees, out List<Employees> _))
                Console.WriteLine("Inactive Employees List Added to Cache");
            else
                Console.WriteLine("Inactive Employees List Error saving in Cache");

            inactiveEmployeesCache = cache.Get<List<Employees>>(keyInactiveEmployees);

            Console.WriteLine();
            Console.WriteLine("InaActive Employees from Cache");

            foreach (var item in inactiveEmployeesCache)
                Console.WriteLine(item.FullName);

            // Add Expiration Token 
        }
    }

}


/// <summary>
/// List Of employees
/// </summary>
public class Employees
{
    public string FullName { get; set; }
    public bool Active { get; set; }

    public static List<Employees> GetAllEmployees()
    {

        List<Employees> empl = new List<Employees>
                {
                    new Employees { FullName = "Carlos Batista", Active = true },
                    new Employees { FullName = "David Perez", Active = true },
                    new Employees { FullName = "Wilberto Cordero", Active = true },
                    new Employees { FullName = "Ramon Rivera", Active = false },

                };

        return empl;
    }

}
