using System;
public class Reflection {
	// Reflection lets us programmatically inspect assemblies
	// - Can be used to operate on otherwise inaccessible types

	// System.Type:
	private void SystemTypeExample() {
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
}