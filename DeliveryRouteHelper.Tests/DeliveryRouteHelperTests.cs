using System;
using System.Linq;
using Xunit;
using DeliveryRouteHelper;
using System.Collections.Generic;

namespace DeliveryRouteHelper.Tests
{
    public class Point_Tests
    {
        [Fact]
        public void IsEquals()
        {
            Point p1 = new Point("Abc");
            Point p2 = new Point("Abc");
            Assert.True(p1 == p2, $"Point '{p1}' should be equal to '{p2}'");
        }
    }

    public class Segment_Tests
    {
        [Fact]
        public void ReversedCorrectly()
        {
            Point p1 = new Point("Abc");
            Point p2 = new Point("deF");
            Segment s = new Segment(p1, p2);
            s.Reverse();
            Assert.True(s.End == p1 && s.Start == p2, $"Segment {s} isn't reversed");
        }
    }

    public class Route_Tests
    {
        private readonly string[][] LondonSegments_Incorrect_Disrupted =
        {
            new string[] { "Теддингтон", "Ноттинг-Хилл" },
            new string[] { "Сити", "Вестминстер" },
            new string[] { "Ноттинг-Хилл", "Южный Кенсингтон" },
            new string[] { "Детфорд", "Фулем" },
            new string[] { "Гринвич", "Сербитон" },
            //new string[] { "Челси", "Блумсбери" },
            new string[] { "Южный Кенсингтон", "Челси" },
            new string[] { "Сербитон", "Детфорд" },
            new string[] { "Вестминстер", "Теддингтон" },
            new string[] { "Блумсбери", "Гринвич" }
        };
        private readonly string[][] LondonSegments_Incorrect_InvalidSegment =
        {
            new string[] { "Теддингтон", "Ноттинг-Хилл" },
            new string[] { "Сити", "Вестминстер" },
            new string[] { "Ноттинг-Хилл", "Южный Кенсингтон" },
            new string[] { "Детфорд", "Фулем" },
            new string[] { "Гринвич", "Сербитон" },
            new string[] { "BAD BLOCK" },
            new string[] { "Челси", "Блумсбери" },
            new string[] { "Южный Кенсингтон", "Челси" },
            new string[] { "Сербитон", "Детфорд" },
            new string[] { "Вестминстер", "Теддингтон" },
            new string[] { "Блумсбери", "Гринвич" }
        };
        private readonly string[][] LondonSegments_Incorrect_EmptyInput = { };
        private readonly string[][] LondonSegments_Correct =
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
        private readonly LinkedList<Segment> LondonRoute_Correct = new LinkedList<Segment>(new Segment[] {
            new Segment(new Point("Сити"), new Point("Вестминстер")),
            new Segment(new Point("Вестминстер"), new Point("Теддингтон")),
            new Segment(new Point("Теддингтон"), new Point("Ноттинг-Хилл")),
            new Segment(new Point("Ноттинг-Хилл"), new Point("Южный Кенсингтон")),
            new Segment(new Point("Южный Кенсингтон"), new Point("Челси")),
            new Segment(new Point("Челси"), new Point("Блумсбери")),
            new Segment(new Point("Блумсбери"), new Point("Гринвич")),
            new Segment(new Point("Гринвич"), new Point("Сербитон")),
            new Segment(new Point("Сербитон"), new Point("Детфорд")),
            new Segment(new Point("Детфорд"), new Point("Фулем"))
        });
        private readonly LinkedList<Segment> LondonRoute_CorrectReversed = new LinkedList<Segment>(new Segment[] {
            new Segment(new Point("Фулем"), new Point("Детфорд")),
            new Segment(new Point("Детфорд"), new Point("Сербитон")),
            new Segment(new Point("Сербитон"), new Point("Гринвич")),
            new Segment(new Point("Гринвич"), new Point("Блумсбери")),
            new Segment(new Point("Блумсбери"), new Point("Челси")),
            new Segment(new Point("Челси"), new Point("Южный Кенсингтон")),
            new Segment(new Point("Южный Кенсингтон"), new Point("Ноттинг-Хилл")),
            new Segment(new Point("Ноттинг-Хилл"), new Point("Теддингтон")),
            new Segment(new Point("Теддингтон"), new Point("Вестминстер")),
            new Segment(new Point("Вестминстер"), new Point("Сити")),
        });

        [Fact]
        public void GotExpectedDisruptedRouteException()
        {
            Route route = new Route("LondonAreas_Incorrect_Disrupted");
            route.AcceptData(LondonSegments_Incorrect_Disrupted);

            var ex = Assert.Throws<DisruptedRouteException>(() => { route.Arrange(); });
            Assert.NotNull(ex);
        }
        [Fact]
        public void GotExpectedEmptyInputException()
        {
            Route route = new Route("LondonAreas_Incorrect_EmptyInput");

            var ex = Assert.Throws<EmptyInputException>(() => { route.AcceptData(LondonSegments_Incorrect_EmptyInput); });
            Assert.NotNull(ex);
        }
        [Fact]
        public void GotExpectedInvalidSegmentException()
        {
            Route route = new Route("LondonAreas_Incorrect_InvalidSegment");

            var ex = Assert.Throws<InvalidSegmentException>(() => { route.AcceptData(LondonSegments_Incorrect_InvalidSegment); });
            Assert.NotNull(ex);
        }

        [Fact]
        public void ArrangedCorrectly()
        {
            Route route = new Route("LondonRoute_Correct");
            route.AcceptData(LondonSegments_Correct);
            route.Arrange();

            LinkedList<Segment> output = new LinkedList<Segment>();
            foreach (Segment segment in route)
            {
                output.AddLast(new Segment(segment));
            }

            Assert.True(Enumerable.SequenceEqual(LondonRoute_Correct, output));
        }
        [Fact]
        public void ReversedCorrectly()
        {
            Route route = new Route("LondonRoute_CorrectReversed");
            route.AcceptData(LondonSegments_Correct);
            route.Arrange();
            route.Reverse();

            LinkedList<Segment> output = new LinkedList<Segment>();
            foreach (Segment segment in route)
            {
                output.AddLast(new Segment(segment));
            }

            Assert.True(Enumerable.SequenceEqual(LondonRoute_CorrectReversed, output));
        }
    }
}
