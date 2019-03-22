using System;
using System.Linq;
using System.Collections.Generic;

using Xunit;

using DeliveryRouteHelper;


namespace DeliveryRouteHelper.Tests
{
    public class Point_Tests
    {
        [Fact]
        public void IsEquals()
        {
            Point point1 = new Point("Abc");
            Point point2 = new Point("Abc");
            Assert.True(point1 == point2, $"Point '{point1}' should be equal to '{point2}'");
        }
    }

    public class Segment_Tests
    {
        [Fact]
        public void ReversedCorrectly()
        {
            Point point1 = new Point("Abc");
            Point point2 = new Point("deF");
            Segment segment = new Segment(point1, point2);
            segment.Reverse();
            Assert.True(segment.End == point1 && segment.Start == point2, $"Segment {segment} isn't reversed");
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
            HashSet<Segment> testData = Util.Util.ConvertData(LondonSegments_Incorrect_Disrupted);
            Route route = new Route("LondonAreas_Incorrect_Disrupted");
            route.AcceptSegments(testData);

            var ex = Assert.Throws<DisruptedRouteException>(() => { route.Arrange(); });
            Assert.NotNull(ex);
        }

        [Fact]
        public void GotExpectedEmptyInputException()
        {
            //Route route = new Route("LondonAreas_Incorrect_EmptyInput");

            var ex = Assert.Throws<EmptyInputException>(() => { Util.Util.ConvertData(LondonSegments_Incorrect_EmptyInput); });
            Assert.NotNull(ex);
        }

        [Fact]
        public void GotExpectedInvalidSegmentException()
        {
            //Route route = new Route("LondonAreas_Incorrect_InvalidSegment");

            var ex = Assert.Throws<InvalidSegmentException>(() => { Util.Util.ConvertData(LondonSegments_Incorrect_InvalidSegment); });
            Assert.NotNull(ex);
        }

        [Theory]
        [InlineData(100)]
        public void ArrangedCorrectly(int numberOfTests)
        {
            Route route = new Route();

            Random randomGenerator = new Random();
            string[][] dataArray = LondonSegments_Correct;
            List<string[]> dataList = new List<string[]>(LondonSegments_Correct);

            for (int testsCounter = 0; testsCounter < numberOfTests; testsCounter++)
            {
                // https://stackoverflow.com/questions/273313/randomize-a-listt
                int n = dataList.Count;
                while (n > 1)
                {
                    n--;
                    int k = randomGenerator.Next(n + 1);
                    var value = dataList[k];
                    dataList[k] = dataList[n];
                    dataList[n] = value;
                }
                dataList.CopyTo(dataArray);
                HashSet<Segment> testData = Util.Util.ConvertData(dataArray);

                route.AcceptSegments(testData);
                route.Arrange();
                LinkedList<Segment> output = new LinkedList<Segment>();
                foreach (Segment segment in route)
                {
                    output.AddLast(new Segment(segment));
                }

                Assert.True(Enumerable.SequenceEqual(LondonRoute_Correct, output));

                route.Reset();
                //output.Clear();
            }
        }

        [Fact]
        public void ReversedCorrectly()
        {
            Route route = new Route("LondonRoute_CorrectReversed");
            HashSet<Segment> testData = Util.Util.ConvertData(LondonSegments_Correct);
            route.AcceptSegments(testData);
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
