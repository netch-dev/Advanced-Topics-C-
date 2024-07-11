using Microsoft.CSharp.RuntimeBinder;
using System.Dynamic;

namespace Netch.AdvancedTopics {
	public class DynamicProgramming {
		public void DynamicObjectExample() {
			// Dynamic is a class that starts out empty and you can add properties to it at runtime
			// We have this for compatibility with other languages
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

		// The .net framework has a BaseClass that allows us to create dynamic objects
		public class Widget : DynamicObject {
			public void WhatIsThis() {
				// We cannot use the this keyword because this is a dynamic object
				//Console.WriteLine(this.World);

				// We can use the This property below
				// This is because it is now a dynamic object so all of the late binding rules apply
				Console.WriteLine(This.World);
			}

			// Cast this to a dynamic object
			private dynamic This => this;

			public override bool TryGetMember(GetMemberBinder binder, out object? result) {
				//return base.TryGetMember(binder, out result);

				result = binder.Name;
				return true;
			}

			public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object? result) {
				//return base.TryGetIndex(binder, indexes, out result);
				if (indexes.Length == 1) {
					result = new string('*', (int)indexes[0]);
					return true;
				}

				result = null;
				return false;
			}
		}

		/*public static void Main(string[] args) {
			dynamic w = new Widget();
			//var w2 = new .Widget() as dynamic;

			// We can intercept this call by override the TryGetMember method
			Console.WriteLine(w.Hello);

			// We can intercept this call by overriding the TryGetIndex method
			Console.WriteLine(w[7]);

			w.WhatIsThis();
		}*/
	}
}