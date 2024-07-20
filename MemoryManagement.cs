using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Text;

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

		// - The garbage collector uses a mark/sweep/compact cycle
		// - Small objects are allocated on the Small Object Heap (SOH), large objects on the Large Object Heap (LOH)
		// - The small object heap is divided into three generations: 0, 1, and 2
		// -- All new objects are allocated in generation 0, and progress towards generation 2 if they survive collection cycles
		// - The LOH has only one gen and does not compact
		// - The LOH is processed together with generation 2 of the SOH

		// - Problem 1: During the mark stage the garbage collector has to inspect each object in the heap
		// -- A very long-living object might be inspected many times
		// -- The solution is a generational garbage collector
		// --- You can visualize this as 3 separate heaps
		// --- All new allocations go into generation 0
		// --- Each collection cycle moves surviving objects to the next generation
		// --- Generation 0 is collected more frequently than generation 1, and 1 more than 2

		// - Problem 2: Copying large objects is expensive
		// -- Large objects are stored on the Large Object Heap (LOH)
		// -- Objects on the large object heap are not compacted
		// -- The LOH is synchronized with the generation 2 SOH heap

		// - Implicit assumptions:
		// - Objects are either short-lived or long-lived
		// - Show-lived objects will be allocated and discarded within a single collection cycle
		// - Objects that survive two collection cycles are long-lived
		// - 90% of all small objects are short-lived
		// - All large objects (85K+) are long-lived
		#endregion

		#region Optimizing for the Garbage Collector
		// - Limit the number of objects you create
		// - Allocate, use, and discard small objects as quickly as possible
		// - Re-use large objects

		// - Use only small short-lived, and large long-lived objects

		// - Increase lifetime or decrease size of large short-lived objects
		// - Decrease lifetime or increase size of small long-lived objects

		#region Allocation Optimizations
		// - The more objects in generation 0 heap, the more work needs to be done so:
		// -- Limit the number of objects you create
		// -- Allocate, use, and discard objects as quickly as possible

		private void AllocationExample() {
			StringBuilder s = new StringBuilder();
			for (int i = 0; i < 10000; i++) {
				s.Append(i.ToString() + "KB");
			}
			// - The problem with the code above is the string concatenation inside the append method
			// - Since strings are immutable, the ToString() method and the addition both create extra string objects on the heap
			// - To avoid this problem, call append twice
			// - The difference is 40,000 less objects on the heap
			s = new StringBuilder();
			for (int i = 0; i < 10000; i++) {
				s.Append(i);
				s.Append("KB");
			}
		}

		private void AllocationExample2() {
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < 10000; i++) {
				arrayList.Add(i);
			}
			// - The problem with the code above is if you store integers in an ArrayList, they are boxed on the heap
			// - To avoid this problem, use a List<int> instead
			// - The difference is 20,000 less boxed integers on the heap
			List<int> list = new List<int>();
			for (int i = 0; i < 10000; i++) {
				list.Add(i);
			}
		}

		/* Allocation Example 3:
		 * public static MyObject obj = new MyObject();
		 * ...
		 * Lots of other code
		 * ...
		 * UseTheObject(obj);
		*/

		// - The problem above is that the object is small, but the gap between use is large
		// - The object will be promoted to generation 2, and will be inspected many times
		// - To avoid this problem, use the object immediately after creation and make it not static

		// - Also set the object to null after use to signal to the garbage collector that it can be deleted
		/*
		 * MyObject obj = new MyObject();
		 * UseTheObject(obj);
		 * obj = null;
		*/

		// -- If you don't like having null assignments all over, you can also wrap it in a method, and have the object reference go out of scope when you exit the method
		// --- Example of wrapping in a method:
		public void ProcessData() {
			// The obj is created and used exclusively in the ProcessData method
			MyObject obj = new MyObject();
			obj.DoSomething();
			// obj goes out of scope here, so no need to set it to null
		}
		#endregion

		#region Lifetime Optimizations
		// - The garbage collector assumes that 90% of all small objects are short-lived, and all large objects are long-lived
		// -- So we should avoid the opposite:
		// --- Avoid large short-lived objects
		// --- Avoid small long-lived objects

		// - To refactor large short-lived objects they should be re-used when possible. object pooling is a common technique
		public void ReuseLargeObjectExample() {
			ArrayList arrayList = new ArrayList(85190);
			// UseTheList(arrayList);
			// ...
			arrayList = new ArrayList(85190);
			// UseTheList(arrayList);

			// The problem with the code above is that the ArrayList is large and short-lived

			// The solution is to simply clear the list instead of creating a new one
			// This way the list is re-used and it's lifetime is increased, effectively becoming long-living
			arrayList.Clear();
			// UseTheList(arrayList);
		}

		// - To refactor small long-lived objects, you should consider using a struct instead of a class
		public void SmallObjectExample() {
			// Create an ArrayList and fill it with Pair objects, each pair contains two integers
			ArrayList arrayList = new ArrayList(85190);
			for (int i = 0; i < 10000; i++) {
				//arrayList.Add(new Pair(i, i + 1));
			}

			// The problem above is that the ArrayList is a large object, so it goes on the large object heap. The list is filled with tiny Pair objects, which are short-lived
			// All of the Pair objects go into the generation 0 heap. Since the ArrayList is keeping them alive, they will be promoted to generation 1 and 2

			// The solution is instead to use two arrays, instead of one ArrayList with Pair objects
			// Because an integer is a value type it will be stored with the array, and not on the heap
			// Now we only have two large arrays in the LOH, and no small objects in the SOH
			int[] array1 = new int[85190];
			int[] array2 = new int[85190];
			for (int i = 0; i < 10000; i++) {
				array1[i] = i;
				array2[i] = i + 1;
			}
		}

		// - Reducing the size of a large short-lived object:
		// -- Strategy 1. Split the object apart in sub-objects, each smaller than 85K

		// -- Strategy 2. Reduce the memory footprint of the object
		public void Strategy2Example() {
			int[] buffer = new int[32768];
			for (int i = 0; i < buffer.Length; i++) {
				//buffer[i] = GetByte(i);
			}

			// The problem above is that the loop fills the buffer with bytes, but the buffer is an array of integers
			// Since an integer is 4 bytes in size, this adds up to 128KB of memory. Above the large object threshold

			// The solution is to use a byte array instead of an integer array
			// Now the buffer is only 32KB in size, and will be stored in the SOH
			byte[] buffer2 = new byte[32768];
			for (int i = 0; i < buffer2.Length; i++) {
				//buffer2[i] = GetByte(i);
			}
		}

		// - Now the opposite: Small long lived object that we must refactor into a large long-lived object
		// -- Strategy 1. Enlarge the memory footprint of the object
		// -- Strategy 2. Combine the object with other objects to make it larger, that can go on the LOH

		/* Strategy 1 Example:
		 * // public static ArrayList list = new ArrayList();
		 * // ...
		 * // lots of other code
		 * // ...
		 * // UseTheList(list);
		 * 
		 * The problem with the code above is that the object is intended to be a long-living object, because it is declared as static
		 * If we also know that the list will eventually contain at least 85K of data, we should initialize it with that capacity
		 * This ensures that the list will be stored on the LOH from the start, and will not have to endure 4 memory copy operations
		 * 
		 * Solution:
		 * // public static ArrayList list = new ArrayList(85190);
		 * // ...
		 * // lots of other code
		 * // ...
		 * // UseTheList(list);
		 */
		#endregion

		#endregion

		#region Finalizers in .NET
		// - A class method that are called when objects are about to be cleaned up by the garbage collector

		// - Finalizers are used to release scarce resources, such as file handles, network connections

		// - The garbage collector processes finalizers in a separate thread
		// -- They are processed in a random order, so finalizers must only process its own object, and never other referenced objects

		// - Finalizers are not guaranteed to run
		// -- If the host process exits, anything remaining in the finalizer queue after 4 seconds is discarded

		// - Objects with finalizers always end up in generation 1 and sometimes in generation 2
		// -- This is because objects with finalizers are kept alive and moved to next generation
		// --- Then the GC will run the finalizer code in the next cycle on the finalizer thread
		// ---- So many small short-lived objects with finalizers are bad, because their lifetime is increased
		// ---- Objects with finalizers should be extremely short and run very quickly
		// ---- Only add finalizers to objects at the edge of an object graph, not on the root object

		// Finalizer example:
		public class FinalizerExampleObject {
			public FinalizerExampleObject() {

			}

			~FinalizerExampleObject() {
				// Class finalizer starts with a tilde
				// It will be called by the garbage collector just before the object is cleaned up
			}
		}
		#endregion

		#region The Dispose Pattern
		// - A downside to finalizers is that they are called by the garbage collector, which is non-deterministic
		// -- This means you don't know when the finalizer will run, or if it will run at all
		// --- The solution is to use the Dispose pattern:

		// - This design pattern introduces a Dispose method that you can call to release resources
		// - The using statement will automatically call Dispose at the end of the using block

		// - The dispose pattern dramatically reduces the length of time that resources are held

		// - For more information on disposable objects, see AssortedTopics.cs
		#endregion

		#region The Stack
		// - The stack tracks method calls
		// - Contains frames which hold parameters, local variables, and return addresses for each method call
		// - A stack frame is removed when returning from a method. All local variables go out of scope at this point
		// - If you have an infinite sequence of methods, the stack will overflow and throw a StackOverflowException
		#endregion

		#region The Heap
		// - Objects are always created on the heap. The variable itself may be on the stack, but the object it refers to is on the heap
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
		// - Boxing takes a value type on the stack and stores it as an object on the heap
		// -- Boxing happens when you assign a value type to a variable, parameter, field or property of type object

		// - Unboxing unpacks a boxed object on the heap, and copies the value type inside back to the stack
		// -- Unboxing happens when you cast an object back to a value type

		private void BoxingAndUnboxing() {
			// Boxing: Converting a value type to a reference type
			// Below the compiler with 'box' int i and move it to the heap since object is a reference type
			int i = 123;
			object o = i; // Boxing

			// Unboxing: Converting a reference type to a value type
			// Below the compiler will 'unbox' object o and move it back to the stack since int is a value type
			int j = (int)o; // Unboxing
		}
		#endregion

		#region Tricks to Improve Memory Allocation

		#region 1. Avoid Boxing and Unboxing
		// - The .NET framework uses boxing and unboxing to make value and reference types interchangeable

		// - Each boxing operation creates one new object on the heap. A boxed object occupies more memory than the original value type

		// - Boxing floods the heap with lots of small objects and puts considerable pressure on the GC
		#endregion

		#region 2. Do not Concatenate Strings; Use StringBuilder
		// - Since strings are immutable, string methods never modify the original string
		// -- Instead, they make a copy, modify the copy, and return the copy as a result
		// --- The reason for this, is it makes strings behave like value types. Meaning they can be assigned and compared by value

		// - Strings are optimized for fast comparison, and not bulk modifications

		// - The StringBuilder class should be used for string concatenation. Since it does not create a new string object for each concatenation
		// -- If you know there will be a lot of concatenations, set the initial capacity of the StringBuilder to the expected size
		// --- The reason for this is because the StringBuilder only has a tiny buffer to start with, and will have to reallocate and copy to new memory each time it runs out of space
		// -- StringBuilder result = new StringBuilder(10000);
		#endregion

		#region 3. Use Structs Instead of Classes
		// - Structs are value types (stored inline), classes are reference types (always refer to an object stored elsewhere on the heap)
		// - Structs are much faster than classes because they don't have a default constructor, cannot be inherited, and are not collected by the GC
		// - Structs only allocate heap memory for their internal fields and do not have the 16-byte overhead that objects on the heap have

		// - Assigning structs copies the value of all interval fields
		// - Assigning classes copies the reference, and you end up with two references stored on the heap

		// - Structs can implement interfaces but cannot inherit
		// - Structs cannot have instance field initializers
		// - Structs cannot have a parameterless constructor
		// - Structs cannot have finalizers

		// - Structs are great but you should only use them in specific scenarios:
		// - When the data you're storing represents a single value, examples: a Point, Vector, Matrix, Complex number
		// - If your data size is very small (24 bytes or less) and you're going to create thousands or millions of instances
		// - If your data is immutable and should assigned and compared by value
		// - If your data will not have to be boxed frequently

		// - In all other scenarios classes are a better choice
		#endregion

		#region 4. Always Pre-Size Collections
		// - Every collection and list in .NET starts out with a certain default capacity
		// -- When it runs out of space it will create a new list of twice the size and copy everything over
		// --- So we should always pre-size collections to the expected size to avoid this

		// - Default collection capacity:
		// -- ArrayList: 4 items
		// -- Queue: 4 items
		// -- Stack: 16 items
		// -- List: 4 items
		// -- Dictionary: 12 items
		// --- If you add hundreds or thousands of items to a collection, it will have to resize many times to accommodate all items-
		#endregion

		#region 5. Avoid Calling ToList in LINQ Expressions
		// - Calling ToList in a LINQ expression can unexpectedly inflate the memory footprint of your application
		// -- To fix this problem, call ToList before running the LINQ query

		// - LINQ is a powerful framework for running queries on enumerable data

		// - var numbers = Enumerable.Range(1, 1000).Where(x => x % 2 == 0);
		// -- This expression above creates a filtered enumerator for a range of 500 odd numbers, but it doesn't actually generate the numbers yet
		// -- The range does not occupy any memory. The enumerator only tracks the current number

		// - You can use LINQ to process very large amount of data, while only using a tiny amount of heap memory
		// -- Because the enumerator will only track the current item
		#endregion

		#endregion
	}
}