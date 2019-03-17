using System;
using DeliveryRouteHelper;

namespace DeliveryRoute
{
    class Program
    {
        static void Main(string[] args)
        {
            // for our case we make simple array
            string[][] LondonAreas =
            {
                new string[] { "Теддингтон", "Ноттинг-Хилл" },
                new string[] { "Сити", "Вестминстер" },
                new string[] { "Ноттинг-Хилл", "Южный Кенсингтон" },
                new string[] { "Детфорд", "Фулем" },
                new string[] { "Гринвич", "Сербитон" },
                new string[] { "Челси", "Блумсбери" },
                new string[] { "Южный Кенсингтон", "Челси" },
                new string[] { "Сербитон", "Детфорд" },
                new string[] { "Вестминстер", "Теддингтон" },
                new string[] { "Блумсбери", "Гринвич" }
            };

            Route route = new Route("Лондон");
            route.AcceptData(LondonAreas);
            route.Arrange();
            Console.WriteLine(route);
        }
    }
}
