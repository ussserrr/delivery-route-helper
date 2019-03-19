using System;
using System.Collections.Generic;
using System.Text;
using DeliveryRouteHelper;


namespace DeliveryRoute
{
    class DeliveryRoute
    {
        static void Main(string[] args)
        {
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
            //HashSet<string[]> m = new HashSet<string[]>(LondonAreas);
            //foreach (string[] s in m)
            //{
            //    Console.WriteLine("{0}, {1}", s[0], s[1]);
            //}
            //return;

            Route route = new Route("Лондон");

            //IEnumerator<Segment> computedRoute = route.GetRouteEnumerator();
            //computedRoute.MoveNext();
            //Console.WriteLine(computedRoute.Current);

            //try
            //{
                //route.AcceptData(LondonAreas);
            //}
            //catch (InvalidSegmentException ex)
            //{
            //    Console.WriteLine("Cannot feed input");
            //    Console.WriteLine(ex);
            //    return;
            //}
            //try
            //{
            //    route.Arrange();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //    return;
            //}
            //Console.WriteLine(route);
            //route.Reverse();
            //Console.WriteLine(route);
            route.Reset();

            //route.Name = "Токио";
            route.AcceptData(LondonAreas);
            route.Arrange();
            Console.WriteLine(route);
            route.Reverse();
            Console.WriteLine(route);
            route.Reset();

            //List<MyClass> list = new List<MyClass>();
            //list.Add(new MyClass(5));
            //Console.WriteLine(list[0].Value);
            //list[0].Method();
            //Console.WriteLine(list[0].Value);
        }

        //class MyClass
        //{
        //    public int Value;

        //    public MyClass(int value)
        //    {
        //        Value = value;
        //    }

        //    public void Method()
        //    {
        //        Value += 10;
        //    }
        //}
    }
}
