using System;
using System.Collections.Generic;
using System.IO;

namespace DeliveryRouteHelper
{
    public class Point : IEquatable<Point>
    {
        // coords, visited { get, set }, etc.
        public string Name;
        public Point(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Point);
        }

        public bool Equals(Point other)
        {
            return other != null &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(Point point1, Point point2)
        {
            return EqualityComparer<Point>.Default.Equals(point1, point2);
        }

        public static bool operator !=(Point point1, Point point2)
        {
            return !(point1 == point2);
        }
    }

    public struct Segment
    {
        public Point Start, End;
        public Segment(Point start, Point end)
        {
            Start = start;
            End = end;
        }
        public override string ToString()
        {
            return String.Format("{0} → {1}", Start.Name, End.Name);
        }
    }

    public class Route
    {
        public string Name;
        private HashSet<Segment> input;
        private LinkedList<Segment> route;

        public Route(string name)
        {
            Name = name;
            route = new LinkedList<Segment>();
        }

        private void Convert(string[][] rawInput)
        {
            HashSet<Segment> result = new HashSet<Segment>();
            for (int i = 0; i < rawInput.Length; i++)
            {
                result.Add(new Segment(
                    new Point(rawInput[i][0]),
                    new Point(rawInput[i][1]))
                    );
            }
            input = result;
        }

        public void AcceptData(Object data)
        {
            if (data.GetType() == typeof(string[][]))
            {
                Convert(data as string[][]);
            }
            else
            {
                // TODO: throw an exception
                Console.WriteLine($"Data type {data.GetType().ToString()} isn't supported");
                input = new HashSet<Segment>();
            }
        }

        //public void Arrange()
        //{
        //    ArrangePrepare();
        //    route = ArrangeRoutine();
        //}

        private LinkedList<Segment> ArrangeRoutine(LinkedList<Segment> chained, HashSet<Segment> notChained)
        {
            if (notChained.Count == 0) return chained;

            notChained.RemoveWhere((Segment segment) =>
            {
                if (segment.Start == chained.Last.Value.End)
                {
                    chained.AddLast(segment);
                    return true;
                }
                if (segment.End == chained.First.Value.Start)
                {
                    chained.AddFirst(segment);
                    return true;
                }
                return false;
            });

            return ArrangeRoutine(chained, notChained);
        }

        public void Arrange()
        {
            LinkedList<Segment> chained = new LinkedList<Segment>();
            HashSet<Segment> notChained = new HashSet<Segment>();

            HashSet<Segment>.Enumerator enumerator = input.GetEnumerator();
            enumerator.MoveNext();
            chained.AddFirst(enumerator.Current);
            while (enumerator.MoveNext())
            {
                notChained.Add(enumerator.Current);
            }

            route = ArrangeRoutine(chained, notChained);
        }

        //public IEnumerable<Segment> GetRouteEnumerator()
        //{
        //    foreach (Segment segment in route)
        //    {
        //        yield return segment;
        //    }
        //}

        public void Display()
        {
            if (route.Count != 0)
            {
                Console.WriteLine($"Route \"{Name}\":");
                Console.WriteLine("----------------------------------------");
                int idx = 0;
                foreach (Segment segment in route)
                {
                    idx++;
                    Console.WriteLine($"{idx:D3}. {segment.Start.Name} → {segment.End.Name}");
                }
            }
            else
            {
                Console.WriteLine($"Route {Name} is currently empty");
            }
        }

        public override string ToString()
        {
            TextWriter stdout = Console.Out;
            StringWriter tmpout = new StringWriter();
            Console.SetOut(tmpout);
            Display();
            Console.SetOut(stdout);
            return tmpout.ToString();
        }
    }
}
