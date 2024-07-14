using Microsoft.CSharp.RuntimeBinder;
using System.Dynamic;

namespace Netch.AdvancedTopics {
	// Summary: 
	// - Dynamic keyword isnt just for interop with dynamically typed languages
	// - Allows us to call members on types, even if those members arent materialized at compile time
	// - Provide a useful interception mechanism

	public class DynamicProgramming {

		#region Dynamic object
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
				// We cannot use the 'this' keyword because this is a dynamic object
				//Console.WriteLine(this.World);

				// We can use the This property below
				// This is because it is now a dynamic object so all of the late binding rules apply
				Console.WriteLine(This.World);
			}

			// Cast this to a dynamic object
			private dynamic This => this;

			public override bool TryGetMember(GetMemberBinder binder, out object? result) {
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

		public void WidgetTestExample() {
			dynamic w = new Widget();
			//var w2 = new .Widget() as dynamic;

			// We can intercept this call by override the TryGetMember method
			Console.WriteLine(w.Hello);

			// We can intercept this call by overriding the TryGetIndex method
			Console.WriteLine(w[7]);

			w.WhatIsThis();
		}
		#endregion

		#region Expando object
		// An object that grows as you give it members
		// It dynamically creates that property and initializes it
		// - It adds elements to a Dictionary behind the scenes, which maps the names to objects
		public void ExpandoObjectExample() {
			dynamic person = new ExpandoObject();
			person.Name = "John";
			person.Age = 30;

			Console.WriteLine($"{person.Name} is {person.Age} years old");

			// You can also nest ExpandoObjects
			person.Address = new ExpandoObject();
			person.Address.City = "London";
			person.Address.Country = "UK";

			// You can also add methods to ExpandoObjects
			person.SayHello = new Action(() => Console.WriteLine("Hello!"));
			person.SayHello();

			// You can also have a events, but it's a bit different
			person.FallsIll = null;

			// After subscribing is when it becomes an event behind the scenes
			person.FallsIll += new EventHandler<dynamic>((sender, eventArgs) => Console.WriteLine($"A doctor has been called for {eventArgs}"));
			EventHandler<dynamic> e = person.FallsIll;
			e?.Invoke(person, person.Name);

			// We can also gain access to the dictionary behind the ExpandoObject
			IDictionary<string, object> dict = (IDictionary<string, object>)person;
			Console.WriteLine(dict.ContainsKey("Name")); // returns true
			Console.WriteLine(dict.ContainsKey("LastName")); // returns false

			// We can manipulate the dictionary directly
			dict["LastName"] = "Smith";
			Console.WriteLine(person.LastName); // returns Smith
		}
		#endregion
	}
}