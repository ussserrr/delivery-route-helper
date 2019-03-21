﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace DeliveryRouteHelper
{
    // Entity representing a single delivery point
    // IEquatable<> implementation helps to compare the instances throughout the code
    public class Point : IEquatable<Point>
    {
        // coords, visited { get, set }, etc.
        public readonly string Name;

        public Point(string name)
        {
            Name = name;
        }

        public Point(Point point)
        {
            Name = string.Copy(point.Name);
        }

        public static Point Copy(Point other)
        {
            return new Point(String.Copy(other.Name));
        }

        // Below are members needed to comply with IEquatable<> interface (auto-generated templates by Visual Studio)
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


    // Entity representing a directed set of 2 Points
    // IEquatable<> implementation helps to compare the instances throughout the code
    public class Segment : IEquatable<Segment>
    {
        public Point Start, End;

        public Segment(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public Segment(Segment segment)
        {
            Start = Point.Copy(segment.Start);
            End = Point.Copy(segment.End);
        }

        public static Segment Copy(Segment other)
        {
            return new Segment(other);
        }

        public override string ToString()
        {
            return string.Format($"{Start.Name} → {End.Name}");
        }

        public void Reverse()
        {
            var tmp = Start;
            Start = End;
            End = tmp;
        }

        // For example for Enumerable.SequenceEqual() (see tests)
        public bool Equals(Segment other)
        {
            return other.Start == Start && other.End == End;
        }
    }

    // Custom exceptions should satisfy the following requirements:
    // https://docs.microsoft.com/ru-ru/dotnet/csharp/programming-guide/exceptions/creating-and-throwing-exceptions
    [Serializable()]
    public class InvalidSegmentException : System.Exception
    {
        public InvalidSegmentException() : base() { }
        public InvalidSegmentException(string message) : base(message) { }
        public InvalidSegmentException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected InvalidSegmentException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    [Serializable()]
    public class EmptyInputException : System.Exception
    {
        public EmptyInputException() : base() { }
        public EmptyInputException(string message) : base(message) { }
        public EmptyInputException(string message, System.Exception inner) : base(message, inner) { }

        protected EmptyInputException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    [Serializable()]
    public class DisruptedRouteException : System.Exception
    {
        public DisruptedRouteException() : base() { }
        public DisruptedRouteException(string message) : base(message) { }
        public DisruptedRouteException(string message, System.Exception inner) : base(message, inner) { }

        protected DisruptedRouteException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    // Core class of the library accepting, converting and arranging the Segments
    // IEnumerable<> allows to get delivery points one by one upon request
    public class Route : IEnumerable<Segment>
    {
        public string Name { get; set; }
        public bool DataAccepted
        {
            get
            {
                return segmentsSet.Count != 0;
            }
        }
        public bool Arranged
        {
            get
            {
                return route.Count != 0;
            }
        }

        // HashSet<Segment> ideally fits to store the input data as it unsorted by definition.
        // So it computes some operations faster due to its features
        private HashSet<Segment> segmentsSet;
        // In opposed, output array is strictly arranged for all of its elements so usage of doubly-linked list
        // is intuitively clear
        private LinkedList<Segment> route;

        public Route(string name)
        {
            Name = name;

            route = new LinkedList<Segment>();
            segmentsSet = new HashSet<Segment>();
        }

        public Route(): this("Unnamed") { }

        public void AcceptData(string[][] rawInput)
        {
            if (rawInput.Length != 0)
            {
                HashSet<Segment> segmentsSetTmp = new HashSet<Segment>();
                for (int i = 0; i < rawInput.Length; i++)
                {
                    if (rawInput[i].Length == 2)
                    {
                        segmentsSetTmp.Add(new Segment(
                            new Point(rawInput[i][0]),
                            new Point(rawInput[i][1])));
                    }
                    else
                    {
                        throw new InvalidSegmentException($"Segment #{i} \"{rawInput[i]}\" is corrupted");
                    }
                }
                segmentsSet = segmentsSetTmp;
            }
            else
            {
                throw new EmptyInputException($"Input {rawInput.GetType()} is empty");
            }
        }

        public void AcceptData(byte[][][] rawInput)
        {
            if (rawInput.Length != 0)
            {
                HashSet<Segment> segmentsSetTmp = new HashSet<Segment>();
                for (int i = 0; i < rawInput.Length; i++)
                {
                    if (rawInput[i].Length == 2)
                    {
                        string[] utf8Strings = new string[2];
                        for (int k = 0; k < 2; k++)
                        {
                            // Convert byte[] into a char[] and then into a string
                            // https://docs.microsoft.com/ru-ru/dotnet/api/system.text.encoding
                            char[] utf8Chars = new char[Encoding.UTF8.GetCharCount(rawInput[i][k], 0, rawInput[i][k].Length)];
                            Encoding.UTF8.GetChars(rawInput[i][k], 0, rawInput[i][k].Length, utf8Chars, 0);
                            utf8Strings[k] = new string(utf8Chars);
                        }

                        segmentsSetTmp.Add(new Segment(new Point(utf8Strings[0]), new Point(utf8Strings[1])));
                    }
                    else
                    {
                        throw new InvalidSegmentException($"Segment #{i} \"{rawInput[i]}\" is corrupted");
                    }
                }
                segmentsSet = segmentsSetTmp;
            }
            else
            {
                throw new EmptyInputException($"Input {rawInput.GetType()} is empty");
            }
        }

        public void Arrange()
        {
            if (segmentsSet.Count != 0)
            {
                LinkedList<Segment> chained = new LinkedList<Segment>();
                HashSet<Segment> notChained = new HashSet<Segment>();

                HashSet<Segment>.Enumerator enumerator = segmentsSet.GetEnumerator();
                enumerator.MoveNext();
                chained.AddFirst(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    notChained.Add(enumerator.Current);
                }

                int chainedCountPrev = chained.Count;

                while (true)
                {
                    // O(N), but N is decreasing on each 'while' step (arithmetic progression from (N-1) to 1)
                    notChained.RemoveWhere((Segment segment) =>
                    {
                        if (segment.Start == chained.Last.Value.End)  // O(1)
                        {
                            chained.AddLast(segment);  // O(1)
                            return true;
                        }
                        if (segment.End == chained.First.Value.Start)  // O(1)
                        {
                            chained.AddFirst(segment);  // O(1)
                            return true;
                        }
                        return false;
                    });

                    if (notChained.Count == 0)  // O(1)
                    {
                        route = chained;  // Assume O(1) as LinkedList<T> is a reference type
                        return;
                    }

                    if (chained.Count == chainedCountPrev)  // O(1)
                    {
                        route.Clear();
                        throw new DisruptedRouteException("Cannot arrange segments into route");
                    }

                    chainedCountPrev = chained.Count;
                }
            }

            throw new EmptyInputException("Input is empty. Use appropriate AcceptData() overload to provide it first");
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

        public void Display()
        {
            Console.WriteLine($"Route \"{Name}\":");
            Console.WriteLine("----------------------------------------");
            if (route.Count != 0)
            {
                int idx = 0;
                foreach (Segment segment in route)
                {
                    idx++;
                    Console.WriteLine($"{idx:D3}. {segment.Start.Name} → {segment.End.Name}");
                }
            }
            else
            {
                Console.WriteLine("empty");
            }
        }

        public void Reset()
        {
            Name = "";
            segmentsSet.Clear();
            segmentsSet.TrimExcess();
            route.Clear();
        }

        public void Reverse()
        {
            foreach (Segment segment in route)
            {
                segment.Reverse();
            }
            // List<T> has Reverse() method while LinkedList<T> doesn't so we convert to it
            List<Segment> tmp = new List<Segment>(route);
            tmp.Reverse();
            route = new LinkedList<Segment>(tmp);
        }

        public IEnumerator<Segment> GetEnumerator()
        {
            return route.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return route.GetEnumerator();
        }
    }
}
