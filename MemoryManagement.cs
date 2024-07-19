using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Netch.AdvancedTopics {
	public class MemoryManagement {
		#region 'ref readonly' variables & the 'in' keyword
		// Overview: 
		// - In parameters [C# 7.2]
		// - Ref readonly variables
		// - Ref struct
		// - Span<T>

		// Summary:
		// Values types can be passed by value or by reference
		// 'in' keyword: pass by reference, but read-only
		// 'ref readonly' lets you expose a member as a readonly reference

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

			// Using the ref readonly Point.Origin here won't make a copy of the value
			double distanceFromOrigin = MeasureDistance(p1, Point.Origin);

			// Because we don't have any ref qualifiers, the compiler will make a copy of the value
			Point copyOfOrigin = Point.Origin; // by-value copy
		}
		#endregion

		#endregion

		#region Garbage Collection
		// - The garbage collector is continuously deleting objects that are no longer in use

		// - Garbage collection disadvantages:
		// -- Too convenient because it's automatic so programmers lose sight of memory footprint
		// -- Code works but allocates much more memory than it actually needs

		#endregion

		#region The Stack
		// - The stack tracks method calls
		// - Contains frames which hold parameters, local variables, and return addresses for each method call
		// - A stack frame is removed when returning from a method. All local variables go out of scope at this point
		// - If you have an infinite sequence of methods, the stack will overflow and throw a StackOverflowException
		#endregion

		#region The Heap
		// - Objects are always created on the heap
		// -- Every time you see the 'new' keyword, you're allocating memory to the heap

		// - Dereferenced objects are not immediately deleted
		// -- The garbage collector will delete them when it runs
		#endregion

		#region Value Types
		// - A type of variable where the type and the value are stored together
		// -- [TYPE] [VALUE]
		// -- [int]  [12345]

		// - Value types can exist both on the stack and heap

		// - Value types are assigned by value, meaning the value is copied over

		// - Value types are compared by value

		// - All numeric types are value types:
		// -- sbyte, byte, char, short, ushort, int, uint, long, ulong, float, double, decimal
		// --- Also: bool, enum, struct
		#endregion

		#region Reference Types
		// - A reference type is a type of variable that refers to a value stored on the heap

		// - Reference types can exist both on the stack and heap, but they will always refer to a value on the heap

		// - Reference types are assigned by reference, meaning the reference is copied over
		// -- When assigning one variable to another, you end up with two references pointing to the same object on the heap.
		// --- When compared they will be equal

		// - Reference types are compared by reference
		// -- If you have two reference type variables pointing at two separate objects, they will not be equal, even if they have identical data
		#endregion

		#region Boxing and Unboxing

		#endregion
	}
}