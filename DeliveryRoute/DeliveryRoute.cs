using System;
using System.Collections.Generic;
using System.Text;

using NLog;

using DeliveryRouteHelper;
using DeliveryRouteHelper.Util;


namespace DeliveryRoute
{
    public static class DeliveryRoute
    {
        private static Logger logger;

        static void Main(string[] args)
        {
            // Use a logging library to easily manage a program output for different configurations
            var nlogconfig = new NLog.Config.LoggingConfiguration();
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            nlogconfig.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            LogManager.Configuration = nlogconfig;
            logger = LogManager.GetCurrentClassLogger();

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
            byte[][][] TokyoWards =
            {
                new byte[][] {Encoding.UTF8.GetBytes("Минато"), Encoding.UTF8.GetBytes("Эдогава")},
                new byte[][] {Encoding.UTF8.GetBytes("Тюо"), Encoding.UTF8.GetBytes("Нэрима")},
                new byte[][] {Encoding.UTF8.GetBytes("Ота"), Encoding.UTF8.GetBytes("Синдзюку")},
                new byte[][] {Encoding.UTF8.GetBytes("Эдогава"), Encoding.UTF8.GetBytes("Ота")},
                new byte[][] {Encoding.UTF8.GetBytes("Сибуя"), Encoding.UTF8.GetBytes("Итабаси")},
                new byte[][] {Encoding.UTF8.GetBytes("Синдзюку"), Encoding.UTF8.GetBytes("Тосима")},
                new byte[][] {Encoding.UTF8.GetBytes("Нэрима"), Encoding.UTF8.GetBytes("Минато")},
                new byte[][] {Encoding.UTF8.GetBytes("Бункё"), Encoding.UTF8.GetBytes("Тюо")},
                new byte[][] {Encoding.UTF8.GetBytes("Тосима"), Encoding.UTF8.GetBytes("Кото")},
                new byte[][] {Encoding.UTF8.GetBytes("Итабаси"), Encoding.UTF8.GetBytes("Бункё")}
            };

            HashSet<Segment> inputData;


            // =============== London ===============
            Route route = new Route("Лондон");
            try
            {
                inputData = Util.ConvertData(LondonAreas);
                logger.Info($"Data {LondonAreas.GetType()} {nameof(LondonAreas)} is converted");
                route.AcceptSegments(inputData);
            }
            catch (Exception ex)
            {
                logger.Error($"Cannot feed input {LondonAreas.GetType()} {nameof(LondonAreas)}: {ex}");
                return;
            }
            try
            {
                route.Arrange();
                logger.Info($"Segments {LondonAreas.GetType()} {nameof(LondonAreas)} were arranged successfully");
            }
            catch (Exception ex)
            {
                logger.Error($"Cannot arrange {LondonAreas.GetType()} {nameof(LondonAreas)}: {ex}");
                return;
            }
            Console.WriteLine(route);
            route.Reset();


            // =============== Tokyo ===============
            route.Name = "Токио";
            try
            {
                inputData = Util.ConvertData(TokyoWards);
                logger.Info($"Data {TokyoWards.GetType()} {nameof(TokyoWards)} is accepted");
                route.AcceptSegments(inputData);
            }
            catch (Exception ex)
            {
                logger.Error($"Cannot feed input {TokyoWards.GetType()} {nameof(TokyoWards)}: {ex}");
                return;
            }
            try
            {
                route.Arrange();
                logger.Info($"Segments {TokyoWards.GetType()} {nameof(TokyoWards)} were arranged successfully");
            }
            catch (Exception ex)
            {
                logger.Error($"Cannot arrange {TokyoWards.GetType()} {nameof(TokyoWards)}: {ex}");
                return;
            }
            Console.WriteLine(route);


            Console.WriteLine("Let's reverse the route:");
            route.Reverse();
            Console.WriteLine(route);


            Console.WriteLine("You can use Enumerator to fetch the segments one by one continuously:");
            IEnumerator<Segment> routeEnumerator = route.GetEnumerator();
            routeEnumerator.MoveNext();
            Console.WriteLine(routeEnumerator.Current);
            routeEnumerator.MoveNext();
            Console.WriteLine(routeEnumerator.Current);
            routeEnumerator.MoveNext();
            Console.WriteLine(routeEnumerator.Current);
            Console.WriteLine("...");
            Console.WriteLine("or to extract the resulted route as a whole in a loop\n");


            Console.WriteLine("Query public properties to get some status information:");
            Console.WriteLine($"DataAccepted: {route.DataAccepted}");
            Console.WriteLine($"Arranged: {route.Arranged}\n");

            Console.WriteLine("If you want to get the next point itself and not the segment use GetRouteAsPoints() method:");
            foreach (Point point in route.GetRouteAsPoints())
            {
                Console.Write($"{point} → ");
            }
            Console.WriteLine("\b\b ");
        }
    }
}
