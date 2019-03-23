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
        public void AreEqual()
        {
            Point point1 = new Point("Abc, 42");
            Point point2 = new Point("Abc, 42");
            Assert.Equal(point1, point2);
        }

        [Fact]
        public void AreNotEqual()
        {
            Point point1 = new Point("Abc, 42");
            Point point2 = new Point("24, xyZ");
            Assert.NotEqual(point1, point2);
        }
    }

    public class Segment_Tests
    {
        [Fact]
        public void ReversedCorrectly()
        {
            Point point1 = new Point("Abc, 42");
            Point point2 = new Point("24, xyZ");
            Segment segment = new Segment(point1, point2);
            segment.Reverse();
            Assert.Equal(segment.End, point1);
            Assert.Equal(segment.Start, point2);
        }
    }

    public class Util_Tests
    {
        private readonly string[][] LondonInput_Incorrect_InvalidSegment =
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
        private readonly string[][] LondonInput_Incorrect_EmptyInput = { };

        [Fact]
        public void GotExpected_EmptyInputException()
        {
            var exception = Assert.Throws<EmptyInputException>(() =>
            {
                Util.Util.ConvertData(LondonInput_Incorrect_EmptyInput);
            });
            Assert.NotNull(exception);
        }

        [Fact]
        public void GotExpected_InvalidSegmentException()
        {
            var exception = Assert.Throws<InvalidSegmentException>(() =>
            {
                Util.Util.ConvertData(LondonInput_Incorrect_InvalidSegment);
            });
            Assert.NotNull(exception);
        }
    }

    public class Route_Tests
    {
        private readonly HashSet<Segment> LondonSegments_NonArrangeable_Disrupted = new HashSet<Segment>
        {
            new Segment(new Point("Теддингтон"), new Point("Ноттинг-Хилл")),
            new Segment(new Point("Сити"), new Point("Вестминстер")),
            new Segment(new Point("Ноттинг-Хилл"), new Point("Южный Кенсингтон")),
            new Segment(new Point("Детфорд"), new Point("Фулем")),
            new Segment(new Point("Гринвич"), new Point("Сербитон")),
            //new Segment(new Point("Челси"), new Point("Блумсбери")),
            new Segment(new Point("Южный Кенсингтон"), new Point("Челси")),
            new Segment(new Point("Сербитон"), new Point("Детфорд")),
            new Segment(new Point("Вестминстер"), new Point("Теддингтон")),
            new Segment(new Point("Блумсбери"), new Point("Гринвич"))
        };
        private readonly HashSet<Segment> LondonSegments_Arrangeable = new HashSet<Segment>
        {
            new Segment(new Point("Теддингтон"), new Point("Ноттинг-Хилл")),
            new Segment(new Point("Сити"), new Point("Вестминстер")),
            new Segment(new Point("Ноттинг-Хилл"), new Point("Южный Кенсингтон")),
            new Segment(new Point("Детфорд"), new Point("Фулем")),
            new Segment(new Point("Гринвич"), new Point("Сербитон")),
            new Segment(new Point("Челси"), new Point("Блумсбери")),
            new Segment(new Point("Южный Кенсингтон"), new Point("Челси")),
            new Segment(new Point("Сербитон"), new Point("Детфорд")),
            new Segment(new Point("Вестминстер"), new Point("Теддингтон")),
            new Segment(new Point("Блумсбери"), new Point("Гринвич"))
        };
        private readonly LinkedList<Segment> LondonRoute_Arranged = new LinkedList<Segment>(new Segment[] {
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
        private readonly LinkedList<Segment> LondonRoute_ArrangedReversed = new LinkedList<Segment>(new Segment[] {
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
        public void GotExpected_DisruptedRouteException()
        {
            Route route = new Route();
            route.AcceptSegments(LondonSegments_NonArrangeable_Disrupted);

            var exception = Assert.Throws<DisruptedRouteException>(() => { route.Arrange(); });
            Assert.NotNull(exception);
        }

        [Theory]
        [InlineData(100)]
        public void ArrangedCorrectly(int numberOfTests)
        {
            Route route = new Route();

            Random randomGenerator = new Random();
            List<Segment> segmentsList = new List<Segment>(LondonSegments_Arrangeable);

            for (int testsCounter = 0; testsCounter < numberOfTests; testsCounter++)
            {
                // Shuffle the list (Fisher-Yates algorithm)
                // https://stackoverflow.com/questions/273313/randomize-a-listt
                for (int i = segmentsList.Count - 1; i > 1; i--)
                {
                    int k = randomGenerator.Next(i + 1);
                    var value = segmentsList[k];
                    segmentsList[k] = segmentsList[i];
                    segmentsList[i] = value;
                }
                //Console.WriteLine(segmentsList[3]);  // to be sure that the list is really has been shuffled
                HashSet<Segment> segmentsSet = new HashSet<Segment>(segmentsList);

                route.AcceptSegments(segmentsSet);
                route.Arrange();
                LinkedList<Segment> output = new LinkedList<Segment>();
                foreach (Segment segment in route)
                {
                    output.AddLast(new Segment(segment));
                }

                Assert.True(Enumerable.SequenceEqual(LondonRoute_Arranged, output));

                route.Reset();
            }
        }

        [Fact]
        public void ReversedCorrectly()
        {
            Route route = new Route();
            route.AcceptSegments(LondonSegments_Arrangeable);
            route.Arrange();
            route.Reverse();

            LinkedList<Segment> output = new LinkedList<Segment>();
            foreach (Segment segment in route)
            {
                output.AddLast(new Segment(segment));
            }

            Assert.True(Enumerable.SequenceEqual(LondonRoute_ArrangedReversed, output), $"{output}");
        }
    }
}
