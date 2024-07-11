using Microsoft.CSharp.RuntimeBinder;

namespace Netch.AdvancedTopics {
	public class DynamicProgramming {
		public void DynamicObjectExample() {
			// Dynamic is a class that starts out empty and you can add properties to it at runtime
			dynamic d = "hello";
			Console.WriteLine(d.GetType()); // returns System.String
			Console.WriteLine(d.Length); // returns 5

			d += " world";
			Console.WriteLine(d); // returns hello world

			try {
				int n = d.Area; // This will compile but throw an exception at runtime
			} catch (RuntimeBinderException e) {
				Console.WriteLine(e.Message);
			}

			// The dynamic type can be re-assigned to a different type
			d = 5;
			Console.WriteLine(d.GetType()); // returns System.Int32
			d--;
			Console.WriteLine(d); // returns 4
		}
	}
}