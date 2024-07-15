namespace Netch.AdvancedTopics {
	public class CleanCode {
		#region Common Code Smells

		#region Poor Names
		// - Code should be self-explanatory and intention revealing
		// -- Not too short, not too long
		// -- Meaningful
		// -- Chosen from problem domain

		// -- Mysterious/poorly named variables, methods, classes, force the reader to look at the implementation
		// -- Long methods, classes, and files are hard to understand
		// -- Meaningless names/comments, comments that are out of date, comments that are redundant

		// - Noisy names:
		// -- theCustomer vs customer
		// -- listOfApprovedCustomers vs approvedCustomers
		#endregion

		#region Inconsistent Naming Conventions
		// - Follow the conventions of the language and the project
		// - This way the project will look like it was written by a single person

		// PascalCase: public class, method, property, event, namespace, enum, type, etc.
		// camelCase: local variable, parameter, method argument, private field, etc.
		public class Customer {
			private int _id;
			public string Name { get; set; }
			public void Charge(int amount) {
				double tax = amount * 0.1;
			}
		}
		#endregion

		#region Poor Method Signatures
		// Orange GetCustomer(int airplane);
		// The method above is not clear about what it does

		// void Parse(int command); vs int Parse(string command);
		#endregion

		#region Long Parameter List 
		// - The more parameters we have in a method, the more difficult it is to understand and use
		// CheckNotifications(null, 1, true, false, DateTime.Now);

		// If you have parameters that are used often together, encapsulate them in a class to reduce the number of parameters

		// Try to limit the number of parameters to 3 or 4
		#endregion

		#region Output Parameters
		// int count = 0;
		// bool more = false;
		// var customers = GetCustomers(pageIndex, out count, out more);

		// It doesn't really make sense to pass data in and return from the arguments.
		// Unless you're working with a method that returns a flag and outs a value

		// Return an object from a method instead of using output parameters
		#endregion

		#region Variable Declarations on the Top
		// In methods with variables declared at the top instead of where theyre used, the reader will have to scroll up and down to understand the code

		// Declare variables close to their usage
		#endregion

		#region Magic Numbers
		// Avoid magic numbers at all times
		#endregion

		#endregion
	}
}