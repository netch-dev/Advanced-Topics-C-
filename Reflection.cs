using System.Diagnostics;

namespace Netch.AdvancedTopics {
	public class Reflection {
		// Reflection lets us programmatically inspect assemblies
		// - Can be used to operate on otherwise inaccessible types

		// System.Type:
		// Allows us to find all sorts of information about a type, like its methods, fields, properties
		public void SystemTypeExample() {
			Type t = typeof(int); // Type contains information about a type
			System.Reflection.MethodInfo[] methods = t.GetMethods(); // Returns an array of MethodInfo objects. MethodInfo contains information about a method

			// There is another way to get the type of an object
			Type t2 = "hello".GetType(); // GetType is available on every object, like ToString()
			string typeFullName = t2.FullName; // Returns the full name of the type, in this case System.String
			System.Reflection.FieldInfo[] t2Fields = t2.GetFields(); // Returns an array of FieldInfo objects

			// For built-in types:
			Type? t3 = Type.GetType("System.Int64");
			string? t3FullName = t3.FullName; // Returns System.Int64
			t3.GetMethods(); // Returns an array of MethodInfo objects

			// For generics:
			Type? t4 = Type.GetType("System.Collections.Generic.List`1"); // `1 is the generic type parameter count, only one parameter for a List. Dictionary with a key and value type would be `2
			string? t4FullName = t4.FullName; // Returns System.Collections.Generic.List`1
			System.Reflection.MethodInfo[] t4Methods = t4.GetMethods(); // Returns an array of MethodInfo objects to explore and call with Reflection
		}

		public void InspectionExample() {
			Type t = typeof(Guid);
			string tFullName = t.FullName; // Returns System.Guid

			System.Reflection.ConstructorInfo[] ctors = t.GetConstructors(); // Returns an array of ConstructorInfo objects
			foreach (System.Reflection.ConstructorInfo c in ctors) {
				Console.Write(" - Guid(");
				System.Reflection.ParameterInfo[] parameters = c.GetParameters();
				for (int i = 0; i < parameters.Length; i++) {
					System.Reflection.ParameterInfo par = parameters[i];
					Console.Write($"{par.ParameterType.Name} {par.Name}");
					if (i + 1 != parameters.Length) {
						Console.Write(", ");
					}
				}
				Console.WriteLine(")");
			}

			Console.WriteLine("=====================================");

			System.Reflection.MethodInfo[] methods = t.GetMethods();
			foreach (System.Reflection.MethodInfo method in methods) {
				Console.WriteLine(method.Name);
			}

			Console.WriteLine("=====================================");
			Console.WriteLine("Events: " + t.GetEvents().Length);
		}

		// Instantiate a type, given you have its type information
		// Use the Activator class to find the most appropriate constructor, and invoke it
		public void ReflectionConstructionExample() {
			Type t = typeof(bool);

			object? activator = Activator.CreateInstance(t);
			Console.WriteLine(activator); // Returns false

			Console.WriteLine("====================================");
			// Invoke the same method using a generic parameter
			// In some case this might fail, for example if the constructor is private
			bool t2 = Activator.CreateInstance<bool>();
			Console.WriteLine(t2); // Returns false

			Console.WriteLine("====================================");
			System.Runtime.Remoting.ObjectHandle? wc = Activator.CreateInstance("System", "System.Timers.Timer");
			// Unwrap the object handle to get the actual object
			System.Timers.Timer? timer = (System.Timers.Timer)wc.Unwrap();
			Console.WriteLine("Timer: " + timer); // Returns System.Timers.Timer

			Console.WriteLine("====================================");
			Type? arrayListType = Type.GetType("System.Collections.ArrayList");
			System.Reflection.ConstructorInfo? arrayListCtor = arrayListType.GetConstructor(Array.Empty<Type>());
			object? arrayList = arrayListCtor?.Invoke(Array.Empty<object>());
			Console.WriteLine("Array List: " + arrayList);

			Console.WriteLine("====================================");
			Type st = typeof(string);
			System.Reflection.ConstructorInfo? stringCtor = st.GetConstructor(new[] { typeof(char[]) });
			object str = stringCtor.Invoke(new object[] { new[] { 'n', 'e', 't', 'c', 'h' } });
			Console.WriteLine("String: " + str);

			Console.WriteLine("====================================");
			// Generics
			Type? listType = Type.GetType("System.Collections.Generic.List`1");
			Console.WriteLine(listType); // Returns System.Collections.Generic.List`1[T]

			Type listOfIntType = listType.MakeGenericType(typeof(int));
			System.Reflection.ConstructorInfo? listOfIntCtor = listOfIntType.GetConstructor(Array.Empty<Type>());
			object? theList = listOfIntCtor?.Invoke(Array.Empty<object>());
			Console.WriteLine(theList);

			Console.WriteLine("====================================");
			// Along with Activator, we can also use Array.CreateInstance to create arrays
			Type charType = typeof(char);
			Array charArray = Array.CreateInstance(charType, 10);

			// We can also call the constructor of the array type
			Type charArrayType = charType.MakeArrayType();
			System.Reflection.ConstructorInfo? charArrayCtor = charArrayType.GetConstructor(new[] { typeof(int) });

			// Calling Invoke returns an object so it must be casted to the array type
			char[] array = (char[])charArrayCtor?.Invoke(new object[] { 20 });
			for (int i = 0; i < array.Length; i++) {
				array[i] = (char)('A' + i);
			}
			foreach (char item in array) {
				Console.Write(item + " ");
			}
		}
	}
}
