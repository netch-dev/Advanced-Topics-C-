using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using static Netch.AdvancedTopics.ExtensionMethods;

namespace Netch.AdvancedTopics {
	// Summary: 
	// - Extension methods allow you to add methods to existing types without modifying them
	// - Can extend a great many types (System.Object, generics). For example giving an integer an additional method
	// - Form the backbone of fluent interfaces and DSLs (Domain Specific Languages)

	// - Extension methods are defined as static methods inside static classes
	// - There is no polymorphism; you cannot override built in methods such as ToString()
	// - EM can be defined on primitive and reference tpyes, your own or build in, on object or type T, on value tuples, delegates

	public class ExtensionMethods {
		public class Foo {
			public string Name => "Foo";
		}

		public class Person {
			public string Name;
			public int Age;
		}

		public void TestExtensionMethods() {
			// Adding an extension method to the Foo class
			Console.WriteLine(new Foo().Measure());

			// Extension method on built-in types
			Console.WriteLine(42.ToBinary());

			// Extension method on a Tuple
			Person person = ("John", 30).ToPerson();
			Console.WriteLine($"{person.Name} is {person.Age} years old");

			// Extension method on generics
			Console.WriteLine(new Tuple<string, int>("John", 30).Measure());

			// Extension method on delegates
			Func<int> calculate = delegate {
				Thread.Sleep(1000);
				return 42;
			};
			Stopwatch st = calculate.Measure();
			Console.WriteLine($"Took {st.ElapsedMilliseconds} ms to run");

			// Being able to extend any type
			"hello".ExtendAnyObject();
		}
	}

	public static class ExtensionMethodsExample {
		public static int Measure(this Foo foo) {
			return foo.Name.Length;
		}

		public static string ToBinary(this int n) {
			return Convert.ToString(n, 2);
		}

		public static void Save(this ISerializable serializable) {
			// ...
		}

		public static Person ToPerson(this (string name, int age) data) {
			return new Person { Name = data.name, Age = data.age };
		}

		public static int Measure<T, U>(this Tuple<T, U> t) {
			return t.Item2.ToString().Length;
		}

		public static Stopwatch Measure(this Func<int> f) {
			Stopwatch st = new Stopwatch();
			st.Start();
			f();
			st.Stop();
			return st;
		}

		public static void ExtendAnyObject(this object o) {
			Console.WriteLine($"I can extend any object - {o.GetType()}");
		}
	}

	public static class ConvenientExtensionExample {
		// name shortening wrappers
		public static StringBuilder al(this StringBuilder sb, string s) {
			return sb.AppendLine(s);
		}

		// combine two or more calls
		// AppendLine()
		// AppendFormat()
		public static StringBuilder AppendFormatLine(this StringBuilder sb, string format, params object[] args) {
			return sb.AppendFormat(format, args).AppendLine();
		}

		// xor a list of values
		public static ulong Xor(this IList<ulong> values) {
			ulong first = values[0];
			foreach (ulong item in values.Skip(1)) {
				first ^= item;
			}
			return first;
		}

		// shortening a method name. not really a fan of this one because it reduces readability but it's an example of what can be done
		public static string f(this string format, params object[] args) {
			return string.Format(format, args);
		}
	}
}