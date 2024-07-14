using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Netch.AdvancedTopics {
	// Overview: 
	// - In parameters [C# 7.2]
	// - Ref readonly variables
	// - Ref struct
	// - Span<T>

	public class MemoryManagement {
		private struct Point {
			public double X, Y;
			public Point(double x, double y) {
				X = x;
				Y = y;
			}

			#region 'ref readonly' variables
			//public static Point Origin = new Point();
			// When you access Origin shown above, because it's a value type, you're copying the entire value and allocating new memory each time

			// To prevent copying the entire value:
			// Use a ref readonly qualifier, you can pass a reference to the value, instead of the value itself
			private static Point origin = new Point();
			public static ref readonly Point Origin => ref origin;
			#endregion

			public override string ToString() {
				return $"({X},{Y})";
			}
		}

		#region The 'in' keyword
		// double MeasureDistance(Point p1, Point p2) { ...
		// If you want to call MeasureDistance without 'in', you have to pass the points by value (entire copy)

		// double MeasureDistance(in Point p1, in Point p2) { ...
		// Using the in keyword, you can pass the points by reference, instead of by value (entire copy), which is more efficient

		// The in keyword is a read-only reference to the value, so you can't modify the value
		// You also can't use that reference to mutate the value

		private double MeasureDistance(in Point p1, in Point p2) {
			double dx = p1.X - p2.X;
			double dy = p1.Y - p2.Y;
			return Math.Sqrt((dx * dx) + (dy * dy));
		}

		// You cannot make an overload of a method that uses 'in' and one that doesn't
		/*		private double MeasureDistance(Point p1, Point p2) {
					double dx = p1.X - p2.X;
					double dy = p1.Y - p2.Y;
					return Math.Sqrt((dx * dx) + (dy * dy));
				}*/

		public void MemoryManagementExample() {
			Point p1 = new Point(1, 1);
			Point p2 = new Point(4, 5);

			double distance = MeasureDistance(p1, p2);
			Console.WriteLine($"Distance: {distance}");

			double distanceFromOrigin = MeasureDistance(p1, Point.Origin);

			// Because we don't have any ref qualifiers, the compiler will make a copy of the value
			Point copyOfOrigin = Point.Origin; // by-value copy
		}

		#endregion


	}
}