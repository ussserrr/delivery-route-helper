using System;
using System.Collections.Generic;
using System.Text;
using DeliveryRouteHelper;
using NLog;

namespace DeliveryRoute
{
    class DeliveryRoute
    {
        private static Logger logger;

        static void Main(string[] args)
        {
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

            Route route = new Route("Лондон");
            try
            {
                route.AcceptData(LondonAreas);
                logger.Info($"Data {LondonAreas.GetType()} {nameof(LondonAreas)} is accepted");
            }
            catch (InvalidSegmentException ex)
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

            route.Name = "Токио";
            try
            {
                route.AcceptData(TokyoWards);
                logger.Info($"Data {TokyoWards.GetType()} {nameof(TokyoWards)} is accepted");
            }
            catch (InvalidSegmentException ex)
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

            Console.WriteLine("You can reverse the route:");
            route.Reverse();
            Console.WriteLine(route);

            Console.WriteLine($"You can use Enumerator to fetch the segments one by one continuously:");
            IEnumerator<Segment> routeEnumerator = route.GetEnumerator();
            routeEnumerator.MoveNext();
            Console.WriteLine(routeEnumerator.Current);
            routeEnumerator.MoveNext();
            Console.WriteLine(routeEnumerator.Current);
            routeEnumerator.MoveNext();
            Console.WriteLine(routeEnumerator.Current);
            Console.WriteLine("or to extract the resulted route as a whole");
        }
    }
}
